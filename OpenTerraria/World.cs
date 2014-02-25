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
                    world[i][j] = new Block(worldPrototype[i][j], new Point(i * 20, j * 20));
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
                for (int j = 0; j < height; j++) {
                    world[i][j] = BlockPrototype.grass;
                }
            }
            return world;
        }
        public void draw(Graphics g) {

        }
    }
}
