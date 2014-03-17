using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace OpenTerraria.Items {
    [Serializable]
    public class ItemInInventory {
        public InventoryItem item;
        public int count;
        public int slot = 0;
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
        public void use() {
            item.use(this);
        }
        /// <summary>
        /// Remove this item from the parent inventory. If this item is not in an inventory, will throw an exception.
        /// </summary>
        public void destroy() {
            MainForm.getInstance().getParentInventory(this).items[slot] = null;
        }
        /// <summary>
        /// Use up the specified amount of the item.
        /// </summary>
        /// <param name="amount">The amount to use up.</param>
        /// <returns>Whether or not there was enough to subtract. If this is false, the amount will be used up.</returns>
        public bool useUp(int amount) {
            count -= amount;
            if (count < 0) {
                return false;
            }
            if (count == 0) {
                destroy();
            }
            return true;
        }
    }
}
