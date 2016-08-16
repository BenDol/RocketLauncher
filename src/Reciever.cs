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
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;

namespace Launcher {

    class Reciever {
        private String file, url, director;
        private DownloadHandler dlHandler;
        private Client client;

        private Asset<XDocument> updateXML;
        private ExternalAsset<XDocument> serverXMLCache;

        public Reciever(ref DownloadHandler dlHandler, Client client) {
            file = Path.Combine(Application.StartupPath, Client.UPDATE_XML_NAME);
            this.dlHandler = dlHandler;
            this.client = client;

            load();
        }

        /*
         * Loads the update XML document as an XAsset.
         */
        private void load() {
            try {
                if (File.Exists(file)) {
                    updateXML = new Asset<XDocument>(XDocument.Load(file), file);
                    validateXML(updateXML.get());

                    var root = from item in updateXML.get().Descendants("Updates")
                        select new {
                            // Attributes
                            url = item.Attribute("url"),
                            director = item.Attribute("director")
                        };

                    // Ensure there is only one <Updates> tag set
                    if (root.Count() < 2) {
                        foreach (var data in root) {
                            url = data.url.Value;
                            director = data.director.Value;
                        }
                    }
                }  else {
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
            url = null;
            director = null;
        }

        /**
         * Ensure the update XML is valid.
         **/
        protected static bool validateXML(XDocument updateXML) {
            var root = from item in updateXML.Descendants("Updates")
                select new {
                    // Attributes
                    url = item.Attribute("url"),
                    director = item.Attribute("director")
                };

            // Ensure there is only one <updates> tag set
            if (root.Count() < 2) {
                foreach (var data in root) {
                    Uri url = new Uri(data.url.Value.Trim());

                    if (url == null) {
                        Logger.log(Logger.TYPE.FATAL, "No 'url' attribute found in the <Updates> tag: "
                            + data.ToString());
                        break;
                    } else if(data.director == null || data.director.Value == null) {
                        Logger.log(Logger.TYPE.FATAL, "No 'director' attribute found in the <Updates> tag: "
                            + data.ToString());
                        break;
                    } else {
                        return true;
                    }
                }
            } else {
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
            Logger.log(Logger.TYPE.DEBUG, "Creating update request...");
            reloadIfModified(); //Ensure the update XML is up to date

            Request request = null;
            try {
                request = new Request(getUrl(), getDirector(), ref dlHandler, getLatestVersion());
                request.setCallback(callback);
                request.send(getServerXMLCache());
            }
            catch (Exception e) {
                Logger.log(Logger.TYPE.FATAL, "Error creating request: " 
                    + e.Message + e.StackTrace);
            }

            return request;
        }

        public void getInitialData(InitAsyncCallback callback) {
            Logger.log(Logger.TYPE.DEBUG, "Getting initial data...");
            Uri directorUrl = getDirectorUrl();

            dlHandler.downloadStringAsync(directorUrl, 
                (object s, DownloadStringCompletedEventArgs e) => {
                Logger.log(Logger.TYPE.DEBUG, "Successfully retreived initial data.");
                try {
                    serverXMLCache = new ExternalAsset<XDocument>(
                        XDocument.Parse(e.Result), directorUrl.OriginalString);

                    var root = from item in serverXMLCache.get().Descendants("Server")
                        select new {
                            name = item.Attribute("name"),
                            target = item.Attribute("target"),
                            launchText = item.Attribute("launchText")
                        };

                    String serverName = "", targetPath = "", launchText = "";
                    foreach (var data in root) {
                        serverName = data.name.Value;
                        targetPath = data.target.Value;
                        launchText = data.launchText.Value;
                        break;
                    }

                    callback.onSuccess(serverName, targetPath, launchText);
                }
                catch(Exception ex) {
                    Logger.log(Logger.TYPE.ERROR, "Was unable to parse server XML: "
                        + ex.Message + ex.StackTrace + " URL: " + directorUrl.OriginalString);

                    callback.onFailure();
                }
            });
        }

        public void getLayout(LayoutAsyncCallback callback) {
            Logger.log(Logger.TYPE.DEBUG, "Getting layout...");
            String layout = null;

            List<Component> components = new List<Component>();

            XAttribute layoutAttr = serverXMLCache.get().Element("Server").Attribute("layout");
            if(layoutAttr != null) {
                layout = layoutAttr.Value;
            }

            if (layout != null) {
                // Download layout xml
                dlHandler.downloadStringAsync(new Uri(getUrl() + "/" + layout), 
                    (object s, DownloadStringCompletedEventArgs e) => {
                        XDocument doc = XDocument.Parse(e.Result);
                        var root = from item in doc.Elements("Layout")
                            select new {
                                components = item.Elements()
                            };
                        
                        foreach (var data in root) {
                            foreach (var f in data.components) {
                                String name = Xml.getAttributeValue(f.Attribute("name"));
                                components.Add(new Component(name, f.Name.LocalName, f));
                            }
                        }

                        callback.onSuccess(components);
                    });
            } else {
                callback.onSuccess(components);
            }
        }

        public void getFonts(FontAsyncCallback callback) {
            Logger.log(Logger.TYPE.DEBUG, "Getting fonts...");
            
            var root = from item in serverXMLCache.get().Descendants("Server")
                select new {
                    fonts = item.Descendants("Font")
                };

            List<FontPackage> packages = new List<FontPackage>();
            foreach (var data in root) {
                foreach (var f in data.fonts) {
                    packages.Add(new FontPackage(f));
                }
            }
                        
            List<String> installedFonts = new List<String>();

            String fontCachePath = client.getFontCacheDir();
            String tmpFontPath = Path.Combine(fontCachePath, "tmpfonts");
            if (!Directory.Exists(fontCachePath)) {
                Directory.CreateDirectory(fontCachePath);
                Directory.CreateDirectory(tmpFontPath);
            }
            else {
                if (!Directory.Exists(tmpFontPath)) {
                    Directory.CreateDirectory(tmpFontPath);
                }
                // Preload already existing fonts
                installedFonts = installPrivateFonts(fontCachePath);
            }

            if (packages.Count > 0) {
                // Download font packages
                foreach (FontPackage package in packages) {
                    if (!FontHandler.doesFontExist(package.getName(), true, true)) {
                        if (package.canDownload()) {
                            Uri packageUrl = new Uri(getUrl() + "/" + package.getPackage());
                            dlHandler.enqueueFile(packageUrl, tmpFontPath,
                                package.getArchiveName(), (Boolean cancelled) => {
                                if (!cancelled) {
                                    ArchiveHandler.extractZip(Path.Combine(tmpFontPath,
                                        package.getArchiveName()), tmpFontPath, false);
                                }
                            });
                        }
                    }
                    else {
                        package.setInstalled(true);
                    }
                }

                dlHandler.setQueueFileCallback(new QueueCallback(() => {
                    installedFonts.AddRange(installPrivateFonts(tmpFontPath));

                    // Finished installing new fonts, now cache them
                    cacheFonts(tmpFontPath);

                    // Apply new fonts to the packages
                    foreach (FontPackage package in packages) {
                        if (FontHandler.doesPrivateFontExist(package.getName(), true)) {
                            package.setInstalled(true); // Ensure package is now installed

                            foreach (KeyValuePair<String, FontApply> pair in package.getApplyMap()) {
                                FontApply fontApply = pair.Value;
                                fontApply.usePrivateFont(package.getName());
                            }
                        }
                    }

                    callback.onSuccess(packages);
                }));

                dlHandler.startFileQueue();
            }
            else {
                callback.onSuccess(packages);
            }
        }

        public List<String> installPrivateFonts(String fontsDir) {
            // Install fonts from specified directory
            List<String> fontNames = new List<String>();
            if (Directory.Exists(fontsDir)) {
                foreach (String fileName in Directory.GetFiles(fontsDir)) {
                    if (fileName.ToLower().EndsWith(".ttf")) {
                        String name = FontHandler.addPrivateFont(Path.Combine(fontsDir, fileName));
                        fontNames.Add(name);
                    }
                }
            }
            return fontNames;
        }

        public void cacheFonts(String fontsDir) {
            if (!Directory.Exists(fontsDir)) {
                Logger.log(Logger.TYPE.WARN, "cacheFonts: Directory does not exist: " 
                    + fontsDir);
                return;
            }

            try {
                String fontCachePath = client.getFontCacheDir();
                if (!Directory.Exists(fontCachePath)) {
                    Directory.CreateDirectory(fontCachePath);
                }

                foreach (String fileName in Directory.GetFiles(fontsDir)) {
                    if (File.Exists(fileName)) {
                        String newPath = Path.Combine(fontCachePath, Path.GetFileName(fileName));
                        if(File.Exists(newPath)) {
                            File.Delete(newPath);
                        }
                        File.Move(fileName, newPath);
                    }
                }
            }
            catch (Exception e) {
                Logger.log(Logger.TYPE.ERROR, "Unable to cache fonts: " + e.Message);
            }
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

            var root = from item in updateXML.get().Descendants("Updates")
                select new {
                    url = item.Attribute("url")
                };
            
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

            var root = from item in updateXML.get().Descendants("Updates")
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

        public List<Update> getPreviousUpdates() {
            XDocument xml = getUpdateXML();

            List<Update> updates = new List<Update>();
            var root = from item in updateXML.get().Descendants("Updates")
                select new {
                    updates = item.Descendants("Update")
                };

            try {
                foreach (var data in root) {
                    var udates = from item in data.updates
                        select new {
                            name = item.Attribute("name"),
                            version = item.Attribute("version"),
                            url = item.Attribute("url"),
                            baseType = item.Attribute("base"),
                            changelogs = item.Descendants("Changelog")
                        };

                    foreach (var u in udates) {
                        Update update = new Update();
                        update.setName(u.name.Value);
                        update.setVersion(Convert.ToDouble(u.version.Value));
                        update.setUrl(u.url.Value);

                        XAttribute baseType = u.baseType;
                        if (baseType != null) {
                            update.setBaseType(baseType.Value);
                        }

                        Changelog changelog = new Changelog(update.getVersion());
                        foreach (var clog in u.changelogs) {
                            foreach(var log in clog.Descendants("Log")) {
                                changelog.addLog(new Changelog.Log(log.Value));
                            }
                            update.setChangelog(changelog);
                            break;
                        }
                        update.setChangelog(changelog);

                        updates.Add(update);
                    }
                }
            }
            catch (Exception e) {
                Logger.log(Logger.TYPE.ERROR, "Failed to get previous updates " 
                    + e.Message + e.StackTrace);
            }

            return updates;
        }

        public String getLogging() {
            reloadIfModified();

            var root = from item in updateXML.get().Descendants("Updates")
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
            var root = from item in doc.Descendants("Updates")
                select new {
                    updates = item.Descendants("Update")
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
                    doc.Element("Updates").AddFirst(update.getXML());
                    doc.Element("Updates").SetAttributeValue("latest", update.getVersion());

                    doc.WriteTo(xw);
                    doc.Save(file);
                }
                catch (Exception e) {
                    Logger.log(Logger.TYPE.FATAL, "Could not stamp the updates: " 
                        + e.Message + e.StackTrace);
                }
            }
        }

        public String getDirector() {
            reloadIfModified();
            return director;
        }

        public Uri getDirectorUrl() {
            reloadIfModified();
            return new Uri((url + "/" + director).Trim()); ;
        }
    }
}
