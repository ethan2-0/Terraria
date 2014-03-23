using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace OpenTerraria.Blocks {
    [Serializable]
    public class BlockPrototypeComputer : BlockPrototype {
        public BlockPrototypeComputer(String imageName, String id, String name, bool solid, int maxStack)
            : base(imageName, id, name, solid, maxStack, Color.White) {
            //We don't really have to do anything here -- it's all handled in BlockPrototype.
        }
        public override Block createNew(Point location) {
            return new BlockComputer(location, this);
        }
    }
}
