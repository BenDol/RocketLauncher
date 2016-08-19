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
using Launcher.Interface;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Net;
using System.Windows.Forms;
using System.Xml.Linq;

namespace Launcher {

    class Appearance {

        private String text;
        private Font font;
        private Color? backColor;
        private BorderStyle? borderStyle;
        private CompositingQuality? compositeQuality;
        private Cursor cursor;
        private Color? disabledTextColor;
        private FlatStyle? flatStyle;
        private Color? foreColor;
        private String image;
        private String imageIndex;
        private String imageKey;
        private String imageList;
        private ContentAlignment? imageAlign;
        private RightToLeft? rightToLeft;
        private SmoothingMode? smoothingMode;
        private ContentAlignment? textAlign;
        private TextRenderingHint? textRenderHint;
        private Boolean? useMnmonic;
        private Boolean? useWaitCursor;

        // GlossyButton
        private Color? activeBackColor;
        private Color? activeBorderColor;
        private Color? borderColor;
        private Color? standbyBackColor;
        private Color? hoverBackColor;
        private Color? standbyBorderColor;
        private Color? hoverBorderColor;

        public Appearance() { }

        public Appearance(XElement element) {
            parse(element);
        }

        public String getText() {
            return text;
        }

        public void setText(String text) {
            this.text = text;
        }

        public Font getFont() {
            return font;
        }

        public void setFont(Font font) {
            this.font = font;
        }

        public Color? getBackColor() {
            return backColor;
        }

        public void setBackColor(Color backColor) {
            this.backColor = backColor;
        }

        public Color? getActiveBackColor() {
            return activeBackColor;
        }

        public void setActiveBackColor(Color activeBackColor) {
            this.activeBackColor = activeBackColor;
        }

        public Color? getActiveBorderColor() {
            return activeBorderColor;
        }

        public void setActiveBorderColor(Color activeBorderColor) {
            this.activeBorderColor = activeBorderColor;
        }

        public Color? getBorderColor() {
            return activeBorderColor;
        }

        public void setBorderColor(Color borderColor) {
            this.borderColor = borderColor;
        }

        public Color? getStandbyBackColor() {
            return standbyBackColor;
        }

        public void setStandbyBackColor(Color standbyBackColor) {
            this.standbyBackColor = standbyBackColor;
        }

        public Color? getHoverBackColor() {
            return hoverBackColor;
        }

        public void setHoverBackColor(Color hoverBackColor) {
            this.hoverBackColor = hoverBackColor;
        }

        public Color? getStandbyBorderColor() {
            return standbyBorderColor;
        }

        public void setStandbyBorderColor(Color standbyBorderColor) {
            this.standbyBorderColor = standbyBorderColor;
        }

        public Color? getHoverBorderColor() {
            return hoverBorderColor;
        }

        public void setHoverBorderColor(Color hoverBorderColor) {
            this.hoverBorderColor = hoverBorderColor;
        }

        public BorderStyle? getBorderStyle() {
            return borderStyle;
        }

        public void setBorderStyle(BorderStyle borderStyle) {
            this.borderStyle = borderStyle;
        }

        public CompositingQuality? getCompositeQuality() {
            return compositeQuality;
        }

        public void setCompositeQuality(CompositingQuality compositeQuality) {
            this.compositeQuality = compositeQuality;
        }

        public Cursor getCursor() {
            return cursor;
        }

        public void setCursor(Cursor cursor) {
            this.cursor = cursor;
        }

        public Color? getDisabledTextColor() {
            return disabledTextColor;
        }

        public void setDisabledTextColor(Color disabledTextColor) {
            this.disabledTextColor = disabledTextColor;
        }

        public FlatStyle? getFlatStyle() {
            return flatStyle;
        }

        public void setFlatStyle(FlatStyle flatStyle) {
            this.flatStyle = flatStyle;
        }

        public Color? getForeColor() {
            return foreColor;
        }

        public void setForeColor(Color foreColor) {
            this.foreColor = foreColor;
        }

        public String getImage() {
            return image;
        }

        public void setImage(String image) {
            this.image = image;
        }

        public String getImageIndex() {
            return imageIndex;
        }

