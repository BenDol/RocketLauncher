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
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Text;
using System.Drawing.Drawing2D;

namespace Launcher.Interface {
    public partial class ControlLabel : Label {
        public ControlLabel() {
            this.SetStyle(ControlStyles.UserPaint, true);

            InitializeComponent();
        }

        Color disabledTextColor = Color.LightGray;

        [Description("The color of the text when disabled")]
        [Category("Appearance")]
        public Color DisabledTextColor {
            get {
                return disabledTextColor;
            }
            set {
                disabledTextColor = value;
            }
        }

        TextRenderingHint textRenderHint = TextRenderingHint.SystemDefault;

        [Description("Set the quality of the text rendering")]
        [Category("Appearance")]
        public TextRenderingHint TextRenderHint {
            get {
                return textRenderHint;
            }
            set {
                textRenderHint = value;
            }
        }

        CompositingQuality compositeQuality = CompositingQuality.Default;

        [Description("Set the composite quality of the text rendering")]
        [Category("Appearance")]
        public CompositingQuality CompositeQuality {
            get {
                return compositeQuality;
            }
            set {
                compositeQuality = value;
            }
        }

        SmoothingMode smoothingMode = SmoothingMode.Default;

        [Description("Set the smoothing mode of the text rendering")]
        [Category("Appearance")]
        public SmoothingMode SmoothingMode {
            get {
                return smoothingMode;
            }
            set {
                smoothingMode = value;
            }
        }

        protected override void OnPaint(PaintEventArgs e) {
            if (Enabled) {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                e.Graphics.TextRenderingHint = textRenderHint;
                base.OnPaint(e);
            }
            else {
                using (Brush aBrush = new SolidBrush(disabledTextColor)) {
                    e.Graphics.CompositingQuality = CompositingQuality.HighQuality;
                    e.Graphics.SmoothingMode = SmoothingMode.HighQuality;
                    e.Graphics.TextRenderingHint = textRenderHint;
                    e.Graphics.DrawString(Text, Font, aBrush, 0f, 0f);
                }
            }
        }
    }
}
