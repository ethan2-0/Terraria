using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace OpenTerraria {
    public abstract class Creature : Entity {
        public abstract int getMaxHealth();
        int health;
        public Creature(String imageName, Point location) : base(imageName, location) {
            health = getMaxHealth();
        }
        public void damage(int amount) {
            health -= amount;
        }
    }
}
