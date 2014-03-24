using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenTerraria.Blocks {
    [Serializable]
    public class PickaxeType {
        #region Declaring constant PickaxeTypes
        public static PickaxeType PICKAXE_IRON = new PickaxeType("Iron", 5);
        public static PickaxeType PICKAXE_STARTING = new PickaxeType("Starting", 2);
        #endregion
        public String name;
        public int hardness;
        public PickaxeType(String name, int hardness) {
            this.name = name;
            this.hardness = hardness;
        }
    }
}
