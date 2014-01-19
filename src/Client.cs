using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net.NetworkInformation;
using System.Net;
using System.IO;
using System.Threading;
using System.Diagnostics;
using System.Xml.Linq;

namespace Updater {
    class Client {
        private static WebClient webClient = new WebClient();

        private Reciever reciever;
        private Ui ui;

        public Client(Ui ui) {
            this.ui = ui;

            reciever = new Reciever();
        }

        public static WebClient getWebClient() {
            return webClient;
        }

        public Reciever getReciever() {
            return reciever;
        }

        public Ui getUi() {
            return ui;
        }
    }
}
