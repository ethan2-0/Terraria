using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using OpenTerraria.Entities;
using System.Windows.Forms;

namespace OpenTerraria.Blocks {
    public class Block : HandlerForEvent{
        public BlockPrototype prototype;
        public Bitmap image;
        public Point location;
        public int lightLevel;
        public Block(BlockPrototype prototype, Point location) {
            this.prototype = prototype;
            this.location = location;
            this.image = prototype.getStartingImage();
            if (prototype.emittedLightLevel > 0) { //We emit light!
                MainForm.getInstance().doneLoadingEventDispatcher.registerHandler(this);
            }
        }
        public Point getWorldLocation() {
            if (location.X % 20 != 0 || location.Y % 20 != 0) {
                //We're not on the grid.
                throw new Exception("I'm not on the grid!");
            }
            Point p = new Point(location.X / 20, location.Y / 20);
            return p;
        }
        public void handle(EventDispatcher dispatcher) {
            if (dispatcher == MainForm.getInstance().doneLoadingEventDispatcher) { //We're done loading!
                if (prototype.emittedLightLevel > 0) {
                    Point myWorldLocation = getWorldLocation();
                    foreach (Block b in MainForm.getInstance().world.blockList) {
                        Point worldLocation = b.getWorldLocation();
                        int distX = Math.Abs(worldLocation.X - myWorldLocation.X);
                        int distY = Math.Abs(worldLocation.Y - myWorldLocation.Y);
                        int totalDist = distX + distY;
                        if (totalDist > prototype.emittedLightLevel) {
                            continue;
                        }
                        b.lightLevel = prototype.emittedLightLevel - totalDist;
                    }
                }
            }
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
            if (xDiff + yDiff > 2000) {
                return;
            }
            Point renderLocation = Util.subtractPoints(location, MainForm.getInstance().viewOffset);
            g.DrawImage(image, renderLocation);
            if (lightLevel > 10) {
                int i;
            }
            Color color = Color.FromArgb((int) (255 - (((double) lightLevel) / 30 * 255)), 0, 0, 0);
            if (prototype == BlockPrototype.oreCoal) {
                int i;
            }
            g.FillRectangle(MainForm.createBrush(color), new Rectangle(renderLocation, new Size(20, 20)));
        }
        public static Block createNewBlock(BlockPrototype prototype, Point location) {
            return prototype.createNew(location);
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
            //To test it
            MessageBox.Show(getName());
        }
    }
}
