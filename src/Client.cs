/*
 * Copyright (c) 2014 RocketLauncher <https://github.com/BenDol/RocketLauncher>
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
using System.Linq;
using System.Windows.Forms;
using System.IO;
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
        private String targetPath;

        public static String UPDATE_XML_NAME = @"update.xml";

        public Client(Ui ui) {
            this.ui = ui;

            dlHandler = new DownloadHandler(ui.getStatusLabel(), ui.getDownloadProgressBar());
            reciever = new Reciever(ref dlHandler, this);

            Logger.log(Logger.TYPE.DEBUG, "Client successfully constructed.");
        }

        public void initialize(Action callback) {
            reciever.getInitialData(new InitAsyncCallback((String name, String path) => {
                setServerName(name);
                setTargetPath(path);

                reciever.getFonts(new FontAsyncCallback((List<FontPackage> packages) => {
                    foreach (FontPackage package in packages) {
                        if (package.isInstalled()) {
                            applyCustomFonts(package.getApplyMap());
                        }
                    }
                },
                () => {
                    Logger.log(Logger.TYPE.FATAL, "Failed to get font packages");
                    MessageBox.Show("Failed to get font packages, check the log file for more details.");
                }));

                reciever.getLayout(new LayoutAsyncCallback((List<Component> components) => {
                    foreach (Component component in components) {
                        ui.addOrModifyComponent(component);
                    }

                    initialized = true;
                    populateUpdateLogs();
                    callback();
                },
                () => {
                    Logger.log(Logger.TYPE.FATAL, "Failed to get layout data");
                    MessageBox.Show("Failed to get layout data, check the log file for more details.");

                    initialized = true;
                    populateUpdateLogs();
                    callback();
                }));
            },
            () => {
                Logger.log(Logger.TYPE.FATAL, "Failed to retrieve server name, " 
                    + "will not initialize application.");
            }));
        }

        public void update() {
            Logger.log(Logger.TYPE.DEBUG, "Initializing update sequence.");
            if (!isInitialized()) {
                Logger.log(Logger.TYPE.WARN, "You are attempting to update" 
                    + " before initializing the client, update cancelled.");
                return;
            }

            Logger.log(Logger.TYPE.INFO, "Checking for updates...");

            ui.getRefreshButton().Enabled = false;

            ui.getTickImage().Visible = false;
            ui.getUpToDateLabel().Visible = false;

            ui.getLaunchButton().Enabled = false;
            ui.getLaunchButton().BtnText = "Updating";

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
                    update.getName(), update.getBaseType(), 0);
            }
        }

        public void clearChangeLog(ImageListBox listBox, TextBox textBox) {
            Logger.log(Logger.TYPE.DEBUG, "Clearing the changelog.");
            textBox.Clear();
            listBox.Items.Clear();
        }

        public void addChangeLog(ImageListBox listBox, Changelog changelog, 
                String updateName, String baseType, int imageIndex) {
            Logger.log(Logger.TYPE.DEBUG, "Add changelog.");

            String text = "[" + changelog.getVersion() + baseType + "] " + updateName;
            ImageListBoxItem item = new ImageListBoxItem(text, imageIndex);

            listBox.Items.Add(item);
        }

        protected void processUpdates(List<Update> updates) {
            Logger.log(Logger.TYPE.DEBUG, "Processing updates...");

            // Process changelog and temp directories
            clearChangeLog(ui.getChangelogListBox(), ui.getChangelogBox());

            if (updates.Count > 0) {
                addUpdateListChangeHandler(ui.getChangelogListBox(), updates,
                    ui.getChangelogBox());

                foreach (Update update in updates) {
                    addChangeLog(ui.getChangelogListBox(), update.getChangelog(), 
                        update.getName(), update.getBaseType(), 2);

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

        public static void addUpdateListChangeHandler(ImageListBox listBox, List<Update> updates,
                TextBox textBox) {
            Logger.log(Logger.TYPE.DEBUG, "Add update list change handler.");
            listBox.SelectedValueChanged += new EventHandler(
                    (object sender, EventArgs e) => {
                ImageListBox lb = sender as ImageListBox;
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

        public ImageListBoxItem getLogItem(Update update, ImageListBox listBox) {
            ImageListBoxItem item = null;
            foreach(ImageListBoxItem it in listBox.Items) {
                if (it.Text.Contains("[" + update.getVersion() + update.getBaseType() + "]")) {
                    item = it;
                    break;
                }
            }
            return item;
        }

        private void downloadUpdates(List<Update> updates) {
            Logger.log(Logger.TYPE.DEBUG, "Downloading " + updates.Count + " updates...");
            // Enqueue each file to the download handler
            foreach (Update update in updates) {
                foreach(GhostFile file in update.getFiles()) {
                    Boolean isArchive = file is Archive;

                    String tmpPath = Path.Combine(update.getTempDir().FullName,
                        isArchive ? ((Archive)file).getExtractTo() : file.getDestination());

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

                        ImageListBox listBox = ui.getChangelogListBox();
                        ImageListBoxItem item = getLogItem(update, listBox);
                        if (item != null) {
                            item.ImageIndex = 0;
                            listBox.Invalidate(listBox.GetItemRectangle(item.Index));
                            listBox.Update();
                        }
                    }
                    else {
                        Logger.log(Logger.TYPE.FATAL, "One of the updates did not succeed, "
                            + "cancelling update process.");

                        ui.getStatusLabel().Text = "Error while patching files, please check the "
                            + "log for more details.";

                        ui.getLaunchButton().BtnText = "Failed";
                        return;
                    }
                }

                ui.getStatusLabel().Text = "Finished updating "
                    + updates.Count + "/" + updates.Count + "!";

                ui.getTickImage().Visible = true;
                ui.getUpToDateLabel().Visible = true;
                
                enablePlay();
            }));

            dlHandler.startFileQueue();
        }

        private bool applyFileChange(GhostFile file, String tmpPath) {
            Logger.log(Logger.TYPE.DEBUG, "Apply file change " + file.getName() + " path: " + tmpPath);
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

                            ArchiveHandler.extractZip(tmpFile, newPath, archive.getCleanDirs());

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
            Logger.log(Logger.TYPE.DEBUG, "Verify file '" + file + "' exists.");
            bool exists = File.Exists(file);
            if(!exists) {
                Logger.log(Logger.TYPE.ERROR, "Could not verify file: " + file);
            }
            return exists;
        }

        public bool ensureDirectory(String dir) {
            Logger.log(Logger.TYPE.DEBUG, "Ensure directory '" + dir + "' exists.");
            bool exists = Directory.Exists(dir);
            if (!exists) {
                try {
                    Directory.CreateDirectory(dir);
                    exists = true;
                }
                catch (Exception ex) {
                    Logger.log(Logger.TYPE.ERROR, "Unable to create directory " 
                        + dir + ex.Message + ex.StackTrace);
                }
            }
            return exists;
        }

        protected void assignTempDirs(Update update) {
            Logger.log(Logger.TYPE.DEBUG, "Assign temporary directories.");

            String path = Path.Combine(getTempPath(), Convert.ToString(update.getVersion()));
            if (Directory.Exists(path)) {
                try {
                    Directory.Delete(path, true);
                } catch(IOException ex) {
                    Logger.log(Logger.TYPE.FATAL, "Unable to delete temp dir: " + ex.Message + ex.StackTrace);
                }
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

        public String getFontCacheDir() {
            return Path.Combine(getTempPath(), "fontcache");
        }

        private void applyCustomFonts(Dictionary<String, FontApply> applyMap) {
            Logger.log(Logger.TYPE.DEBUG, "Applying " + applyMap.Count + " custom fonts");

            foreach (KeyValuePair<String, FontApply> pair in applyMap) {
                String to = pair.Key;
                FontApply fontApply = pair.Value;

                foreach (Control control in ui.getControls()) {
                    if (control.Name.Equals(to)) {
                        control.Font = fontApply.getFont();
                    }
                }
            }
        }

        public static String getLogging() {
            String file = Path.Combine(Application.StartupPath,
                Client.UPDATE_XML_NAME);

            String logging = "";
            XDocument xml = Xml.load(file);
            if (xml != null) {
                var root = from item in xml.Descendants("Updates")
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

        public String getTargetPath() {
            return targetPath;
        }

        public void setTargetPath(String path) {
            this.targetPath = path;
        }

        private void enablePlay() {
            Logger.log(Logger.TYPE.DEBUG, "Enable Play button");
            ui.getRefreshButton().Enabled = true;
            ui.getLaunchButton().Enabled = true;
            ui.getLaunchButton().BtnText = "Play";
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

        public void executeTarget() {
            Logger.log(Logger.TYPE.DEBUG, "Executing target application (" + getTargetPath() + ").");
            Process p = new Process();
            p.StartInfo.FileName = getTargetPath();
            p.StartInfo.Arguments = "";
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardOutput = true;
            p.Start();

            exitApplication();
        }

        public void exitApplication() {
            try {
                Environment.Exit(0);
            }
            catch (Win32Exception ex) {
                Logger.log(Logger.TYPE.WARN, "Exception caught during exit process: "
                    + ex.Message + ex.StackTrace);
            }
        }
    }
}
