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

    class Accessibility {

        private String name;
        private String description;
        private AccessibleRole? role;

        public Accessibility() { }

        public Accessibility(XElement element) {
            parse(element);
        }

        public Accessibility(String name, String description, AccessibleRole role) {
            this.name = name;
            this.description = description;
            this.role = role;
        }

        public String getName() {
            return name;
        }

        public void setName(String name) {
            this.name = name;
        }

        public String getDescription() {
            return description;
        }

        public void setDescription(String description) {
            this.description = description;
        }

        public AccessibleRole? getRole() {
            return role;
        }

        public void setRole(AccessibleRole role) {
            this.role = role;
        }

        internal void parse(XElement element) {
            XElement root = element.Element("Accessibility");
            try {
                setName(root.Element("AccessibleName").Value);
            } catch(NullReferenceException) {}

            try {
                setDescription(root.Element("AccessibleDescription").Value);
            } catch (NullReferenceException) { }

            try {
                setRole(EnumUtils.parseEnum<AccessibleRole>(root.Element("AccessibleRole").Value));
            } catch (NullReferenceException) { }
        }

        /**
         * Merge the Control object. 
         **/
        internal void merge<T>(ref T control) where T : Control {
            if(name != null) control.AccessibleName = name;
            if (description != null) control.AccessibleDescription = description;
            if (role != null) control.AccessibleRole = (AccessibleRole)role;
        }

        /**
         * Load the Control object. 
         **/
        internal void load<T>(ref T control) where T: Control {
            control.AccessibleName = getName();
            control.AccessibleDescription = getDescription();
            control.AccessibleRole = (AccessibleRole)getRole();
        }
    }
}