        public void setImageIndex(String imageIndex) {
            this.imageIndex = imageIndex;
        }

        public String getImageKey() {
            return imageKey;
        }

        public void setImageKey(String imageKey) {
            this.imageKey = imageKey;
        }

        public String getImageList() {
            return imageList;
        }

        public void setImageList(String imageList) {
            this.imageList = imageList;
        }

        public ContentAlignment? getImageAlign() {
            return imageAlign;
        }

        public void setImageAlign(ContentAlignment imageAlign) {
            this.imageAlign = imageAlign;
        }

        public RightToLeft? getRightToLeft() {
            return rightToLeft;
        }

        public void setRightToLeft(RightToLeft rightToLeft) {
            this.rightToLeft = rightToLeft;
        }

        public SmoothingMode? getSmoothingMode() {
            return smoothingMode;
        }

        public void setSmoothingMode(SmoothingMode smoothingMode) {
            this.smoothingMode = smoothingMode;
        }

        public ContentAlignment? getTextAlign() {
            return textAlign;
        }

        public void setTextAlign(ContentAlignment textAlign) {
            this.textAlign = textAlign;
        }

        public TextRenderingHint? getTextRenderHint() {
            return textRenderHint;
        }

        public void setTextRenderHint(TextRenderingHint textRenderHint) {
            this.textRenderHint = textRenderHint;
        }

        public Boolean? getUseMnmonic() {
            return useMnmonic;
        }

        public void setUseMnmonic(Boolean useMnmonic) {
            this.useMnmonic = useMnmonic;
        }

        public Boolean? getUseWaitCursor() {
            return useWaitCursor;
        }

        public void setUseWaitCursor(Boolean useWaitCursor) {
            this.useWaitCursor = useWaitCursor;
        }

