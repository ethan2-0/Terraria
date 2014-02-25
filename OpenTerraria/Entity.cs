using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace OpenTerraria {
    public abstract class Entity {
        public Bitmap image;
        public Point location;
        public PointF momentum;
        public Entity(String imageName, Point location) {
            this.image = Reference.getImage(imageName);
            this.location = location;
            momentum = new PointF(0, 0);
        }
        public void draw(Graphics g) {
            g.DrawImage(image, location);
        }
        public void move() {
            location = Util.addPoints(location, Util.convertToPoint(momentum));
        }
        public void stop() {
            momentum = new PointF(0, 0);
        }
    }
}
