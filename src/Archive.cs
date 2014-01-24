using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Updater {

    class Archive : GhostFile {
        public Archive(String name, String extractTo, String mime, Uri url) {
            this.name = name;
            this.destination = extractTo;
            this.mimeType = mime;
            this.url = url; 
        }

        public String getExtractTo() {
            return getDestination();
        }

        public void setExtractTo(String extractTo) {
            setDestination(extractTo);
        }
    }
}
