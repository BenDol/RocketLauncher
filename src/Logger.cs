using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Updater {
    class Logger {
        private static String defaultFile = Path.Combine(Application.StartupPath, 
            System.AppDomain.CurrentDomain.FriendlyName + ".log");

        public enum TYPE {
            OFF = 0,
            DEBUG = 1,
            INFO = 2,
            WARN = 3,
            ERROR = 4,
            FATAL = 5
        }

        public static void log(TYPE type, String msg) {
            if (canLog(type)) {
                String stamp = createStamp(type);
                if (stamp != null) {
                    System.IO.File.AppendAllText(defaultFile, stamp + msg + Environment.NewLine);
                }
            }
        }

        public static void log(TYPE type, String msg, String file) {
            if (canLog(type)) {
                String stamp = createStamp(type);
                if (stamp != null) {
                    System.IO.File.AppendAllText(file, stamp + msg + Environment.NewLine);
                }
            }
        }

        public static bool canLog(TYPE type) {
            char[] delimiters = { ' ', ',', '.', ':', '\t', ';' };

            short[] loggingSettings = ConfigurationManager.AppSettings["logging"].Split(
                delimiters).Select(x => Convert.ToInt16(x)).ToArray();

            return loggingSettings.Contains((short)type);
        }

        public static String createStamp(TYPE type) {
            String descriptor = DateTime.Now.ToString("dd-MM-yyyy HH.mm.ss");
            
            switch (type) {
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

                default:
                    descriptor = null;
                    break;
            }
            return descriptor;
        }
    }
}
