using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace OpenTerraria {
    public class Player : Creature {
        public Player(Point location) : base("player.png", location, new Size(20, 40)) {
            MainForm.getInstance().KeyDown += new System.Windows.Forms.KeyEventHandler(Player_KeyDown);
            MainForm.getInstance().KeyUp += new System.Windows.Forms.KeyEventHandler(Player_KeyUp);
            MainForm.getInstance().KeyPress += new KeyPressEventHandler(Player_KeyPress);
        }

        void Player_KeyPress(object sender, KeyPressEventArgs e) {
            if (e.KeyChar == (char)Keys.Space && isOnGround) {
                momentum.Y = -15;
            }
        }

        void Player_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e) {
            if (e.KeyCode == Keys.Left || e.KeyCode == Keys.Right) {
                momentum.X = 0;
            }
        }

        void Player_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e) {
            if (e.KeyCode == Keys.Left) {
                momentum.X = -7;
            } else if (e.KeyCode == Keys.Right) {
                momentum.X = 7;
            }
        }
        public override int getMaxHealth() {
            return 20;
        }
    }
}
