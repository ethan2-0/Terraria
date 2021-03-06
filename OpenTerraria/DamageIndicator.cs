﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace OpenTerraria {
    public class DamageIndicator {
        public String amount;
        public Point location;
        public int ticksLived = 0;
        public const int ticksToLive = 5;
        public Color color;
        public DamageIndicator(Point location, String amount) {
            color = Color.FromArgb(MainForm.random.Next(255), MainForm.random.Next(255), MainForm.random.Next(255));
            MainForm.getInstance().damageIndicators.Add(this);
            this.location = Util.addPoints(location, new Point(MainForm.random.Next(40) - 20, MainForm.random.Next(40) - 20));
            this.amount = amount;
        }
        public virtual void tick() {
            ticksLived++;
            if (ticksLived > ticksToLive) {
                MainForm.getInstance().damageIndicators.Remove(this);
            }
            location.Y--;
        }
        public virtual void draw(Graphics g) {
            g.DrawString(amount, MainForm.getNormalFont(10), MainForm.createBrush(color), Util.subtractPoints(location, MainForm.getInstance().viewOffset));
        }
    }
}
