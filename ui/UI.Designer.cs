/*
 * Copyright (c) 2010-2014 Launcher <https://github.com/BenDol/RocketLauncher>
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
using System.Drawing;

namespace Launcher.Interface {
    partial class Ui {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Ui));
            this.btnMinimize = new System.Windows.Forms.Button();
            this.imgTick = new System.Windows.Forms.PictureBox();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.btnRepair = new System.Windows.Forms.PictureBox();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.imgListDLIcon = new System.Windows.Forms.ImageList(this.components);
            this.imgListArrow = new System.Windows.Forms.ImageList(this.components);
            this.imgLogo = new System.Windows.Forms.PictureBox();
            this.customTabControl1 = new System.Windows.Forms.CustomTabControl();
            this.tabChangelog = new System.Windows.Forms.TabPage();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.lboxChangelog = new Controls.Development.ImageListBox();
            this.txtboxChangelog = new System.Windows.Forms.TextBox();
            this.tabUpdatelogs = new System.Windows.Forms.TabPage();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.lboxUpdatelogs = new Controls.Development.ImageListBox();
            this.txtBoxUpdate = new System.Windows.Forms.TextBox();
            this.lblName = new Launcher.Interface.ControlLabel();
            this.btnClose = new Launcher.Interface.GlossyButton();
            this.pBarMain = new Launcher.Interface.PrettyProgressBar();
            this.btnPlay = new Launcher.Interface.GlossyButton();
            this.lblStatus = new Launcher.Interface.ControlLabel();
            this.lblUptodate = new Launcher.Interface.ControlLabel();
            ((System.ComponentModel.ISupportInitialize)(this.imgTick)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnRepair)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.imgLogo)).BeginInit();
            this.customTabControl1.SuspendLayout();
            this.tabChangelog.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.tabUpdatelogs.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnMinimize
            // 
            this.btnMinimize.BackgroundImage = global::Launcher.Properties.Resources.minimize_icon;
            this.btnMinimize.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnMinimize.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnMinimize.FlatAppearance.BorderSize = 0;
            this.btnMinimize.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMinimize.ForeColor = System.Drawing.Color.Transparent;
            this.btnMinimize.Location = new System.Drawing.Point(469, 5);
            this.btnMinimize.Name = "btnMinimize";
            this.btnMinimize.Size = new System.Drawing.Size(16, 16);
            this.btnMinimize.TabIndex = 1;
            this.btnMinimize.UseVisualStyleBackColor = true;
            this.btnMinimize.MouseClick += new System.Windows.Forms.MouseEventHandler(this.btnMinimize_MouseClick);
            this.btnMinimize.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnMinimize_MouseDown);
            this.btnMinimize.MouseEnter += new System.EventHandler(this.btnMinimize_MouseEnter);
            this.btnMinimize.MouseLeave += new System.EventHandler(this.btnMinimize_MouseLeave);
            // 
            // imgTick
            // 
            this.imgTick.Enabled = false;
            this.imgTick.Image = global::Launcher.Properties.Resources.tick_blue;
            this.imgTick.Location = new System.Drawing.Point(475, 277);
            this.imgTick.Name = "imgTick";
            this.imgTick.Size = new System.Drawing.Size(27, 26);
            this.imgTick.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.imgTick.TabIndex = 6;
            this.imgTick.TabStop = false;
            this.imgTick.Visible = false;
            // 
            // btnRefresh
            // 
            this.btnRefresh.BackColor = System.Drawing.Color.Transparent;
            this.btnRefresh.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnRefresh.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnRefresh.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnRefresh.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRefresh.ForeColor = System.Drawing.SystemColors.Control;
            this.btnRefresh.Image = global::Launcher.Properties.Resources.refresh_normal;
            this.btnRefresh.Location = new System.Drawing.Point(5, 304);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(45, 47);
            this.btnRefresh.TabIndex = 5;
            this.btnRefresh.UseVisualStyleBackColor = false;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            this.btnRefresh.MouseEnter += new System.EventHandler(this.btnRefresh_MouseEnter);
            this.btnRefresh.MouseLeave += new System.EventHandler(this.btnRefresh_MouseLeave);
            // 
            // btnRepair
            // 
            this.btnRepair.Image = global::Launcher.Properties.Resources.repair;
            this.btnRepair.Location = new System.Drawing.Point(479, 37);
            this.btnRepair.Name = "btnRepair";
            this.btnRepair.Size = new System.Drawing.Size(24, 24);
            this.btnRepair.TabIndex = 14;
            this.btnRepair.TabStop = false;
            this.btnRepair.MouseClick += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseClick);
            this.btnRepair.MouseEnter += new System.EventHandler(this.pictureBox1_MouseEnter);
            this.btnRepair.MouseLeave += new System.EventHandler(this.pictureBox1_MouseLeave);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "icon_tick1.png");
            this.imageList1.Images.SetKeyName(1, "icon_tick2.png");
            this.imageList1.Images.SetKeyName(2, "new_icon2.png");
            // 
            // imgListDLIcon
            // 
            this.imgListDLIcon.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imgListDLIcon.ImageStream")));
            this.imgListDLIcon.TransparentColor = System.Drawing.Color.Transparent;
            this.imgListDLIcon.Images.SetKeyName(0, "new_icon1.png");
            // 
            // imgListArrow
            // 
            this.imgListArrow.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imgListArrow.ImageStream")));
            this.imgListArrow.TransparentColor = System.Drawing.Color.Transparent;
            this.imgListArrow.Images.SetKeyName(0, "new_icon3.gif");
            // 
            // imgLogo
            // 
            this.imgLogo.Enabled = false;
            this.imgLogo.Image = global::Launcher.Properties.Resources.sheep_launcher;
            this.imgLogo.Location = new System.Drawing.Point(388, 7);
            this.imgLogo.Name = "imgLogo";
            this.imgLogo.Size = new System.Drawing.Size(64, 64);
            this.imgLogo.TabIndex = 15;
            this.imgLogo.TabStop = false;
            // 
            // customTabControl1
            // 
            this.customTabControl1.Controls.Add(this.tabChangelog);
            this.customTabControl1.Controls.Add(this.tabUpdatelogs);
            this.customTabControl1.DisplayStyle = System.Windows.Forms.TabStyle.Chrome;
            // 
            // 
            // 
            this.customTabControl1.DisplayStyleProvider.BorderColor = System.Drawing.SystemColors.ControlDark;
            this.customTabControl1.DisplayStyleProvider.BorderColorHot = System.Drawing.SystemColors.ControlDark;
            this.customTabControl1.DisplayStyleProvider.BorderColorSelected = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(157)))), ((int)(((byte)(185)))));
            this.customTabControl1.DisplayStyleProvider.CloserColor = System.Drawing.Color.DarkGray;
            this.customTabControl1.DisplayStyleProvider.CloserColorActive = System.Drawing.Color.White;
            this.customTabControl1.DisplayStyleProvider.FocusTrack = false;
            this.customTabControl1.DisplayStyleProvider.HotTrack = true;
            this.customTabControl1.DisplayStyleProvider.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.customTabControl1.DisplayStyleProvider.Opacity = 1F;
            this.customTabControl1.DisplayStyleProvider.Overlap = 16;
            this.customTabControl1.DisplayStyleProvider.Padding = new System.Drawing.Point(7, 5);
            this.customTabControl1.DisplayStyleProvider.Radius = 16;
            this.customTabControl1.DisplayStyleProvider.ShowTabCloser = false;
            this.customTabControl1.DisplayStyleProvider.TextColor = System.Drawing.SystemColors.ControlText;
            this.customTabControl1.DisplayStyleProvider.TextColorDisabled = System.Drawing.SystemColors.ControlDark;
            this.customTabControl1.DisplayStyleProvider.TextColorSelected = System.Drawing.SystemColors.ControlText;
            this.customTabControl1.HotTrack = true;
            this.customTabControl1.Location = new System.Drawing.Point(4, 53);
            this.customTabControl1.Name = "customTabControl1";
            this.customTabControl1.SelectedIndex = 0;
            this.customTabControl1.Size = new System.Drawing.Size(502, 220);
            this.customTabControl1.TabIndex = 2;
            // 
            // tabChangelog
            // 
            this.tabChangelog.Controls.Add(this.splitContainer2);
            this.tabChangelog.Location = new System.Drawing.Point(4, 25);
            this.tabChangelog.Name = "tabChangelog";
            this.tabChangelog.Padding = new System.Windows.Forms.Padding(3);
            this.tabChangelog.Size = new System.Drawing.Size(494, 191);
            this.tabChangelog.TabIndex = 0;
            this.tabChangelog.Text = "Changelog";
            this.tabChangelog.ToolTipText = "New updates changelog";
            this.tabChangelog.UseVisualStyleBackColor = true;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(3, 3);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.lboxChangelog);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.txtboxChangelog);
            this.splitContainer2.Size = new System.Drawing.Size(488, 185);
            this.splitContainer2.SplitterDistance = 162;
            this.splitContainer2.TabIndex = 0;
            // 
            // lboxChangelog
            // 
            this.lboxChangelog.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lboxChangelog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lboxChangelog.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.lboxChangelog.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lboxChangelog.FormattingEnabled = true;
            this.lboxChangelog.ImageList = this.imageList1;
            this.lboxChangelog.Location = new System.Drawing.Point(0, 0);
            this.lboxChangelog.Name = "lboxChangelog";
            this.lboxChangelog.Size = new System.Drawing.Size(162, 185);
            this.lboxChangelog.TabIndex = 3;
            // 
            // txtboxChangelog
            // 
            this.txtboxChangelog.BackColor = System.Drawing.SystemColors.Window;
            this.txtboxChangelog.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtboxChangelog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtboxChangelog.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtboxChangelog.Location = new System.Drawing.Point(0, 0);
            this.txtboxChangelog.Multiline = true;
            this.txtboxChangelog.Name = "txtboxChangelog";
            this.txtboxChangelog.ReadOnly = true;
            this.txtboxChangelog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtboxChangelog.Size = new System.Drawing.Size(322, 185);
            this.txtboxChangelog.TabIndex = 4;
            this.txtboxChangelog.TabStop = false;
            // 
            // tabUpdatelogs
            // 
            this.tabUpdatelogs.Controls.Add(this.splitContainer1);
            this.tabUpdatelogs.Location = new System.Drawing.Point(4, 25);
            this.tabUpdatelogs.Name = "tabUpdatelogs";
            this.tabUpdatelogs.Padding = new System.Windows.Forms.Padding(3);
            this.tabUpdatelogs.Size = new System.Drawing.Size(494, 191);
            this.tabUpdatelogs.TabIndex = 1;
            this.tabUpdatelogs.Text = "Update Logs";
            this.tabUpdatelogs.ToolTipText = "Previous update information";
            this.tabUpdatelogs.UseVisualStyleBackColor = true;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(3, 3);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.lboxUpdatelogs);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.txtBoxUpdate);
            this.splitContainer1.Size = new System.Drawing.Size(488, 183);
            this.splitContainer1.SplitterDistance = 162;
            this.splitContainer1.SplitterWidth = 1;
            this.splitContainer1.TabIndex = 0;
            // 
            // lboxUpdatelogs
            // 
            this.lboxUpdatelogs.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lboxUpdatelogs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lboxUpdatelogs.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.lboxUpdatelogs.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lboxUpdatelogs.FormattingEnabled = true;
            this.lboxUpdatelogs.ImageList = this.imageList1;
            this.lboxUpdatelogs.Location = new System.Drawing.Point(0, 0);
            this.lboxUpdatelogs.Name = "lboxUpdatelogs";
            this.lboxUpdatelogs.Size = new System.Drawing.Size(162, 183);
            this.lboxUpdatelogs.TabIndex = 15;
            // 
            // txtBoxUpdate
            // 
            this.txtBoxUpdate.BackColor = System.Drawing.Color.White;
            this.txtBoxUpdate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtBoxUpdate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtBoxUpdate.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBoxUpdate.Location = new System.Drawing.Point(0, 0);
            this.txtBoxUpdate.Multiline = true;
            this.txtBoxUpdate.Name = "txtBoxUpdate";
            this.txtBoxUpdate.ReadOnly = true;
            this.txtBoxUpdate.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtBoxUpdate.Size = new System.Drawing.Size(325, 183);
            this.txtBoxUpdate.TabIndex = 0;
            // 
            // lblName
            // 
            this.lblName.AutoEllipsis = true;
            this.lblName.AutoSize = true;
            this.lblName.CompositeQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
            this.lblName.DisabledTextColor = System.Drawing.Color.SteelBlue;
            this.lblName.Enabled = false;
            this.lblName.Font = new System.Drawing.Font("Tahoma", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblName.ForeColor = System.Drawing.Color.SteelBlue;
            this.lblName.Location = new System.Drawing.Point(5, 7);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(150, 33);
            this.lblName.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            this.lblName.TabIndex = 11;
            this.lblName.Text = "Gamename";
            this.lblName.TextRenderHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;
            this.lblName.Visible = false;
            // 
            // btnClose
            // 
            this.btnClose.ActiveBackColor = System.Drawing.Color.Transparent;
            this.btnClose.ActiveBorderColor = System.Drawing.Color.Transparent;
            this.btnClose.BackColor = System.Drawing.Color.Transparent;
            this.btnClose.BackgroundImage = global::Launcher.Properties.Resources.close_icon;
            this.btnClose.BorderColor = System.Drawing.Color.Transparent;
            this.btnClose.BtnText = "";
            this.btnClose.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnClose.ForeColor = System.Drawing.Color.Transparent;
            this.btnClose.HoverBackColor = System.Drawing.Color.Transparent;
            this.btnClose.HoverBorderColor = System.Drawing.Color.Transparent;
            this.btnClose.Location = new System.Drawing.Point(490, 5);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(16, 16);
            this.btnClose.StandbyBackColor = System.Drawing.Color.Transparent;
            this.btnClose.StandbyBorderColor = System.Drawing.Color.Transparent;
            this.btnClose.TabIndex = 10;
            this.btnClose.TabStop = false;
            this.btnClose.MouseClick += new System.Windows.Forms.MouseEventHandler(this.btnClose_MouseClick);
            this.btnClose.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnClose_MouseDown);
            this.btnClose.MouseEnter += new System.EventHandler(this.btnClose_MouseEnter);
            this.btnClose.MouseLeave += new System.EventHandler(this.btnClose_MouseLeave);
            // 
            // pBarMain
            // 
            this.pBarMain.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.pBarMain.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pBarMain.ForeColor = System.Drawing.SystemColors.Highlight;
            this.pBarMain.Location = new System.Drawing.Point(54, 310);
            this.pBarMain.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.pBarMain.Name = "pBarMain";
            this.pBarMain.Size = new System.Drawing.Size(368, 37);
            this.pBarMain.TabIndex = 9;
            this.pBarMain.TabStop = false;
            this.pBarMain.Value = 0F;
            // 
            // btnPlay
            // 
            this.btnPlay.ActiveBackColor = System.Drawing.Color.DarkSeaGreen;
            this.btnPlay.ActiveBorderColor = System.Drawing.Color.White;
            this.btnPlay.BackColor = System.Drawing.Color.DarkSeaGreen;
            this.btnPlay.BorderColor = System.Drawing.Color.White;
            this.btnPlay.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.btnPlay.BtnText = "Updating";
            this.btnPlay.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnPlay.Enabled = false;
            this.btnPlay.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnPlay.HoverBackColor = System.Drawing.Color.MediumAquamarine;
            this.btnPlay.HoverBorderColor = System.Drawing.Color.White;
            this.btnPlay.Location = new System.Drawing.Point(427, 309);
            this.btnPlay.Name = "btnPlay";
            this.btnPlay.Size = new System.Drawing.Size(76, 39);
            this.btnPlay.StandbyBackColor = System.Drawing.Color.DarkSeaGreen;
            this.btnPlay.StandbyBorderColor = System.Drawing.Color.White;
            this.btnPlay.TabIndex = 6;
            // 
            // lblStatus
            // 
            this.lblStatus.AutoEllipsis = true;
            this.lblStatus.AutoSize = true;
            this.lblStatus.CompositeQuality = System.Drawing.Drawing2D.CompositingQuality.Default;
            this.lblStatus.DisabledTextColor = System.Drawing.Color.Black;
            this.lblStatus.Enabled = false;
            this.lblStatus.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStatus.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblStatus.Location = new System.Drawing.Point(7, 274);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(93, 13);
            this.lblStatus.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.Default;
            this.lblStatus.TabIndex = 3;
            this.lblStatus.Text = "Status: Waiting...";
            this.lblStatus.TextRenderHint = System.Drawing.Text.TextRenderingHint.SystemDefault;
            // 
            // lblUptodate
            // 
            this.lblUptodate.AutoSize = true;
            this.lblUptodate.CompositeQuality = System.Drawing.Drawing2D.CompositingQuality.Default;
            this.lblUptodate.DisabledTextColor = System.Drawing.Color.Black;
            this.lblUptodate.Enabled = false;
            this.lblUptodate.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblUptodate.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblUptodate.Location = new System.Drawing.Point(412, 283);
            this.lblUptodate.Name = "lblUptodate";
            this.lblUptodate.Size = new System.Drawing.Size(62, 13);
            this.lblUptodate.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.Default;
            this.lblUptodate.TabIndex = 7;
            this.lblUptodate.Text = "Up to date!";
            this.lblUptodate.TextRenderHint = System.Drawing.Text.TextRenderingHint.SystemDefault;
            this.lblUptodate.Visible = false;
            // 
            // Ui
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(5F, 11F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(511, 355);
            this.Controls.Add(this.imgLogo);
            this.Controls.Add(this.btnRepair);
            this.Controls.Add(this.customTabControl1);
            this.Controls.Add(this.lblName);
            this.Controls.Add(this.btnMinimize);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.pBarMain);
            this.Controls.Add(this.btnPlay);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.lblUptodate);
            this.Controls.Add(this.imgTick);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Tahoma", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Ui";
            this.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Launcher";
            this.MaximumSizeChanged += new System.EventHandler(this.Ui_MaximumSizeChanged);
            this.SizeChanged += new System.EventHandler(this.Ui_SizeChanged);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.Ui_Paint);
            ((System.ComponentModel.ISupportInitialize)(this.imgTick)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnRepair)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.imgLogo)).EndInit();
            this.customTabControl1.ResumeLayout(false);
            this.tabChangelog.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            this.splitContainer2.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.tabUpdatelogs.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ControlLabel lblStatus;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.PictureBox imgTick;
        private ControlLabel lblUptodate;
        private GlossyButton btnPlay;
        private PrettyProgressBar pBarMain;
        private GlossyButton btnClose;
        private System.Windows.Forms.Button btnMinimize;
        private ControlLabel lblName;
        private System.Windows.Forms.TextBox txtboxChangelog;
        private System.Windows.Forms.CustomTabControl customTabControl1;
        private System.Windows.Forms.TabPage tabChangelog;
        private System.Windows.Forms.TabPage tabUpdatelogs;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TextBox txtBoxUpdate;
        private System.Windows.Forms.PictureBox btnRepair;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private Controls.Development.ImageListBox lboxChangelog;
        private System.Windows.Forms.ImageList imageList1;
        private Controls.Development.ImageListBox lboxUpdatelogs;
        private System.Windows.Forms.ImageList imgListDLIcon;
        private System.Windows.Forms.ImageList imgListArrow;
        private System.Windows.Forms.PictureBox imgLogo;
    }
}

