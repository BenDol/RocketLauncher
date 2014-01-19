using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace Updater {

    class Request {
        private XDocument serverXML;

        Action onCompleted;

        private Uri url;
        private DateTime dateTime;
        private DownloadHandler dlHandler;

        private bool dead = false;

        public Request(Uri url, DownloadHandler dlHandler) {
            this.url = url;
            this.dlHandler = dlHandler;
            this.dateTime = new DateTime();
        }

        public void loadServerXML() {
            dlHandler.downloadStringAsync(url, (object sender, DownloadStringCompletedEventArgs e) => {
                try {
                    serverXML = XDocument.Parse(e.Result);
                }
                catch (XmlException ex) {
                    Logger.log(Logger.TYPE.DEBUG, "Error while requesting server XML: " 
                        + ex.Message + ex.StackTrace);

                    dead = true;

                    if (onCompleted != null) {
                        onCompleted();
                    }
                }
            });
        }

        public void send() {
            loadServerXML();
        }

        public void setOnCompleted(Action onCompleted) {
            this.onCompleted = onCompleted;
        }

        public bool isDead() {
            return dead;
        }
    }
}
