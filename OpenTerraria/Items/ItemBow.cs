using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTerraria.Entities;
using System.Drawing;
using OpenTerraria.Blocks;

namespace OpenTerraria.Items {
    [Serializable]
    public class ItemBow : ItemTool {
        public ItemBow() : base("Bow", "bow.png", "visualstudio2013", PickaxeType.PICKAXE_IRON) {

        }
        public override void use(ItemInInventory item) {
            ThrownStone stone = new ThrownStone(Util.subtractPoints(MainForm.getInstance().player.location, new Point(5, 5)), MainForm.getInstance().getCursorPos());
        }
    }
}
