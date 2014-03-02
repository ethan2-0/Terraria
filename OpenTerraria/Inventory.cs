using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenTerraria {
    public class Inventory {
        public ItemInInventory[] items;
        public Inventory(int size) {
            items = new ItemInInventory[size];
        }
        public bool isFull() {
            check();
            for (int i = 0; i < items.Count(); i++) {
                if (items[i] == null) {
                    return false;
                }
            }
            return true;
        }
        public bool hasSpaceFor(InventoryItem type, int count) {
            check();
            for (int i = 0; i < items.Count(); i++) {
                if ((items[i].item == type && items[i].getCount() < items[i].getItem().getMaxStackSize() - count) || items[i] == null) {
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// Get the first empty slot in an inventory.
        /// </summary>
        /// <returns>The 0-based number of the empty slot, or -1 if the inventory is full.</returns>
        public int getFirstEmptySlot() {
            check();
            if (isFull()) {
                return -1;
            }
            for (int i = 0; i < items.Count(); i++) {
                if (items[i] == null) {
                    return i;
                }
            }
            //We don't know what happened, throw an exception
            throw new Exception("What happened? We thought the inventory wasn't full, but it was!");
        }
        public bool addItem(InventoryItem item, int count) {
            check();
            if (!hasSpaceFor(item, count)) {
                return false;
            }
            for (int i = 0; i < items.Count(); i++) {
                if (items[i] == null || (items[i].getItem() == item && items[i].count + count > item.getMaxStackSize())) {
                    if (items[i] == null) {
                        items[i] = new ItemInInventory(item, count);
                    } else {
                        items[i].count += count;
                    }
                    check();
                    return true;
                }
            }
            throw new Exception("What happened? We got through the for loop without returning?");
        }
        public void check() {
            for (int i = 0; i < items.Count(); i++) {
                if (items[i].count == 0) {
                    items[i] = null;
                }
                if (items[i].count > items[i].getItem().getMaxStackSize()) {
                    throw new Exception("Count > MaxStack?");
                }
            }
        }
    }
}
