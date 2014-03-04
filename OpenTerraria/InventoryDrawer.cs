using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace OpenTerraria {
    public class InventoryDrawer {
        Inventory inventory;
        public InventoryDrawer(Inventory inventory) {
            this.inventory = inventory;
        }
        public void render(Graphics g, Point p) {
            int rows = (int) Math.Ceiling((double) inventory.items.Count() / 10);
            g.FillRectangle(MainForm.createBrush(Color.Gray), new Rectangle(p, new Size(255, (20 * rows) + 10)));
            int column = 0;
            int row = 0;
            foreach (ItemInInventory item in inventory.items) {
                renderItem(item, Util.addPoints(p, new Point(column * 25, row * 25)), g);
                column++;
                if (column >= 10) {
                    column = 0;
                    row++;
                }
            }
        }
        public void renderItem(ItemInInventory item, Point location, Graphics g) {
            if (item == null) {
                //Nothing to do
                return;
            }
            g.DrawImage(item.getItem().getImage(), Util.addPoints(location, new Point(5, 5)));
            g.DrawString(item.count.ToString(), MainForm.getNormalFont(10), MainForm.createBrush(Color.LightGray), (PointF) location);
        }
        public ItemInInventory getItemAtLocation(Point p) {
            int rows = (int)Math.Ceiling((double)inventory.items.Count() / 10);
            int column = 0;
            int row = 0;
            foreach (ItemInInventory item in inventory.items) {
                Point pointToCheck = Util.addPoints(p, new Point(column * 25, row * 25));

                column++;
                if (column >= 10) {
                    column = 0;
                    row++;
                }
            }
            return null;
        }
    }
}
