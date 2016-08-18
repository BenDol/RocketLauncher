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
using System.Drawing;
using System.Windows.Forms;
using Controls.Development;
using System.IO;

namespace Launcher.Interface {

    partial class Ui : Form {

        private String[] assemblies = {
            null,
            "System.Windows.Forms, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089",
        };

        private const int WM_NCHITTEST = 0x84;
        private const int HTCLIENT = 0x1;
        private const int HTCAPTION = 0x2;

        private Client client;
        private Connecting connecting;

        public Ui(Connecting connecting) {
            this.connecting = connecting;

            InitializeComponent();
        }

        protected override void WndProc(ref Message message) {
            base.WndProc(ref message);

            if (message.Msg == WM_NCHITTEST && (int)message.Result == HTCLIENT)
                message.Result = (IntPtr)HTCAPTION;
        }

        public new void Show() {
            connecting.Hide();
            base.Show();
        }

        public Control.ControlCollection getControls() {
            return Controls;
        }

        public void addOrModifyComponent(Component component) {
            addOrModifyComponent(component, 0);
        }

        public void addOrModifyComponent(Component component, int assemblyIndex) {
            if (assemblyIndex < assemblies.Length) {
                String name = component.getName();
                if (name != null) {
                    Control[] exists = find(name);
                    if (exists.Length < 1) {
                        // Add component
                        try {
                            Control control = (Control)Activator.CreateInstance(assemblies[assemblyIndex], component.getType()).Unwrap();
                            component.load(ref control);
                            Controls.Add(control);
                        } catch (Exception ex) when (ex is TypeLoadException || ex is ArgumentNullException || ex is FileNotFoundException) {
                            addOrModifyComponent(component, ++assemblyIndex);
                        }
                    } else {
                        // Modify component
                        component.merge(ref exists[0]);
                    }
                }
            }
        }

        public Control[] find(String name) {
            if(name == "Ui") {
                return new Control[] { this };
            } else {
                return Controls.Find(name, true);
            }
        }

        public Connecting getConnectingForm() {
            return connecting;
        }

        public Client getClient() {
            return client;
        }

        public void setClient(ref Client client) {
            this.client = client;
        }

        public TextBox getChangelogBox() {
            return txtboxChangelog;
        }

        public PrettyProgressBar getDownloadProgressBar() {
            return pBarMain;
        }

        public ControlLabel getStatusLabel() {
            return lblStatus;
        }

        public ControlLabel getNameLabel() {
            return lblName;
        }

        public GlossyButton getLaunchButton() {
            return btnLaunch;
        }

        public ControlLabel getUpToDateLabel() {
            return lblUptodate;
        }

        public PictureBox getTickImage() {
            return imgTick;
        }

        public ImageListBox getChangelogListBox() {
            return lboxChangelog;
        }

        public ImageListBox getUpdatesListBox() {
            return lboxUpdatelogs;
        }

        public TextBox getUpdatelogsTextBox() {
            return txtBoxUpdate;
        }

        public Button getRefreshButton() {
            return btnRefresh;
        }

        private void btnRefresh_Click(object sender, EventArgs e) {
            client.refresh();
        }

        private void btnRefresh_MouseEnter(object sender, EventArgs e) {
            this.btnRefresh.Image = global::Launcher.Properties.Resources.refresh_hover;
        }

        private void btnRefresh_MouseLeave(object sender, EventArgs e) {
            this.btnRefresh.Image = global::Launcher.Properties.Resources.refresh_normal;
        }

        private void btnPlay_Click(object sender, EventArgs e) {

        }

        private void btnPlay_MouseEnter(object sender, EventArgs e) {
            //this.btnPlay.Image = global::Launcher.Properties.Resources.play_hover;
        }

        private void btnPlay_MouseLeave(object sender, EventArgs e) {
            //this.btnPlay.Image = global::Launcher.Properties.Resources.play_normal;
        }

        private void btnClose_MouseEnter(object sender, EventArgs e) {
            this.btnClose.BackgroundImage = global::Launcher.Properties.Resources.close_hover_icon;
        }

        private void btnClose_MouseLeave(object sender, EventArgs e) {
            this.btnClose.BackgroundImage = global::Launcher.Properties.Resources.close_icon;
        }

        private void btnClose_MouseDown(object sender, MouseEventArgs e) {
            this.btnClose.BackgroundImage = global::Launcher.Properties.Resources.close_icon;
        }

        private void btnClose_MouseClick(object sender, MouseEventArgs e) {
            client.exitApplication();
        }

        private void btnMinimize_MouseEnter(object sender, EventArgs e) {
            this.btnMinimize.BackgroundImage = global::Launcher.Properties.Resources.minimize_hover_icon;
        }

        private void btnMinimize_MouseLeave(object sender, EventArgs e) {
            this.btnMinimize.BackgroundImage = global::Launcher.Properties.Resources.minimize_icon;
        }

        private void btnMinimize_MouseDown(object sender, MouseEventArgs e) {
            this.btnMinimize.BackgroundImage = global::Launcher.Properties.Resources.minimize_icon;
        }

        private void btnMinimize_MouseClick(object sender, MouseEventArgs e) {
            this.WindowState = FormWindowState.Minimized;
        }

        private void Ui_Paint(object sender, PaintEventArgs e) {
            ControlPaint.DrawBorder(e.Graphics, this.ClientRectangle,
                Color.Black, 1, ButtonBorderStyle.Solid,
                Color.Black, 1, ButtonBorderStyle.Solid,
                Color.Black, 1, ButtonBorderStyle.Solid,
                Color.Black, 1, ButtonBorderStyle.Solid);
        }

        private void Ui_MaximumSizeChanged(object sender, EventArgs e) {
            if (WindowState != System.Windows.Forms.FormWindowState.Minimized) {
                WindowState = System.Windows.Forms.FormWindowState.Normal;
            }
        }

        private void Ui_SizeChanged(object sender, EventArgs e) {
            if( WindowState != System.Windows.Forms.FormWindowState.Minimized) {
                WindowState = System.Windows.Forms.FormWindowState.Normal;
            }
        }

        private void pictureBox1_MouseEnter(object sender, EventArgs e) {
            this.btnRepair.Image = global::Launcher.Properties.Resources.repair_hover;
        }

        private void pictureBox1_MouseLeave(object sender, EventArgs e) {
            this.btnRepair.Image = global::Launcher.Properties.Resources.repair;
        }

        private void pictureBox1_MouseClick(object sender, MouseEventArgs e) {
            this.btnRepair.Image = global::Launcher.Properties.Resources.repair;
        }

        private void btnPlay_MouseClick(object sender, MouseEventArgs e) {
            client.executeTarget();
        }

        private void imgLogo_MouseEnter(object sender, EventArgs e) {
            this.imgLogo.Image = global::Launcher.Properties.Resources.sheep_launcher_hover;
        }

        private void imgLogo_MouseLeave(object sender, EventArgs e) {
            this.imgLogo.Image = global::Launcher.Properties.Resources.sheep_launcher;
        }
    }
}
