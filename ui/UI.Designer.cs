using System.Drawing;
namespace Updater {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Ui));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtboxChangelog = new System.Windows.Forms.TextBox();
            this.lblStatus = new System.Windows.Forms.Label();
            this.lblUptodate = new System.Windows.Forms.Label();
            this.btnMinimize = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.btnClose = new Updater.GlossyButton();
            this.prettyProgress1 = new Updater.PrettyProgressBar();
            this.btnPlay = new Updater.GlossyButton();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtboxChangelog);
            this.groupBox1.Location = new System.Drawing.Point(6, 121);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(437, 182);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Changelog";
            // 
            // txtboxChangelog
            // 
            this.txtboxChangelog.BackColor = System.Drawing.SystemColors.Window;
            this.txtboxChangelog.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtboxChangelog.Location = new System.Drawing.Point(6, 17);
            this.txtboxChangelog.Multiline = true;
            this.txtboxChangelog.Name = "txtboxChangelog";
            this.txtboxChangelog.ReadOnly = true;
            this.txtboxChangelog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtboxChangelog.Size = new System.Drawing.Size(425, 159);
            this.txtboxChangelog.TabIndex = 0;
            this.txtboxChangelog.TabStop = false;
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Location = new System.Drawing.Point(7, 307);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(76, 11);
            this.lblStatus.TabIndex = 3;
            this.lblStatus.Text = "Status: Waiting...";
            // 
            // lblUptodate
            // 
            this.lblUptodate.AutoSize = true;
            this.lblUptodate.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblUptodate.Location = new System.Drawing.Point(346, 313);
            this.lblUptodate.Name = "lblUptodate";
            this.lblUptodate.Size = new System.Drawing.Size(62, 13);
            this.lblUptodate.TabIndex = 7;
            this.lblUptodate.Text = "Up to date!";
            this.lblUptodate.Visible = false;
            // 
            // btnMinimize
            // 
            this.btnMinimize.BackgroundImage = global::Updater.Properties.Resources.minimize_icon;
            this.btnMinimize.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnMinimize.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnMinimize.FlatAppearance.BorderSize = 0;
            this.btnMinimize.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMinimize.ForeColor = System.Drawing.Color.Transparent;
            this.btnMinimize.Location = new System.Drawing.Point(410, 3);
            this.btnMinimize.Name = "btnMinimize";
            this.btnMinimize.Size = new System.Drawing.Size(16, 16);
            this.btnMinimize.TabIndex = 1;
            this.btnMinimize.UseVisualStyleBackColor = true;
            this.btnMinimize.MouseClick += new System.Windows.Forms.MouseEventHandler(this.btnMinimize_MouseClick);
            this.btnMinimize.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnMinimize_MouseDown);
            this.btnMinimize.MouseEnter += new System.EventHandler(this.btnMinimize_MouseEnter);
            this.btnMinimize.MouseLeave += new System.EventHandler(this.btnMinimize_MouseLeave);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(410, 306);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(27, 26);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 6;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Visible = false;
            // 
            // btnRefresh
            // 
            this.btnRefresh.BackColor = System.Drawing.Color.Transparent;
            this.btnRefresh.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnRefresh.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnRefresh.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnRefresh.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRefresh.ForeColor = System.Drawing.SystemColors.Control;
            this.btnRefresh.Image = global::Updater.Properties.Resources.refresh_normal;
            this.btnRefresh.Location = new System.Drawing.Point(5, 335);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(45, 47);
            this.btnRefresh.TabIndex = 4;
            this.btnRefresh.UseVisualStyleBackColor = false;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            this.btnRefresh.MouseEnter += new System.EventHandler(this.btnRefresh_MouseEnter);
            this.btnRefresh.MouseLeave += new System.EventHandler(this.btnRefresh_MouseLeave);
            // 
            // pictureBox2
            // 
            this.pictureBox2.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox2.BackgroundImage = global::Updater.Properties.Resources.logo;
            this.pictureBox2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.pictureBox2.Location = new System.Drawing.Point(142, 8);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(143, 107);
            this.pictureBox2.TabIndex = 11;
            this.pictureBox2.TabStop = false;
            // 
            // btnClose
            // 
            this.btnClose.ActiveBackColor = System.Drawing.Color.Transparent;
            this.btnClose.ActiveBorderColor = System.Drawing.Color.Transparent;
            this.btnClose.BackColor = System.Drawing.Color.Transparent;
            this.btnClose.BackgroundImage = global::Updater.Properties.Resources.close_icon;
            this.btnClose.BorderColor = System.Drawing.Color.Transparent;
            this.btnClose.BtnText = "";
            this.btnClose.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnClose.ForeColor = System.Drawing.Color.Transparent;
            this.btnClose.HoverBackColor = System.Drawing.Color.Transparent;
            this.btnClose.HoverBorderColor = System.Drawing.Color.Transparent;
            this.btnClose.Location = new System.Drawing.Point(431, 3);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(16, 16);
            this.btnClose.StandbyBackColor = System.Drawing.Color.Transparent;
            this.btnClose.StandbyBorderColor = System.Drawing.Color.Transparent;
            this.btnClose.TabIndex = 10;
            this.btnClose.MouseClick += new System.Windows.Forms.MouseEventHandler(this.btnClose_MouseClick);
            this.btnClose.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnClose_MouseDown);
            this.btnClose.MouseEnter += new System.EventHandler(this.btnClose_MouseEnter);
            this.btnClose.MouseLeave += new System.EventHandler(this.btnClose_MouseLeave);
            // 
            // prettyProgress1
            // 
            this.prettyProgress1.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.prettyProgress1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.prettyProgress1.ForeColor = System.Drawing.SystemColors.Highlight;
            this.prettyProgress1.Location = new System.Drawing.Point(54, 341);
            this.prettyProgress1.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.prettyProgress1.Name = "prettyProgress1";
            this.prettyProgress1.Size = new System.Drawing.Size(307, 37);
            this.prettyProgress1.TabIndex = 9;
            this.prettyProgress1.Value = 0F;
            // 
            // btnPlay
            // 
            this.btnPlay.ActiveBackColor = System.Drawing.Color.DarkSeaGreen;
            this.btnPlay.ActiveBorderColor = System.Drawing.Color.White;
            this.btnPlay.BackColor = System.Drawing.Color.DarkSeaGreen;
            this.btnPlay.BorderColor = System.Drawing.Color.White;
            this.btnPlay.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.btnPlay.BtnText = "Play";
            this.btnPlay.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnPlay.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnPlay.HoverBackColor = System.Drawing.Color.MediumAquamarine;
            this.btnPlay.HoverBorderColor = System.Drawing.Color.White;
            this.btnPlay.Location = new System.Drawing.Point(367, 340);
            this.btnPlay.Name = "btnPlay";
            this.btnPlay.Size = new System.Drawing.Size(76, 39);
            this.btnPlay.StandbyBackColor = System.Drawing.Color.DarkSeaGreen;
            this.btnPlay.StandbyBorderColor = System.Drawing.Color.White;
            this.btnPlay.TabIndex = 8;
            // 
            // Ui
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(5F, 11F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(451, 398);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.btnMinimize);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.prettyProgress1);
            this.Controls.Add(this.btnPlay);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.lblUptodate);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Tahoma", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Ui";
            this.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Text = "Updater";
            this.Load += new System.EventHandler(this.Ui_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox txtboxChangelog;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label lblUptodate;
        private GlossyButton btnPlay;
        private PrettyProgressBar prettyProgress1;
        private GlossyButton btnClose;
        private System.Windows.Forms.Button btnMinimize;
        private System.Windows.Forms.PictureBox pictureBox2;
    }
}

