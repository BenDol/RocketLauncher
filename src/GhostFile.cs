using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Updater {
    class GhostFile {
        protected String name;
        protected String destination;
        protected String mimeType;
        protected Uri path;

        public GhostFile() {
        }

        public GhostFile(String name, String dest, String mime, Uri path) {
            this.name = name;
            this.destination = dest;
            this.mimeType = mime;
            this.path = path; 
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

        public Uri getPath() {
            return path;
        }
        public void setPath(Uri path) {
            this.path = path;
        }
    }
}
