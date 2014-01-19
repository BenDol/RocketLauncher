using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace Updater {

    /**
     * Requests the server to see if there is an update available.
     **/
    class Request {
        private XDocument serverXML;

        RequestCallback callback;

        private Uri url;
        private DateTime dateTime;
        private DownloadHandler dlHandler;
        private Double lastVersion;

        private bool dead = false;

        public Request(Uri url, DownloadHandler dlHandler, Double lastVersion) {
            this.url = url;
            this.dlHandler = dlHandler;
            this.lastVersion = lastVersion;
            this.dateTime = new DateTime();
        }

        public void loadServerXML() {
            dlHandler.downloadStringAsync(url, (object sender, DownloadStringCompletedEventArgs e) => {
                try {
                    serverXML = XDocument.Parse(e.Result);

                    if (callback != null) {
                        callback.onSuccess(getUpdates());
                    }
                }
                catch (XmlException ex) {
                    Logger.log(Logger.TYPE.DEBUG, "Error while requesting server XML: " 
                        + ex.Message + ex.StackTrace);

                    dead = true;

                    if (callback != null) {
                        callback.onFailure();
                    }
                }
            });
        }

        private List<UpdateNode> getUpdates() {
            var root = from item in serverXML.Descendants("server")
                select new {
                    updates = item.Descendants("update")
                };

            List<UpdateNode> updates = new List<UpdateNode>();
            foreach (var data in root) {
                foreach (var update in data.updates) {
                    String versionStr = update.Attribute("version").Value;
                    try {
                        double version = Convert.ToDouble(versionStr);
                        if (version > lastVersion) {
                            updates.Add(new UpdateNode(this, update.Value, version));
                            Logger.log(Logger.TYPE.DEBUG, "Found a new update: " + versionStr);
                        }
                    }
                    catch (FormatException e) {
                        Logger.log(Logger.TYPE.ERROR, "Problem converting 'version' for update " 
                            + versionStr + e.Message);
                    }
                }
            }
            return updates;
        }

        public void send() {
            loadServerXML();
        }

        public void setCallback(RequestCallback callback) {
            this.callback = callback;
        }

        public bool isDead() {
            return dead;
        }
    }
}
