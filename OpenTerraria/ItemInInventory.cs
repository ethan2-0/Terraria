using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenTerraria {
    public class ItemInInventory {
        public InventoryItem item;
        public int count;
        public ItemInInventory(InventoryItem item, int count) {
            this.item = item;
            this.count = count;
        }
        public int getCount() {
            return count;
        }
        public InventoryItem getItem() {
            return item;
        }
    }
}
