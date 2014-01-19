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
        private WebClient webClient = new WebClient();

        private Reciever reciever;
        private DownloadHandler downloadHandler;
        private Ui ui;

        public Client(Ui ui) {
            this.ui = ui;

            downloadHandler = new DownloadHandler(webClient, ui);
            reciever = new Reciever(downloadHandler);
        }

        public void update() {
            reciever.sendRequest(new RequestCallback((List<UpdateNode> updateNodes) => {
                //
            }, () => {
                Logger.log(Logger.TYPE.WARN, "Failed to make update request, " 
                    + "ensure your XML files have not been corrupted.");
            }));
        }

        public WebClient getWebClient() {
            return webClient;
        }

        public Reciever getReciever() {
            return reciever;
        }

        public Ui getUi() {
            return ui;
        }
    }
}
