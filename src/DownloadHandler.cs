using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows.Forms;

namespace Updater {

    class DownloadHandler {

        public class QueueBlock<T> {
            Uri url;
            Action<T> callback;

            public QueueBlock(Uri url, Action<T> callback) {
                this.url = url;
                this.callback = callback;
            }

            public Uri getUrl() {
                return url;
            }

            public void invokeCallback(T result) {
                if (callback != null) {
                    callback(result);
                }
            }
        }

        private Stopwatch fileDownloadElapsed = new Stopwatch();

        private long fileSize = 0;
        private long fileBytesDownloaded = 0;
        private int KBps = 0;
        private float totalPercent = 0;

        private bool busy = false;
        private bool downloadComplete = false;

        private QueueCallback queueStringCallback;

        private Queue<QueueBlock<String>> queueString =
            new Queue<QueueBlock<String>>();

        private QueueCallback queueFileCallback;

        private Queue<QueueBlock<Boolean>> queueFile =
            new Queue<QueueBlock<Boolean>>();

        private Uri currentUrl;
        private String description = String.Empty;

        private Ui ui;

        public DownloadHandler(Ui ui) {
            this.ui = ui;
        }

        public void downloadFileAsync(Uri url, EventHandler<AsyncCompletedEventArgs> completed) {
            downloadFileAsync(url, completed, false);
        }

        public void downloadFileAsync(Uri url, EventHandler<AsyncCompletedEventArgs> completed,
                bool multiple) {
            if (!isBusy()) {
                prepare(url);

                if (!multiple) {
                    ui.getDownloadProgressBar().Value = 0;
                }

                description = "Status: Downloading " + getCurrentFileName();
                ui.getStatusLabel().Text = description;

                WebClient webClient = new WebClient();
                webClient.DownloadFileCompleted += new AsyncCompletedEventHandler(
                    (object sender, AsyncCompletedEventArgs e) => {
                        downloadCompleted();
                        completed(sender, e);
                });
                webClient.DownloadProgressChanged += new DownloadProgressChangedEventHandler(
                    (object sender, DownloadProgressChangedEventArgs e) => {
                        downloadProgressChanged(e, multiple);
                });

                webClient.DownloadStringAsync(currentUrl);
                fileDownloadElapsed.Start();
            }
        }

        public void downloadStringAsync(Uri url, EventHandler<DownloadStringCompletedEventArgs> completed) {
            downloadStringAsync(url, completed, false);
        }

        public void downloadStringAsync(Uri url, EventHandler<DownloadStringCompletedEventArgs> completed,
                bool multiple) {
            if(!isBusy()) {
                prepare(url);

                if (!multiple) {
                    ui.getDownloadProgressBar().Value = 0;
                }

                WebClient webClient = new WebClient();
                description = "Status: Downloading " + getCurrentFileName();
                ui.getStatusLabel().Text = description;

                webClient.DownloadStringCompleted += new DownloadStringCompletedEventHandler(
                    (object sender, DownloadStringCompletedEventArgs e) => {
                        downloadCompleted();
                        completed(sender, e);
                });
                webClient.DownloadProgressChanged += new DownloadProgressChangedEventHandler(
                    (object sender, DownloadProgressChangedEventArgs e) => {
                        downloadProgressChanged(e, multiple);
                });

                webClient.DownloadStringAsync(currentUrl);
                fileDownloadElapsed.Start();
            }
        }

        private void downloadCompleted() {
            busy = false;
            downloadComplete = true;

            ui.getStatusLabel().Text += " - Finished!";
        }

        private void downloadProgressChanged(DownloadProgressChangedEventArgs e, bool multiple) {
            fileSize = e.TotalBytesToReceive / 1024;

            fileBytesDownloaded = e.BytesReceived / 1024;
            if (fileBytesDownloaded > 0 && fileDownloadElapsed.ElapsedMilliseconds > 1000) {
                KBps = (int) (fileBytesDownloaded / (fileDownloadElapsed.ElapsedMilliseconds / 1000));
            }

            ui.getStatusLabel().Text = description + "\n" + fileBytesDownloaded +
                " KB / " + fileSize + " KB - " + KBps + " KB/s";

            ProgressBar progressBar = ui.getDownloadProgressBar();

            float percentage = 0;
            if (fileBytesDownloaded > 0) {
                percentage = (fileBytesDownloaded / (float)fileSize) * 100f;
            }

            if (multiple) {
                long totalSize = getQueueDownloadSize<String>(queueString);
                if (totalSize > 0) {
                    percentage = (fileBytesDownloaded / (float)totalSize) * 100f;
                }
            }

            ui.getDownloadProgressBar().Value = (int)percentage;
        }

        public void enqueueString(Uri url, Action<String> callback) {
            if (!isBusy()) {
                queueString.Enqueue(new QueueBlock<String>(url, callback));
            }
        }

        public void enqueueFile(Uri url, Action<Boolean> callback) {
            if (!isBusy()) {
                queueFile.Enqueue(new QueueBlock<Boolean>(url, callback));
            }
        }

        public void startStringQueue() {
            ui.getDownloadProgressBar().Value = 0;

            nextString();
        }

        public void nextString() {
            if (queueString.Any()) {
                QueueBlock<String> block = queueString.Dequeue();
                if (block != null) {
                    downloadStringAsync(block.getUrl(), (object sender, DownloadStringCompletedEventArgs e) => {
                        block.invokeCallback(e.Result);
                        nextString();
                    }, true);
                }
            }
            else {
                if (queueStringCallback != null) {
                    queueStringCallback.onFinished();
                    queueStringCallback = null;
                }
            }
        }

        public void startFileQueue() {
            ui.getDownloadProgressBar().Value = 0;

            nextFile();
        }

        public void nextFile() {
            if (queueFile.Any()) {
                QueueBlock<Boolean> block = queueFile.Dequeue();
                if (block != null) {
                    downloadFileAsync(block.getUrl(), (object sender, AsyncCompletedEventArgs e) => {
                        block.invokeCallback(e.Cancelled);
                        nextFile();
                    }, true);
                }
            }
            else {
                if (queueFileCallback != null) {
                    queueFileCallback.onFinished();
                    queueFileCallback = null;
                }
            }
        }

        public void setQueueStringCallback(QueueCallback callback) {
            queueStringCallback = callback;
        }

        public void setQueueFileCallback(QueueCallback callback) {
            queueFileCallback = callback;
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

        private static long getQueueDownloadSize<T>(Queue<QueueBlock<T>> queue) {
            long size = 0;
            foreach (QueueBlock<T> block in queue) {
                size += getFileSize(block.getUrl());
            }
            return size;
        }

        public void prepare(Uri url) {
            busy = true;
            downloadComplete = false;
            fileDownloadElapsed.Reset();
            currentUrl = url;
            ui.getStatusLabel().Text = "";
        }

        public bool isBusy() {
            return busy;
        }

        public String getCurrentFileName() {
            if (currentUrl != null) {
                String path = currentUrl.OriginalString;
                return path.Substring(path.LastIndexOf('/') + 1).Trim();
            }
            return null;
        }
    }
}
