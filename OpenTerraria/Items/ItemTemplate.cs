﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenTerraria.Items {
    [Serializable]
    public class ItemTemplate {
        #region Declaring constant items
        public static ItemTemplate stick = new ItemTemplate("Stick", "stick.png", 255);
        public static ItemTemplate wool = new ItemTemplate("Strand of Wool", "wool.png", 255);
        public static ItemTemplate ironBar = new ItemTemplate("Iron Bar", "ironBar.png", 99);
        #endregion
        public String name;
        public String imagename;
        public int maxStack;
        private TemplatedItem item;
        public ItemTemplate(String name, String imagename, int maxStack) {
            this.name = name;
            this.imagename = imagename;
            this.maxStack = maxStack;
            item = new TemplatedItem(name, imagename, maxStack);
        }
        public TemplatedItem createNew() {
            return item;
        }
    }
}
