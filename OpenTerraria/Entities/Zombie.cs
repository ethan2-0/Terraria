using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using OpenTerraria.Items;

namespace OpenTerraria.Entities {
    [Serializable]
    public class Zombie : EnemyMeleeCreature {
        public Zombie(Point location) : base("zombie.png", location, new Size(16, 32), 0) {
            contactDamage = 4;
        }
        public override void die() {
            base.die();
            MainForm.getInstance().player.inventory.addItem(ItemTemplate.wool.createNew(), 1);
        }
    }
}
