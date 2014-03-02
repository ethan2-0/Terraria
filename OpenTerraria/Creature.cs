using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace OpenTerraria {
    public abstract class Creature : EntityWithInventory {
        public abstract int getMaxHealth();
        public int health;
        public Creature(String imageName, Point location, Size hitBox, int inventorySize) : base(imageName, location, hitBox, inventorySize) {
            health = getMaxHealth();
        }
        public void damage(int amount) {
            health -= amount;
        }
    }
}
