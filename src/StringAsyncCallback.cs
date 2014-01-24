using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Updater {
    class StringAsyncCallback {
        private Action<String> success;
        private Action failure;

        public StringAsyncCallback(Action<String> onSuccess, Action onFailure) {
            this.success = onSuccess;
            this.failure = onFailure;
        }

        public void onSuccess(String value) {
            success(value);
        }

        public void onFailure() {
            failure();
        }
    }
}
