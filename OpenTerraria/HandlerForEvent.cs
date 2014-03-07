using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenTerraria {
    public interface HandlerForEvent {
        /// <summary>
        /// Handle an event from the specified <code>EventDispatcher</code>.
        /// </summary>
        /// <param name="dispatcher">The EventDispatcher dispatching the event.</param>
        void handle(EventDispatcher dispatcher);
    }
}
