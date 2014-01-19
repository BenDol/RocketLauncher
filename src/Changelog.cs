using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Updater {
    class Changelog {
        List<Log> logs = new List<Log>();

        public Changelog() {
        }

        public Changelog(List<Log> logs) {
            this.logs = logs;
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
    }
}
