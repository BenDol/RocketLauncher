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
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using System.Configuration;

namespace Launcher.Interface {
    public partial class GlossyButton : UserControl {
        // Import the Gdi32 DLL
        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]

        // Import the Method from DLL. With this method you can create a Form with rounded corners
        private static extern IntPtr CreateRoundRectRgn(int leftRect, int topRect, 
            int rightRect, int bottomRect, int wEllipse, int hEllipse);

        // Create a Pen that will draw the border of the button.
        Pen pen = new Pen(Color.Transparent);

        Color standbyBackColor = Color.Transparent;

        [Description("The standby back color associated with the control")]
        [Category("Appearance")]
        public Color StandbyBackColor {
            get {
                return standbyBackColor;
            }
            set {
                standbyBackColor = value;
            }
        }

        Color hoverBackColor = Color.Transparent;

        [Description("The mouse hovering back color associated with the control")]
        [Category("Appearance")]
        public Color HoverBackColor {
            get {
                return hoverBackColor;
            }
            set {
                hoverBackColor = value;
            }
        }

        Color activeBackColor = Color.Transparent;

        [Description("The mouse click back color associated with the control")]
        [Category("Appearance")]
        public Color ActiveBackColor {
            get {
                return activeBackColor;
            }
            set {
                activeBackColor = value;
            }
        }

        Color standbyBorderColor = Color.Transparent;

        [Description("The standby border color associated with the control")]
        [Category("Appearance")]
        public Color StandbyBorderColor {
            get {
                return standbyBorderColor;
            }
            set {
                standbyBorderColor = value;
            }
        }

        Color hoverBorderColor = Color.Transparent;

        [Description("The mouse hovering border color associated with the control")]
        [Category("Appearance")]
        public Color HoverBorderColor {
            get {
                return hoverBorderColor;
            }
            set {
                hoverBorderColor = value;
            }
        }

        Color activeBorderColor = Color.Transparent;

        [Description("The mouse click border color associated with the control")]
        [Category("Appearance")]
        public Color ActiveBorderColor {
            get {
                return activeBorderColor;
            }
            set {
                activeBorderColor = value;
            }
        }

        [Description("The border color associated with the control")]
        [Category("Appearance")]
        public Color BorderColor {
            get {
                return pen.Color;
            }
            set {
                pen.Color = value;
            }
        }

        // Configure the BtnText Property
        [Description("The text associated with the control")]
        [Category("Appearance")]
        public string BtnText {
            get {
                return label1.Text;
            }
            set {
                label1.Text = value;
                // Center the label
                centreLabel(label1);
            }
        }

        // Set the font of label1 through the Font Property of GlossyButton
        public override Font Font {
            get {
                return label1.Font;
            }
            set {
                label1.Font = value;
                // Center the label
                centreLabel(label1);
            }
        }

        public GlossyButton() {
            InitializeComponent();
        }

        protected void centreLabel(Label label) {
            int width = (this.Width / 2) - (label.Width / 2);
            int height = (this.Height / 2) - (label.Height / 2);
            label.Location = new Point(width, height);
        }

        protected void onMouseEnter() {
            BorderColor = HoverBorderColor; BackColor = HoverBackColor; this.Invalidate();
        }

        protected void onMouseDown() {
            BorderColor = ActiveBorderColor; BackColor = ActiveBackColor; this.Invalidate();
        }

        protected void NormalStyle() {
            BorderColor = StandbyBorderColor; BackColor = StandbyBackColor; this.Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e) {
            base.OnPaint(e);
            // Create the rounded corners for your form (in this case your button)
            this.Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, this.Width + 1, this.Height + 1, 3, 3));
            // Create a transparent white gradient
            LinearGradientBrush lb = new LinearGradientBrush(new Rectangle(0, 0, this.Width, this.Height), Color.FromArgb(150, Color.White), Color.FromArgb(50, Color.White), LinearGradientMode.Vertical);
            // Draw the gradient
            e.Graphics.FillRectangle(lb, 2, 2, this.Width - 6, this.Height / 2);
            // Draw the border
            e.Graphics.DrawRectangle(pen, 0, 0, this.Width - 3, this.Height - 3);
        }
        protected override void OnSizeChanged(EventArgs e) {
            base.OnSizeChanged(e);
            // Center the label
            centreLabel(label1);
        }

        protected override void OnMouseEnter(EventArgs e) {
            base.OnMouseEnter(e); onMouseEnter();
        }
        protected override void OnMouseLeave(EventArgs e) {
            base.OnMouseLeave(e); NormalStyle();
        }
        protected override void OnMouseDown(MouseEventArgs e) {
            base.OnMouseDown(e); onMouseDown();
        }
        protected override void OnMouseUp(MouseEventArgs e) {
            base.OnMouseUp(e); onMouseEnter();
        }

        private void label1_MouseEnter(object sender, EventArgs e) {
            onMouseEnter();
        }

        private void label1_MouseLeave(object sender, EventArgs e) {
            NormalStyle();
        }

        private void label1_MouseDown(object sender, MouseEventArgs e) {
            onMouseDown();
        }

        private void label1_MouseUp(object sender, MouseEventArgs e) {
            onMouseEnter();
        }
    }
}
