using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Updater {
    class UpdateNode {
        private Request request;
        private String url;
        private Double version;

        public UpdateNode(Request request, String url, Double version) {
            if (request == null) {
                Logger.log(Logger.TYPE.ERROR, "Provided a null request to UpdateNode");
            }
            this.request = request;

            if (url == null) {
                Logger.log(Logger.TYPE.ERROR, "Provided a null url to UpdateNode");
            }
            this.url = url;

            if (version == null) {
                Logger.log(Logger.TYPE.ERROR, "Provided a null version to UpdateNode");
            }
            this.version = version;
        }

        public Request getRequest() {
            return request;
        }

        public void setRequest(Request request) {
            this.request = request;
        }

        public String getUrl() {
            return url;
        }

        public void setUrl(String url) {
            this.url = url;
        }

        public Double getVersion() {
            return version;
        }

        public void setVersion(Double version) {
            this.version = version;
        }
    }
}
