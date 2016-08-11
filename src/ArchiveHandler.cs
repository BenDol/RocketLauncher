using System;
using System.IO;
using System.IO.Compression;

namespace Launcher {
    class ArchiveHandler {

        public static void extractZip(String file, String toDir, bool cleanDirs) {
            Logger.log(Logger.TYPE.DEBUG, "Extracting zip archive " + file + " to " 
                + toDir + " cleanDirs: " + cleanDirs);

            if (!File.Exists(file)) {
                Logger.log(Logger.TYPE.ERROR, "Cannot extract " + file + " as is does not exist.");
                return;
            }
            if (!Directory.Exists(toDir)) {
                Directory.CreateDirectory(toDir);
            }
            using (ZipArchive a = ZipFile.OpenRead(file)) {
                foreach (ZipArchiveEntry entry in a.Entries) {
                    if (!entry.FullName.EndsWith("/", StringComparison.OrdinalIgnoreCase)) {
                        entry.ExtractToFile(Path.Combine(toDir, entry.FullName), true);
                    }
                    else {
                        String dir = entry.FullName.Replace("/", "//");

                        bool createDir = !Directory.Exists(dir);
                        if (!createDir) {
                            if (cleanDirs) {
                                Directory.Delete(dir, true);
                                createDir = true;
                            }
                        }
                        if (createDir) {
                            Directory.CreateDirectory(dir);
                        }
                    }
                }
            }
        }

    }
}
