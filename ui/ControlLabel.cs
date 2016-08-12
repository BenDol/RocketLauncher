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
