using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace OpenTerraria {
    public interface InventoryItem {
        int getMaxStackSize();
        Bitmap getImage();
        String getName();
        void use();
    }
}
