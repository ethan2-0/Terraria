using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace OpenTerraria.Blocks {
    [Serializable]
    public class BlockPrototypeFurnace : BlockPrototype {
        public BlockPrototypeFurnace(String imageName, String id, String name, bool solid, int maxStack)
            : base(imageName, id, name, solid, maxStack, Color.FromArgb(102, 102, 102)) {
            //We don't really have to do anything here -- it's all handled in BlockPrototype.
        }
        public override Block createNew(Point location) {
            return new BlockFurnace(location, this);
        }
    }
}
