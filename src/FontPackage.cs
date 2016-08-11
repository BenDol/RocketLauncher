/*
 * Copyright (c) 2014 RocketLauncher <https://github.com/BenDol/RocketLauncher>
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
using System.Drawing;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Launcher {
    class FontPackage {
        String name;
        String package;
        String archiveName;

        Boolean installed = false;

        Dictionary<String, FontApply> applyMap = new Dictionary<String, FontApply>();

        public FontPackage(XElement elem) {
            setName(elem);
            setArchiveName(elem);
            setPackage(elem);

            try {
                foreach (var apply in elem.Descendants("Apply")) {
                    addApply(new FontApply(apply, getName()));
                }
            }
            catch(Exception e) {
                Logger.log(Logger.TYPE.ERROR, "Error parsing font apply styles: " 
                    + e.Message);
            }
        }

        public String getName() {
            return name;
        }

        protected void setName(XElement elem) {
            try {
                this.name = elem.Attribute("name").Value;
            }
            catch (Exception e) {
                Logger.log(Logger.TYPE.ERROR, "Unable to parse 'name': " + e.Message);
            }
        }

        public String getPackage() {
            return package;
        }

        protected void setPackage(XElement elem) {
            try {
                this.package = elem.Attribute("package").Value;
            }
            catch (Exception e) {
                Logger.log(Logger.TYPE.DEBUG, "Unable to parse 'package': " + e.Message);
            }
        }

        public String getArchiveName() {
            return archiveName;
        }

        protected void setArchiveName(XElement elem) {
            try {
                this.archiveName = elem.Attribute("archive").Value;
            }
            catch (Exception e) {
                Logger.log(Logger.TYPE.DEBUG, "Unable to parse 'archive': " + e.Message);
            }
        }

        public Boolean canDownload() {
            return package != null && !package.Equals("") 
                && archiveName != null && !archiveName.Equals("");
        }

        public Boolean isInstalled() {
            return installed;
        }

        public void setInstalled(Boolean installed) {
            this.installed = installed;
        }

        public Dictionary<String, FontApply> getApplyMap() {
            return applyMap;
        }

        public void addApply(FontApply fontApply) {
            applyMap.Remove(fontApply.getTo());
            applyMap.Add(fontApply.getTo(), fontApply);
        }
    }
}
