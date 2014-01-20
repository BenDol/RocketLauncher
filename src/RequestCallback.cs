using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Updater {
    class RequestCallback {
        private Action<List<Update>, TimeSpan> success;
        private Action failure;

        public RequestCallback(Action<List<Update>, TimeSpan> onSuccess, Action onFailure) {
            this.success = onSuccess;
            this.failure = onFailure;
        }

        public void onSuccess(List<Update> updates, TimeSpan response) {
            success(updates, response);
        }

        public void onFailure() {
            failure();
        }
    }
}
