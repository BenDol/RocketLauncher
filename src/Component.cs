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
using System.Linq;
using System.Windows.Forms;
using System.Xml.Linq;

namespace Launcher {

    class Component {

        private String name;
        private Accessibility accessibility;
        private Appearance appearance;
        private Behavior behavior;
        private Design design;
        private Focus focus;
        private Layout layout;

        public Component(String name) {
            this.name = name;
        }

        public Component(String name, XElement element) {
            this.name = name;
            parse(element);
        }

        public String getName() {
            return name;
        }

        public void setName(String name) {
            this.name = name;
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

        public Design getDesign() {
            return design;
        }

        public void setDesign(Design design) {
            this.design = design;
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

        public Control compile() {
            Control control = new Control();
            getAccessibility().load(ref control);
            getAppearance().load(ref control);
            getBehavior().load(ref control);

            return control;
        }

        internal void parse(XElement element) {
            setAccessibility(new Accessibility(element));
            setAppearance(new Appearance(element));
            setBehavior(new Behavior(element));
            setDesign(new Design(element));
            setFocus(new Focus(element));
            setLayout(new Layout(element));
        }
    }
}
