using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Updater {
    class Logger {
        private static String defaultFile = Path.Combine(Application.StartupPath, 
            System.AppDomain.CurrentDomain.FriendlyName + ".log");

        public enum TYPE {
            OFF,
            FATAL,
            ERROR,
            WARN,
            INFO,
            DEBUG
        }

        public static void log(TYPE type, String msg) {
            String stamp = createStamp(type);
            if (stamp != null) {
                System.IO.File.AppendAllText(defaultFile, stamp + msg + Environment.NewLine);
            }
        }

        public static void log(TYPE type, String msg, String file) {
            String stamp = createStamp(type);
            if (stamp != null) {
                System.IO.File.AppendAllText(file, stamp + msg + Environment.NewLine);
            }
        }

        public static String createStamp(TYPE type) {
            String descriptor = DateTime.Now.ToString("dd-MM-yyyy HH.mm.ss");
            switch (type) {
                case TYPE.OFF:
                    descriptor = null;
                    break;

                case TYPE.DEBUG:
                    descriptor += " [DEBUG]: ";
                    break;

                case TYPE.ERROR:
                    descriptor += " [ERROR]: ";
                    break;

                case TYPE.FATAL:
                    descriptor += " [FATAL]: ";
                    break;

                case TYPE.INFO:
                    descriptor += " [INFO]: ";
                    break;

                case TYPE.WARN:
                    descriptor += " [WARN]: ";
                    break;
            }
            return descriptor;
        }
    }
}
