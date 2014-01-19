using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;

namespace Updater {

    class DownloadHandler {
        private WebClient webClient;
        private Stopwatch fileDownloadElapsed = new Stopwatch();

        private long fileSize = 0;
        private long fileBytesDownloaded = 0;
        private int KBps = 0;

        private bool busy = false;
        private bool downloadComplete = false;

        private Uri currentUrl;
        private String labelText = String.Empty;

        private Ui ui;

        public DownloadHandler(WebClient webClient, Ui ui) {
            this.webClient = webClient;
            this.ui = ui;
        }

        public void downloadFileAsync(Uri url, EventHandler<AsyncCompletedEventArgs> completed) {
            if (!isBusy()) {
                prepare(url);

                webClient.DownloadFileCompleted += new AsyncCompletedEventHandler(
                    (object sender, AsyncCompletedEventArgs e) => {
                        downloadCompleted();
                });
                webClient.DownloadFileCompleted += new AsyncCompletedEventHandler(completed);
                webClient.DownloadProgressChanged += new DownloadProgressChangedEventHandler(downloadProgressChanged);

                webClient.DownloadStringAsync(currentUrl);
                fileDownloadElapsed.Start();
            }
        }

        public void downloadStringAsync(Uri url, EventHandler<DownloadStringCompletedEventArgs> completed) {
            if(!isBusy()) {
                prepare(url);

                webClient.DownloadStringCompleted += new DownloadStringCompletedEventHandler(
                    (object sender, DownloadStringCompletedEventArgs e) => {
                        downloadCompleted();
                });
                webClient.DownloadStringCompleted += new DownloadStringCompletedEventHandler(completed);
                webClient.DownloadProgressChanged += new DownloadProgressChangedEventHandler(downloadProgressChanged);

                webClient.DownloadStringAsync(currentUrl);
                fileDownloadElapsed.Start();
            }
        }

        private void downloadCompleted() {
            busy = false;
            downloadComplete = true;
        }

        private void downloadProgressChanged(object sender, DownloadProgressChangedEventArgs e) {
            fileSize = e.TotalBytesToReceive / 1024;

            fileBytesDownloaded = e.BytesReceived / 1024;
            if (fileBytesDownloaded > 0 && fileDownloadElapsed.ElapsedMilliseconds > 1000) {
                KBps = (int) (fileBytesDownloaded / (fileDownloadElapsed.ElapsedMilliseconds / 1000));
            }

            labelText = "Status: Downloading " + getCurrentFileName() + "\n" + fileBytesDownloaded + 
                " KB / " + fileSize + " KB - " + KBps + " KB/s";

            ui.getStatusLabel().Text = labelText;
            ui.getDownloadProgressBar().Value = e.ProgressPercentage;
        }

        /// <summary>
        /// Returns file size (Kb) of a Uri
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        private static long getFileSize(Uri uri) {
            try {
                WebRequest webRequest = HttpWebRequest.Create(uri);
                using (WebResponse response = webRequest.GetResponse()) {
                    long size = response.ContentLength;
                    return size / 1024;
                }
            }
            catch { return 1; }
        }

        public void prepare(Uri url) {
            if (!isBusy()) {
                busy = true;
                downloadComplete = false;
                fileDownloadElapsed.Reset();
                currentUrl = url;
            }
        }

        public bool isBusy() {
            return busy;
        }

        public String getCurrentFileName() {
            if (currentUrl != null) {
                String path = currentUrl.OriginalString;
                return path.Substring(path.LastIndexOf('/') + 1);
            }
            return null;
        }
    }
}
