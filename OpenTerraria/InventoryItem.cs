using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace OpenTerraria {
    public interface InventoryItem {
        public int getMaxStackSize();
        public Bitmap getImage();
        public String getName();
    }
}
