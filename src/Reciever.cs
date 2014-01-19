using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml.Linq;

namespace Updater {
    class Reciever {
        private Uri url;
        private String latestVersion;

        private XDocument updateXML;

        private List<UpdateNode> oldUpdates = new List<UpdateNode>();
        private List<UpdateNode> newUpdates = new List<UpdateNode>();

        public Reciever() {
            loadUpdateXML();
        }

        /**
         * Loads the update XML updating the recievers URL, latest version & update data.
         * Note that this will reparse the update lists putting all the updates marked as 'old'.
         **/
        public void loadUpdateXML() {
            updateXML = XDocument.Load(Path.Combine(Application.StartupPath, @"update.xml"));

            oldUpdates.Clear();
            newUpdates.Clear();

            var root = from item in updateXML.Descendants("updates")
                select new {
                    // Attributes
                    url = item.Attribute("url"),
                    latest = item.Attribute("latest"),

                    // Children
                    updates = item.Descendants("update")
                };

            // Ensure there is only one <updates> tag set
            if (root.Count() < 2) {
                foreach (var data in root) {

                    // Populate current updates
                    foreach (var update in data.updates) {
                        oldUpdates.Add(new UpdateNode(this, update.Value, update.Attribute("version").Value));
                    }

                    url = new Uri(data.url.Value);
                    latestVersion = data.latest.Value;

                    if (url == null) {
                        Logger.log(Logger.TYPE.FATAL, "No 'url' attribute found in the <updates> tag: " 
                            + data.ToString());
                    }
                    break;
                }
            }
            else {
                Logger.log(Logger.TYPE.FATAL, "No <updates> parent tag found in the xml file");
            }
        }

        public Uri getUrl() {
            return url;
        }

        public String getLatestVersion() {
            return latestVersion;
        }

        public XDocument getUpdateXML() {
            return updateXML;
        }

        public List<UpdateNode> getNewUpdates() {
            return newUpdates;
        }

        public List<UpdateNode> getOldUpdates() {
            return oldUpdates;
        }
    }
}
