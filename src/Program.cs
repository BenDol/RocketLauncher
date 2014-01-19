using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Updater {
    static class Program {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args) {
            Ui ui = new Ui();
            Client client = new Client(ui);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(ui);
        }
    }
}
