using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Updater {
    class Asset<T> {
        private T asset;
        private String file;
        private DateTime lastModified;

        public Asset(T asset, String file) {
            this.asset = asset;
            this.file = file;

            if (File.Exists(file)) {
                try {
                    FileInfo fi = new FileInfo(file);
                    lastModified = fi.LastWriteTime;
                }
                catch (IOException e) {
                    Logger.log(Logger.TYPE.DEBUG, e.Message + e.StackTrace);
                }
            }
        }

        public DateTime getLastModified() {
            return lastModified;
        }

        public T get() {
            return asset;
        }

        public void clear() {
            asset = default(T);
            file = null;
        }
    }
}
