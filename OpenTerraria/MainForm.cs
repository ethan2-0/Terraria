using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace OpenTerraria {
    public partial class MainForm : Form {
        private static MainForm instance;
        public PointF viewOffset;
        public World world;
        public List<Entity> entities;
        public Player player;
        public MainForm() {
            entities = new List<Entity>();
            instance = this;
            viewOffset = new Point(0, 0);
            world = World.createWorld(500, 500);
            player = new Player(new Point(5, 5));
            InitializeComponent();
            this.Paint += new PaintEventHandler(MainForm_Paint);
        }
        public static MainForm getInstance() {
            return instance;
        }
        void MainForm_Paint(object sender, PaintEventArgs e) {
            paint(e.Graphics);
        }
        public void paint(Graphics graphics) {
            Bitmap b = new Bitmap(this.Width, this.Height);
            Graphics g = Graphics.FromImage(b);
            Pen blackPen = createPen(Color.Black);
            Brush blackBrush = createBrush(Color.Black);
            g.Clear(Color.SkyBlue);
            g.DrawString("OpenTerraria " + player.location.ToString(), getNormalFont(), blackBrush, new PointF(5, 5));
            //g.DrawImage(Reference.getImage("grass.png"), new Point(30, 30));
            world.draw(g);
            foreach (Entity e in entities) {
                e.draw(g);
            }
            graphics.DrawImage(b, new Point(0, 0));
        }
        public Pen createPen(Color color) {
            return new Pen(createBrush(color));
        }
        public Brush createBrush(Color color) {
            return new SolidBrush(color);
        }
        public Font getNormalFont() {
            return getNormalFont(10);
        }
        public Font getNormalFont(float size) {
            return new Font("Times", size);
        }
        private void GameTimer_Tick(object sender, EventArgs e) {
            paint(this.CreateGraphics());
            foreach (Entity entity in entities) {
                entity.update();
            }
        }

        private void MainForm_Load(object sender, EventArgs e) {

        }
    }
}
