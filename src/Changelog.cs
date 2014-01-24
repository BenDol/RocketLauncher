using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Updater {
    class Changelog {

        // Log data object
        public class Log {
            String text;

            public Log(String text) {
                this.text = text;
            }

            public String getText() {
                return text;
            }

            public void setText(String text) {
                this.text = text;
            }
        }

        Double version;
        List<Log> logs;

        public Changelog(Double version) 
            : this(version, new List<Log>()) {
        }

        public Changelog(Double version, List<Log> logs) {
            this.version = version;
            this.logs = logs;
        }

        public void setVersion(double version) {
            this.version = version;
        }

        public double getVersion() {
            return version;
        }

        public List<Log> getLogs() {
            return logs;
        }

        public void setLogs(List<Log> logs) {
            this.logs = logs;
        }

        public void addLog(Log log) {
            logs.Add(log);
        }

        public bool isEmpty() {
            return logs.Count < 1;
        }

        public override String ToString() {
            String changelog = "Version: " + getVersion() + Environment.NewLine;
            foreach (Log log in getLogs()) {
                changelog += "* " + log.getText() + Environment.NewLine;
            }
            return changelog;
        }
    }
}
