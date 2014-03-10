using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTerraria.Entities;
using System.Drawing;

namespace OpenTerraria.Items {
    public class ItemBow : ItemTool {
        public ItemBow() : base("Bow", "bow.png", "visualstudio2013") {

        }
        public override void use(ItemInInventory item) {
            ThrownStone stone = new ThrownStone(Util.subtractPoints(MainForm.getInstance().player.location, new Point(5, 5)), MainForm.getInstance().getCursorPos());
        }
    }
}
