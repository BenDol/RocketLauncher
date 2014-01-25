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
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace Updater.Interface {
    public partial class PrettyProgressBar : UserControl {
        public PrettyProgressBar() {
            InitializeComponent();

            label1.ForeColor = Color.Black;
            this.ForeColor = SystemColors.Highlight; // Set the default color of the Pbar 
        }
        protected float percent = 0.0f; // Protected because we don't want this to be accessed from the outside
        // Create a Value property for the Pbar
        public float Value {
            get {
                return percent;
            }
            set {
                // Maintain the Value between 0 and 100
                if (value < 0) {
                    value = 0;
                } else if (value > 100) {
                    value = 100;
                }
                percent = value;
                label1.Text = value.ToString() + "%"; 
                // Redraw the Pbar every time the Value changes
                this.Invalidate(); 
            }
        }

        protected override void OnPaint(PaintEventArgs e) {
            base.OnPaint(e);
            Brush b = new SolidBrush(this.ForeColor); // Create a brush that will draw the background of the Pbar
            // Create a linear gradient that will be drawn over the background. FromArgb means you can use the Alpha value wich is the transparency
            LinearGradientBrush lb = new LinearGradientBrush(new Rectangle(0, 0, this.Width, this.Height), Color.FromArgb(255, Color.White), Color.FromArgb(50, Color.White), LinearGradientMode.ForwardDiagonal);     
            // Calculate how much has the Pbar to be filled for "x" %
            int width = (int)((percent / 100) * this.Width);
            e.Graphics.FillRectangle(b, 0, 0, width, this.Height);
            e.Graphics.FillRectangle(lb, 0, 0, width, this.Height);
            b.Dispose(); lb.Dispose(); 
        }

        private void Pbar_SizeChanged(object sender, EventArgs e) {
            // Maintain the label in the center of the Pbar
            label1.Location = new Point((this.Width / 2) + label1.Width, (this.Height / 2) - (label1.Height / 2));
        }
    }
}
