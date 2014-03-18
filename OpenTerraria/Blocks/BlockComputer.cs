using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace OpenTerraria.Blocks {
    public class BlockComputer : Block {
        private interface Algorithm {
            String invoke();
        }
        public bool showingConsole = false;
        RichTextBox textbox;
        static Dictionary<String, Algorithm> algorithms;
        String algorithm = "fibonachi";
        public BlockComputer(Point location, BlockPrototypeComputer prototype)
            : base(prototype, location) {
                textbox = new RichTextBox();
                textbox.Multiline = true;
                textbox.Visible = true;
                textbox.Width = 500;
                textbox.Height = 500;
                textbox.Font = new Font("Courier", 14);
                textbox.BackColor = Color.Black;
                textbox.ForeColor = Color.White;
                textbox.ReadOnly = true;
                textbox.BorderStyle = BorderStyle.None;
                MainForm.getInstance().Controls.Add(textbox);
                textbox.Hide();
                algorithms = new Dictionary<String, Algorithm>();
                algorithms.Add("fibonachi", new AlgorithmFibbonachi());
                MainForm.getInstance().KeyDown += new KeyEventHandler(BlockComputer_KeyDown);
        }

        void BlockComputer_KeyDown(object sender, KeyEventArgs e) {
            if (e.KeyCode == Keys.Escape) {
                showingConsole = false;
            }
        }
        public override void use() {
            showingConsole = !showingConsole;
        }
        public String getNext() {
            return algorithms[algorithm].invoke() + "\n";
        }
        public override void draw(Graphics g) {
            base.draw(g);
            if (showingConsole) {
                textbox.Show();
                textbox.Width = (MainForm.getInstance().Width / 8 * 6);
                textbox.Left = MainForm.getInstance().Width / 2 - textbox.Width / 2;
                textbox.Top = MainForm.getInstance().Height / 2 - textbox.Height / 2;
                if (Cave.CaveGenerator.random.Next(10) > 6) {
                    textbox.Text += getNext();
                }
                textbox.ScrollToCaret();
            } else {
                textbox.Hide();
            }
        }
        private class AlgorithmFibbonachi : Algorithm {
            long lastInt = 0;
            long lastLastInt = 1;
            public String invoke() {
                long current = lastInt + lastLastInt;
                lastLastInt = lastInt;
                lastInt = current;
                return current.ToString();
            }
        }
    }
}
