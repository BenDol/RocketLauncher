using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Updater {
    class Xml {
        /**
         * Static load specific xml document
         **/
        public static XDocument load(String file) {
            XDocument xml = null;
            try {
                if (File.Exists(file)) {
                    xml = XDocument.Load(file);
                }
                else {
                    throw new FileNotFoundException("The file " + file + " does not exist.");
                }
            }
            catch (IOException e) {
                Logger.log(Logger.TYPE.WARN, e.Message + e.StackTrace);
            }

            return xml;
        }
    }
}
