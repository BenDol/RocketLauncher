using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Updater {
    class QueueCallback {
        private Action finished;

        public QueueCallback(Action onFinished) {
            this.finished = onFinished;
        }

        public void onFinished() {
            finished();
        }
    }
}
