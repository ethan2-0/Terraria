using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using OpenTerraria.Items;

namespace OpenTerraria {
    [Serializable]
    public class Inventory {
        public ItemInInventory[] items;
        public InventoryDrawer drawer;
        public Inventory(int size) {
            items = new ItemInInventory[size];
            MainForm.getInstance().inventories.Add(this);
            drawer = new InventoryDrawer(this);
        }
        public void draw(Point p, Graphics g) {
            drawer.render(g, p);
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
                if (items[i] == null || ((items[i].item == type && items[i].getCount() < items[i].getItem().getMaxStackSize() - count))) {
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
                if (items[i] == null || (items[i].item == item && items[i].count + count < item.getMaxStackSize())) {
                    if (items[i] == null) {
                        items[i] = new ItemInInventory(item, count);
                        items[i].slot = i;
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
                if (items[i] != null && items[i].count == 0) {
                    items[i] = null;
                }
                if (items[i] != null && items[i].count > items[i].getItem().getMaxStackSize()) {
                    throw new Exception("Count > MaxStack?");
                }
            }
        }
        public bool contains(ItemInInventory i) {
            return indexOf(i) != -1;
        }
        public int indexOf(ItemInInventory item) {
            for (int i = 0; i < items.Count(); i++) {
                if (items[i] == item) {
                    return i;
                }
            }
            return -1;
        }
        public bool hasAmount(InventoryItem item, int amount) {
            int totalAmount = 0;
            foreach (ItemInInventory i in items) {
                if (i == null) {
                    continue;
                }
                if (i.item == item) {
                    totalAmount += i.count;
                }
            }
            return totalAmount >= amount;
        }
        public bool removeAmount(InventoryItem item, int inAmount) {
            int amount = inAmount;
            if (!hasAmount(item, amount)) {
                return false;
            }
            int index = 0;
            foreach(ItemInInventory i in items) {
                index++;
                if (inAmount <= 0) { //If we're done
                    break;
                }
                if (i == null || i.item != item) {
                    continue;
                }
                if (i.count <= 1) { //It should never be less than zero, but you never know...
                    items[index - 1] = null;
                }
                if (i.count >= amount) {
                    i.count -= amount;
                    return true;
                } else {
                    amount -= i.count;
                    i.count = 0;
                }
            }
            return true;
        }
    }
}
