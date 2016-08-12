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
using System.Linq;
using System.Net;
using System.Reflection;
using System.Xml;
using System.Xml.Linq;

namespace Launcher {

    /**
     * Requests the server to see if there is an update available.
     **/
    class Request {
        private XDocument serverXML;
        private RequestAsyncCallback callback;

        private Uri url;
        private Uri directorUrl;
        private String director;
        private DateTime dateTime;
        private DownloadHandler dlHandler;
        private Double lastVersion;
        private TimeSpan responseTime = new TimeSpan();

        Dictionary<UpdateNode, String> results = new Dictionary<UpdateNode, String>();

        List<UpdateNode> updateNodes = new List<UpdateNode>();
        List<Update> updates = new List<Update>();

        private bool dead = false;

        public Request(Uri url, String director, ref DownloadHandler dlHandler, Double lastVersion) {
            this.url = url;
            this.director = director;
            this.dlHandler = dlHandler;
            this.lastVersion = lastVersion;
        }

        public Uri getDirectorPath() {
            if(directorUrl == null) {
                directorUrl = new Uri(url.AbsoluteUri + "/" + director);
            }
            return directorUrl;
        }

        public void send() {
            send(null);
        }

        public void send(XDocument serverXMLCached) {
            Logger.log(Logger.TYPE.DEBUG, "Sending request...");
            if (isDead()) {
                return;
            }

            dateTime = DateTime.Now;

            if (serverXMLCached == null) {
                dlHandler.downloadStringAsync(getDirectorPath(), (object s1,
                        DownloadStringCompletedEventArgs e) => {
                    Logger.log(Logger.TYPE.DEBUG, "Successfully recieved the server XML.");
                    try {
                        serverXML = XDocument.Parse(e.Result);
                        requestUpdates();
                    }
                    catch (Exception ex) {
                        Logger.log(Logger.TYPE.ERROR, "Error while parsing server XML: "
                            + ex.Message + ex.StackTrace + " URL: " + getDirectorPath().OriginalString);

                        kill();

                        if (callback != null) {
                            callback.onFailure();
                        }
                    }
                });
            } else {
                serverXML = serverXMLCached;
                requestUpdates();
            }
        }

        private void requestUpdates() {
            updateNodes = getUpdateNodes();

            foreach (UpdateNode node in updateNodes) {
                Uri updateUrl = new Uri(url.AbsoluteUri + "/" + node.getFullPath());
                dlHandler.enqueueString(updateUrl, node.getFileName(),
                    (String result) => {
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

                setResponseTime(DateTime.Now);

                if (callback != null) {
                    callback.onSuccess(updates, responseTime);
                }

                kill(); // Kill the request
            }));

            dlHandler.startStringQueue();
        }

        private List<UpdateNode> getUpdateNodes() {
            List<UpdateNode> updateNodes = new List<UpdateNode>();
            if (isDead()) {
                return updateNodes;
            }

            try {
                var root = from item in serverXML.Descendants("Server")
                    select new {
                        updates = item.Descendants("Update")
                    };

                foreach (var data in root) {
                    foreach (var update in data.updates) {
                        String versionStr = update.Attribute("version").Value.Trim();
                        try {
                            XAttribute dir = update.Attribute("dir");
                            double version = Convert.ToDouble(versionStr);
                            if (version > lastVersion) {
                                UpdateNode updateNode = new UpdateNode(this, update.Value.Trim(), version, dir != null ? dir.Value : "");
                                updateNodes.Add(updateNode);
                                Logger.log(Logger.TYPE.DEBUG, "Found a new update: " + versionStr + ", url: " + updateNode.getFullPath());
                            }
                        }
                        catch (FormatException e) {
                            Logger.log(Logger.TYPE.ERROR, "Problem converting 'version' for update "
                                + versionStr + e.Message);
                        }
                    }
                }
            }
            catch (Exception ex) {
                Logger.log(Logger.TYPE.ERROR, "Problem while getting update nodes "
                    + ex.Message + ex.StackTrace);
            }
            return updateNodes;
        }

        private Update compileUpdate(UpdateNode node, XDocument xml) {
            Update update = new Update(node);
            if (isDead()) {
                return update;
            }

            try {
                var root = from item in xml.Descendants("Update")
                    select new {
                        changelog = item.Descendants("Changelog"),
                        files = item.Descendants("File"),
                        archives = item.Descendants("Archive"),
                        name = item.Attribute("name"),
                        baseType = item.Attribute("base")
                    };

                foreach (var data in root) {

                    // Set Attributes
                    update.setName(data.name.Value);
                    if (data.baseType != null) {
                        update.setBaseType(data.baseType.Value);
                    }

                    // Add the changelog data
                    foreach (var clog in data.changelog) {
                        Changelog changelog = new Changelog(update.getVersion());
                        foreach (var log in clog.Descendants("Log")) {
                            changelog.addLog(new Changelog.Log(log.Value.Trim()));
                        }
                        update.setChangelog(changelog);
                        break;
                    }

                    // Add the file data
                    foreach (var f in data.files) {
                        String name = Xml.getAttributeValue(f.Attribute("name"));
                        String destination = Xml.getAttributeValue(f.Attribute("destination"));
                        String mime = Xml.getAttributeValue(f.Attribute("mime"), "none");

                        update.addFile(new GhostFile(name, destination, mime,
                            new Uri(url + "/" + node.getDir() + "/" + name)));
                    }

                    // Add the archive data
                    foreach (var f in data.archives) {
                        String name = Xml.getAttributeValue(f.Attribute("name"));
                        String extractTo = Xml.getAttributeValue(f.Attribute("extractTo"));
                        String mime = Xml.getAttributeValue(f.Attribute("mime"), "none");

                        Boolean cleanDirs = false;
                        if (f.Attribute("cleanDirs") != null) {
                            cleanDirs = Convert.ToBoolean(f.Attribute("cleanDirs").Value.Trim());
                        }

                        update.addFile(new Archive(name, extractTo, mime, 
                            new Uri(url + "/" + node.getDir() + "/" + name), cleanDirs));
                    }
                }
            }
            catch (Exception ex) {
                Logger.log(Logger.TYPE.ERROR, "Problem while compiling updates "
                    + ex.Message + ex.StackTrace);
            }

            return update;
        }

        public static List<Update> orderUpdates(List<Update> updates) {
            updates.Sort(delegate(Update u1, Update u2) {
                return u1.getVersion().CompareTo(u2.getVersion());
            });
            return updates;
        }

        public void setCallback(RequestAsyncCallback callback) {
            this.callback = callback;
        }

        public DateTime getDateTime() {
            return dateTime;
        }

        public void setResponseTime(DateTime time) {
            this.responseTime = time - dateTime;
        }

        public TimeSpan getResponseTime() {
            return responseTime;
        }

        public void kill() {
            dead = true;
            serverXML = null;
            callback = null;
            url = null;
            director = null;
            directorUrl = null;
            dlHandler = null;
            lastVersion = 0;
        }

        public bool isDead() {
            return dead;
        }
    }
}
