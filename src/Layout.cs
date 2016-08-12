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
using System.Drawing.Printing;
using System.Xml.Linq;

namespace Launcher {

    class Layout {
        private String anchor;
        private Boolean autoSize;
        private String dock;
        private Point location;
        private Margins margin;
        private Margins padding;
        private Size maxSize;
        private Size minSize;
        private Size size;

        public Layout(XElement element) {
            parse(element);
        }

        public String getAnchor() {
            return anchor;
        }

        public void setAnchor(String anchor) {
            this.anchor = anchor;
        }

        public Boolean getAutoSize() {
            return autoSize;
        }

        public void setAutoSize(Boolean autoSize) {
            this.autoSize = autoSize;
        }

        public String getDock() {
            return dock;
        }

        public void setDock(String dock) {
            this.dock = dock;
        }

        public Point getLocation() {
            return location;
        }

        public void setLocation(Point location) {
            this.location = location;
        }

        public Margins getMargin() {
            return margin;
        }

        public void setMargin(Margins margin) {
            this.margin = margin;
        }

        public Margins getPadding() {
            return padding;
        }

        public void setPadding(Margins padding) {
            this.padding = padding;
        }

        public Size getMaxSize() {
            return maxSize;
        }

        public void setMaxSize(Size maxSize) {
            this.maxSize = maxSize;
        }

        public Size getMinSize() {
            return minSize;
        }

        public void setMinSize(Size minSize) {
            this.minSize = minSize;
        }

        public Size getSize() {
            return size;
        }

        public void setSize(Size size) {
            this.size = size;
        }

        private void parse(XElement element) {
            XElement root = element.Element("Layout");
            try {
                setAnchor(root.Element("GenerateMember").Value);
            } catch (NullReferenceException) { }

            try {
                setAutoSize(Convert.ToBoolean(root.Element("AutoSize").Value));
            } catch (NullReferenceException) { }

            try {
                setDock(root.Element("Dock").Value);
            } catch (NullReferenceException) { }

            XElement location = root.Element("Location");
            if (location != null) {
                int x = Convert.ToInt32(Xml.getAttributeValue(location.Attribute("X"), "0"));
                int y = Convert.ToInt32(Xml.getAttributeValue(location.Attribute("Y"), "0"));
                setLocation(new Point(x, y));
            }

            XElement margin = root.Element("Margin");
            if (margin != null) {
                int left = Convert.ToInt32(Xml.getAttributeValue(margin.Attribute("Left"), "0"));
                int top = Convert.ToInt32(Xml.getAttributeValue(margin.Attribute("Top"), "0"));
                int right = Convert.ToInt32(Xml.getAttributeValue(margin.Attribute("Right"), "0"));
                int bottom = Convert.ToInt32(Xml.getAttributeValue(margin.Attribute("Bottom"), "0"));
                setMargin(new Margins(left, right, top, bottom));
            }

            XElement padding = root.Element("Padding");
            if (padding != null) {
                int left = Convert.ToInt32(Xml.getAttributeValue(padding.Attribute("Left"), "0"));
                int top = Convert.ToInt32(Xml.getAttributeValue(padding.Attribute("Top"), "0"));
                int right = Convert.ToInt32(Xml.getAttributeValue(padding.Attribute("Right"), "0"));
                int bottom = Convert.ToInt32(Xml.getAttributeValue(padding.Attribute("Bottom"), "0"));
                setPadding(new Margins(left, right, top, bottom));
            }

            XElement maxSize = root.Element("MaximumSize");
            if (maxSize != null) {
                int width = Convert.ToInt32(Xml.getAttributeValue(maxSize.Attribute("Width"), "0"));
                int height = Convert.ToInt32(Xml.getAttributeValue(maxSize.Attribute("Height"), "0"));
                setMaxSize(new Size(width, height));
            }

            XElement minSize = root.Element("MinimumSize");
            if (minSize != null) {
                int width = Convert.ToInt32(Xml.getAttributeValue(minSize.Attribute("Width"), "0"));
                int height = Convert.ToInt32(Xml.getAttributeValue(minSize.Attribute("Height"), "0"));
                setMinSize(new Size(width, height));
            }

            XElement size = root.Element("Size");
            if (size != null) {
                int width = Convert.ToInt32(Xml.getAttributeValue(size.Attribute("Width"), "0"));
                int height = Convert.ToInt32(Xml.getAttributeValue(size.Attribute("Height"), "0"));
                setSize(new Size(width, height));
            }
        }
    }
}