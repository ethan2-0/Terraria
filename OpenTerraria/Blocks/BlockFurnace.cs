using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using OpenTerraria.Items;
using System.Windows.Forms;

namespace OpenTerraria.Blocks {
    public class BlockFurnace : Block {
        public CraftingManager craftingManager;
        public bool craftingWindowShown = false;
        public BlockFurnace(Point location, BlockPrototypeFurnace prototype) : base(prototype, location) {
            initCraftingManager();
            MainForm.getInstance().GameTimer.Tick += new EventHandler(GameTimer_Tick);
        }

        void GameTimer_Tick(object sender, EventArgs e) {
            if (!MainForm.getInstance().inventory) {
                craftingWindowShown = false;
            }
            if (!craftingWindowShown) {
                craftingManager.hide();
            }
        }
        private void initCraftingManager() {
            List<Recepie> recepies = new List<Recepie>();

            Dictionary<InventoryItem, int> input1 = new Dictionary<InventoryItem,int>();
            input1.Add(BlockPrototype.stone, 8);
            KeyValuePair<InventoryItem, int> output1;
            output1 = new KeyValuePair<InventoryItem, int>(BlockPrototype.dirt, 1);
            Recepie recepie1 = new Recepie(input1, output1);
            recepies.Add(recepie1);

            craftingManager = new CraftingManager(recepies);
        }
        public override void use() {
            craftingWindowShown = !craftingWindowShown;
            MainForm.getInstance().inventory = craftingWindowShown;
        }
        public override void draw(Graphics g) {
            base.draw(g);
            if (craftingWindowShown) {
                craftingManager.render(g, new Point(5, 160));
            }
        }
    }
}
