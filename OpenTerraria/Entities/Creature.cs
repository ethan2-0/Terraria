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
                Reference.playSoundAsync("hit2.wav");
                DamageIndicator indicator = new DamageIndicator(location, amount.ToString());
                Particle.spawnParticlesAround(Util.addPoints(location, new Point(hitBox.Width / 2, hitBox.Height / 2)), Color.FromArgb(255, 255, 38, 41), amount / 2);
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
            Particle.spawnParticlesAround(Util.addPoints(location, new Point(hitBox.Width / 2, hitBox.Height / 2)), Color.FromArgb(255, 255, 38, 41), 50);
        }
        public virtual void jump() {
            if (ticksSinceOnGround <= 5) {
                momentum.Y = -8;
            }
        }
    }
}
