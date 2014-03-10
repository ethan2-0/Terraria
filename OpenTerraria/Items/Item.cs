using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace OpenTerraria.Items {
    public abstract class Item : InventoryItem {
        String name;
        String imageName;
        public Item(String name, String imageName) {
            this.name = name;
            this.imageName = imageName;
        }
        public virtual Bitmap getImage() {
            return Reference.getImage(imageName);
        }
        public virtual String getName() {
            return name;
        }
        public virtual int getMaxStackSize() {
            return 255;
        }
        public virtual void use(ItemInInventory item) {
            //Do nothing
        }
    }
}
