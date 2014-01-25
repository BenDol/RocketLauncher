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

            short[] loggingSettings = null;
            try {
                loggingSettings = ConfigurationManager.AppSettings["logging"].Split(
                    delimiters).Select(x => Convert.ToInt16(x)).ToArray();
            }
            catch(FormatException e) {
                Logger.log(Logger.TYPE.ERROR, "Format issue with the logger, make sure "
                    + "the logger settings are correct. " + e.Message + e.StackTrace);
            }

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
