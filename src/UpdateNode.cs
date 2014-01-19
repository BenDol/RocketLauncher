using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Updater {
    class UpdateNode {
        private Reciever reciever;
        private String url;
        private String version;

        public UpdateNode(Reciever reciever, String url, String version) {
            if (reciever == null) {
                Logger.log(Logger.TYPE.ERROR, "Provided a null reciever to UpdateNode");
            }
            this.reciever = reciever;

            if (url == null) {
                Logger.log(Logger.TYPE.ERROR, "Provided a null url to UpdateNode");
            }
            this.url = url;

            if (version == null) {
                Logger.log(Logger.TYPE.ERROR, "Provided a null version to UpdateNode");
            }
            this.version = version;
        }

        public Reciever getReciever() {
            return reciever;
        }

        public void setReciever(Reciever reciever) {
            this.reciever = reciever;
        }

        public String getUrl() {
            return url;
        }

        public void setUrl(String url) {
            this.url = url;
        }

        public String getVersion() {
            return version;
        }

        public void setVersion(String version) {
            this.version = version;
        }
    }
}
