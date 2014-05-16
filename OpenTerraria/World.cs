using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using OpenTerraria.Blocks;
using OpenTerraria.Entities;
using OpenTerraria.Cave;

namespace OpenTerraria {
    [Serializable]
    public class World {
        public static Dictionary<BlockPrototype, int> veinBlocks = new Dictionary<BlockPrototype, int>();
        public Block[][] blocks;
        /// <summary>
        /// A list of blocks. Note that it is not 100% accurate (although close).
        /// </summary>
        public List<Block> blockList;
        public int width, height;
        /// <summary>
        /// This constructor should only be used internally. Use World.newWorld() instead.
        /// </summary>
        private World(int width, int height, Block[][] blocks) {
            this.blockList = new List<Block>();
            this.blocks = blocks;
            this.width = width;
            this.height = height;
        }
        public static World createWorld(int width, int height) {
            BlockPrototype[][] worldPrototype = generateWorld(width, height);
            Block[][] world = new Block[width][];
            List<Block> blockList = new List<Block>();
            for (int i = 0; i < width; i++) {
                world[i] = new Block[height];
                for (int j = 0; j < height; j++) {
                    Block b = Block.createNewBlock(worldPrototype[i][j], new Point(i * 20, j * 20));
                    world[i][j] = b;
                    blockList.Add(b);
                }
            }
            World w = new World(width, height, world);
            w.blockList = blockList;
            return w;
        }
        private static BlockPrototype[][] generateWorld(int width, int height) {
            BlockPrototype[][] world = new BlockPrototype[width][];
            for (int i = 0; i < width; i++) {
                world[i] = new BlockPrototype[height];
            }
            /*for (int i = 0; i < width; i++) {
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
            }*/
            int level = 250;
            List<int> results = new List<int>();
            Random random = new Random();
            int index = 0;
            int timesSinceLastTree = 0;
            int sandTime = 0;
            foreach (BlockPrototype[] blocks in world) {
                index++;
                int rand = random.Next(18);
                results.Add(rand);
                if (rand <= 4) {
                    level++;
                } else if (rand > 11) {
                    level--;
                } //Otherwise, make no change.
                if (level > 450) {
                    level = 345;
                }
                if (level < 20) {
                    level = 40;
                }
                if (random.Next(20) > 18) {
                    sandTime = (random.Next(10));
                }
                if (sandTime > 0) {
                    sandTime--;
                    blocks[level] = BlockPrototype.sand;
                } else {
                    blocks[level] = BlockPrototype.grass;
                }
                int blocksDown = 0;
                for (int i = level; i < height; i++) {
                    blocksDown++;
                    if (blocksDown == 1) {
                        //Possibility of generating tree
                        //Tore it out, replacing with better algorithm (again)
                        /*blocks[i] = BlockPrototype.grass;
                        if (random.Next(40) > 35) {
                            blocks[i] = BlockPrototype.log;
                            int treeHeight = random.Next(6, 20);
                            for (int j = i - 1; j > (i - treeHeight); j--) {
                                blocks[j] = BlockPrototype.log;
                                if (random.Next(10) > 7) {
                                    if(index > (width - 5)) {
                                        continue;
                                    }
                                    world[index][j] = BlockPrototype.log;
                                }
                                if (random.Next(10) < 3) {
                                    try {
                                        world[index - 1][j] = BlockPrototype.log;
                                    } catch (Exception e) {
                                        //It happens
                                    }
                                }
                            }
                        }*/
                        //Possibility of generating a tree
                        try {
                            if (random.Next(40) > 36) {
                                if (timesSinceLastTree > 8) {
                                    char[][] tree = TreeGenerator.getRandomTree();
                                    for (int j = 0; j < tree.Count(); j++) {
                                        for (int k = 0; k < tree[j].Count(); k++) {
                                            if (tree[j][k] == ' ') {
                                                continue;
                                            } else if (tree[j][k] == 'g') {
                                                world[index + j][i - k] = BlockPrototype.leaves;
                                            } else if (tree[j][k] == 'L') {
                                                world[index + j][i - k] = BlockPrototype.log;
                                            }
                                        }
                                    }
                                }
                                timesSinceLastTree = 0;
                            } else {
                                timesSinceLastTree++;
                            }
                        } catch (IndexOutOfRangeException e) {
                            //It happens, but we'll end up with a pretty messed up tree
                        }
                    } else if (sandTime > 0 && blocksDown < 4 && random.Next(100) > 20) {
                        blocks[i] = BlockPrototype.sand;
                    } else if (blocksDown < 8) {
                        blocks[i] = BlockPrototype.dirt;
                    } else {
                        /*if (random.Next(40) > 38) {
                            blocks[i] = BlockPrototype.oreCoal;
                        } else if (random.Next(100) > 98) {
                            blocks[i] = BlockPrototype.ironOre;
                        } else {*/
                        blocks[i] = BlockPrototype.stone;
                        //}
                    }
                }
                for (int i = 0; i < blocks.Count(); i++) {
                    if (blocks[i] == null) {
                        blocks[i] = BlockPrototype.air;
                    }
                }
                
                /*for (int i = height - 1; i > 0; i--) {
                    if (blocks[i] == BlockPrototype.air && random.Next(100) == 3) {
                        int treeHeight = 6;
                        for (int j = i + i; j > (i - treeHeight); j--) {
                            if (j >= height) {
                                continue;
                            }
                            blocks[j] = BlockPrototype.log;
                            if (random.Next(4) > 3) {
                                world[index + 1][j] = BlockPrototype.log;
                            }
                            if (random.Next(4) < 2) {
                                world[index - 1][j] = BlockPrototype.log;
                            }
                        }
                    }
                    if (blocks[i] == BlockPrototype.air) {
                        break;
                    }
                }*/
            }
            /*int ind = 0; //Standing for index, but apparently index is already defined
            foreach (BlockPrototype[] blocks in world) { //Generate leaves
                ind++;
                for (int i = 0; i < blocks.Count(); i++) {
                    try {
                        if (world[ind][i] == BlockPrototype.log && random.Next(4) > 2 && world[ind - 2][i] == BlockPrototype.air) {
                            blocks[i] = BlockPrototype.leaves;
                        }
                        if (world[ind - 2][i] == BlockPrototype.log && random.Next(4) > 2 && world[ind][i] == BlockPrototype.air) {
                            blocks[i] = BlockPrototype.leaves;
                        }
                    } catch (Exception e) {
                        //Do nothing, it happens
                    }
                }
            }
            ind = 0;
            foreach (BlockPrototype[] blocks in world) { //Generate leaves for the second time
                ind++;
                for (int i = 0; i < blocks.Count(); i++) {
                    try {
                        if ((world[ind][i] == BlockPrototype.log || world[ind][i] == BlockPrototype.leaves) && random.Next(4) > 2 && world[ind][i] == BlockPrototype.air) {
                            blocks[i] = BlockPrototype.leaves;
                        }
                        if ((world[ind - 2][i] == BlockPrototype.log || world[ind - 2][i] == BlockPrototype.leaves) && random.Next(4) > 2 && world[ind - 2][i] == BlockPrototype.air) {
                            blocks[i] = BlockPrototype.leaves;
                        }
                    } catch (Exception e) {
                        //Do nothing, it happens
                    }
                }
            }*/
            /*if (false) { //Ore generation is disabled
                veinBlocks.Add(BlockPrototype.oreCoal, 3);
                veinBlocks.Add(BlockPrototype.dirt, 10);
                veinBlocks.Add(BlockPrototype.ironOre, 2);
                int ind = 0;
                LoadingForm lf = new LoadingForm();
                lf.Show();
                int blocksToGo = world.Count();
                foreach (BlockPrototype[] blocks in world) {
                    lf.setProgress("Generating Ores...", (int)((((double)ind) / ((double)blocksToGo)) * 100D));
                    ind++;
                    if (ind % 50 != 0) {
                        continue;
                    }
                    for (int i = 0; i < blocks.Count(); i++) {
                        if (CaveGenerator.random.Next(200) < 199) {
                            continue;
                        }
                        BlockPrototype generatedOre = null;
                        int num = CaveGenerator.random.Next(veinBlocks.Count);
                        int k = 0;
                        foreach (KeyValuePair<BlockPrototype, int> pair in veinBlocks) {
                            k++;
                            if (k >= num) {
                                generatedOre = pair.Key;
                                break;
                            }
                        }
                        if (generatedOre == null) {
                            continue;
                        }
                        if (blocks[i] == BlockPrototype.stone) {
                            for (int j = 0; j < veinBlocks[generatedOre]; j++) {
                                int xDiff = CaveGenerator.random.Next(20) - 10;
                                int yDiff = CaveGenerator.random.Next(10) - 5;
                                try {
                                    //if (i < 2 || world[ind + xDiff - 1][i + yDiff] == generatedOre || world[ind + xDiff + 1][i + yDiff] == generatedOre || world[ind + xDiff][i + yDiff - 1] == generatedOre || world[ind + xDiff][i + yDiff + 1] == generatedOre) {
                                    world[ind + xDiff][i + yDiff] = generatedOre;
                                    /*} else {
                                        j--;
                                        continue;
                                    }* / //<-- I made this not an uncomment for technical reasons
                                } catch (IndexOutOfRangeException e) {
                                    break;
                                }
                            }
                        }
                        i += 25;
                    }
                }
            }*/
            /*veinBlocks.Add(BlockPrototype.oreCoal, 3);
            veinBlocks.Add(BlockPrototype.dirt, 10);
            veinBlocks.Add(BlockPrototype.ironOre, 2);
            for (int i = 0; i < world.Count(); i++) {
                for (int j = 0; j < world[i].Count(); j++) {
                    if (random.Next(100) > 98) {
                        //Decide which block to place
                        int total = 0;
                        foreach (BlockPrototype prototype in veinBlocks) {

                        }
                    }
                }
            }*/
            CaveGenerator.generateCaves(world);
            return world;
        }
        public void updateSkyLighting() {
            for (int i = 0; i < blocks.Count(); i++) {
                updateSkyLightForColumn(i);
            }
        }
        public void updateSkyLightForColumn(int across) {
            Block[] column = blocks[across];
            int intensity = 30;
            for (int i = 0; i < column.Count(); i++) {
                if (!column[i].prototype.lightBlocking) {
                    column[i].setLightLevel(intensity);
                } else {
                    intensity -= 6;
                    if (intensity <= 0) {
                        break;
                    } else {
                        column[i].setLightLevel(intensity);
                        column[i].setEmittedLightLevel(intensity);
                    }
                }
            }
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
            Block b = getBlockAt((int)Math.Ceiling((double)x / 20), (int)Math.Ceiling((double)y / 20));
            return b;
        }
        public bool isInsideBlock(int x, int y) {
            if (isInsideBlockPhysical(x, y)) {
                return true;
            }
            Player player = MainForm.getInstance().player;
            if (isInsideBlockPhysical(x + 6, y - player.hitBox.Height + 12)) {
                return true;
            }
            if (isInsideBlockPhysical(x - player.hitBox.Width + 6, y + 12)) {
                return true;
            }
            if (isInsideBlockPhysical(x - player.hitBox.Width + 6, y - player.hitBox.Height + 12)) {
                return true;
            }
            return false;
        }
        public bool isInsideBlockPhysical(int x, int y) {
            Block block = getBlockAtPixels(x, y);
            MainForm instance = MainForm.getInstance();
            if (block != null) {
                BlockPrototype prototype = block.getPrototype();
                return prototype.isSolid();
            } else {
                return false;
            }
        }
    }
}
