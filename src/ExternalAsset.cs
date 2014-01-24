using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Updater {
    class ExternalAsset<T> {
        private T asset;
        private String url;
        private DateTime retrievedOn;

        public ExternalAsset(T asset, String url)
            : this(asset, url, DateTime.Now) {
        }

        public ExternalAsset(T asset, String url, DateTime retrievedOn) {
            this.asset = asset;
            this.url = url;
            this.retrievedOn = retrievedOn;
        }

        public DateTime getRetrievedOn() {
            return retrievedOn;
        }

        public String getUrl() {
            return url;
        }

        public T get() {
            return asset;
        }

        public void clear() {
            asset = default(T);
            url = null;
        }
    }
}
