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
using System.Windows.Forms;
using System.Xml.Linq;

namespace Launcher {

    class Appearance {

        private String text;
        private Font font;
        private Color backColor;
        private BorderStyle borderStyle;
        private CompositingQuality compositeQuality;
        private Cursor cursor;
        private Color disabledTextColor;
        private FlatStyle flatStyle;
        private Color foreColor;
        private String image;
        private String imageIndex;
        private String imageKey;
        private String imageList;
        private ContentAlignment imageAlign;
        private RightToLeft rightToLeft;
        private SmoothingMode smoothingMode;
        private ContentAlignment textAlign;
        private TextRenderingHint textRenderHint;
        private Boolean useMnmonic;
        private Boolean useWaitCursor;

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

        public Color getBackColor() {
            return backColor;
        }

        public void setBackColor(Color backColor) {
            this.backColor = backColor;
        }

        public BorderStyle getBorderStyle() {
            return borderStyle;
        }

        public void setBorderStyle(BorderStyle borderStyle) {
            this.borderStyle = borderStyle;
        }

        public CompositingQuality getCompositeQuality() {
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

        public Color getDisabledTextColor() {
            return disabledTextColor;
        }

        public void setDisabledTextColor(Color disabledTextColor) {
            this.disabledTextColor = disabledTextColor;
        }

        public FlatStyle getFlatStyle() {
            return flatStyle;
        }

        public void setFlatStyle(FlatStyle flatStyle) {
            this.flatStyle = flatStyle;
        }

        public Color getForeColor() {
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

        public ContentAlignment getImageAlign() {
            return imageAlign;
        }

        public void setImageAlign(ContentAlignment imageAlign) {
            this.imageAlign = imageAlign;
        }

        public RightToLeft getRightToLeft() {
            return rightToLeft;
        }

        public void setRightToLeft(RightToLeft rightToLeft) {
            this.rightToLeft = rightToLeft;
        }

        public SmoothingMode getSmoothingMode() {
            return smoothingMode;
        }

        public void setSmoothingMode(SmoothingMode smoothingMode) {
            this.smoothingMode = smoothingMode;
        }

        public ContentAlignment getTextAlign() {
            return textAlign;
        }

        public void setTextAlign(ContentAlignment textAlign) {
            this.textAlign = textAlign;
        }

        public TextRenderingHint getTextRenderHint() {
            return textRenderHint;
        }

        public void setTextRenderHint(TextRenderingHint textRenderHint) {
            this.textRenderHint = textRenderHint;
        }

        public Boolean getUseMnmonic() {
            return useMnmonic;
        }

        public void setUseMnmonic(Boolean useMnmonic) {
            this.useMnmonic = useMnmonic;
        }

        public Boolean getUseWaitCursor() {
            return useWaitCursor;
        }

        public void setUseWaitCursor(Boolean useWaitCursor) {
            this.useWaitCursor = useWaitCursor;
        }

        private void parse(XElement element) {
            XElement root = element.Element("Appearance");
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
        internal void load<T>(ref T control) where T : Control {
            control.Text = getText();
            control.Font = getFont();
            control.BackColor = getBackColor();
            control.Cursor = getCursor();
            control.ForeColor = getForeColor();
            control.RightToLeft = getRightToLeft();
            control.UseWaitCursor = getUseWaitCursor();

            if(control is ListBox) {
                (control as ListBox).BorderStyle = getBorderStyle();
            }

            if(control is Label) {
                (control as Label).TextAlign = getTextAlign();
                (control as Label).UseMnemonic = getUseMnmonic();
            }

            if(control is ControlLabel) {
                (control as ControlLabel).CompositeQuality = getCompositeQuality();
                (control as ControlLabel).DisabledTextColor = getDisabledTextColor();
                (control as ControlLabel).SmoothingMode = getSmoothingMode();
                (control as ControlLabel).TextRenderHint = getTextRenderHint();
            }

            if(control is ButtonBase) {
                (control as ButtonBase).FlatStyle = getFlatStyle();
            }
        }
    }
}
