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
using System.Xml.Linq;

namespace Launcher {

    class Design {

        private Boolean generateMember;
        private Boolean locked;
        private String modifiers;

        public Design() { }

        public Design(XElement element) {
            parse(element);
        }

        public Boolean getGenerateMember() {
            return generateMember;
        }

        public void setGenerateMember(Boolean generateMember) {
            this.generateMember = generateMember;
        }

        public Boolean getLocked() {
            return locked;
        }

        public void setLocked(Boolean locked) {
            this.locked = locked;
        }

        public String getModifiers() {
            return modifiers;
        }

        public void setModifiers(String modifiers) {
            this.modifiers = modifiers;
        }

        private void parse(XElement element) {
            XElement root = element.Element("Design");
            try {
                setGenerateMember(Convert.ToBoolean(root.Element("GenerateMember").Value));
            } catch (NullReferenceException) { }

            try {
                setLocked(Convert.ToBoolean(root.Element("Locked").Value));
            } catch (NullReferenceException) { }

            try {
                setModifiers(root.Element("Modifiers").Value);
            } catch (NullReferenceException) { }
        }
    }
}
