using System;
using System.IO;
using System.IO.Compression;

namespace Launcher {
    class ArchiveHandler {

        public static void extractZip(String filePath, String toDir, bool cleanDirs) {
            Logger.log(Logger.TYPE.DEBUG, "Extracting zip archive " + filePath + " to " + toDir + " cleanDirs: " + cleanDirs);

            if (!File.Exists(filePath)) {
                Logger.log(Logger.TYPE.ERROR, "Cannot extract " + filePath + " as is does not exist.");
                return;
            }
            if (!Directory.Exists(toDir)) {
                Directory.CreateDirectory(toDir);
            }

            using (var fileStream = new FileStream(filePath, FileMode.Open))
            using (var archive = new ZipArchive(fileStream)) {
                foreach (ZipArchiveEntry file in archive.Entries) {
                    string completeFileName = Path.Combine(toDir, file.FullName);
                    string directory = Path.GetDirectoryName(completeFileName);

                    if (Directory.Exists(directory)) {
                        if (cleanDirs) {
                            Directory.Delete(directory, true);
                            Directory.CreateDirectory(directory);
                        }
                    } else {
                        Directory.CreateDirectory(directory);
                    }

                    if (file.Name != "") {
                        file.ExtractToFile(completeFileName, true);
                    }
                }
            }
        }
    }
}
