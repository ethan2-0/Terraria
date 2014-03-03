﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace OpenTerraria {
    public partial class MainForm : Form {
        private static MainForm instance;
        public Point viewOffset;
        public World world;
        public List<Entity> entities;
        public Player player;
        public bool shouldRender = true;
        public Thread renderThread;
        public bool rendering;
        public bool shouldCancelFormClose = true;
        Graphics graphics, offg;
        Bitmap screen;
        public bool debugMenu = false;
        public MainForm() {
            entities = new List<Entity>();
            instance = this;
            viewOffset = new Point(0, 0);
            world = World.createWorld(500, 500);
            respawnPlayer();
            InitializeComponent();
            this.FormClosing += new FormClosingEventHandler(MainForm_FormClosing);
            //this.Paint += new PaintEventHandler(MainForm_Paint);
            this.Resize += new EventHandler(MainForm_Resize);
            this.KeyDown += new KeyEventHandler(MainForm_KeyDown);
        }

        void MainForm_KeyDown(object sender, KeyEventArgs e) {
            if (e.KeyCode == Keys.F3) {
                debugMenu = !debugMenu;
            }
        }
        public void respawnPlayer() {
            player = new Player(new Point(2000, 150));
        }
        void MainForm_Resize(object sender, EventArgs e) {
            
        }

        void MainForm_FormClosing(object sender, FormClosingEventArgs e) {
            return;
            e.Cancel = shouldCancelFormClose;
            shouldCancelFormClose = false;
            shouldRender = false;
            while (rendering) {
                Thread.Sleep(5);
            }
            this.Close();
        }
        public static MainForm getInstance() {
            return instance;
        }
        void MainForm_Paint(object sender, PaintEventArgs e) {
            paint();
        }
        public void paint() {
            //Bitmap b = new Bitmap(this.Width, this.Height);
            //Graphics g = Graphics.FromImage(b);
            this.DoubleBuffered = true;
            Pen blackPen = createPen(Color.Black);
            Brush blackBrush = createBrush(Color.Black);
            offg.Clear(Color.SkyBlue);
            //
            //Hud
            //
            //Health bar
            offg.FillRectangle(createBrush(Color.Red), new Rectangle(new Point(5, 5), new Size(player.getMaxHealth() / 2, 15)));
            offg.FillRectangle(createBrush(Color.Green), new Rectangle(new Point(5, 5), new Size(player.health / 2, 15)));

            //g.DrawImage(Reference.getImage("grass.png"), new Point(30, 30));
            world.draw(offg);
            foreach (Entity e in entities) {
                e.draw(offg);
            }
            if (debugMenu) {
                offg.DrawString("OpenTerraria " + player.ToString() + " Ground: " + player.isOnGround, getNormalFont(8), blackBrush, new Point(0, 30));
            }
            graphics.DrawImage(screen, new Point(0, 0));
            //graphics.DrawImage(b, new Point(0, 0));
            //b.Dispose();
            //graphics.Dispose();
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
            for (int i = 0; i < entities.Count; i++) {
                entities[i].update();
            }
            int offsetX = player.location.X - this.Width / 2;
            int offsetY = player.location.Y - this.Height / 2;
            viewOffset = new Point(offsetX, offsetY);
            paint();
        }
        public void renderThreadRunner(Object o) {
            while (shouldRender) {
                paint();
            }
        }
        private void MainForm_Load(object sender, EventArgs e) {
            //renderThread = new Thread(new ParameterizedThreadStart(renderThreadRunner));
            //renderThread.Start();
            graphics = this.CreateGraphics();
            screen = new Bitmap(this.Width, this.Height);
            offg = Graphics.FromImage(screen);
        }
    }
}
