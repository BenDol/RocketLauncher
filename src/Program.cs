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
using System.Collections.Generic;
using System.Reflection;
using System.Windows.Forms;
using Launcher.Interface;

namespace Launcher {
    static class Program {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args) {
            Application.SetCompatibleTextRenderingDefault(false);

            // Load embedded resources
            setupAssemblyResolve();

            Connecting con = new Connecting();
            Ui ui = new Ui(con);

            Client client = new Client(ui);
            ui.setClient(ref client);

            // Initialize the client then run the application
            client.initialize(() => {
                DownloadHandler dlHandler = client.getDownloadHandler();
                if (dlHandler != null) {
                    dlHandler.setProgressBar(ui.getDownloadProgressBar());
                }

                client.update(true);

                // Display main window
                ui.Show();
            });

            Application.Run(con);
        }

        static Dictionary<string, Assembly> libs = new Dictionary<string, Assembly>();

        static void setupAssemblyResolve() {
            AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(
                    (object sender, ResolveEventArgs args) => {
                
                string keyName = new AssemblyName(args.Name).Name;

                if (libs.ContainsKey(keyName)) {
                    return libs[keyName];
                }

                using (var stream = Assembly.GetExecutingAssembly()
                    .GetManifestResourceStream("Launcher.dlls." + keyName + ".dll")) {

                    Assembly assembly = null;
                    if (stream != null) {
                        byte[] buffer = new byte[stream.Length];
                        stream.Read(buffer, 0, buffer.Length);
                        assembly = Assembly.Load(buffer);
                        libs[keyName] = assembly;
                    }
                    return assembly;
                }
            });
        }
    }
}
