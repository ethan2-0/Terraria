using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using OpenTerraria.Items;
using OpenTerraria.Entities;

namespace OpenTerraria.Blocks {
    [Serializable]
    public class BlockPrototype : InventoryItem {
        #region Declaring constant BlockPrototypes
        public static BlockPrototype grass = new BlockPrototype("grass.png", "OpenTerraria:Grass", "Grass", true, 999, Color.FromArgb(84, 55, 27), 10);
        public static BlockPrototype air = new BlockPrototype("air.png", "OpenTerraria:Air", "Air", false, 999, Color.Transparent).setLightBlocking(false);
        public static BlockPrototype glass = new BlockPrototype("glass.png", "OpenTerraria:Glass", "Glass", true, 999, Color.White, 10).setLightBlocking(false);
        public static BlockPrototype sand = new BlockPrototype("sand.png", "OpenTerraria:Sand", "Sand", true, 999, Color.FromArgb(255, 229, 180), 10, true);
        public static BlockPrototype stone = new BlockPrototype("stone.png", "OpenTerraria:Stone", "Stone", true, 999, Color.FromArgb(102, 102, 102));
        public static BlockPrototype dirt = new BlockPrototype("dirt.png", "OpenTerraria:Dirt", "Dirt", true, 999, Color.FromArgb(84, 55, 27), 8);
        public static BlockPrototype leaves = new BlockPrototype("leaves.png", "OpenTerraria:Leaves", "Leaves", false, 999, Color.FromArgb(32, 76, 10)).setLightBlocking(false);
        public static BlockPrototype log = new BlockPrototype("log.png", "OpenTerraria:Log", "Wood", false, 999, Color.FromArgb(56, 43, 26));
        public static BlockPrototype planks = new BlockPrototype("planks.png", "OpenTerraria:Planks", "Wooden Planks", true, 999, Color.FromArgb(173, 133, 84));
        public static BlockPrototype oreCoal = new BlockPrototype("coalOre.png", "OpenTerraria:CoalOre", "Coal Ore", true, 255, Color.FromArgb(102, 102, 102));
        public static BlockPrototype torch = new BlockPrototype("torch.png", "OpenTerraria:Torch", "Torch", false, 499, Color.Transparent, 1).setLightBlocking(false);
        public static BlockPrototype furnace = new BlockPrototypeFurnace("furnace.png", "OpenTerraria:Furnace", "Furnace", true, 999);
        public static BlockPrototype ironOre = new BlockPrototype("ironOre.png", "OpenTerraria:IronOre", "Iron Ore", true, 99, Color.FromArgb(102, 102, 102));
        public static BlockPrototype computer = new BlockPrototypeComputer("computer.png", "OpenTerraria:Computer", "Computer", false, 99);
        #endregion
        static BlockPrototype() {
            air.breakableBy = "visualstudio2010";
            torch.emittedLightLevel = 25;
            //oreCoal.emittedLightLevel = 29;
        }
        /// <summary>
        /// This feild should <b>NEVER</b> be changed, or accessed directly. Use getID() instead.
        /// </summary>
        private String id;
        public bool lightBlocking = true;
        private int maxStackSize;
        private Bitmap startingImage;
        public String name;
        public bool falls;
        /// <summary>
        /// The tool the block is breakable by. Blocks that are supposed to be
        /// unbreakable have "visualstudio2010" in this feild. Tools that are
        /// not supposed to break anything have "visualstudio2013" in this feild.
        /// </summary>
        public String breakableBy = "pickaxe";
        private bool solid;
        /// <summary>
        /// The hardness of the block. Number of hits it can take.
        /// </summary>
        public int hardness;
        /// <summary>
        /// The emitted light level. Weird things could happen if it gets over 15.
        /// </summary>
        public int emittedLightLevel = 0;
        public Color color;
        /// <summary>
        /// Create a new BlockPrototype with the specified image path and name.
        /// </summary>
        /// <param name="imageName">The image path, including .png. For example, grass.png.</param>
        /// <param name="id">The ID of the block. For example, OpenTerraria:Grass. Case sensitive.</param>
        public BlockPrototype(String imageName, String id, String name, bool solid, int maxStack, Color color, int hardness = 20, bool falls = false) {
            this.color = color;
            this.id = id;
            this.startingImage = Reference.getImage(imageName);
            this.name = name;
            this.solid = solid;
            this.maxStackSize = maxStack;
            this.hardness = hardness;
            this.falls = falls;
        }
        public BlockPrototype setLightBlocking(bool lightBlocking) {
            this.lightBlocking = lightBlocking;
            return this;
        }
        public bool isSolid() {
            return solid;
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
            Point cursorBlock = mainform.getCursorBlockLocation();//new Point(mainform.player.blockX, mainform.player.blockY);//getCursorBlockLocation();
            Block block = w.getBlockAt(cursorBlock.X, cursorBlock.Y);
            if (block == null) {
                return;
            }
            if(Util.distanceBetween(block.location, mainform.player.location) > 200) {
                return;
            }
            if (block.prototype.id == "OpenTerraria:Air") {
                w.blocks[cursorBlock.X][cursorBlock.Y].prepareForRemoval();
                w.blocks[cursorBlock.X][cursorBlock.Y] = Block.createNewBlock(this, new Point(cursorBlock.X * 20, cursorBlock.Y * 20));
                w.updateSkyLightForColumn(cursorBlock.X);
            }
            Inventory inventory = MainForm.getInstance().getParentInventory(item);
            inventory.removeAmount(this, 1);
            LightingEngine.doFullLightingUpdate(false);
        }
        public virtual Block createNew(Point location) {
            if (this == sand) {
                int i = 0;
            }
            return new Block(this, location);
        }
    }
}
