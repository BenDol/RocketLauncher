using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Updater {
    static class Program {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args) {
            System.Threading.Thread.Sleep(1000); // Give the calling application time to exit
            // These are EXAMPLES
            args = new string[2];
            args[0] = "Changelog=http://swfupload.googlecode.com/svn/swfupload/tags/swfupload_v2.1.0_core/FlashDevelop/Core%20Changelog.txt";
            args[1] = "URIs=http://www.crazythemes.com/images/games-wallpapers-1920x1080.jpg;http://download.thinkbroadband.com/10MB.zip";
            if (args.Length == 0) {
                MessageBox.Show("Can not run program without parameters", "Error");
                return;
            }

            try {
                foreach (string arg in args) {
                    if (arg.StartsWith("URIs=")) {
                        string[] uris = arg.Substring(arg.IndexOf('=') + 1).Split(';');
                        foreach (string uri in uris) {
                            if (uri.Length > 0) {
                                UI.uriFiles.Add(new Uri(uri));
                            }
                        }
                    }
                    else if (arg.StartsWith("Changelog=")) {
                        UI.uriChangelog = new Uri(arg.Substring(arg.IndexOf('=') + 1));
                    }
                }
            }
            catch { MessageBox.Show("Missing or faulty parameter(s)", "Error"); }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new UI());
        }
    }
}
