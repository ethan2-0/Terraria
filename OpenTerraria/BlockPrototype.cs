using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace OpenTerraria {
    public class BlockPrototype {
        /// <summary>
        /// This feild should <b>NEVER</b> be changed, or accessed directly. Use getID() instead.
        /// </summary>
        private String id;
        private Bitmap startingImage;
        public String name;
        /// <summary>
        /// Create a new BlockPrototype with the specified image path and name.
        /// </summary>
        /// <param name="imageName">The image path, including .png. For example, grass.png.</param>
        /// <param name="id">The ID of the block. For example, OpenTerraria:Grass. Case sensitive.</param>
        public BlockPrototype(String imageName, String id, String name) {
            this.id = id;
            this.startingImage = Reference.getImage(imageName);
            this.name = name;
        }
        public String getID() {
            return id;
        }
        public Bitmap getStartingImage() {
            return startingImage;
        }
    }
}
