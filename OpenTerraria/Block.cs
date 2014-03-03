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
        public virtual BlockPrototype getPrototype() {
            return prototype;
        }
        public virtual void draw(Graphics g) {
            Player thePlayer = MainForm.getInstance().player;
            int xDiff = Math.Abs(location.X - thePlayer.location.X);
            int yDiff = Math.Abs(location.Y - thePlayer.location.Y);
            if (location.X == 2000) {
                int i = 0;
            }
            if (xDiff + yDiff > 1150) {
                return;
            }
            g.DrawImage(image, Util.subtractPoints(location, MainForm.getInstance().viewOffset));
        }
        public static Block createNewBlock(BlockPrototype prototype, Point location) {
            return new Block(prototype, location);
        }
        public virtual int getMaxStackSize() {
            return 999;
        }
        public virtual Bitmap getImage() {
            return prototype.getStartingImage();
        }
        public virtual String getName() {
            return prototype.name;
        }
        public virtual void use() {
            //Do nothing yet
        }
    }
}
