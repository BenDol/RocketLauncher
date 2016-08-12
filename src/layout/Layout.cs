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
using System.Windows.Forms;
using System.Xml.Linq;

namespace Launcher {

    class Layout {
        private AnchorStyles? anchor;
        private Boolean? autoSize;
        private DockStyle? dock;
        private Point location;
        private Padding margin;
        private Padding padding;
        private Size maxSize;
        private Size minSize;
        private Size size;

        public Layout(XElement element) {
            parse(element);
        }

        public AnchorStyles? getAnchor() {
            return anchor;
        }

        public void setAnchor(AnchorStyles anchor) {
            this.anchor = anchor;
        }

        public Boolean? getAutoSize() {
            return autoSize;
        }

        public void setAutoSize(Boolean autoSize) {
            this.autoSize = autoSize;
        }

        public DockStyle? getDock() {
            return dock;
        }

        public void setDock(DockStyle dock) {
            this.dock = dock;
        }

        public Point getLocation() {
            return location;
        }

        public void setLocation(Point location) {
            this.location = location;
        }

        public Padding getMargin() {
            return margin;
        }

        public void setMargin(Padding margin) {
            this.margin = margin;
        }

        public Padding getPadding() {
            return padding;
        }

        public void setPadding(Padding padding) {
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
                setAnchor(EnumUtils.parseEnum<AnchorStyles>(root.Element("Anchor").Value));
            } catch (NullReferenceException) { }

            try {
                setAutoSize(Convert.ToBoolean(root.Element("AutoSize").Value));
            } catch (NullReferenceException) { }

            try {
                setDock(EnumUtils.parseEnum<DockStyle>(root.Element("Dock").Value));
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
                setMargin(new Padding(left, right, top, bottom));
            }

            XElement padding = root.Element("Padding");
            if (padding != null) {
                int left = Convert.ToInt32(Xml.getAttributeValue(padding.Attribute("Left"), "0"));
                int top = Convert.ToInt32(Xml.getAttributeValue(padding.Attribute("Top"), "0"));
                int right = Convert.ToInt32(Xml.getAttributeValue(padding.Attribute("Right"), "0"));
                int bottom = Convert.ToInt32(Xml.getAttributeValue(padding.Attribute("Bottom"), "0"));
                setPadding(new Padding(left, right, top, bottom));
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

        /**
         * Merge the Control object.
         **/
        internal void merge<T>(ref T control) where T : Control {
            if (anchor != null) control.Anchor = (AnchorStyles)anchor;
            if (location != null) control.Location = location;
            if (margin != null) control.Margin = margin;
            if (padding != null) control.Padding = padding;
            if (maxSize != null) control.MaximumSize = maxSize;
            if (minSize != null) control.MinimumSize = minSize;
            if (size != null) control.Size = size;

            if (control is Label) {
                if (autoSize != null) (control as Label).AutoSize = (Boolean)autoSize;
            }

            if (control is SplitContainer) {
                if (dock != null) (control as SplitContainer).Dock = (DockStyle)dock;
            }
        }

        /**
         * Load the Control object.
         **/
        internal void load<T>(ref T control) where T : Control {
            control.Anchor = (AnchorStyles)getAnchor();
            control.Location = getLocation();
            control.Margin = getMargin();
            control.Padding = getPadding();
            control.MaximumSize = getMaxSize();
            control.MinimumSize = getMinSize();
            control.Size = getSize();

            if (control is Label) {
                (control as Label).AutoSize = (Boolean)getAutoSize();
            }

            if(control is SplitContainer) {
                (control as SplitContainer).Dock = (DockStyle)getDock();
            }
        }
    }
}