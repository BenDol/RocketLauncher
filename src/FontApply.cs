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
using System.Xml.Linq;

namespace Launcher {
    class FontApply {
        String to;
        Font font;

        public FontApply(XElement elem, String fontName) {
            setTo(elem);
            setFont(elem, fontName);
        }

        public String getTo() {
            return to;
        }

        protected void setTo(XElement elem) {
            try {
                to = elem.Attribute("to").Value;
            }
            catch (Exception e) {
                Logger.log(Logger.TYPE.ERROR, "FontApply: Unable to parse 'to'. " + e.Message);
            }
        }

        public Font getFont() {
            return font;
        }

        protected void setFont(XElement elem, String fontName) {
            float size = 0;
            try {
                size = float.Parse(elem.Attribute("size").Value);
            }
            catch (Exception e) {
                Logger.log(Logger.TYPE.ERROR, "FontApply: Unable to parse 'size'. " + e.Message);
                return;
            }

            GraphicsUnit unit;
            try {
                unit = EnumUtils.parseEnum<GraphicsUnit>(elem.Attribute("unit").Value);
            }
            catch (Exception e) {
                Logger.log(Logger.TYPE.ERROR, "FontApply: Unable to parse 'unit'. " + e.Message);
                return;
            }

            bool gdiVerticalFont;
            try {
                gdiVerticalFont = Convert.ToBoolean(elem.Attribute("gdiverticalfont").Value);
            }
            catch (Exception e) {
                Logger.log(Logger.TYPE.ERROR, "FontApply: Unable to parse 'gdiverticalfont'. " + e.Message);
                return;
            }

            byte gdiCharSet;
            try {
                gdiCharSet = Convert.ToByte(elem.Attribute("gdicharset").Value);
            }
            catch (Exception e) {
                Logger.log(Logger.TYPE.ERROR, "FontApply: Unable to parse 'gdicharset'. " + e.Message);
                return;
            }

            bool bold;
            try {
                bold = Convert.ToBoolean(elem.Attribute("bold").Value);
            }
            catch (Exception e) {
                Logger.log(Logger.TYPE.ERROR, "FontApply: Unable to parse 'bold'. " + e.Message);
                return;
            }

            bool italic;
            try {
                italic = Convert.ToBoolean(elem.Attribute("italic").Value);
            }
            catch (Exception e) {
                Logger.log(Logger.TYPE.ERROR, "FontApply: Unable to parse 'italic'. " + e.Message);
                return;
            }

            bool strikeout;
            try {
                strikeout = Convert.ToBoolean(elem.Attribute("strikeout").Value);
            }
            catch (Exception e) {
                Logger.log(Logger.TYPE.ERROR, "FontApply: Unable to parse 'strikeout'. " + e.Message);
                return;
            }

            bool underline;
            try {
                underline = Convert.ToBoolean(elem.Attribute("underline").Value);
            }
            catch (Exception e) {
                Logger.log(Logger.TYPE.ERROR, "FontApply: Unable to parse 'underline'. " + e.Message);
                return;
            }

            FontStyle fontStyle = FontStyle.Regular;
            if (bold) {
                fontStyle = FontStyle.Bold;
            }
            if (italic) {
                fontStyle |= FontStyle.Italic;
            }
            if (strikeout) {
                fontStyle |= FontStyle.Strikeout;
            }
            if (underline) {
                fontStyle |= FontStyle.Underline;
            }

            this.font = new Font(fontName, size, fontStyle, unit, gdiCharSet, gdiVerticalFont);
        }

        public void usePrivateFont(String fontName) {
            FontStyle fontStyle = FontStyle.Regular;
            if (font.Bold) {
                fontStyle = FontStyle.Bold;
            }
            if (font.Italic) {
                fontStyle |= FontStyle.Italic;
            }
            if (font.Strikeout) {
                fontStyle |= FontStyle.Strikeout;
            }
            if (font.Underline) {
                fontStyle |= FontStyle.Underline;
            }

            FontFamily fontFamily = FontHandler.getPrivateFontFamily(fontName);
            font = new Font(fontFamily, font.Size, font.Style, font.Unit, font.GdiCharSet, font.GdiVerticalFont);
        }
    }
}
