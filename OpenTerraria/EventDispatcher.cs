using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenTerraria {
    public class EventDispatcher {
        List<HandlerForEvent> handlers;
        public EventDispatcher() {
            handlers = new List<HandlerForEvent>();
        }
        public void registerHandler(HandlerForEvent handler) {
            if(!handlers.Contains(handler)) {
                handlers.Add(handler);
            }
        }
        public void dispatch(bool shouldUseLoadingForm) {
            int processed = 0;
            LoadingForm lf = null;
            if (shouldUseLoadingForm) {
                lf = new LoadingForm();
                lf.Show();
            }
            for (int i = 0; i < handlers.Count(); i++) {
                HandlerForEvent handler = handlers[i];
                processed++;
                if (lf != null && processed % 10 == 0) {
                    lf.setProgress("Dispatching Events...", (int)(((double)processed / (double)handlers.Count) * 100D));
                }
                handler.handle(this);
            }
            if (lf != null) {
                lf.Close();
            }
        }
        public void unregisterHandler(HandlerForEvent handler) {
            if (handlers.Contains(handler)) {
                handlers.Remove(handler);
            }
        }
        public void dispatch() {
            dispatch(false);
        }
    }
}
