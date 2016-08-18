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

    class Component {

        private String name, type;
        private Accessibility accessibility;
        private Appearance appearance;
        private Behavior behavior;
        private Focus focus;
        private Layout layout;

        public Component(String name, String type) {
            this.name = name;
            this.type = type;
        }

        public Component(String name, String type, XElement element) : this(name, type) {
            parse(element);
        }

        public String getName() {
            return name;
        }

        public void setName(String name) {
            this.name = name;
        }

        public String getType() {
            return type;
        }

        public Accessibility getAccessibility() {
            return accessibility;
        }

        public void setAccessibility(Accessibility accessibility) {
            this.accessibility = accessibility;
        }

        public Appearance getAppearance() {
            return appearance;
        }

        public void setAppearance(Appearance appearance) {
            this.appearance = appearance;
        }

        public Behavior getBehavior() {
            return behavior;
        }

        public void setBehavior(Behavior behavior) {
            this.behavior = behavior;
        }
        
        public Focus getFocus() {
            return focus;
        }

        public void setFocus(Focus focus) {
            this.focus = focus;
        }

        public Layout getLayout() {
            return layout;
        }

        public void setLayout(Layout layout) {
            this.layout = layout;
        }

        internal void merge<T>(ref T control) where T : Control {
            getAccessibility().merge(ref control);
            getAppearance().merge(ref control);
            getBehavior().merge(ref control);
            getFocus().merge(ref control);
            getLayout().merge(ref control);
        }

        internal void load<T>(ref T control) where T : Control {
            control.Name = name;
            getAccessibility().load(ref control);
            getAppearance().load(ref control);
            getBehavior().load(ref control);
            getFocus().load(ref control);
            getLayout().load(ref control);
        }

        internal void parse(XElement element) {
            setAccessibility(new Accessibility(element));
            setAppearance(new Appearance(element));
            setBehavior(new Behavior(element));
            setFocus(new Focus(element));
            setLayout(new Layout(element));
        }
    }
}
