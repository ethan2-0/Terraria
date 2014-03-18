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
            /*for (int i = 0; i < 2; i++) {
                updating = true;
                shouldUseLoadingForm = useLoadingForm;
                Thread t = new Thread(new ParameterizedThreadStart(doFullLightingUpdateThread));
                t.Start();
                /*while (updating) {
                    Thread.Sleep(1);
                }
            }*/
            doFullLightingUpdateThread(null);
        }
        public static void doFullLightingUpdateThread(Object o) {
            updating = true;
            bool useLoadingForm = shouldUseLoadingForm;
            foreach (Block block in MainForm.getInstance().world.blockList) {
                block.lightLevel = 0;
                //block.emittedLightLevel = block.prototype.emittedLightLevel;
            }
            MainForm.getInstance().world.updateSkyLighting();
            fullLightingUpdateEventDispatcher.dispatch();
            updating = false;
        }
    }
}
