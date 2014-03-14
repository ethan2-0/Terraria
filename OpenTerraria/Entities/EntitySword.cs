using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace OpenTerraria.Entities {
    public class EntitySword : Entity {
        private List<Entity> entitiesHurt;
        public Entity thrownBy;
        public int ticksLived = 0;
        public const int ticksToLive = 5;
        public EntitySword(Point location, Entity thrownBy)
            : base("air.png", location, new System.Drawing.Size(10, 40)) {
                entitiesHurt = new List<Entity>();
                this.thrownBy = thrownBy;
                bool shouldRemove = false;
                foreach (Entity e in MainForm.getInstance().entities) {
                    if (e is EntitySword && e != this) {
                        shouldRemove = true;
                    }
                }
                if (shouldRemove) {
                    MainForm.getInstance().entities.Remove(this);
                }
        }
        public override void move() {
            ticksLived++;
            if (ticksLived > ticksToLive) {
                MainForm.getInstance().entities.Remove(this);
                MainForm.getInstance().player.swordSide = "";
            }
            if (!isOnGround) {
                //Counteract gravity
                if (occasionalTicks >= 1) {
                    momentum.Y--;
                }
            }
            base.move();
            foreach (Entity e in MainForm.getInstance().entities) {
                if (e is Creature) {
                    Creature c = (Creature)e;
                    if (new Rectangle(location, new Size(hitBox.Width, hitBox.Height + 10)).Contains(Util.addPoints(c.location, new Point(0, 20))) && !entitiesHurt.Contains(c) && thrownBy != c) {
                        c.damage(40);
                        entitiesHurt.Add(c);
                        c.momentumLock.X = this.momentum.X;
                        c.momentum.Y = 0;
                        c.momentumLockTicks = 5;
                    }
                }
            }
        }
    }
}
