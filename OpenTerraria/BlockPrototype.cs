using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace OpenTerraria {
    public class BlockPrototype : InventoryItem {
        public static BlockPrototype grass = new BlockPrototype("grass.png", "OpenTerraria:Grass", "Grass", true);
        public static BlockPrototype air = new BlockPrototype("air.png", "OpenTerraria:Air", "Air", false);
        /// <summary>
        /// This feild should <b>NEVER</b> be changed, or accessed directly. Use getID() instead.
        /// </summary>
        private String id;
        private Bitmap startingImage;
        public String name;
        public bool solid;
        /// <summary>
        /// Create a new BlockPrototype with the specified image path and name.
        /// </summary>
        /// <param name="imageName">The image path, including .png. For example, grass.png.</param>
        /// <param name="id">The ID of the block. For example, OpenTerraria:Grass. Case sensitive.</param>
        public BlockPrototype(String imageName, String id, String name, bool solid) {
            this.id = id;
            this.startingImage = Reference.getImage(imageName);
            this.name = name;
            this.solid = solid;
        }
        public String getID() {
            return id;
        }
        public Bitmap getStartingImage() {
            return startingImage;
        }
        public override string ToString() {
            return "{BlockPrototype, ID=" + id + ", starting name=" + name + "}";
        }

    }
}
