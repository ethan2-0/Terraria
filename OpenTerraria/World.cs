using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenTerraria {
    public class World {
        public Block[] blocks;
        int width, height;
        public World(int width, int height, Block[] blocks) {
            this.blocks = blocks;
            this.width = width;
            this.height = height;
        }
    }
}
