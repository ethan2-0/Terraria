using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace OpenTerraria.Entities {
    public class BossWhale : Creature, Boss {
        public BossWhale(int x) : base("whale.png", new Point(x, 100), new Size(200, 60), 0, "Whale") {
            MainForm.getInstance().player.location = new Point(location.X - 100, 100);
        }
        public override int getMaxHealth() {
            return 2000;
        }
    }
}
