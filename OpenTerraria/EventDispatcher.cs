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
            handlers.Add(handler);
        }
        public void dispatch() {
            foreach(HandlerForEvent handler in handlers) {
                handler.handle(this);
            }
        }
    }
}
