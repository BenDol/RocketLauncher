/*
 * Copyright (c) 2010-2014 Launcher <https://github.com/BenDol/RocketLauncher>
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
 */
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net.NetworkInformation;
using System.Net;
using System.IO;
using System.IO.Compression;
using System.Threading;
using System.Diagnostics;
using System.Xml.Linq;
using Launcher.Interface;
using Controls.Development;

namespace Launcher {
    class Client {
        private Reciever reciever;
        private DownloadHandler dlHandler;
        private Ui ui;

        private Boolean initialized = false;
        private String serverName;

        public static String UPDATE_XML_NAME = @"update.xml";

        public Client(Ui ui) {
            this.ui = ui;

            dlHandler = new DownloadHandler(ui.getStatusLabel(), 
                ui.getDownloadProgressBar());

            reciever = new Reciever(ref dlHandler);
        }

        public void initialize(Action callback) {
            reciever.getServerName(new StringAsyncCallback((String name) => {
                setServerName(name);
                initialized = true;

                populateUpdateLogs();

                callback();
            },
            () => {
                Logger.log(Logger.TYPE.FATAL, "Failed to retrieve server name, " 
                    + "will not initialize application.");
            }));
        }

        public void update() {
            if (!isInitialized()) {
                Logger.log(Logger.TYPE.WARN, "You are attempting to update" 
                    + " before initializing the client, update cancelled.");
                return;
            }

            Logger.log(Logger.TYPE.INFO, "Checking for updates...");

            ui.getRefreshButton().Enabled = false;

            ui.getTickImage().Visible = false;
            ui.getUpToDateLabel().Visible = false;

            ui.getPlayButton().Enabled = false;
            ui.getPlayButton().BtnText = "Updating";

            reciever.sendRequest(new RequestAsyncCallback((List<Update> updates, TimeSpan response) => {
                Logger.log(Logger.TYPE.INFO, "Found " + updates.Count + " new updates in "
                    + response.Milliseconds + "ms");

                processUpdates(updates);
            },
            () => {
                Logger.log(Logger.TYPE.WARN, "Failed to make update request, "
                    + "ensure your XML files have not been corrupted.");
            }));
        }

        public void refresh() {
            clearChangeLog(ui.getChangelogListBox(), ui.getChangelogBox());
            populateUpdateLogs();

            update();
        }

        private void populateUpdateLogs() {
            List<Update> updates = reciever.getPreviousUpdates();

            addUpdateListChangeHandler(ui.getUpdatesListBox(), 
                updates, ui.getUpdatelogsTextBox());

            clearChangeLog(ui.getUpdatesListBox(), ui.getUpdatelogsTextBox());
            foreach(Update update in updates) {
                addChangeLog(ui.getUpdatesListBox(), update.getChangelog(), 
                    update.getName(), update.getBaseType());
            }
        }

        public void clearChangeLog(ListBox listBox, TextBox textBox) {
            textBox.Clear();
            listBox.Items.Clear();
        }

        public void addChangeLog(ListBox listBox, Changelog changelog, 
                String updateName, String baseType) {

            String text = "[" + changelog.getVersion() + baseType + "] " + updateName;
            ImageListBoxItem item = new ImageListBoxItem(text, 0);

            listBox.Items.Add(item);
        }

        protected void processUpdates(List<Update> updates) {
            // Process changelog and temp directories
            clearChangeLog(ui.getChangelogListBox(), ui.getChangelogBox());

            if (updates.Count > 0) {
                addUpdateListChangeHandler(ui.getChangelogListBox(), updates,
                    ui.getChangelogBox());

                foreach (Update update in updates) {
                    addChangeLog(ui.getChangelogListBox(), update.getChangelog(), 
                        update.getName(), update.getBaseType());

                    assignTempDirs(update);
                }

                // Start downloading updates
                downloadUpdates(updates);
            }
            else {
                ui.getStatusLabel().Text = "Everything is up to date!";
                ui.getDownloadProgressBar().Value = 100;

                enablePlay();
            }
        }

        public static void addUpdateListChangeHandler(ListBox listBox, List<Update> updates,
                TextBox textBox) {
            listBox.SelectedValueChanged += new EventHandler(
                    (object sender, EventArgs e) => {
                ListBox lb = sender as ListBox;
                if (lb != null) {
                    foreach (Update update in updates) {
                        double version = update.getVersion();
                        String baseType = update.getBaseType();

                        if (lb.SelectedItem.ToString().Contains("[" + version + baseType + "]")) {
                            textBox.Text = update.getChangelog().ToString();
                            break;
                        }
                    }
                }
            });
        }

