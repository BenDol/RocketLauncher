using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net.NetworkInformation;
using System.Net;
using System.IO;
using System.Threading;
using System.Diagnostics;

namespace Updater {

    public partial class Ui : Form {
        public Ui() {
            InitializeComponent();

            /*if (uriFiles.Count == 0) {
                Environment.Exit(0);
            }
            else {
                foreach (Uri uri in uriFiles) {
                    string uriPath = uri.OriginalString;
                    currentFile = uriPath.Substring(uriPath.LastIndexOf('/') + 1);
                    if (File.Exists(currentFile)) {
                        lblStatus.Text = "Status: Deleting " + currentFile;
                        File.Delete(currentFile);
                    }
                }
                currentFile = string.Empty;
                lblStatus.Text = "Preparing to download file(s)...";
                Thread t = new Thread(new ThreadStart(downloadFiles)); // Making a new thread because I prefer downloading 1 file at a time
                t.Start();
            }*/
        }

        public TextBox getChangelogBox() {
            return txtboxChangelog;
        }

        public ProgressBar getDownloadProgressBar() {
            return progbarProgress;
        }

        public Label getStatusLabel() {
            return lblStatus;
        }
    }
}
