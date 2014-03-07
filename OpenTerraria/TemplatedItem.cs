using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenTerraria {
    public class TemplatedItem : Item {
        int maxStack;
        public TemplatedItem(String name, String imagename, int maxStack) : base(name, imagename) {
            this.maxStack = maxStack;
        }
        public override int getMaxStackSize() {
            return maxStack;
        }
    }
}
