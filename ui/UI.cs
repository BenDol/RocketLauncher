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

    partial class Ui : Form {

        private const int WM_NCHITTEST = 0x84;
        private const int HTCLIENT = 0x1;
        private const int HTCAPTION = 0x2;

        private Client client;

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

        protected override void WndProc(ref Message message) {
            base.WndProc(ref message);

            if (message.Msg == WM_NCHITTEST && (int)message.Result == HTCLIENT)
                message.Result = (IntPtr)HTCAPTION;
        }

        public Client getClient() {
            return client;
        }

        public void setClient(Client client) {
            this.client = client;
        }

        public TextBox getChangelogBox() {
            return txtboxChangelog;
        }

        public PrettyProgressBar getDownloadProgressBar() {
            return prettyProgress1;
        }

        public Label getStatusLabel() {
            return lblStatus;
        }

        private void btnRefresh_Click(object sender, EventArgs e) {
            client.update();
        }

        private void btnRefresh_MouseEnter(object sender, EventArgs e) {
            this.btnRefresh.Image = global::Updater.Properties.Resources.refresh_hover;
        }

        private void btnRefresh_MouseLeave(object sender, EventArgs e) {
            this.btnRefresh.Image = global::Updater.Properties.Resources.refresh_normal;
        }

        private void btnPlay_Click(object sender, EventArgs e) {

        }

        private void btnPlay_MouseEnter(object sender, EventArgs e) {
            //this.btnPlay.Image = global::Updater.Properties.Resources.play_hover;
        }

        private void btnPlay_MouseLeave(object sender, EventArgs e) {
            //this.btnPlay.Image = global::Updater.Properties.Resources.play_normal;
        }

        private void btnClose_MouseEnter(object sender, EventArgs e) {
            this.btnClose.BackgroundImage = global::Updater.Properties.Resources.close_hover_icon;
        }

        private void btnClose_MouseLeave(object sender, EventArgs e) {
            this.btnClose.BackgroundImage = global::Updater.Properties.Resources.close_icon;
        }

        private void btnClose_MouseDown(object sender, MouseEventArgs e) {
            this.btnClose.BackgroundImage = global::Updater.Properties.Resources.close_icon;
        }

        private void btnClose_MouseClick(object sender, MouseEventArgs e) {
            try {
                Environment.Exit(0);
            }
            catch (Win32Exception ex) {
                Logger.log(Logger.TYPE.WARN, "Exception caught during exit process: " 
                    + ex.Message + ex.StackTrace);
            }
        }

        private void btnMinimize_MouseEnter(object sender, EventArgs e) {
            this.btnMinimize.BackgroundImage = global::Updater.Properties.Resources.minimize_hover_icon;
        }

        private void btnMinimize_MouseLeave(object sender, EventArgs e) {
            this.btnMinimize.BackgroundImage = global::Updater.Properties.Resources.minimize_icon;
        }

        private void btnMinimize_MouseDown(object sender, MouseEventArgs e) {
            this.btnMinimize.BackgroundImage = global::Updater.Properties.Resources.minimize_icon;
        }

        private void btnMinimize_MouseClick(object sender, MouseEventArgs e) {
            this.WindowState = FormWindowState.Minimized;
        }

        private void Ui_Load(object sender, EventArgs e) {

        }
    }
}
