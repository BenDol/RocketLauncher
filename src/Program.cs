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
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Ui ui = new Ui();
            Client client = new Client(ui);

            // Attempt to update first thing
            client.update();

            Application.Run(ui);
        }
    }
}
