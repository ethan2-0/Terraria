using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using OpenTerraria.Entities;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace OpenTerraria.Blocks {
    [Serializable]
    public class Block : HandlerForEvent{
        public BlockPrototype prototype;
        public Point location;
        public int lightLevel;
        public int emittedLightLevel;
        public bool registeredLightingUpdate = false;
        public int brokenness = 0;
        public int occasionalTicks = 0;
        public Block(BlockPrototype prototype, Point location) {
            this.prototype = prototype;
            this.location = location;
            this.emittedLightLevel = prototype.emittedLightLevel;
            if (prototype.emittedLightLevel > 0) { //We emit light!
                LightingEngine.fullLightingUpdateEventDispatcher.registerHandler(this);
            }
            MainForm.getInstance().Load += new EventHandler(Block_Load);
            MainForm.getInstance().GameTimer.Tick += new EventHandler(GameTimer_Tick);
        }

        void Block_Load(object sender, EventArgs e) {
            
        }

        void GameTimer_Tick(object sender, EventArgs e) {
            if (!registeredLightingUpdate && prototype.emittedLightLevel > 0) { //We emit light!
                LightingEngine.fullLightingUpdateEventDispatcher.registerHandler(this);
                registeredLightingUpdate = true;
            } else if(registeredLightingUpdate) {
                LightingEngine.fullLightingUpdateEventDispatcher.unregisterHandler(this);
                registeredLightingUpdate = false;
            }
            occasionalTicks++;
            if (occasionalTicks > 20) {
                if (brokenness > 0) {
                    brokenness--;
                }
            }
            if (prototype == BlockPrototype.sand) {
                int i = 0;
            }
            if (prototype.falls) {
                Block below = getBlockAtRelativePosition(0, 1);
                if (below.prototype.getID().Equals(BlockPrototype.air.getID())) { //There's nothing below us!
                    MainForm.getInstance().world.blocks[getWorldLocation().X][getWorldLocation().Y + 1] = this; //We go down there ...
                    MainForm.getInstance().world.blocks[getWorldLocation().X][getWorldLocation().Y] = null; // ...and remove the existing block down there.
                }
            }
        }
        public Block getBlockAtRelativePosition(int x, int y) {
            Point worldLocation = getWorldLocation();
            int newX = worldLocation.X + x;
            int newY = worldLocation.X + x;
            return MainForm.getInstance().world.blocks[newX][newY];
        }
        public Point getWorldLocation() {
            if (location.X % 20 != 0 || location.Y % 20 != 0) {
                //We're not on the grid.
                throw new Exception("I'm not on the grid!");
            }
            Point p = new Point(location.X / 20, location.Y / 20);
            return p;
        }
        public virtual void handle(EventDispatcher dispatcher) {
            if (dispatcher == LightingEngine.fullLightingUpdateEventDispatcher) { //Lighting update
                updateLighting();
            }
        }
        public void updateLighting() {
            if (prototype.emittedLightLevel > 0) {
                Point myWorldLocation = getWorldLocation();
                foreach (Block[] blocks in MainForm.getInstance().world.blocks) {
                    if (Math.Abs(blocks[0].getWorldLocation().X - myWorldLocation.X) < emittedLightLevel) {
                        foreach (Block b in blocks) {
                            Point worldLocation = b.getWorldLocation();
                            int distX = Math.Abs(worldLocation.X - myWorldLocation.X);
                            int distY = Math.Abs(worldLocation.Y - myWorldLocation.Y);
                            int totalDist = distX + distY;
                            if (totalDist < prototype.emittedLightLevel) {
                                int candaditeLightLevel = prototype.emittedLightLevel - totalDist;
                                if (b.lightLevel < candaditeLightLevel) {
                                    b.lightLevel = candaditeLightLevel;
                                }
                            }
                        }
                    }
                }
            } else {
                //LightingEngine.fullLightingUpdateEventDispatcher.unregisterHandler(this);
            }
        }
        public virtual void setEmittedLightLevel(int level) {
            if (level <= 0) {
                LightingEngine.fullLightingUpdateEventDispatcher.unregisterHandler(this);
            } else {
                LightingEngine.fullLightingUpdateEventDispatcher.registerHandler(this);
            }
        }
        public virtual BlockPrototype getPrototype() {
            return prototype;
        }
        public virtual void setLightLevel(int level) {
            lightLevel = level;
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
            g.DrawImage(getImage(), renderLocation);
            if (lightLevel > 10) {
                int i;
            }
            double newBrokenness = (((double)brokenness) / ((double)prototype.hardness));
            newBrokenness = (newBrokenness > 1 ? 1 : newBrokenness);
            Color color = Color.FromArgb((int)(255 - (((double)lightLevel) / 30 * 255)), 0, 0, 0);
            g.FillRectangle(MainForm.createBrush(color), new Rectangle(renderLocation, new Size(20, 20)));
            if (newBrokenness > 0) {
                Color color2 = Color.FromArgb((int)(newBrokenness * 255), 255, 0, 0);
                g.FillRectangle(MainForm.createBrush(color2), new Rectangle(renderLocation, new Size(20, 20)));
            }
        }
        public static Block createNewBlock(BlockPrototype prototype, Point location) {
            return prototype.createNew(location);
        }
        public virtual int getMaxStackSize() {
            return 999;
        }
        public virtual Bitmap getImage() {
            return prototype.getImage();
        }
        public virtual String getName() {
            return prototype.name;
        }
        public virtual void use() {
            //To test it
            MessageBox.Show(getName());
        }
        public virtual void prepareForRemoval() {
            LightingEngine.fullLightingUpdateEventDispatcher.unregisterHandler(this);
        }
    }
}
