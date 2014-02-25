using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace OpenTerraria {
    public abstract class Entity {
        public Bitmap image;
        public Point location;
        public Point momentum;
        public Size hitBox;
        public Entity(String imageName, Point location, Size hitBox) {
            this.image = Reference.getImage(imageName);
            this.location = location;
            this.hitBox = hitBox;
            momentum = new Point(0, 0);
            MainForm.getInstance().entities.Add(this);
        }
        public void draw(Graphics g) {
            g.DrawImage(image, location);
        }
        public void move() {
            int candaditeX = location.X + momentum.X;
            bool isInsideBlock = MainForm.getInstance().world.isInsideBlock(candaditeX, location.Y);
            if (isInsideBlock) {
                candaditeX = location.X;
            }
            int candaditeY = location.Y + momentum.Y;
            isInsideBlock = MainForm.getInstance().world.isInsideBlock(location.X, candaditeY + (hitBox.Height / 4));
            if (isInsideBlock) {
                candaditeY = location.Y;
            }
            location = new Point(candaditeX, candaditeY);
        }
        public void stop() {
            momentum = new Point(0, 0);
        }
        public void update() {
            //Gravity
            momentum.Y++;
            move();
        }
    }
}
