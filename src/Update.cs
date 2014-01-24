using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Updater {
    class Update : UpdateNode {

        Changelog changelog;
        DirectoryInfo tempDir;
        Boolean success;
        List<GhostFile> files = new List<GhostFile>();

        public Update(UpdateNode node) {
            this.request = node.getRequest();
            this.url = node.getUrl();
            this.version = node.getVersion();
            this.success = false;

            this.changelog = new Changelog(version);
        }

        public Changelog getChangelog() {
            return changelog;
        }

        public void setChangelog(Changelog changelog) {
            this.changelog = changelog;
        }

        public List<GhostFile> getFiles() {
            return files;
        }

        public void setFiles(List<GhostFile> files) {
            this.files = files;
        }

        public void addFile(GhostFile file) {
            files.Add(file);
        }

        public DirectoryInfo getTempDir() {
            return tempDir;
        }

        public void setTempDir(DirectoryInfo dir) {
            tempDir = dir;
        }

        public Boolean isSuccess() {
            return success;
        }

        public void setSuccess(Boolean success) {
            this.success = success;
        }

        public XElement getXML() {
            XElement xml = new XElement("update");

            Changelog changeLog = getChangelog();
            XElement changeLogXML = null;

            if(changeLog != null && !changeLog.isEmpty()) {
                changeLogXML = new XElement("changelog");
                foreach(Changelog.Log log in changeLog.getLogs()) {
                    XElement logXML = new XElement("log");
                    logXML.SetValue(log.getText());
                    changeLogXML.Add(logXML);
                }
                xml.Add(changeLogXML);
            }

            xml.SetAttributeValue("version", getVersion());
            xml.SetAttributeValue("url", getUrl());

            return xml;
        }

        public override String ToString() {
            String debugInfo = getVersion() + "\n" + getUrl() + "\n";

            debugInfo += "\nFiles:\n";
            getFiles().ForEach(x => {
                debugInfo += "\t" + x.getName() + "\n";
                debugInfo += "\t" + x.getUrl() + "\n";
                debugInfo += "\t" + x.getMimeType() + "\n";
                debugInfo += "\t" + x.getDestination() + "\n";
                debugInfo += "\n";
            });
            
            debugInfo += "\nChangelog:\n";
            getChangelog().getLogs().ForEach(x => {
                debugInfo += "\t" + x.getText() + "\n";
            });
            return debugInfo;
        }
    }
}
