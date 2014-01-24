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
        private DownloadHandler downloadHandler;
        private Ui ui;

        private Boolean initialized = false;
        private String serverName;

        public Client(Ui ui) {
            this.ui = ui;

            downloadHandler = new DownloadHandler(ui.getStatusLabel(), 
                ui.getDownloadProgressBar());

            reciever = new Reciever(ref downloadHandler);
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
                assignTempDir(update);
            }


        }

        protected void assignTempDir(Update update) {
            String path = Path.Combine(getTempPath(), Convert.ToString(update.getVersion()));
            if (Directory.Exists(path)) {
                Directory.Delete(path);
            }
            update.setTempDir(Directory.CreateDirectory(path));
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
            return downloadHandler;
        }
         
        public Ui getUi() {
            return ui;
        }

        public bool isInitialized() {
            return initialized;
        }
    }
}
