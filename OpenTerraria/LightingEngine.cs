using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTerraria.Blocks;
using System.Threading;

namespace OpenTerraria {
    public class LightingEngine {
        public static EventDispatcher fullLightingUpdateEventDispatcher;
        private static bool shouldUseLoadingForm;
        public static bool updating = false;
        static LightingEngine() {
            fullLightingUpdateEventDispatcher = new EventDispatcher();
        }
        public static void doFullLightingUpdate() {
            doFullLightingUpdate(true);
        }
        public static void doFullLightingUpdate(bool useLoadingForm) {
            updating = true;
            shouldUseLoadingForm = useLoadingForm;
            Thread t = new Thread(new ParameterizedThreadStart(doFullLightingUpdateThread));
            t.Start();
        }
        public static void doFullLightingUpdateThread(Object o) {
            updating = true;
            bool useLoadingForm = shouldUseLoadingForm;
            foreach (Block block in MainForm.getInstance().world.blockList) {
                block.lightLevel = 0;
                block.emittedLightLevel = block.prototype.emittedLightLevel;
            }
            MainForm.getInstance().world.updateSkyLighting();
            fullLightingUpdateEventDispatcher.dispatch(useLoadingForm);
            updating = false;
        }
    }
}
