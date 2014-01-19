using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Updater {
    class Log {
        String text;

        public Log(String text) {
            this.text = text;
        }

        public String getText() {
            return text;
        }

        public void setText(String text) {
            this.text = text;
        }
    }
}
