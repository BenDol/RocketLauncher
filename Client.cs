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
        private WebClient webClient;
        private Reciever reciever;

        public Client() {
            webClient = new WebClient();
            reciever = new Reciever();
        }

        public WebClient getWebClient() {
            return webClient;
        }

        public Reciever getReciever() {
            return reciever;
        }
    }
}
