using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Updater {

    class Request {
        private XDocument serverXML;

        private Uri url;
        private DateTime dateTime;

        public Request(Uri url) {
            this.url = url;
            this.dateTime = new DateTime();
        }

        public void loadServerXML() {
            String xml = Client.getWebClient().DownloadString(url);
            serverXML = XDocument.Parse(xml);
        }
    }
}
