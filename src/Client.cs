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
using System.Threading;
using System.Diagnostics;
using System.Xml.Linq;

namespace Updater {
    class Client {
        private Reciever reciever;
        private DownloadHandler dlHandler;
        private Ui ui;

        private Boolean initialized = false;
        private String serverName;

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

        public void clearChangeLog() {
            ui.getChangelogBox().Clear();
        }

        public void addChangeLog(Changelog changelog) {
            ui.getChangelogBox().Text = changelog + Environment.NewLine + ui.getChangelogBox().Text;
        }

        protected void processUpdates(List<Update> updates) {
            // Process changelog and temp directories
            clearChangeLog();

            foreach(Update update in updates) {
                addChangeLog(update.getChangelog());
                assignTempDirs(update);
            }

            // Start downloading updates
            downloadUpdates(updates);
        }

        private void downloadUpdates(List<Update> updates) {
            // Enqueue each file to the download handler
            foreach (Update update in updates) {

                foreach(GhostFile file in update.getFiles()) {
                    if (file is Archive) {
                        Archive archive = (Archive)file;

                        dlHandler.enqueueFile(archive.getUrl(), Path.Combine(update.getTempDir().FullName,
                            archive.getExtractTo()), archive.getName(),
                        (Boolean cancelled) => {
                            if (!cancelled) {
                                //applyChange();
                                Logger.log(Logger.TYPE.DEBUG, "Completed downloading archive " 
                                    + archive.getName());
                            }
                        });
                    }
                    else {
                        dlHandler.enqueueFile(file.getUrl(), Path.Combine(update.getTempDir().FullName,
                            file.getDestination()), file.getName(),
                        (Boolean cancelled) => {
                            if (!cancelled) {
                                //applyChange();
                                Logger.log(Logger.TYPE.DEBUG, "Completed downloading file "
                                    + file.getName());
                            }
                        });
                    }
                }
            }

            dlHandler.setQueueFileCallback(new QueueCallback(() => {
                Logger.log(Logger.TYPE.DEBUG, "Completed all the downloads");
            }));

            dlHandler.startFileQueue();
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

        protected void ensureDestination(Update update) {
        }

        public String getTempPath() {
            return Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                getServerName()
            );
        }

        public String getServerName() {
            return serverName;
        }

        public void setServerName(String name) {
            this.serverName = name;

            ui.getNameLabel().Text = name;
            ui.getNameLabel().Visible = true;
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
