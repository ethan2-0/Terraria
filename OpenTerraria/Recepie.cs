using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace OpenTerraria {
    public class Recepie {
        public Dictionary<InventoryItem, int> input;
        public KeyValuePair<InventoryItem, int> output;
        public Recepie(Dictionary<InventoryItem, int> input, KeyValuePair<InventoryItem, int> output) {
            this.input = input;
            this.output = output;
        }
    }
}
