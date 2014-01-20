using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Reflection;
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

        Dictionary<UpdateNode, String> results = new Dictionary<UpdateNode, String>();

        List<UpdateNode> updateNodes = new List<UpdateNode>();
        List<Update> updates = new List<Update>();

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
            dlHandler.downloadStringAsync(url, (object s1, DownloadStringCompletedEventArgs e1) => {
                try {
                    serverXML = XDocument.Parse(e1.Result);
                    updateNodes = getUpdateNodes();

                    foreach (UpdateNode node in updateNodes) {
                        dlHandler.enqueueString(new Uri(node.getUrl()), (String result) => {
                            try {
                                results.Add(node, result);
                            }
                            catch (ArgumentException ex1) {
                                Logger.log(Logger.TYPE.ERROR, "Error in string queue callback: "
                                    + ex1.Message + ex1.StackTrace);
                            }
                            catch (TargetInvocationException ex2) {
                                Logger.log(Logger.TYPE.ERROR, "Error in string queue callback: "
                                    + ex2.Message + ex2.StackTrace);
                            }
                        });
                    }

                    // Process results when the queue has finished
                    dlHandler.setQueueStringCallback(new QueueCallback(() => {
                        foreach (KeyValuePair<UpdateNode, String> entry in results) {
                            try {
                                updates.Add(compileUpdate(entry.Key, XDocument.Parse(entry.Value)));
                            }
                            catch(XmlException e) {
                                Logger.log(Logger.TYPE.ERROR, "Error parsing update " + entry.Key.getVersion() 
                                    + " xml " + e.Message + e.StackTrace);
                            }
                        }

                        if (callback != null) {
                            callback.onSuccess(updates);
                        }
                    }));

                    dlHandler.startStringQueue();
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

        private List<UpdateNode> getUpdateNodes() {
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
            return updateNodes;
        }

        private Update compileUpdate(UpdateNode node, XDocument xml) {
            Update update = new Update(node);
            if (isDead()) {
                return update;
            }

            var root = from item in xml.Descendants("update")
                select new {
                    changelog = item.Descendants("changelog"),
                    files = item.Descendants("file"),
                    archives = item.Descendants("archive")
                };

            // Add the changelog data
            foreach (var data in root) {
                foreach (var clog in data.changelog) {
                    Changelog changelog = new Changelog();
                    foreach (var log in clog.Descendants("log")) {
                        changelog.addLog(new Changelog.Log(log.Value));
                    }
                    update.setChangelog(changelog);
                    break;
                }
            }

            // Add the file data
            foreach (var data in root) {
                foreach (var f in data.files) {
                    String name = f.Attribute("name").Value;
                    String destination = f.Attribute("destination").Value;
                    String mime = f.Attribute("mime").Value;

                    update.addFile(new GhostFile(name, destination, mime, new Uri(f.Value)));
                }
            }

            // Add the archive data
            foreach (var data in root) {
                foreach (var f in data.archives) {
                    String name = f.Attribute("name").Value;
                    String extractTo = f.Attribute("extractTo").Value;
                    String mime = f.Attribute("mime").Value;

                    update.addFile(new Archive(name, extractTo, mime, new Uri(f.Value)));
                }
            }

            return update;
        }

        public static List<Update> orderUpdates(List<Update> updates) {
            updates.Sort(delegate(Update u1, Update u2) {
                return u1.getVersion().CompareTo(u2.getVersion());
            });
            return updates;
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
            updates.Clear();
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