        private void parse(XElement element) {
            XElement root = element.Element("Appearance");
            if (root == null) {
                Logger.log(Logger.TYPE.DEBUG, "No Appearance element found.");
                return;
            }
            try {
                setText(root.Element("Text").Value);
            } catch (NullReferenceException) {}
            

            XElement font = root.Element("Font");
            if(font != null) {
                FontFamily family = new FontFamily(font.Value);
                float size = (float)Convert.ToDouble(Xml.getAttributeValue(font.Attribute("size"), "12"));
                GraphicsUnit unit = EnumUtils.parseEnum<GraphicsUnit>(Xml.getAttributeValue(font.Attribute("unit"), "Point"));
                FontStyle bold = Convert.ToBoolean(Xml.getAttributeValue(font.Attribute("bold"), "false")) ? FontStyle.Bold : 0;
                FontStyle italic = Convert.ToBoolean(Xml.getAttributeValue(font.Attribute("italic"), "false")) ? FontStyle.Italic : 0;
                FontStyle strikeout = Convert.ToBoolean(Xml.getAttributeValue(font.Attribute("strikeout"), "false")) ? FontStyle.Strikeout : 0;
                FontStyle underline = Convert.ToBoolean(Xml.getAttributeValue(font.Attribute("underline"), "false")) ? FontStyle.Underline : 0;
                byte gdicharset = Convert.ToByte(Xml.getAttributeValue(font.Attribute("gdicharset"), "0"));
                Boolean gdiverticalfont = Convert.ToBoolean(Xml.getAttributeValue(font.Attribute("gdiverticalfont"), "false"));

                setFont(new Font(family, size, (bold | italic | strikeout | underline), unit, gdicharset, gdiverticalfont));
            }

            try {
                setBackColor(Color.FromName(root.Element("BackColor").Value));
            } catch (NullReferenceException) {}

            try {
                setActiveBackColor(Color.FromName(root.Element("ActiveBackColor").Value));
            } catch (NullReferenceException) { }

            try {
                setActiveBorderColor(Color.FromName(root.Element("ActiveBorderColor").Value));
            } catch (NullReferenceException) { }

            try {
                setBorderColor(Color.FromName(root.Element("BorderColor").Value));
            } catch (NullReferenceException) { }

            try {
                setStandbyBackColor(Color.FromName(root.Element("StandbyBackColor").Value));
            } catch (NullReferenceException) { }

            try {
                setHoverBackColor(Color.FromName(root.Element("HoverBackColor").Value));
            } catch (NullReferenceException) { }

            try {
                setStandbyBorderColor(Color.FromName(root.Element("StandbyBorderColor").Value));
            } catch (NullReferenceException) { }

            try {
                setHoverBorderColor(Color.FromName(root.Element("HoverBorderColor").Value));
            } catch (NullReferenceException) { }

            try {
                setBorderStyle(EnumUtils.parseEnum<BorderStyle>(root.Element("BorderStyle").Value));
            } catch (NullReferenceException) { }

            try {
                setCompositeQuality(EnumUtils.parseEnum<CompositingQuality>(root.Element("CompositeQuality").Value));
            } catch (NullReferenceException) { }

            try {
                switch(root.Element("Cursor").Value) {
                    case "AppStarting":
                        setCursor(Cursors.AppStarting);
                        break;
                    case "Arrow":
                        setCursor(Cursors.Arrow);
                        break;
                    case "Cross":
                        setCursor(Cursors.Cross);
                        break;
                    case "Default":
                        setCursor(Cursors.Default);
                        break;
                    case "Hand":
                        setCursor(Cursors.Hand);
                        break;
                    case "Help":
                        setCursor(Cursors.Help);
                        break;
                    case "HSplit":
                        setCursor(Cursors.HSplit);
                        break;
                    case "IBeam":
                        setCursor(Cursors.IBeam);
                        break;
                    case "No":
                        setCursor(Cursors.No);
                        break;
                    case "NoMove2D":
                        setCursor(Cursors.NoMove2D);
                        break;
                    case "NoMoveHoriz":
                        setCursor(Cursors.NoMoveHoriz);
                        break;
                    case "NoMoveVert":
                        setCursor(Cursors.NoMoveVert);
                        break;
                    case "PanEast":
                        setCursor(Cursors.PanEast);
                        break;
                    case "PanNE":
                        setCursor(Cursors.PanNE);
                        break;
                    case "PanNorth":
                        setCursor(Cursors.PanNorth);
                        break;
                    case "PanNW":
                        setCursor(Cursors.PanNW);
                        break;
                    case "PanSE":
                        setCursor(Cursors.PanSE);
                        break;
                    case "PanSouth":
                        setCursor(Cursors.PanSouth);
                        break;
                    case "PanSW":
                        setCursor(Cursors.PanSW);
                        break;
                    case "PanWest":
                        setCursor(Cursors.PanWest);
                        break;
                    case "SizeAll":
                        setCursor(Cursors.SizeAll);
                        break;
                    case "SizeNESW":
                        setCursor(Cursors.SizeNESW);
                        break;
                    case "SizeNS":
                        setCursor(Cursors.SizeNS);
                        break;
                    case "SizeNWSE":
                        setCursor(Cursors.SizeNWSE);
                        break;
                    case "SizeWE":
                        setCursor(Cursors.SizeWE);
                        break;
                    case "UpArrow":
                        setCursor(Cursors.UpArrow);
                        break;
                    case "VSplit":
                        setCursor(Cursors.VSplit);
                        break;
                    case "WaitCursor":
                        setCursor(Cursors.WaitCursor);
                        break;
                }
            } catch (NullReferenceException) { }

            try {
                setDisabledTextColor(Color.FromName(root.Element("DisabledTextColor").Value));
            } catch (NullReferenceException) { }

            try {
                setFlatStyle(EnumUtils.parseEnum<FlatStyle>(root.Element("FlatStyle").Value));
            } catch (NullReferenceException) { }

            try {
                setForeColor(Color.FromName(root.Element("ForeColor").Value));
            } catch (NullReferenceException) { }

            try {
                setImage(root.Element("Image").Value);
            } catch (NullReferenceException) { }

            try {
                setImageIndex(root.Element("ImageIndex").Value);
            } catch (NullReferenceException) { }

            try {
                setImageKey(root.Element("ImageKey").Value);
            } catch (NullReferenceException) { }

            try {
                setImageList(root.Element("ImageList").Value);
            } catch (NullReferenceException) { }

            try {
                setImageAlign(EnumUtils.parseEnum<ContentAlignment>(root.Element("ImageAlign").Value));
            } catch (NullReferenceException) { }

            try {
                setRightToLeft(EnumUtils.parseEnum<RightToLeft>(root.Element("RightToLeft").Value));
            } catch (NullReferenceException) { }

            try {
                setSmoothingMode(EnumUtils.parseEnum<SmoothingMode>(root.Element("SmoothingMode").Value));
            } catch (NullReferenceException) { }

            try {
                setTextAlign(EnumUtils.parseEnum<ContentAlignment>(root.Element("TextAlign").Value));
            } catch (NullReferenceException) { }

            try {
                setTextRenderHint(EnumUtils.parseEnum<TextRenderingHint>(root.Element("TextRenderHint").Value));
            } catch (NullReferenceException) { }

            try {
                setUseMnmonic(Convert.ToBoolean(root.Element("UseMnmonic").Value));
            } catch (NullReferenceException) { }

            try {
                setUseWaitCursor(Convert.ToBoolean(root.Element("UseWaitCursor").Value));
            } catch (NullReferenceException) { }
        }

