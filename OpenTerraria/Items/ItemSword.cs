using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTerraria.Entities;
using System.Drawing;

namespace OpenTerraria.Items {
    [Serializable]
    public class ItemSword : ItemTool {
        public ItemSword() : base("Sword", "sword.png", "visualstudio2013") {
            
        }
        public override void use(ItemInInventory item) {
            Point locationStart = (MainForm.getInstance().getCursorWorldLocation().X > MainForm.getInstance().player.location.X) ? Util.addPoints(MainForm.getInstance().player.location, new Point(10, 0)) : Util.addPoints(MainForm.getInstance().player.location, new Point(-1, 0));
            EntitySword sword = new EntitySword(locationStart, MainForm.getInstance().player);
            sword.momentum.X = (MainForm.getInstance().getCursorWorldLocation().X > MainForm.getInstance().player.location.X) ? 5 : -5;
            if (MainForm.getInstance().entities.Contains(sword)) { //It's still there
                MainForm.getInstance().player.swordSide = (MainForm.getInstance().getCursorWorldLocation().X > MainForm.getInstance().player.location.X) ? "right" : "left";
            }
        }
    }
}
