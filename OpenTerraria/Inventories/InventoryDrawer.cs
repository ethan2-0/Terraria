using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using OpenTerraria.Items;

namespace OpenTerraria {
    public class InventoryDrawer {
        Inventory inventory;
        public Dictionary<int, Point> lastRenderedPositions;
        public Rectangle lastRenderedRectangle;
        public InventoryDrawer(Inventory inventory) {
            lastRenderedPositions = new Dictionary<int, Point>();
            this.inventory = inventory;
        }
        public void render(Graphics g, Point p) {
            int rows = (int) Math.Ceiling((double) inventory.items.Count() / 10);
            Rectangle rectangle = new Rectangle(p, new Size(255, (22 * rows) + 10));
            lastRenderedRectangle = rectangle;
            g.FillRectangle(MainForm.createBrush(Reference.guiColor), rectangle);
            g.DrawRectangle(MainForm.createPen(Color.FromArgb(127, 127, 127)), rectangle);
            int column = 0;
            int row = 0;
            for (int i = 0; i < inventory.items.Count(); i++) {
                ItemInInventory item = inventory.items[i];
                Point renderLocation = Util.addPoints(p, new Point(column * 25, row * 25));
                renderItem(item, renderLocation, g, i);
                lastRenderedPositions[i] = renderLocation;
                column++;
                if (column >= 10) {
                    column = 0;
                    row++;
                }
            }
        }
        public void renderItem(ItemInInventory item, Point location, Graphics g, bool forceRender, int index) {
            if (item != null) {
                lastRenderedPositions[/*Util.indexOf(item, inventory.items)*/index] = location;
            }
            if ((item == null || item == MainForm.getInstance().movingItem) && !forceRender) {
                //Don't render it!
                return;
            }
            g.DrawImage(item.getItem().getImage(), Util.addPoints(location, new Point(5, 5)));
            g.DrawString(item.count.ToString(), MainForm.getNormalFont(10), MainForm.createBrush(Color.LightGray), (PointF) location);
        }
        public void renderItem(ItemInInventory item, Point location, Graphics g, int index) {
            renderItem(item, location, g, false, index);
        }
        /// <summary>
        /// Get the <code>ItemInInventory</code> in the <code>InventoryDrawer</code>.
        /// </summary>
        /// <param name="p">The top-left of where the inventory would normall be rendered.</param>
        /// <returns>The <code>ItemInInventory</code>, or null if it is not within this inventory.</returns>
        public int getItemAtLocation(Point p) {
            /*int rows = (int)Math.Ceiling((double)inventory.items.Count() / 10);
            int column = 0;
            int row = 0;
            foreach (ItemInInventory item in inventory.items) {
                Point pointToCheck = Util.addPoints(p, new Point(column * 25, row * 25));
                Rectangle rect = new Rectangle(pointToCheck, new Size(25, 25));
                if (rect.Contains(p)) {
                    return item;
                }
                column++;
                if (column >= 10) {
                    column = 0;
                    row++;
                }
            }
            return null;*/
            for (int i = 0; i < inventory.items.Count(); i++) {
                ItemInInventory item = inventory.items[i];
                try {
                    int index = i;
                    Rectangle rect = new Rectangle(lastRenderedPositions[index], new Size(25, 25));
                    if (rect.Contains(p)) {
                        return i;
                    }
                } catch (KeyNotFoundException e) {
                    continue;
                }
            }
            return -1;
        }
    }
}
