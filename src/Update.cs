/*
 * Copyright (c) 2010-2014 Launcher <https://github.com/BenDol/RocketLauncher>
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
using System.Xml.Linq;

namespace Launcher {
    class Update : UpdateNode {

        String name;
        String baseType;
        Changelog changelog;
        DirectoryInfo tempDir;
        Boolean success;
        List<GhostFile> files = new List<GhostFile>();

        public Update() {
        }

        public Update(UpdateNode node) {
            this.request = node.getRequest();
            this.url = node.getUrl();
            this.version = node.getVersion();
            this.success = false;

            this.changelog = new Changelog(version);
        }

        public Changelog getChangelog() {
            return changelog;
        }

        public void setChangelog(Changelog changelog) {
            this.changelog = changelog;
        }

        public List<GhostFile> getFiles() {
            return files;
        }

        public void setFiles(List<GhostFile> files) {
            this.files = files;
        }

        public void addFile(GhostFile file) {
            files.Add(file);
        }

        public DirectoryInfo getTempDir() {
            return tempDir;
        }

        public void setTempDir(DirectoryInfo dir) {
            tempDir = dir;
        }

        public Boolean isSuccess() {
            return success;
        }

        public void setSuccess(Boolean success) {
            this.success = success;
        }

        public String getName() {
            return name;
        }

        public void setName(String name) {
            this.name = name;
        }

        public String getBaseType() {
            return baseType;
        }

        public void setBaseType(String baseType) {
            this.baseType = baseType;
        }

        public XElement getXML() {
            XElement xml = new XElement("update");

            Changelog changeLog = getChangelog();
            XElement changeLogXML = null;

            if(changeLog != null && !changeLog.isEmpty()) {
                changeLogXML = new XElement("changelog");
                foreach(Changelog.Log log in changeLog.getLogs()) {
                    XElement logXML = new XElement("log");
                    logXML.SetValue(log.getText());
                    changeLogXML.Add(logXML);
                }
                xml.Add(changeLogXML);
            }

            xml.SetAttributeValue("name", getName());
            xml.SetAttributeValue("version", getVersion());
            xml.SetAttributeValue("base", getBaseType());
            xml.SetAttributeValue("url", getUrl());

            return xml;
        }

        public override String ToString() {
            String debugInfo = getName() + "\n" + getBaseType() 
                + "\n" + getVersion() + "\n" + getUrl() + "\n";

            debugInfo += "\nFiles:\n";
            getFiles().ForEach(x => {
                debugInfo += "\t" + x.getName() + "\n";
                debugInfo += "\t" + x.getUrl() + "\n";
                debugInfo += "\t" + x.getMimeType() + "\n";
                debugInfo += "\t" + x.getDestination() + "\n";
                debugInfo += "\n";
            });
            
            debugInfo += "\nChangelog:\n";
            getChangelog().getLogs().ForEach(x => {
                debugInfo += "\t" + x.getText() + "\n";
            });
            return debugInfo;
        }
    }
}
