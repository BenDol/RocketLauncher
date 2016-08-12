﻿/*
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

    class Focus {
        private Boolean? causesValidation;

        public Focus(XElement element) {
            parse(element);
        }

        public Focus(Boolean causesValidation) {
            this.causesValidation = causesValidation;
        }

        public Boolean? getCausesValidation() {
            return causesValidation;
        }

        public void setCausesValidation(Boolean causesValidation) {
            this.causesValidation = causesValidation;
        }

        private void parse(XElement element) {
            XElement root = element.Element("Focus");
            try {
                setCausesValidation(Convert.ToBoolean(root.Element("CausesValidation").Value));
            } catch (NullReferenceException) { }
        }

        /**
         * Merge the Control object.
         **/
        internal void merge<T>(ref T control) where T : Control {
            if(causesValidation != null) control.CausesValidation = (Boolean)causesValidation;
        }

        /**
         * Load the Control object.
         **/
        internal void load<T>(ref T control) where T : Control {
            control.CausesValidation = (Boolean)getCausesValidation();
        }
    }
}