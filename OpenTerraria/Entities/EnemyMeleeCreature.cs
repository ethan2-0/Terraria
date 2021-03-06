﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace OpenTerraria.Entities {
    [Serializable]
    public class EnemyMeleeCreature : Creature {
        public Entity entityBeingTracked = null;
        public EnemyMeleeCreature(String imageName, Point location, Size hitBox, int inventorySize, String name)
            : base(imageName, location, hitBox, inventorySize, name) {

        }
        public override void update() {
            base.update();
            aiTick();
        }
        public virtual void aiTick() {
            if (entityBeingTracked != null && getDistanceTo(entityBeingTracked) > 500) {
                entityBeingTracked = null;
            }
            int dist = getDistanceTo(MainForm.getInstance().player);
            if (entityBeingTracked == null && getDistanceTo(MainForm.getInstance().player) < 250) {
                entityBeingTracked = MainForm.getInstance().player;
            }
            if (entityBeingTracked != null) {
                if (Math.Abs(this.location.Y - entityBeingTracked.location.Y) > 40) {
                    jump();
                }
                if (location.X < entityBeingTracked.location.X) {
                    momentum.X = 3;
                }
                if (location.X > entityBeingTracked.location.X) {
                    momentum.X = -3;
                }
            }
        }
        public override int getMaxHealth() {
            return 100;
        }
    }
}
