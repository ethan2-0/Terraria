using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace OpenTerraria.Items {
    public interface InventoryItem {
        int getMaxStackSize();
        Bitmap getImage();
        /// <summary>
        /// Get the name of the item.
        /// </summary>
        /// <returns>The name of the item.</returns>
        String getName();
        /// <summary>
        /// Use the item.
        /// </summary>
        /// <param name="item">The <code>ItemInInventory</code> housing the item. Can be null if none.</param>
        void use(ItemInInventory item);
    }
}
