using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using OpenTerraria.Blocks;

namespace OpenTerraria.Items {
    public class ItemTool : Item {
        String toolType;
        public ItemTool(String name, String imagename, String toolType) : base(name, imagename) {
            this.toolType = toolType;
        }
        public override int getMaxStackSize() {
            return 1;
        }
        public override void use(ItemInInventory item) {
            base.use(item);
            MainForm instance = MainForm.getInstance();
            Point cursorLocation = MainForm.getInstance().getCursorBlockLocation();
            if(cursorLocation.X < instance.world.blocks.Count() && cursorLocation.Y < instance.world.blocks[5].Count()) {
                if (instance.world.getBlockAt(cursorLocation.X, cursorLocation.Y) != null && instance.world.getBlockAt(cursorLocation.X, cursorLocation.Y).prototype.breakableBy == toolType) {
                    instance.player.inventory.addItem(instance.world.blocks[cursorLocation.X][cursorLocation.Y].prototype, 1);
                    instance.world.blocks[cursorLocation.X][cursorLocation.Y] = Block.createNewBlock(BlockPrototype.air, new Point(cursorLocation.X * 20, cursorLocation.Y * 20));
                }
            }
        }
        public static ItemTool createPickaxe() {
            return new ItemTool("Pickaxe", "pickaxe.png", "pickaxe");
        }
    }
}
