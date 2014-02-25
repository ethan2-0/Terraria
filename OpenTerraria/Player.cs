using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenTerraria {
    public class Player : Creature {
        public Player() {
        }
        public override int getMaxHealth() {
            return 20;
        }
    }
}
