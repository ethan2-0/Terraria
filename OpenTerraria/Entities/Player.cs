using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using OpenTerraria.Items;
using OpenTerraria.Blocks;

namespace OpenTerraria.Entities {
    public class Player : Creature, HandlerForEvent {
        public Inventory hotbar;
        public int hotbarSelectedIndex = 1;
        public String swordSide = "";
        public Player(Point location) : base("player.png", location, new Size(20, 40), 40) {
            MainForm.getInstance().KeyDown += new System.Windows.Forms.KeyEventHandler(Player_KeyDown);
            MainForm.getInstance().KeyUp += new System.Windows.Forms.KeyEventHandler(Player_KeyUp);
            MainForm.getInstance().KeyPress += new KeyPressEventHandler(Player_KeyPress);
            MainForm.getInstance().drawEventDispatcher.registerHandler(this);
            hotbar = new Inventory(10);
            hotbar.addItem(ItemTool.createPickaxe(), 1);
            hotbar.addItem(new ItemBow(), 1);
            hotbar.addItem(new ItemSword(), 1);
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
            if (swordSide == "left") {
                g.DrawImage(Reference.getImage("swordBig.png"), Util.subtractPoints(Util.subtractPoints(location, new Point(5, 0)), MainForm.getInstance().viewOffset));
            }
            if (swordSide == "right") {
                g.DrawImage(Reference.getImage("swordBig.png"), Util.subtractPoints(Util.addPoints(location, new Point(15, 0)), MainForm.getInstance().viewOffset));
            }
        }
    }
}
