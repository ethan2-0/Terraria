using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using OpenTerraria.Items;

namespace OpenTerraria {
    public class CraftingManager {
        public List<Recepie> recepies;
        ListBox listbox;
        private bool hasAddedToContainer = false;
        private Label craftLabel;
        public CraftingManager(List<Recepie> recepies) {
            this.recepies = recepies;
            listbox = new ListBox();
            listbox.Items.AddRange(createListOfItemNames().ToArray());
            listbox.Visible = false;
            listbox.Width = 100;
            listbox.Height = 400;
            listbox.Font = MainForm.getNormalFont(10);
            listbox.ForeColor = Color.DarkOrange;
            listbox.BackColor = Color.Black;
            listbox.BorderStyle = BorderStyle.FixedSingle;
            listbox.MouseWheel += new MouseEventHandler(listbox_MouseWheel);
            craftLabel = new Label();
            craftLabel.Text = "Craft";
            craftLabel.AutoSize = false;
            craftLabel.TextAlign = ContentAlignment.MiddleCenter;
            craftLabel.Width = 75;
            craftLabel.Height = 35;
            craftLabel.BackColor = Color.DarkBlue;
            craftLabel.ForeColor = Color.DarkOrange;
            craftLabel.Font = MainForm.getNormalFont(12);
        }

        void listbox_MouseWheel(object sender, MouseEventArgs e) {
            MainForm.getInstance().MainForm_MouseWheel(sender, e);
        }
        private List<String> createListOfItemNames() {
            List<String> items = new List<String>();
            foreach (Recepie r in recepies) {
                items.Add(r.output.Value + " X " + r.output.Key.getName());
            }
            return items;
        }
        public void render(Graphics g, Point location) {
            if (!hasAddedToContainer) {
                hasAddedToContainer = true;
                MainForm.getInstance().Controls.Add(listbox);
                MainForm.getInstance().Controls.Add(craftLabel);
                craftLabel.Click += new EventHandler(craftLabel_Click);
            }
            g.FillRectangle(MainForm.createBrush(Reference.guiColor), new Rectangle(location, new Size(215, 410)));
            listbox.Visible = true;
            craftLabel.Visible = true;
            listbox.Location = Util.addPoints(location, new Point(5, 5));
            craftLabel.Location = Util.addPoints(location, new Point(120, 365));
            if (listbox.SelectedIndex != -1) {
                String s = "Ingredients:\n";
                foreach (KeyValuePair<InventoryItem, int> pair in recepies[listbox.SelectedIndex].input) {
                    s += pair.Value + " X " + pair.Key.getName();
                }
                g.DrawString(s, MainForm.getNormalFont(10), MainForm.createBrush(Color.DarkOrange), Util.addPoints(location, new Point(105, 5)));
            }
        }

        void craftLabel_Click(object sender, EventArgs e) {
            if (listbox.SelectedIndex == -1) {
                return;
            }
            MainForm mainform = MainForm.getInstance();
            Recepie currentRecepie = recepies[listbox.SelectedIndex];
            foreach (KeyValuePair<InventoryItem, int> pair in currentRecepie.input) {
                if (!mainform.player.inventory.hasAmount(pair.Key, pair.Value)) {
                    MessageBox.Show("Need " + pair.Key.getName() + " X " + pair.Value + ".\nNote that only ingredients in your inventory count.");
                    return;
                }
            }
            foreach (KeyValuePair<InventoryItem, int> pair in currentRecepie.input) {
                mainform.player.inventory.removeAmount(pair.Key, pair.Value);
            }
            mainform.player.inventory.addItem(currentRecepie.output.Key, currentRecepie.output.Value);
        }
        public void hide() {
            listbox.Hide();
            craftLabel.Hide();
        }
    }
}
