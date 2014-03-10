using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace OpenTerraria.Entities {
    public class ThrownStone : Entity {
        private Point occasionalMovement;
        /// <summary>
        /// Create a new ThrownStone.
        /// </summary>
        /// <param name="location">The starting location of the stone</param>
        /// <param name="thrownAt">The location, <b>from MainForm.getCursorPos()</b>, to throw the stone at.</param>
        public ThrownStone(Point location, Point thrownAt) : base("thrownStone.png", location, new Size(8, 8)) {
            double ratio = ((double)thrownAt.X) / ((double)thrownAt.Y);
            /*int throwspeed = 2;
            momentum.X = (int) (1 / ratio) * throwspeed;
            momentum.Y = (int)(Math.Abs(1 * ratio) * throwspeed);*/
            occasionalMovement = new Point();
            int throwspeed = 6;
            /*momentum.X = (int)((1 * ratio) * throwspeed);
            momentum.Y = -((int)((1 / ratio) * throwspeed));*/
            momentum.X = thrownAt.X / 120;
            momentum.Y = -(thrownAt.Y / 60);
        }
        public override void update() {
            base.update();
            bool hitSomething = false;
            foreach (Entity e in MainForm.getInstance().entities) {
                if (e == this) {
                    continue;
                }
                if (new Rectangle(e.location, e.hitBox).Contains(location)) {
                    if (e is Creature) {
                        hitSomething = true;
                        ((Creature)e).damage(20);
                    }
                }
            }
            if (hitSomething) {
                MainForm.getInstance().entities.Remove(this);
            }
        }
        public override void onCollision() {
            base.onCollision();
            MainForm.getInstance().entities.Remove(this);
        }
    }
}
