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
using System.Windows.Forms;
using System.Xml.Linq;

namespace Launcher {

    class Behavior {

        private Boolean enabled;
        private Boolean visible;
        private Boolean allowDrop;
        private Boolean autoEllipsis;
        private String contextMenuStrip;
        private Boolean useCompatTextRendering;
        
        private int tabIndex;

        public Behavior(XElement element) {
            parse(element);
        }

        public Boolean getAllowDrop() {
            return allowDrop;
        }

        public void setAllowDrop(Boolean allowDrop) {
            this.allowDrop = allowDrop;
        }

        public Boolean getAutoEllipsis() {
            return autoEllipsis;
        }

        public void setAutoEllipsis(Boolean autoEllipsis) {
            this.autoEllipsis = autoEllipsis;
        }

        public String getContextMenuStrip() {
            return contextMenuStrip;
        }

        public void setContextMenuStrip(String contextMenuStrip) {
            this.contextMenuStrip = contextMenuStrip;
        }

        public Boolean getEnabled() {
            return enabled;
        }

        public void setEnabled(Boolean enabled) {
            this.enabled = enabled;
        }

        public int getTabIndex() {
            return tabIndex;
        }

        public void setTabIndex(int tabIndex) {
            this.tabIndex = tabIndex;
        }

        public Boolean getUseCompatTextRendering() {
            return useCompatTextRendering;
        }

        public void setUseCompatTextRendering(Boolean useCompatTextRendering) {
            this.useCompatTextRendering = useCompatTextRendering;
        }

        public Boolean getVisible() {
            return visible;
        }

        public void setVisible(Boolean visible) {
            this.visible = visible;
        }

        private void parse(XElement element) {
            XElement root = element.Element("Behavior");
            try {
                setAllowDrop(Convert.ToBoolean(root.Element("AllowDrop").Value));
            } catch (NullReferenceException) { }

            try {
                setAutoEllipsis(Convert.ToBoolean(root.Element("AutoEllipsis").Value));
            } catch (NullReferenceException) { }

            try {
                setContextMenuStrip(root.Element("ContextMenuStrip").Value);
            } catch (NullReferenceException) { }

            try {
                setEnabled(Convert.ToBoolean(root.Element("Enabled").Value));
            } catch (NullReferenceException) { }

            try {
                setTabIndex(Convert.ToInt32(root.Element("TabIndex").Value));
            } catch (NullReferenceException) { }

            try {
                setUseCompatTextRendering(Convert.ToBoolean(root.Element("UseCompatibleTextRendering").Value));
            } catch (NullReferenceException) { }

            try {
                setVisible(Convert.ToBoolean(root.Element("Visible").Value));
            } catch (NullReferenceException) { }
        }

        /**
         * Load the Control object.
         **/
        internal void load<T>(ref T control) where T : Control {
            control.AllowDrop = getAllowDrop();
            
        }
    }
}
