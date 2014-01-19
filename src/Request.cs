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
        private  RequestCallback callback;

        private Uri url;
        private DateTime dateTime;
        private DownloadHandler dlHandler;
        private Double lastVersion;

        List<UpdateNode> updateNodes = new List<UpdateNode>();

        private bool dead = false;

        public Request(Uri url, DownloadHandler dlHandler, Double lastVersion) {
            this.url = url;
            this.dlHandler = dlHandler;
            this.lastVersion = lastVersion;
        }

        public void send() {
            if (isDead()) {
                return;
            }

            dateTime = new DateTime();
            dlHandler.downloadStringAsync(url, (object sender, DownloadStringCompletedEventArgs e) => {
                try {
                    serverXML = XDocument.Parse(e.Result);

                    updateNodes = getUpdates();
                    if (callback != null) {
                        callback.onSuccess(updateNodes);
                    }
                }
                catch (XmlException ex) {
                    Logger.log(Logger.TYPE.ERROR, "Error while requesting server XML: " 
                        + ex.Message + ex.StackTrace);

                    kill();

                    if (callback != null) {
                        callback.onFailure();
                    }
                }
            });
        }

        private List<UpdateNode> getUpdates() {
            List<UpdateNode> updateNodes = new List<UpdateNode>();
            if (isDead()) {
                return updateNodes;
            }

            var root = from item in serverXML.Descendants("server")
                select new {
                    updates = item.Descendants("update")
                };

            foreach (var data in root) {
                foreach (var update in data.updates) {
                    String versionStr = update.Attribute("version").Value;
                    try {
                        double version = Convert.ToDouble(versionStr);
                        if (version > lastVersion) {
                            updateNodes.Add(new UpdateNode(this, update.Value, version));
                            Logger.log(Logger.TYPE.DEBUG, "Found a new update: " + versionStr);
                        }
                    }
                    catch (FormatException e) {
                        Logger.log(Logger.TYPE.ERROR, "Problem converting 'version' for update " 
                            + versionStr + e.Message);
                    }
                }
            }
            return orderUpdateNodes(updateNodes);
        }

        public List<UpdateNode> orderUpdateNodes(List<UpdateNode> updateNodes) {
            updateNodes.Sort(delegate(UpdateNode u1, UpdateNode u2) {
                return u1.getVersion().CompareTo(u2.getVersion());
            });
            return updateNodes;
        }

        public void setCallback(RequestCallback callback) {
            this.callback = callback;
        }

        public DateTime getDateTime() {
            return dateTime;
        }

        public void kill() {
            dead = true;
            updateNodes.Clear();
            serverXML = null;
            callback = null;
            url = null;
            dlHandler = null;
            lastVersion = 0;
        }

        public bool isDead() {
            return dead;
        }
    }
}
