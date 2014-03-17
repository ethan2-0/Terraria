using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using OpenTerraria.Items;
using OpenTerraria.Blocks;
using System.Drawing.Drawing2D;

namespace OpenTerraria.Entities {
    [Serializable]
    public class Player : Creature, HandlerForEvent {
        public Inventory hotbar;
        public int hotbarSelectedIndex = 1;
        public String swordSide = "";
        public Player(Point location) : base("player.png", location, new Size(20, 40), 40) {
            registerHandlers();
            hotbar = new Inventory(10);
            hotbar.addItem(ItemTool.createPickaxe(), 1);
            hotbar.addItem(new ItemBow(), 1);
            hotbar.addItem(new ItemSword(), 1);
            hotbar.addItem(BlockPrototype.torch, 50);
            hotbar.addItem(BlockPrototype.furnace, 1);
        }
        public void registerHandlers() {
            MainForm.getInstance().KeyDown += new System.Windows.Forms.KeyEventHandler(Player_KeyDown);
            MainForm.getInstance().KeyUp += new System.Windows.Forms.KeyEventHandler(Player_KeyUp);
            MainForm.getInstance().KeyPress += new KeyPressEventHandler(Player_KeyPress);
            MainForm.getInstance().drawEventDispatcher.registerHandler(this);
        }
        void Player_KeyPress(object sender, KeyPressEventArgs e) {
            if (e.KeyChar == (char)Keys.Space) {
                jump();
            }
        }

        void Player_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e) {
            if (e.KeyCode == Keys.Left || e.KeyCode == Keys.Right || e.KeyCode == Keys.D || e.KeyCode == Keys.A) {
                momentum.X = 0;
            }
        }

        void Player_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e) {
            if (e.KeyCode == Keys.Left || e.KeyCode == Keys.A) {
                momentum.X = -8;
            } else if (e.KeyCode == Keys.Right || e.KeyCode == Keys.D) {
                momentum.X = 8;
            }
        }
        public override int getMaxHealth() {
            return 100;
        }
        public override void die() {
            base.die();
            MessageBox.Show("You dead.");
            MainForm.getInstance().respawnPlayer();
        }
        public void handle(EventDispatcher dispatcher) {
            if (dispatcher == MainForm.getInstance().drawEventDispatcher) {
                
            }
        }
        public override void draw(Graphics g) {
            base.draw(g);
            Point renderLocation = Util.subtractPoints(location, MainForm.getInstance().viewOffset);
            EntitySword currentSword = null;
            foreach (Entity e in MainForm.getInstance().entities) {
                if (e is EntitySword) {
                    currentSword = (EntitySword)e;
                }
            }
            if (currentSword != null) {
                Matrix m = new Matrix();
                float angle = ((float) currentSword.ticksLived) / ((float) EntitySword.ticksToLive) * 180F;
                angle /= 2;
                if (swordSide == "left") {
                    angle = 360 - angle;
                }
                m.RotateAt(angle, Util.addPoints(renderLocation, new Point((swordSide == "left" ? 5 : 16), (swordSide == "left" ? 20 : 13))));
                g.Transform = m;
                if (swordSide == "left") {
                    g.DrawImage(Reference.getImage("sword.png"), Util.subtractPoints(renderLocation, new Point(5, 0)));
                }
                if (swordSide == "right") {
                    g.DrawImage(Reference.getImage("sword.png"), Util.addPoints(renderLocation, new Point(15, -5)));
                }
                g.Transform = new Matrix();
            }
        }
    }
}
