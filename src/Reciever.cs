using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml.Linq;

namespace Updater {

    class Reciever {
        private String file;
        private Asset<XDocument> updateXML;
        private DownloadHandler dlHandler;

        public Reciever(DownloadHandler dlHandler) {
            file = Path.Combine(Application.StartupPath, @"update.xml");
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
                    validateXML();
                }
                else {
                    throw new FileNotFoundException("The file update.xml does not exist.");
                }
            }
            catch (IOException e) {
                Logger.log(Logger.TYPE.WARN, e.Message + e.StackTrace);
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
        protected bool validateXML() {
            var root = from item in updateXML.get().Descendants("updates")
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

        public Request sendRequest(RequestCallback callback) {
            reloadIfModified(); //Ensure the update XML is up to date

            Request request = new Request(getUrl(), dlHandler, getLatestVersion());
            request.setCallback(callback);
            request.send();

            return request;
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
    }
}
