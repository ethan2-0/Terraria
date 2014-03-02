using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace OpenTerraria {
    public class World {
        public Block[][] blocks;
        int width, height;
        /// <summary>
        /// This constructor should only be used internally. Use World.newWorld() instead.
        /// </summary>
        private World(int width, int height, Block[][] blocks) {
            this.blocks = blocks;
            this.width = width;
            this.height = height;
        }
        public static World createWorld(int width, int height) {
            BlockPrototype[][] worldPrototype = generateWorld(width, height);
            Block[][] world = new Block[width][];
            for (int i = 0; i < width; i++) {
                world[i] = new Block[height];
                for (int j = 0; j < height; j++) {
                    world[i][j] = Block.createNewBlock(worldPrototype[i][j], new Point(i * 20, j * 20));
                }
            }
            World w = new World(width, height, world);
            return w;
        }
        private static BlockPrototype[][] generateWorld(int width, int height) {
            BlockPrototype[][] world = new BlockPrototype[width][];
            for (int i = 0; i < width; i++) {
                world[i] = new BlockPrototype[height];
            }
            for (int i = 0; i < width; i++) {
                for (int j = 10; j < height; j++) {
                    world[i][j] = BlockPrototype.grass;
                }
            }
            for (int i = 0; i < width; i++) {
                for (int j = 0; j < height; j++) {
                    if (world[i][j] == null) {
                        world[i][j] = BlockPrototype.air;
                    }
                }
            }
            return world;
        }
        public void draw(Graphics g) {
            int startX = 0;//MainForm.getInstance().player.blockX - 20;
            int endX = width;//MainForm.getInstance().player.blockX + 20;
            //MessageBox.Show("Starting to loop.");
            for (int i = startX; i < endX; i++) {
                for (int j = 0; j < height; j++) {
                    Block b = getBlockAt(i, j);
                    if (b != null) {
                        b.draw(g);
                    }
                }
            }
            //MessageBox.Show("Done looping");
        }
        public Block getBlockAt(int x, int y) {
            try {
                return blocks[x][y];
            } catch (Exception e) {
                return null;
            }
        }
        public Block getBlockAtPixels(int x, int y) {
            Block b = getBlockAt((int) Math.Ceiling((double) x / 20), (int) Math.Ceiling((double) y / 20));
            return b;
        }
        public bool isInsideBlock(int x, int y) {
            Block block = getBlockAtPixels(x, y);
            if (block != null) {
                BlockPrototype prototype = block.getPrototype();
                return prototype.solid;
            } else {
                return false;
            }
        }
    }
}
