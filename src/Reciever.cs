﻿/*
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
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;

namespace Updater {

    class Reciever {
        private String file;
        private DownloadHandler dlHandler;

        private Asset<XDocument> updateXML;
        private ExternalAsset<XDocument> serverXMLCache;

        public Reciever(ref DownloadHandler dlHandler) {
            file = Path.Combine(Application.StartupPath, Client.UPDATE_XML_NAME);
            this.dlHandler = dlHandler;

            load();
        }

        /**
         * Loads the update XML document as an XAsset.
         **/
        private void load() {
            try {
                if (File.Exists(file)) {
                    updateXML = new Asset<XDocument>(XDocument.Load(file), file);
                    validateXML(updateXML.get());
                }
                else {
                    throw new FileNotFoundException("The file " + file + " does not exist.");
                }
            }
            catch (Exception e) {
                MessageBox.Show(e.Message);
            }
        }

        /**
         * Reloads the update XML document.
         **/
        public void reload() {
            reset();
            load();
        }

        /**
         * Clear the upate XML document.
         **/
        protected void reset() {
            updateXML.clear();
        }

        /**
         * Ensure the update XML is valid.
         **/
        protected static bool validateXML(XDocument updateXML) {
            var root = from item in updateXML.Descendants("updates")
                select new {
                    // Attributes
                    url = item.Attribute("url"),
                    latest = item.Attribute("latest")
                };

            // Ensure there is only one <updates> tag set
            if (root.Count() < 2) {
                foreach (var data in root) {
                    Uri url = new Uri(data.url.Value.Trim());

                    if (url == null) {
                        Logger.log(Logger.TYPE.FATAL, "No 'url' attribute found in the <updates> tag: "
                            + data.ToString());
                        break;
                    }
                    else {
                        return true;
                    }
                }
            }
            else {
                Logger.log(Logger.TYPE.FATAL, "No <updates> parent tag found in the xml file");
            }

            return false;
        }

        public bool reloadIfModified() {
            if (File.Exists(file)) {
                try {
                    FileInfo fi = new FileInfo(file);
                    if (fi.LastWriteTime > updateXML.getLastModified()) {
                        reload();
                        return true;
                    }
                }
                catch (IOException e) {
                    Logger.log(Logger.TYPE.WARN, e.Message + e.StackTrace);
                }
            }
            return false;
        }

        public Request sendRequest(RequestAsyncCallback callback) {
            reloadIfModified(); //Ensure the update XML is up to date

            Request request = null;
            try {
                request = new Request(getUrl(), ref dlHandler, getLatestVersion());
                request.setCallback(callback);
                request.send(getServerXMLCache());
            }
            catch (Exception e) {
                Logger.log(Logger.TYPE.FATAL, "Error creating request: " 
                    + e.Message + e.StackTrace);
            }

            return request;
        }

        public void getServerName(StringAsyncCallback callback) {
            Uri url = getUrl();

            dlHandler.downloadStringAsync(url, 
                (object s, DownloadStringCompletedEventArgs e) => {
                    try {
                        serverXMLCache = new ExternalAsset<XDocument>(
                            XDocument.Parse(e.Result), url.OriginalString);

                        var root = from item in serverXMLCache.get().Descendants("server")
                            select new {
                                name = item.Attribute("name")
                            };

                        String serverName = "";
                        foreach (var data in root) {
                            serverName = data.name.Value;
                            break;
                        }

                        callback.onSuccess(serverName);
                    }
                    catch(Exception ex) {
                        Logger.log(Logger.TYPE.ERROR, "Was unable to parse server XML: "
                            + ex.Message + ex.StackTrace + " URL: " + url.OriginalString);

                        callback.onFailure();
                    }
            });
        }

        /**
         * Returns the server XML cache if it hasn't been longer than 
         * 3 minutes since the last cache, otherwise it will return null.
         **/
        public XDocument getServerXMLCache() {
            XDocument XMLCache = serverXMLCache.get();

            TimeSpan span = DateTime.Now - serverXMLCache.getRetrievedOn();
            if (span.Minutes > 3) {
                XMLCache = null;
            }

            return XMLCache;
        }

        /**
         * Retrieves the 'url' from the XML document.
         * Note that if the modified date doesn't match the asset it will retrieve
         * the document contents again. Returns null if failed to retrieve from the document.
         * */
        public Uri getUrl() {
            reloadIfModified();

            var root = from item in updateXML.get().Descendants("updates")
                select new {
                    url = item.Attribute("url")
                };

            String url = null;
            foreach (var data in root) {
                url = data.url.Value;
            }
            return new Uri(url.Trim());
        }

        /**
         * Retrieves the 'latest'(version) from the XML document.
         * Note that if the modified date doesn't match the asset it will retrieve 
         * the document contents again. Returns null if failed to retrieve from the document.
         * */
        public double getLatestVersion() {
            reloadIfModified();

            var root = from item in updateXML.get().Descendants("updates")
                select new {
                    latest = item.Attribute("latest")
                };

            double latestVersion = 0;
            try {
                foreach(var data in root) {
                    latestVersion = Convert.ToDouble(data.latest.Value.Trim());
                    break;
                }
            }
            catch(FormatException e) {
                Logger.log(Logger.TYPE.ERROR, "Could not convert latest version: " + e.Message);
            }
            return latestVersion;
        }

        public XDocument getUpdateXML() {
            reloadIfModified();

            return updateXML.get();
        }

        public String getLogging() {
            reloadIfModified();

            var root = from item in updateXML.get().Descendants("updates")
                select new {
                    logging = item.Attribute("logging")
                };

            String logging = "";
            foreach (var data in root) {
                logging = data.logging.Value;
            }
            return logging;
        }

        public XElement getUpdateElement(Update update, XDocument doc) {
            var root = from item in doc.Descendants("updates")
                select new {
                    updates = item.Descendants("update")
                };

            XElement element = null;
            try {
                foreach (var data in root) {
                    foreach (var u in data.updates) {
                        if(update.getVersion() == Convert.ToDouble(
                                u.Attribute("version").Value)) {
                            element = u;
                            break;
                        }
                    }
                }
            }
            catch(FormatException e) {
                Logger.log(Logger.TYPE.ERROR, "Could not convert latest version: " + e.Message);
            }

            return element;
        }

        public void stampUpdate(Update update) {
            // Use the current updateXML object, do not repull
            XDocument doc = updateXML.get();

            XElement updateElement = getUpdateElement(update, doc);
            if (updateElement != null) {
                updateElement.Remove();
            }

            StringBuilder sb = new StringBuilder();
            XmlWriterSettings xws = new XmlWriterSettings();
            xws.OmitXmlDeclaration = true;
            xws.Indent = true;

            using (XmlWriter xw = XmlWriter.Create(sb, xws)) {
                try {
                    doc.Element("updates").AddFirst(update.getXML());
                    doc.Element("updates").SetAttributeValue("latest", 
                        update.getVersion());

                    doc.WriteTo(xw);
                    doc.Save(file);
                }
                catch (Exception e) {
                    Logger.log(Logger.TYPE.FATAL, "Could not stamp the updates: " 
                        + e.Message + e.StackTrace);
                }
            }
        }
    }
}
