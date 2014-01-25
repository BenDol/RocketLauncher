/*
 * Copyright (c) 2010-2014 Updater <https://github.com/BenDol/Basic-Updater>
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
 */
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
        private String mime;

        public Asset(T asset, String file) : this(asset, file, "") {
        }

        public Asset(T asset, String file, String mime) {
            this.asset = asset;
            this.file = file;
            this.mime = mime;

            if (File.Exists(file)) {
                try {
                    FileInfo fi = new FileInfo(file);
                    lastModified = fi.LastWriteTime;
                }
                catch (IOException e) {
                    Logger.log(Logger.TYPE.WARN, e.Message + e.StackTrace);
                }
            }
        }

        public DateTime getLastModified() {
            return lastModified;
        }

        public String getMimeType() {
            return mime;
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
