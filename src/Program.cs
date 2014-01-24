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
            Application.SetCompatibleTextRenderingDefault(false);

            Connecting con = new Connecting();
            Ui ui = new Ui(con);

            Client client = new Client(ui);
            ui.setClient(ref client);

            // Initialize the client then run the application
            client.initialize(() => {
                DownloadHandler dlHandler = client.getDownloadHandler();
                if (dlHandler != null) {
                    dlHandler.setProgressBar(ui.getDownloadProgressBar());
                }

                client.update();

                // Display main window
                con.Hide();
                ui.Show();
            });

            Application.Run(con);
        }
    }
}
