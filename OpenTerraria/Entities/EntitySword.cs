﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace OpenTerraria.Entities {
    public class EntitySword : Entity {
        private List<Entity> entitiesHurt;
        public Entity thrownBy;
        public int ticksLived = 0;
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
            if (ticksLived > 5) {
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
                    if (new Rectangle(location, hitBox).Contains(c.location) && !entitiesHurt.Contains(c) && thrownBy != c) {
                        c.damage(40);
                        entitiesHurt.Add(c);
                    }
                }
            }
        }
    }
}
