using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Updater {

    class Archive : GhostFile {
        public Archive(String name, String extractTo, String mime, Uri path) {
            this.name = name;
            this.destination = extractTo;
            this.mimeType = mime;
            this.path = path; 
        }

        public String getExtractTo() {
            return getDestination();
        }

        public void setExtractTo(String extractTo) {
            setDestination(extractTo);
        }
    }
}
