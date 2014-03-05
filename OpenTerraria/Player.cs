﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace OpenTerraria {
    public class Player : Creature {
        public Inventory hotbar;
        public int hotbarSelectedIndex;
        public Player(Point location) : base("player.png", location, new Size(20, 40), 40) {
            MainForm.getInstance().KeyDown += new System.Windows.Forms.KeyEventHandler(Player_KeyDown);
            MainForm.getInstance().KeyUp += new System.Windows.Forms.KeyEventHandler(Player_KeyUp);
            MainForm.getInstance().KeyPress += new KeyPressEventHandler(Player_KeyPress);
            MainForm.getInstance().MouseClick += new MouseEventHandler(Player_MouseClick);
            inventory.addItem(BlockPrototype.grass, 50);
            inventory.items[9] = new ItemInInventory(BlockPrototype.grass, 6);
            inventory.items[8] = new ItemInInventory(BlockPrototype.grass, 6);
            hotbar = new Inventory(10);
            hotbar.addItem(BlockPrototype.grass, 5);
            hotbar.items[4] = new ItemInInventory(BlockPrototype.grass, 6);
        }

        void Player_MouseClick(object sender, MouseEventArgs e) {
            hotbar.items[hotbarSelectedIndex].use();
        }
        void Player_KeyPress(object sender, KeyPressEventArgs e) {
            if (e.KeyChar == (char)Keys.Space) {
                jump();
            }
        }

        void Player_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e) {
            if (e.KeyCode == Keys.Left || e.KeyCode == Keys.Right) {
                momentum.X = 0;
            }
        }

        void Player_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e) {
            if (e.KeyCode == Keys.Left) {
                momentum.X = -8;
            } else if (e.KeyCode == Keys.Right) {
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
    }
}
