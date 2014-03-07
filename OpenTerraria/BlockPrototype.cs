using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace OpenTerraria {
    public class BlockPrototype : InventoryItem {
        
        public static BlockPrototype grass = new BlockPrototype("grass.png", "OpenTerraria:Grass", "Grass", true, 999);
        public static BlockPrototype air = new BlockPrototype("air.png", "OpenTerraria:Air", "Air", false, 999);
        /// <summary>
        /// This feild should <b>NEVER</b> be changed, or accessed directly. Use getID() instead.
        /// </summary>
        private String id;
        private int maxStackSize;
        private Bitmap startingImage;
        public String name;
        public String breakableBy = "pickaxe";
        public bool solid;
        /// <summary>
        /// Create a new BlockPrototype with the specified image path and name.
        /// </summary>
        /// <param name="imageName">The image path, including .png. For example, grass.png.</param>
        /// <param name="id">The ID of the block. For example, OpenTerraria:Grass. Case sensitive.</param>
        public BlockPrototype(String imageName, String id, String name, bool solid, int maxStack) {
            this.id = id;
            this.startingImage = Reference.getImage(imageName);
            this.name = name;
            this.solid = solid;
            this.maxStackSize = maxStack;
        }
        public String getID() {
            return id;
        }
        public virtual Bitmap getStartingImage() {
            return startingImage;
        }
        public override string ToString() {
            return "{BlockPrototype, ID=" + id + ", starting name=" + name + "}";
        }
        public virtual int getMaxStackSize() {
            return maxStackSize;
        }
        public virtual Bitmap getImage() {
            return startingImage;
        }
        public virtual String getName() {
            return name;
        }
        public virtual void use(ItemInInventory item) {
            if (!(item.item is BlockPrototype)) {
                throw new ArgumentException("The provided item is not a BlockPrototype.");
            }
            BlockPrototype b = (BlockPrototype)item.item;
            /*Point p = MainForm.getInstance().getCursorBlockLocation();
            if (MainForm.getInstance().world.blocks[p.X][p.Y].prototype == BlockPrototype.air) {
                Point pos = MainForm.getInstance().getTotalCursorPos();
                MainForm.getInstance().world.blocks[p.X][p.Y] = Block.createNewBlock(this, pos);
                item.useUp(1);
            }*/
            Player player = MainForm.getInstance().player;
            MainForm mainform = MainForm.getInstance();
            World w = mainform.world;
            Point cursorBlock = MainForm.getInstance().getCursorBlockLocation();
            Block block = w.getBlockAt(cursorBlock.X, cursorBlock.Y);
            if (block.prototype == BlockPrototype.air) {
                w.blocks[cursorBlock.X][cursorBlock.Y] = Block.createNewBlock(this, new Point(cursorBlock.X * 20, cursorBlock.Y * 20));
            }
        }
    }
}
