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

        public Client(Ui ui) {
            this.ui = ui;
            this.ui.setClient(this);

            downloadHandler = new DownloadHandler(ui);
            reciever = new Reciever(downloadHandler);
        }

        public void update() {
            Logger.log(Logger.TYPE.INFO, "Checking for updates...");

            reciever.sendRequest(new RequestCallback((List<Update> updates, TimeSpan response) => {
                Logger.log(Logger.TYPE.INFO, "Found " + updates.Count + " new updates in "
                    + response.Milliseconds + "ms");

                clearChangeLog();
                foreach(Update update in updates) {
                    addChangeLog(update);
                }
            },
            () => {
                Logger.log(Logger.TYPE.WARN, "Failed to make update request, "
                    + "ensure your XML files have not been corrupted.");
            }));
        }

        public void clearChangeLog() {
            ui.getChangelogBox().Clear();
        }

        public void addChangeLog(Update update) {
            ui.getChangelogBox().Text += update.getChangelog() + Environment.NewLine;
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
    }
}
