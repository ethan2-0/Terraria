using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

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
            for (int i = 0; i < width; i++) {
                for (int j = 0; j < height; j++) {
                    getBlockAt(i, j).draw(g);
                }
            }
        }
        public Block getBlockAt(int x, int y) {
            return blocks[x][y];
        }
        public Block getBlockAtPixels(int x, int y) {
            Block b = getBlockAt((int) Math.Ceiling((double) x / 20), (int) Math.Ceiling((double) y / 20));
            return b;
        }
        public bool isInsideBlock(int x, int y) {
            BlockPrototype prototype = getBlockAtPixels(x, y).getPrototype();
            return prototype.solid;
        }
    }
}