        private void downloadUpdates(List<Update> updates) {
            // Enqueue each file to the download handler
            foreach (Update update in updates) {

                foreach(GhostFile file in update.getFiles()) {
                    Boolean isArchive = file is Archive;

                    String tmpPath = Path.Combine(update.getTempDir().FullName,
                        isArchive ? ((Archive)file).getExtractTo() 
                        : file.getDestination());

                    dlHandler.enqueueFile(file.getUrl(), tmpPath, file.getName(),
                    (Boolean cancelled) => {
                        if (!cancelled) {
                            Logger.log(Logger.TYPE.DEBUG, "Completed downloading " 
                                + (isArchive ? "archive " : "file ") + file.getName());
                        }
                    });
                }
            }

            dlHandler.setQueueFileCallback(new QueueCallback(() => {
                Logger.log(Logger.TYPE.DEBUG, "Completed all the downloads");

                ui.getStatusLabel().Text = "Patching files...";
                foreach (Update update in updates) {
                    foreach (GhostFile file in update.getFiles()) {
                        String tmpPath = Path.Combine(update.getTempDir().FullName,
                            file is Archive ? ((Archive)file).getExtractTo()
                            : file.getDestination());

                        update.setSuccess(applyFileChange(file, tmpPath));
                    }

                    if (update.isSuccess()) {
                        reciever.stampUpdate(update);
                    }
                    else {
                        Logger.log(Logger.TYPE.FATAL, "One of the updates did not succeed, "
                            + "cancelling update process.");

                        ui.getStatusLabel().Text = "Error while patching files, please check the "
                            +"log for more details.";

                        ui.getPlayButton().BtnText = "Failed";
                        return;
                    }
                }

                ui.getStatusLabel().Text = "Finished updating "
                    + updates.Count + "/" + updates.Count + "!";

                ui.getTickImage().Visible = true;
                ui.getUpToDateLabel().Visible = true;
                ui.getRefreshButton().Enabled = true;

                enablePlay();
            }));

            dlHandler.startFileQueue();
        }

        private bool applyFileChange(GhostFile file, String tmpPath) {
            bool success = true;
            String tmpFile = Path.Combine(tmpPath, file.getName());
            String newPath = Path.Combine(getRootDir(), ((file is Archive) ? 
                ((Archive)file).getExtractTo() : file.getDestination()));

            if (verifyFile(tmpFile)) {
                if (ensureDirectory(newPath)) {
                    if (file is Archive) {
                        try {
                            Archive archive = (Archive)file;
                            if (archive.getCleanDirs()) {
                                // TODO: Clean root directory
                            }

                            using (ZipArchive a = ZipFile.OpenRead(tmpFile)) {
                                foreach (ZipArchiveEntry entry in a.Entries) {
                                    if (!entry.FullName.EndsWith("/", StringComparison.OrdinalIgnoreCase)) {
                                        entry.ExtractToFile(Path.Combine(newPath, entry.FullName), true);
                                    }
                                    else {
                                        String dir = entry.FullName.Replace("/", "//");

                                        bool createDir = !Directory.Exists(dir);
                                        if (!createDir) {
                                            if (archive.getCleanDirs()) {
                                                Directory.Delete(dir, true);
                                                createDir = true;
                                            }
                                        }
                                        if (createDir) {
                                            Directory.CreateDirectory(dir);
                                        }
                                    }
                                }
                            }

                            Logger.log(Logger.TYPE.DEBUG, "Extracted archive " + tmpFile + " to " + newPath);
                        }
                        catch (Exception ex) {
                            Logger.log(Logger.TYPE.ERROR, "Unable to patch new file to required"
                                + " path. " + file.getName() + " " + ex.Message + ex.StackTrace);

                            success = false;
                        }
                    }
                    else {
                        try {
                            File.Copy(tmpFile, Path.Combine(newPath, file.getName()), true);

                            Logger.log(Logger.TYPE.DEBUG, "Copied " + tmpFile + " to " + newPath);
                        }
                        catch (Exception ex) {
                            Logger.log(Logger.TYPE.ERROR, "Unable to patch new file to required"
                                + " path. " + file.getName() + " " + ex.Message + ex.StackTrace);

                            success = false;
                        }
                    }
                }
            }
            return success;
        }

        public bool verifyFile(String file) {
            bool exists = File.Exists(file);
            if(!exists) {
                Logger.log(Logger.TYPE.ERROR, "Could not verify temp file: " + file);
            }
            return exists;
        }

        public bool ensureDirectory(String dir) {
            bool exists = Directory.Exists(dir);
            if (!exists) {
                try {
                    Directory.CreateDirectory(dir);
                }
                catch (Exception ex) {
                    Logger.log(Logger.TYPE.ERROR, "Unable to create directory " 
                        + dir + ex.Message + ex.StackTrace);

                    exists = false;
                }
            }
            return exists;
        }

        protected void assignTempDirs(Update update) {
            String path = Path.Combine(getTempPath(), Convert.ToString(update.getVersion()));
            if (Directory.Exists(path)) {
                Directory.Delete(path, true);
            }
            update.setTempDir(Directory.CreateDirectory(path));

            foreach(GhostFile file in update.getFiles()) {
                file.setTempDir(Directory.CreateDirectory(Path.Combine(
                    path, file.getDestination())));
            }
        }

        public String getRootDir() {
            return Application.StartupPath;
        }

        public String getTempPath() {
            return Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                getServerName()
            );
        }

        public static String getLogging() {
            String file = Path.Combine(Application.StartupPath,
                Client.UPDATE_XML_NAME);

            String logging = "";
            XDocument xml = Xml.load(file);
            if (xml != null) {
                var root = from item in xml.Descendants("updates")
                    select new {
                        logging = item.Attribute("logging")
                    };

                foreach (var data in root) {
                    logging = data.logging.Value;
                }
            }
            return logging;
        }

        public String getServerName() {
            return serverName;
        }

        public void setServerName(String name) {
            this.serverName = name;

            ui.getNameLabel().Text = name;
            ui.getNameLabel().Visible = true;
        }

        private void enablePlay() {
            ui.getPlayButton().Enabled = true;
            ui.getPlayButton().BtnText = "Play";
        }

        public Reciever getReciever() {
            return reciever;
        }

        public DownloadHandler getDownloadHandler() {
            return dlHandler;
        }
         
        public Ui getUi() {
            return ui;
        }

        public bool isInitialized() {
            return initialized;
        }
    }
}
