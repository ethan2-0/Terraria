using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace OpenTerraria {
    public class Block {
        private BlockPrototype prototype;
        public Bitmap image;
        public Point location;
        private Block(BlockPrototype prototype, Point location) {
            this.prototype = prototype;
            this.location = location;
            this.image = prototype.getStartingImage();
        }
        public BlockPrototype getPrototype() {
            return prototype;
        }
        public void draw(Graphics g) {
            g.DrawImage(image, location);
        }
        public static Block createNewBlock(BlockPrototype prototype, Point location) {
            return new Block(prototype, location);
        }
    }
}
