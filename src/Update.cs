using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Updater {
    class Update : UpdateNode {

        Changelog changelog;
        DirectoryInfo tempDir;
        List<GhostFile> files = new List<GhostFile>();

        public Update(UpdateNode node) {
            this.request = node.getRequest();
            this.url = node.getUrl();
            this.version = node.getVersion();

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

        public override String ToString() {
            String debugInfo = getVersion() + "\n" + getUrl() + "\n";

            debugInfo += "\nFiles:\n";
            getFiles().ForEach(x => {
                debugInfo += "\t" + x.getName() + "\n";
                debugInfo += "\t" + x.getPath() + "\n";
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
