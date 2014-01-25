/*
 * Copyright (c) 2010-2014 Updater <https://github.com/BenDol/Basic-Updater>
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
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Updater.Interface;

namespace Updater {

    class DownloadHandler {

        public class QueueBlock<T> {
            Uri url;
            String destination;
            String fileName;
            Action<T> callback;
            long size = 0;

            public QueueBlock(Uri url, String fileName, Action<T> callback)
                : this(url, fileName, null, callback) {
            }

            public QueueBlock(Uri url, String fileName, String destination, Action<T> callback) {
                this.url = url;
                this.fileName = fileName;
                this.destination = destination;
                this.callback = callback;
                this.size = getFileSize(url);
            }

            public Uri getUrl() {
                return url;
            }

            public long getSize() {
                return size;
            }

            public String getDestination() {
                return destination;
            }

            public String getFileName() {
                return fileName;
            }

            public void invokeCallback(T result) {
                if (callback != null) {
                    callback(result);
                }
            }
        }

        private Stopwatch fileDownloadElapsed = new Stopwatch();

        private int KBps = 0;
        private long fileSize = 0;
        private long fileBytesDownloaded = 0;
        private long totalDownloaded = 0;
        private long queueDownloadSize;

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

        private Label statusLabel;
        private PrettyProgressBar progressBar;

        public DownloadHandler(Label statusLabel, PrettyProgressBar progressBar) {
            this.statusLabel = statusLabel;
            this.progressBar = progressBar;
        }

        public void downloadFileAsync(Uri url, String dest, EventHandler<AsyncCompletedEventArgs> completed) {
            downloadFileAsync(url, dest, completed, null);
        }

        public void downloadFileAsync(Uri url, String dest, EventHandler<AsyncCompletedEventArgs> completed,
                QueueBlock<Boolean> block) {
            if (!isBusy()) {
                prepare(url);

                if (block == null && progressBar != null) {
                    progressBar.Value = 0;
                }

                description = "Status: Downloading " + getCurrentFileName();
                if (statusLabel != null) {
                    statusLabel.Text = description;
                }

                WebClient webClient = new WebClient();
                webClient.DownloadFileCompleted += new AsyncCompletedEventHandler(
                    (object sender, AsyncCompletedEventArgs e) => {
                        downloadCompleted(block);
                        completed(sender, e);
                });

                webClient.DownloadProgressChanged += new DownloadProgressChangedEventHandler(
                    (object sender, DownloadProgressChangedEventArgs e) => {
                        downloadProgressChanged(e, block);
                });

                webClient.DownloadFileAsync(currentUrl, dest);
                fileDownloadElapsed.Start();
            }
        }

        public void downloadStringAsync(Uri url, EventHandler<DownloadStringCompletedEventArgs> completed) {
            downloadStringAsync(url, completed, null);
        }

        public void downloadStringAsync(Uri url, EventHandler<DownloadStringCompletedEventArgs> completed,
                QueueBlock<String> block) {
            if(!isBusy()) {
                prepare(url);

                if (block == null && progressBar != null) {
                    progressBar.Value = 0;
                }

                description = "Status: Downloading " + getCurrentFileName();
                if (statusLabel != null) {
                    statusLabel.Text = description;
                }

                WebClient webClient = new WebClient();
                webClient.DownloadStringCompleted += new DownloadStringCompletedEventHandler(
                    (object sender, DownloadStringCompletedEventArgs e) => {
                        downloadCompleted(block);
                        completed(sender, e);
                });

                webClient.DownloadProgressChanged += new DownloadProgressChangedEventHandler(
                    (object sender, DownloadProgressChangedEventArgs e) => {
                        downloadProgressChanged(e, block);
                });

                webClient.DownloadStringAsync(currentUrl);
                fileDownloadElapsed.Start();
            }
        }

        private void downloadCompleted<T>(QueueBlock<T> block) {
            busy = false;
            downloadComplete = true;

            if (block != null) {
                totalDownloaded += block.getSize();
            }

            if (statusLabel != null) {
                statusLabel.Text += " - Finished!";
            }
        }

        private void downloadProgressChanged<T>(DownloadProgressChangedEventArgs e, 
                QueueBlock<T> block) {
            fileSize = e.TotalBytesToReceive / 1024;

            fileBytesDownloaded = e.BytesReceived / 1024;
            if (fileBytesDownloaded > 0 && fileDownloadElapsed.ElapsedMilliseconds > 1000) {
                KBps = (int) (fileBytesDownloaded / (fileDownloadElapsed.ElapsedMilliseconds / 1000));
            }

            if (statusLabel != null) {
                statusLabel.Text = description + "\n" + fileBytesDownloaded +
                    " KB / " + fileSize + " KB - " + KBps + " KB/s";
            }

            float percentage = 0;
            if (fileBytesDownloaded > 0) {
                percentage = (fileBytesDownloaded / (float)fileSize) * 100f;
            }

            if (block != null) {
                long totalSize = queueDownloadSize;
                if (totalSize > 0) {
                    percentage = ((fileBytesDownloaded + totalDownloaded) / 
                        (float)totalSize) * 100f;
                }
            }

            if (progressBar != null) {
                progressBar.Value = (int)percentage;
            }
        }

        public void enqueueString(Uri url, String fileName, 
                Action<String> callback) {
            if (!isBusy()) {
                queueString.Enqueue(new QueueBlock<String>(url, fileName, 
                    callback));
            }
        }

        public void startStringQueue() {
            if (progressBar != null) {
                progressBar.Value = 0;
            }

            queueDownloadSize = getQueueDownloadSize(queueString);

            nextString();
        }

        public void nextString() {
            if (queueString.Any()) {
                QueueBlock<String> block = queueString.Dequeue();
                if (block != null) {
                    downloadStringAsync(block.getUrl(), (object sender, DownloadStringCompletedEventArgs e) => {
                        block.invokeCallback(e.Result);
                        nextString();
                    }, block);
                }
            }
            else {
                totalDownloaded = 0;
                queueDownloadSize = 0;

                if (queueStringCallback != null) {
                    queueStringCallback.onFinished();
                    queueStringCallback = null;
                }
            }
        }

        public void enqueueFile(Uri url, String destination, String fileName, 
                Action<Boolean> callback) {
            if (!isBusy()) {
                queueFile.Enqueue(new QueueBlock<Boolean>(url, 
                    fileName, destination, callback));
            }
        }

        public void startFileQueue() {
            if (progressBar != null) {
                progressBar.Value = 0;
            }

            queueDownloadSize = getQueueDownloadSize(queueFile);

            nextFile();
        }

        public void nextFile() {
            if (queueFile.Any()) {
                QueueBlock<Boolean> block = queueFile.Dequeue();
                if (block != null) {
                    downloadFileAsync(block.getUrl(), Path.Combine(block.getDestination(), 
                        block.getFileName()), 
                        (object sender, AsyncCompletedEventArgs e) => {
                            block.invokeCallback(e.Cancelled);
                            nextFile();
                    }, block);
                }
            }
            else {
                totalDownloaded = 0;
                queueDownloadSize = 0;

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

        private long getQueueDownloadSize<T>(Queue<QueueBlock<T>> queue) {
            long size = 0;
            foreach (QueueBlock<T> block in queue) {
                size += block.getSize();
            }
            return size;
        }

        private static long fetchQueueDownloadSize<T>(Queue<QueueBlock<T>> queue) {
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
            if (statusLabel != null) {
                statusLabel.Text = "";
            }
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

        public Label getStatusLabel() {
            return statusLabel;
        }

        public void setStatusLabel(Label statusLabel) {
            this.statusLabel = statusLabel;
        }

        public PrettyProgressBar getProgressBar() {
            return progressBar;
        }

        public void setProgressBar(PrettyProgressBar progressBar) {
            this.progressBar = progressBar;
        }
    }
}