        /**
         * Load the Control object.
         **/
        internal void merge<T>(ref T control) where T : Control {
            load(ref control);
        }

        /**
         * Load the Control object.
         **/
        internal void load<T>(ref T control) where T : Control {
            if (font != null) control.Font = font;
            if (backColor != null) control.BackColor = (Color)backColor;
            if (cursor != null) control.Cursor = cursor;
            if (foreColor != null) control.ForeColor = (Color)foreColor;
            if (rightToLeft != null) control.RightToLeft = (RightToLeft)rightToLeft;
            if (useWaitCursor != null) control.UseWaitCursor = (Boolean)useWaitCursor;

            if(control is GlossyButton) {
                if(activeBackColor != null) (control as GlossyButton).ActiveBackColor = (Color)activeBackColor;
                if(activeBorderColor != null) (control as GlossyButton).ActiveBorderColor = (Color)activeBorderColor;
                if(borderColor != null) (control as GlossyButton).BorderColor = (Color)borderColor;
                if (standbyBackColor != null) (control as GlossyButton).StandbyBackColor = (Color)standbyBackColor;
                if (hoverBackColor != null) (control as GlossyButton).HoverBackColor = (Color)hoverBackColor;
                if (standbyBorderColor != null) (control as GlossyButton).StandbyBorderColor = (Color)standbyBorderColor;
                if (hoverBorderColor != null) (control as GlossyButton).HoverBorderColor = (Color)hoverBorderColor;
                if (text != null && text != "") (control as GlossyButton).BtnText = text;
            } else {
                if (text != null && text != "") control.Text = text;
            }

            if (control is ListBox) {
                if (borderStyle != null) (control as ListBox).BorderStyle = (BorderStyle)borderStyle;
            }

            if (control is Label) {
                if (textAlign != null) (control as Label).TextAlign = (ContentAlignment)textAlign;
                if (useMnmonic != null) (control as Label).UseMnemonic = (Boolean)useMnmonic;
            }

            if (control is ControlLabel) {
                if (compositeQuality != null) (control as ControlLabel).CompositeQuality = (CompositingQuality)compositeQuality;
                if (disabledTextColor != null) (control as ControlLabel).DisabledTextColor = (Color)disabledTextColor;
                if (smoothingMode != null) (control as ControlLabel).SmoothingMode = (SmoothingMode)smoothingMode;
                if (textRenderHint != null) (control as ControlLabel).TextRenderHint = (TextRenderingHint)textRenderHint;
            }

            if (control is ButtonBase) {
                if (flatStyle != null) (control as ButtonBase).FlatStyle = (FlatStyle)flatStyle;
            }

            if (image != null && image != "") {
                if (control is PictureBox) {
                    (control as PictureBox).Load(image);
                } else {
                    var request = WebRequest.Create(image);

                    using (var response = request.GetResponse())
                    using (var stream = response.GetResponseStream()) {
                        control.BackgroundImage = Bitmap.FromStream(stream);
                    }
                }
            }
        }
    }
}
