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
        
        internal static List<Uri> uriFiles = new List<Uri>();
        internal static Uri uriChangelog = null;
        private static long fileSize = 0, fileBytesDownloaded = 0;
        private static Stopwatch fileDownloadElapsed = new Stopwatch();
        bool updateComplete = false, fileComplete = false;
        string currentFile = string.Empty, labelText = string.Empty;
        int progbarValue = 0, KBps = 0;

        public Ui() {
            InitializeComponent();
            if (uriFiles.Count == 0) {
                Environment.Exit(0);
            }
            else {
                if (uriChangelog != null) {
                    lblStatus.Text = "Status: Downloading changelog...";
                    try {
                        string log = webClient.DownloadString(uriChangelog);
                        log = log.Replace("\n", Environment.NewLine);
                        txtboxChangelog.Text = log;
                    }
                    catch (WebException ex) { }
                }

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
                Thread t = new Thread(new ThreadStart(DownloadFiles)); // Making a new thread because I prefer downloading 1 file at a time
                t.Start();
            }
        }

        private void DownloadFiles() {

            foreach (Uri uri in uriFiles) {
                try {
                    fileComplete = false;
                    fileDownloadElapsed.Reset();
                    fileDownloadElapsed.Start();
                    string uriPath = uri.OriginalString;
                    currentFile = uriPath.Substring(uriPath.LastIndexOf('/') + 1);
                    webClient.DownloadFileAsync(uri, currentFile);
                    webClient.DownloadFileCompleted += new AsyncCompletedEventHandler(DownloadFileCompleted);
                    webClient.DownloadProgressChanged += new DownloadProgressChangedEventHandler(DownloadProgressChanged);
                    while (!fileComplete) { Thread.Sleep(1000); }
                }
                catch { continue; }
            }
            currentFile = string.Empty;
            updateComplete = true;
        }

        void DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e) {
            progbarValue = e.ProgressPercentage;
            fileSize = e.TotalBytesToReceive / 1024;
            fileBytesDownloaded = e.BytesReceived / 1024;
            if (fileBytesDownloaded > 0 && fileDownloadElapsed.ElapsedMilliseconds > 1000) {
                KBps = (int)(fileBytesDownloaded / (fileDownloadElapsed.ElapsedMilliseconds / 1000));
            }
            labelText = "Status: Downloading " + currentFile +
                        "\n" + fileBytesDownloaded + " KB / " + fileSize + " KB - " + KBps + " KB/s";
        }

        void DownloadFileCompleted(object sender, AsyncCompletedEventArgs e) {
            progbarValue = 0;
            fileComplete = true;
        }

        /// <summary>
        /// Returns file size (Kb) of a Uri
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        private long GetFileSize(Uri uri) {
            try {
                WebRequest webRequest = HttpWebRequest.Create(uri);
                using (WebResponse response = webRequest.GetResponse()) {
                    long size = response.ContentLength;
                    return size / 1024;
                }
            }
            catch { return 1; }
        }

        private void timerMultiPurpose_Tick(object sender, EventArgs e) {
            if (updateComplete) {
                updateComplete = false;
                lblStatus.Text = "Status: Complete";
                progbarProgress.Value = 0;
                MessageBox.Show("Update complete!");
                Environment.Exit(0);
            }
            else {
                progbarProgress.Value = progbarValue;
                lblStatus.Text = labelText;
            }
        }

        private void UI_FormClosing(object sender, FormClosingEventArgs e) {
            Environment.Exit(0);
        }

        public ProgressBar getProgressBar() {
            return progbarProgress;
        }
    }
}
