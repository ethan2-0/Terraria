using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace OpenTerraria {
    public abstract class Entity {
        public Bitmap image;
        public Point location;
        public Entity(String imageName, Point location) {
            this.image = Reference.getImage(imageName);
            this.location = location;
        }
        public void draw(Graphics g) {

        }
    }
}
