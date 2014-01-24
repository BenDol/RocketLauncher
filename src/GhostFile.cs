using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Updater {
    class GhostFile {
        protected String name;
        protected String destination;
        protected String mimeType;
        protected Uri url;
        protected DirectoryInfo tempDir;

        public GhostFile() {
        }

        public GhostFile(String name, String dest, String mime, Uri url) {
            this.name = name;
            this.destination = dest;
            this.mimeType = mime;
            this.url = url;
        }

        public String getName() {
            return name;
        }
        public void setName(String name) {
            this.name = name;
        }

        public String getDestination() {
            return destination;
        }
        public void setDestination(String destination) {
            this.destination = destination;
        }

        public String getMimeType() {
            return mimeType;
        }
        public void setMimeType(String mimeType) {
            this.mimeType = mimeType;
        }

        public Uri getUrl() {
            return url;
        }
        public void setUrl(Uri url) {
            this.url = url;
        }

        public DirectoryInfo getTempDir() {
            return tempDir;
        }
        public void setTempDir(DirectoryInfo tempDir) {
            this.tempDir = tempDir;
        }
    }
}
