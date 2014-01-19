using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Updater {
    class RequestCallback {
        private Action<List<UpdateNode>> success;
        private Action failure;

        public RequestCallback(Action<List<UpdateNode>> onSuccess, Action onFailure) {
            this.success = onSuccess;
            this.failure = onFailure;
        }

        public void onSuccess(List<UpdateNode> updates) {
            success(updates);
        }

        public void onFailure() {
            failure();
        }
    }
}
