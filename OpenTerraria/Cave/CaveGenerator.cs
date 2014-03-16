using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTerraria.Blocks;

namespace OpenTerraria.Cave {
    public class CaveGenerator {
        public static Random random = new Random();
        public static BlockPrototype[][] generateCaves(BlockPrototype[][] world) {
            int numCaves = random.Next(world.Count() / 10, world.Count() / 8);
            for (int i = 0; i < numCaves; i++) {
                try {
                    int xStart = random.Next(world.Count() - 50);
                    int xLength = random.Next(250);
                    if (xStart + xLength > world.Count()) {
                        i--;
                        continue;
                    }
                    int xEnd = xStart + xLength;
                    int y = 0;
                    while (world[xStart][y] != BlockPrototype.grass && world[xStart][y] != BlockPrototype.dirt) {
                        y++;
                    }
                    y += random.Next(15, world[xStart].Count() / 2);
                    for (int x = xStart; x <= xEnd; x++) {
                        int rand = random.Next(3);
                        if (rand == 2) { //There's a tendancy to go down
                            y--;
                        } else if(y == 1) {
                            y++;
                        }
                        int surface = 0;
                        for (int index = 0; index < world[x].Count(); index++) {
                            if (world[x][index] != BlockPrototype.air) {
                                surface = index;
                                break;
                            }
                        }
                        if (Math.Abs(y - surface) < 10) { //If it's within 10 blocks of the surface
                            y -= 2; //Make it go down.
                        }
                        for (int yNow = y - 2; yNow <= (y + 2); yNow++) {
                            world[x][yNow] = BlockPrototype.air;
                        }
                    }
                } catch (IndexOutOfRangeException e) {
                    continue; //It happens, no way around it. However, since we hit the bottom,
                              //we should discontinue this cave.
                }
            }
            return null;
        }
    }
}