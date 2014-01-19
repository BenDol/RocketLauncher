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

            }
            this.reciever = reciever;
            this.url = url;
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
