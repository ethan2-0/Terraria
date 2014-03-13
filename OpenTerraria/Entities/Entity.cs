using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace OpenTerraria.Entities {
    public abstract class Entity {
        public Bitmap image;
        public Point location;
        public int blockX, blockY;
        public Point momentum;
        public Size hitBox;
        public bool isOnGround = false;
        public int occasionalTicks = 0;
        public int contactDamage = 0;
        public Entity(String imageName, Point location, Size hitBox) {
            this.image = Reference.getImage(imageName);
            this.location = location;
            this.hitBox = hitBox;
            momentum = new Point(0, 0);
            MainForm.getInstance().entities.Add(this);
        }
        public virtual void draw(Graphics g) {
            g.DrawImage(image, Util.subtractPoints(location, MainForm.getInstance().viewOffset));
        }
        public virtual void move() {
            //Collisions
            int candaditeX = location.X + momentum.X;
            int candaditeY = location.Y + momentum.Y;
            bool collided = false;
            bool isInsideBlock = MainForm.getInstance().world.isInsideBlock(candaditeX, location.Y);
            if (isInsideBlock) {
                candaditeX = location.X;
                collided = true;
            }
            isInsideBlock = MainForm.getInstance().world.isInsideBlock(candaditeX, candaditeY);
            isOnGround = false;
            if (isInsideBlock) {
                candaditeY = location.Y;
                isOnGround = true;
                collided = true;
            }
            if (collided) {
                onCollision();
            }
            //Update pixel location
            location = new Point(candaditeX, candaditeY);
            //Update block location
            blockX = (int) Math.Floor((double) location.X / 20);
            blockY = (int) Math.Floor((double) location.Y / 20);
        }
        public void stop() {
            momentum = new Point(0, 0);
        }
        public virtual void update() {
            //Occasional ticks
            occasionalTicks++;
            if (occasionalTicks >= 2) {
                occasionalTicks = 0;
                //Gravity
                if (!isOnGround) {
                    momentum.Y++;
                }
                //Contact damage
                foreach (Entity e in MainForm.getInstance().entities) {
                    if (e == this || (this is EnemyMeleeCreature && e is EnemyMeleeCreature)) {
                        continue;
                    }
                    if (e.getDistanceTo(this) < 20 ) {
                        if (e is Creature) {
                            ((Creature)e).damage(contactDamage);
                        }
                    }
                }
            }
            //Movement
            move();
            
        }
        public override string ToString() {
            return "{Entity, Type=" + this.GetType().ToString() + ", Location={X=" + location.X + ", Y=" + location.Y + "}, BlockLocation={X=" + blockX + ", Y=" + blockY + "}}";
        }
        public int getDistanceTo(Point p) {
            return (int) Math.Sqrt(Math.Abs(Math.Abs((location.X - p.X) * (location.X - p.X)) + Math.Abs((location.Y - p.Y) * (location.Y - p.Y))));
        }
        public int getDistanceTo(Entity entity) {
            return getDistanceTo(entity.location);
        }
        public virtual void onCollision() {

        }
    }
}
