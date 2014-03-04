using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace OpenTerraria {
    public abstract class Entity {
        public Bitmap image;
        public Point location;
        public int blockX, blockY;
        public Point momentum;
        public Size hitBox;
        public bool isOnGround = false;
        public int gravityTicks = 0;
        public Entity(String imageName, Point location, Size hitBox) {
            this.image = Reference.getImage(imageName);
            this.location = location;
            this.hitBox = hitBox;
            momentum = new Point(0, 0);
            MainForm.getInstance().entities.Add(this);
        }
        public virtual void draw(Graphics g) {
            g.DrawImage(image, Util.subtractPoints(location, MainForm.getInstance().viewOffset));
        }
        public virtual void move() {
            //Collisions
            int candaditeX = location.X + momentum.X;
            bool isInsideBlock = MainForm.getInstance().world.isInsideBlock(candaditeX, location.Y);
            if (isInsideBlock) {
                candaditeX = location.X;
            }
            int candaditeY = location.Y + momentum.Y;
            isInsideBlock = MainForm.getInstance().world.isInsideBlock(location.X, candaditeY + (hitBox.Height / 4));
            isOnGround = false;
            if (isInsideBlock) {
                candaditeY = location.Y;
                isOnGround = true;
            }
            //Update pixel location
            location = new Point(candaditeX, candaditeY);
            //Update block location
            blockX = (int) Math.Floor((double) location.X / 20);
            blockY = (int) Math.Floor((double) location.Y / 20);
        }
        public void stop() {
            momentum = new Point(0, 0);
        }
        public virtual void update() {
            //Gravity
            gravityTicks++;
            if (gravityTicks >= 2) {
                gravityTicks = 0;
                momentum.Y++;
            }
            //Movement
            move();
        }
        public override string ToString() {
            return "{Entity, Type=" + this.GetType().ToString() + ", Location={X=" + location.X + ", Y=" + location.Y + "}, BlockLocation={X=" + blockX + ", Y=" + blockY + "}}";
        }
    }
}
