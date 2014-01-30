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
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace Launcher {
    class FontHandler {

        private static PrivateFontCollection privateFonts = new PrivateFontCollection();

        public static String addPrivateFont(String uri) {
            privateFonts.AddFontFile(uri);

            FontFamily fontFamily = privateFonts.Families[
                privateFonts.Families.Count() - 1];

            return fontFamily.Name;
        }

        public static PrivateFontCollection getPrivateFonts() {
            return privateFonts;
        }

        public static FontFamily getPrivateFontFamily(String fontName) {
            FontFamily fontFamily = null;
            foreach (FontFamily ff in privateFonts.Families) {
                if (ff.Name.Equals(fontName)) {
                    fontFamily = ff;
                    break;
                }
            }
            return fontFamily;
        }

        public static bool doesPrivateFontExist(string fontFamilyName, bool ignoreCase) {
            bool result = false;

            StringComparison comparison = StringComparison.Ordinal;
            if (ignoreCase) {
                comparison = StringComparison.OrdinalIgnoreCase;
            }

            try {
                foreach (var fontFamily in privateFonts.Families) {
                    if (fontFamily.Name.Equals(fontFamilyName, comparison)) {
                        result = true;
                        break;
                    }
                }
            }
            catch (ArgumentException) {
                result = false;
            }

            return result;
        }

        public static bool doesFontExist(string fontFamilyName, 
                bool checkPrivateFonts, bool ignoreCase) {
            bool result = false;

            StringComparison comparison = StringComparison.Ordinal;
            if (ignoreCase) {
                comparison = StringComparison.OrdinalIgnoreCase;
            }

            try {
                if (checkPrivateFonts) {
                    foreach (var fontFamily in privateFonts.Families) {
                        if (fontFamily.Name.Equals(fontFamilyName, comparison)) {
                            result = true;
                            break;
                        }
                    }
                }

                if (!result) {
                    var fontCollection = new InstalledFontCollection();
                    foreach (var fontFamily in fontCollection.Families) {
                        if (fontFamily.Name.Equals(fontFamilyName, comparison)) {
                            result = true;
                            break;
                        }
                    }
                }
            }
            catch (ArgumentException) {
                result = false;
            }

            return result;
        }
    }
}
