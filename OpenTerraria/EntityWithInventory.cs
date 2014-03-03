using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace OpenTerraria {
    public abstract class EntityWithInventory : Entity {
        public Inventory inventory;
        public EntityWithInventory(String imageName, Point location, Size hitBox, int inventorySize) : base(imageName, location, hitBox) {
            inventory = new Inventory(inventorySize);
        }
    }
}
