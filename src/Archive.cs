using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Updater {

    class Archive : GhostFile {

        Boolean cleanDirs;

        public Archive(String name, String extractTo, String mime, Uri url,
                Boolean cleanDirs) {
            this.name = name;
            this.destination = extractTo;
            this.mimeType = mime;
            this.url = url;
            this.cleanDirs = cleanDirs;
        }

        public String getExtractTo() {
            return getDestination();
        }

        public void setExtractTo(String extractTo) {
            setDestination(extractTo);
        }

        public Boolean getCleanDirs() {
            return cleanDirs;
        }

        public void setCleanDirs(Boolean cleanDirs) {
            this.cleanDirs = cleanDirs;
        }
    }
}
