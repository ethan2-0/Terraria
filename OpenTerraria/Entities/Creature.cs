using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace OpenTerraria.Entities {
    [Serializable]
    public abstract class Creature : EntityWithInventory {
        public abstract int getMaxHealth();
        public int health;
        public String name;
        public Creature(String imageName, Point location, Size hitBox, int inventorySize, String name) : base(imageName, location, hitBox, inventorySize) {
            health = getMaxHealth();
            this.name = name;
        }
        public void damage(int amount) {
            health -= amount;
            if (amount > 0) {
                DamageIndicator indicator = new DamageIndicator(location, amount.ToString());
            }
        }
        public override void update() {
            base.update();
            if (blockY > MainForm.getInstance().world.height) {
                this.health -= 5;
            }
            if (this.health <= 0) {
                die();
            }
        }
        public virtual void die() {
            MainForm.getInstance().entities.Remove(this);
        }
        public virtual void jump() {
            if (isOnGround) {
                momentum.Y = -8;
            }
        }
    }
}
