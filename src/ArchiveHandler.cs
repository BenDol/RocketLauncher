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
            } else if (cleanDirs) {
                Directory.Delete(toDir, true);
                Directory.CreateDirectory(toDir);
            }

            using (var fileStream = new FileStream(filePath, FileMode.Open))
            using (var archive = new ZipArchive(fileStream)) {
                foreach (ZipArchiveEntry file in archive.Entries) {
                    string completeFileName = Path.Combine(toDir, file.FullName);
                    string directory = Path.GetDirectoryName(completeFileName);
                    
                    if (!Directory.Exists(directory)) {
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
