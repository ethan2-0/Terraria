using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenTerraria {
    public class LightingEngine {
        public static EventDispatcher fullLightingUpdateEventDispatcher;
        static LightingEngine() {
            fullLightingUpdateEventDispatcher = new EventDispatcher();
        }
        public static void doFullLightingUpdate() {
            MainForm.getInstance().world.updateSkyLighting();
            fullLightingUpdateEventDispatcher.dispatch(true);
        }
    }
}
