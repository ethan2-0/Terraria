using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace OpenTerraria {
    public class Zombie : EnemyMeleeCreature {
        public Zombie(Point location) : base("zombie.png", location, new Size(16, 32), 0) {
            contactDamage = 5;
        }
    }
}
