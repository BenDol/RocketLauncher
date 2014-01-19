using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Updater {
    class Update : UpdateNode {

        Changelog changeLog = new Changelog();
        List<GhostFile> files = new List<GhostFile>();

        public Update(UpdateNode node) {
            this.request = node.getRequest();
            this.url = node.getUrl();
            this.version = node.getVersion();
        }

    }
}
