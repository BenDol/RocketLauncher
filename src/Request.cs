/*
 * Copyright (c) 2010-2014 Updater <https://github.com/BenDol/Basic-Updater>
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
        private RequestAsyncCallback callback;

        private Uri url;
        private DateTime dateTime;
        private DownloadHandler dlHandler;
        private Double lastVersion;
        private TimeSpan responseTime = new TimeSpan();

        Dictionary<UpdateNode, String> results = new Dictionary<UpdateNode, String>();

        List<UpdateNode> updateNodes = new List<UpdateNode>();
        List<Update> updates = new List<Update>();

        private bool dead = false;

        public Request(Uri url, ref DownloadHandler dlHandler, Double lastVersion) {
            this.url = url;
            this.dlHandler = dlHandler;
            this.lastVersion = lastVersion;
        }

        public void send() {
            send(null);
        }

        public void send(XDocument serverXMLCached) {
            if (isDead()) {
                return;
            }

            dateTime = DateTime.Now;

            if (serverXMLCached == null) {
                dlHandler.downloadStringAsync(url, (object s1,
                        DownloadStringCompletedEventArgs e1) => {
                    try {
                        serverXML = XDocument.Parse(e1.Result);
                        requestUpdates();
                    }
                    catch (Exception ex) {
                        Logger.log(Logger.TYPE.ERROR, "Error while requesting server XML: "
                            + ex.Message + ex.StackTrace + " URL: " + url.OriginalString);

                        kill();

                        if (callback != null) {
                            callback.onFailure();
                        }
                    }
                });
            }
            else {
                serverXML = serverXMLCached;
                requestUpdates();
            }
        }

        private void requestUpdates() {
            updateNodes = getUpdateNodes();

            foreach (UpdateNode node in updateNodes) {
                dlHandler.enqueueString(new Uri(node.getUrl()), node.getFileName(),
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
                var root = from item in serverXML.Descendants("server")
                    select new {
                        updates = item.Descendants("update")
                    };

                foreach (var data in root) {
                    foreach (var update in data.updates) {
                        String versionStr = update.Attribute("version").Value.Trim();
                        try {
                            double version = Convert.ToDouble(versionStr);
                            if (version > lastVersion) {
                                updateNodes.Add(new UpdateNode(this, update.Value.Trim(), version));
                                Logger.log(Logger.TYPE.DEBUG, "Found a new update: " + versionStr);
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
                var root = from item in xml.Descendants("update")
                    select new {
                        changelog = item.Descendants("changelog"),
                        files = item.Descendants("file"),
                        archives = item.Descendants("archive"),
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
                        foreach (var log in clog.Descendants("log")) {
                            changelog.addLog(new Changelog.Log(log.Value.Trim()));
                        }
                        update.setChangelog(changelog);
                        break;
                    }

                    // Add the file data
                    foreach (var f in data.files) {
                        String name = f.Attribute("name").Value.Trim();
                        String destination = f.Attribute("destination").Value.Trim();
                        String mime = f.Attribute("mime").Value.Trim();

                        update.addFile(new GhostFile(name, destination, mime,
                            new Uri(f.Value.Trim())));
                    }

                    // Add the archive data
                    foreach (var f in data.archives) {
                        String name = f.Attribute("name").Value.Trim();
                        String extractTo = f.Attribute("extractTo").Value.Trim();
                        String mime = f.Attribute("mime").Value.Trim();

                        Boolean cleanDirs = false;
                        if (f.Attribute("cleanDirs") != null) {
                            cleanDirs = Convert.ToBoolean(f.Attribute("cleanDirs").Value.Trim());
                        }

                        update.addFile(new Archive(name, extractTo, mime, 
                            new Uri(f.Value.Trim()), cleanDirs));
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
            dlHandler = null;
            lastVersion = 0;
        }

        public bool isDead() {
            return dead;
        }
    }
}
