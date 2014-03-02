using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenTerraria {
    public class Inventory {
        public InventoryItem[] items;
        public Inventory(int size) {
            items = new InventoryItem[size];
        }
    }
}
