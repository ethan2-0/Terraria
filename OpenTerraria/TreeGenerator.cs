using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenTerraria {
    public class TreeGenerator {
        public static List<char[][]> trees;
        private static Random random;
        //In trees, L is log and g is leaves. Space has no effect on anything.
        //Note that trees can be of any height.
        public static char[][] tree1 = new char[][] {
            new char[]{' ',' ',' ',' ','g',' ',' ',' ',' '},
            new char[]{' ',' ',' ','g','L','g',' ',' ',' '},
            new char[]{' ',' ','g','L','L','g','g',' ',' '},
            new char[]{' ',' ','L','g','L','g',' ',' ',' '},
            new char[]{' ',' ',' ','g','L','g','g',' ',' '},
            new char[]{' ',' ','g','g','L','L','L','g',' '},
            new char[]{' ',' ','g','L','L','g','g',' ',' '},
            new char[]{' ',' ','g','g','L','g',' ',' ',' '},
            new char[]{' ',' ',' ','g','L','g',' ',' ',' '},
            new char[]{' ',' ',' ','g','L','L',' ',' ',' '}
        };
        public static char[][] tree2 = new char[][] {
            new char[]{' ',' ',' ','g','g','g',' ',' ',' '},
            new char[]{' ',' ','g','L','L','g',' ',' ',' '},
            new char[]{' ',' ',' ','g','L','g','g',' ',' '},
            new char[]{' ',' ','g','g','L','g','g','g',' '},
            new char[]{' ',' ','g','L','L','L','L','g','g'},
            new char[]{' ',' ','g','L','L','g','g','g',' '},
            new char[]{' ',' ',' ','g','L','g',' ',' ',' '},
            new char[]{' ',' ',' ','g','L','L','g',' ',' '},
            new char[]{' ',' ',' ','L','L','g',' ',' ',' '},
            new char[]{' ',' ','g','g','L','g',' ',' ',' '},
            new char[]{' ',' ',' ','g','L','g',' ',' ',' '},
            new char[]{' ',' ',' ','g','L','L',' ',' ',' '}
        };
        static TreeGenerator() {
            trees = new List<char[][]>();
            trees.Add(processTree(tree1));
            trees.Add(processTree(tree2));
            random = new Random();
        }
        public static char[][] processTree(char[][] tree) {
            char[][] newTree = new char[tree[0].Count()][];
            for (int i = 0; i < newTree.Count(); i++) {
                newTree[i] = new char[tree.Count()];
            }
            int count = tree.Count();
            for (int i = 0; i < tree.Count(); i++) {
                for (int j = 0; j < tree[i].Count(); j++) {
                    newTree[j][newTree[j].Count() - i - 1] = tree[i][j];
                }
            }
            return newTree;
        }
        public static char[][] getRandomTree() {
            return trees[random.Next(trees.Count)];
        }
    }
}
