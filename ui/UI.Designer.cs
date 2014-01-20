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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtboxChangelog = new System.Windows.Forms.TextBox();
            this.progbarProgress = new System.Windows.Forms.ProgressBar();
            this.lblStatus = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtboxChangelog);
            this.groupBox1.Location = new System.Drawing.Point(6, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(261, 174);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Changelog";
            // 
            // txtboxChangelog
            // 
            this.txtboxChangelog.BackColor = System.Drawing.SystemColors.Window;
            this.txtboxChangelog.Location = new System.Drawing.Point(6, 17);
            this.txtboxChangelog.Multiline = true;
            this.txtboxChangelog.Name = "txtboxChangelog";
            this.txtboxChangelog.ReadOnly = true;
            this.txtboxChangelog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtboxChangelog.Size = new System.Drawing.Size(249, 151);
            this.txtboxChangelog.TabIndex = 0;
            this.txtboxChangelog.TabStop = false;
            // 
            // progbarProgress
            // 
            this.progbarProgress.ForeColor = System.Drawing.SystemColors.GrayText;
            this.progbarProgress.Location = new System.Drawing.Point(6, 219);
            this.progbarProgress.Name = "progbarProgress";
            this.progbarProgress.Size = new System.Drawing.Size(261, 24);
            this.progbarProgress.Step = 1;
            this.progbarProgress.TabIndex = 2;
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Location = new System.Drawing.Point(5, 189);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(76, 11);
            this.lblStatus.TabIndex = 3;
            this.lblStatus.Text = "Status: Waiting...";
            // 
            // Ui
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(5F, 11F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(274, 247);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.progbarProgress);
            this.Controls.Add(this.groupBox1);
            this.Font = new System.Drawing.Font("Tahoma", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.MaximizeBox = false;
            this.Name = "Ui";
            this.Text = "Updater";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ProgressBar progbarProgress;
        private System.Windows.Forms.TextBox txtboxChangelog;
        private System.Windows.Forms.Label lblStatus;
    }
}

