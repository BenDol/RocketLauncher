using System;

namespace Launcher
{
    class MissingAttributeException : Exception {

        private String source, attr;

        public MissingAttributeException(String source, String attr) 
            : this(source, attr, "Missing mandatory attribute '" + attr + "' from " + source) {}

        public MissingAttributeException(String source, String attr, String message) 
            : base(message) {
            this.source = source;
            this.attr = attr;
        }

        public String getSource() {
            return source;
        }

        public String getAttr() {
            return attr;
        }
    }
}
