using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace Updater {
    class FontHandler {

        [DllImport("gdi32.dll", EntryPoint = "AddFontResourceW", SetLastError = true)]
        public static extern int AddFontResource([In][MarshalAs(UnmanagedType.LPWStr)]
            string lpFileName);

        public static void installLocalFont(String uri) {
            int result = AddFontResource(uri);

            int error = Marshal.GetLastWin32Error();
            if (error != 0) {
                Console.WriteLine(new Win32Exception(error).Message);
            }
            else {
                if (result == 0) {
                    Logger.log(Logger.TYPE.DEBUG, "Font is already installed: " + uri);
                }
                else {
                    Logger.log(Logger.TYPE.DEBUG, "Font installed successfully:" + uri);
                }
            }
        }

        public static void installExternalFont(String url) {
            
        }
    }
}
