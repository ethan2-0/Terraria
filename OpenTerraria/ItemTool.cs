using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace OpenTerraria {
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
            try {
                instance.world.blocks[cursorLocation.X][cursorLocation.Y] = Block.createNewBlock(BlockPrototype.air, new Point(cursorLocation.X * 20, cursorLocation.Y * 20));
            } catch (IndexOutOfRangeException e) {
                return;
            }
        }
        public static ItemTool createPickaxe() {
            return new ItemTool("Pickaxe", "pickaxe.png", "pickaxe");
        }
    }
}
