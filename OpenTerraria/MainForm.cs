using System;
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
        public bool inventory = false;
        public List<Point> clicksToClaim;
        public ItemInInventory movingItem = null;
        public List<Inventory> inventories;
        public MainForm() {
            entities = new List<Entity>();
            clicksToClaim = new List<Point>();
            inventories = new List<Inventory>();
            instance = this;
            viewOffset = new Point(0, 0);
            world = World.createWorld(500, 500);
            respawnPlayer();
            InitializeComponent();
            this.FormClosing += new FormClosingEventHandler(MainForm_FormClosing);
            //this.Paint += new PaintEventHandler(MainForm_Paint);
            this.Resize += new EventHandler(MainForm_Resize);
            this.KeyDown += new KeyEventHandler(MainForm_KeyDown);
            this.MouseClick += new MouseEventHandler(MainForm_MouseClick);
        }

        void MainForm_MouseClick(object sender, MouseEventArgs e) {
            clicksToClaim.Add(e.Location);
        }

        void MainForm_KeyDown(object sender, KeyEventArgs e) {
            if (e.KeyCode == Keys.F3) {
                debugMenu = !debugMenu;
            }
            if (e.KeyCode == Keys.I) {
                inventory = !inventory;
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

            //g.DrawImage(Reference.getImage("grass.png"), new Point(30, 30));
            world.draw(offg);
            foreach (Entity e in entities) {
                e.draw(offg);
            }
            //
            //Hud
            //
            //Health bar
            offg.FillRectangle(createBrush(Color.Red), new Rectangle(new Point(5, 5), new Size(player.getMaxHealth() / 2, 15)));
            offg.FillRectangle(createBrush(Color.Green), new Rectangle(new Point(5, 5), new Size(player.health / 2, 15)));
            //Inventory
            int position = 0;
            int row = 0;
            if (inventory) {
                offg.FillRectangle(createBrush(Color.Gray), new Rectangle(new Point(5, 50), new Size(255, 105)));
                foreach (ItemInInventory item in player.inventory.items) {
                    position++;
                    if (position >= 11) {
                        position = 0;
                        row++;
                    }
                    if (item != null) {
                        Point location = new Point(position * 25 - 15, 55 + row * 25);
                        Rectangle r = new Rectangle(location, new Size(20, 20));
                        for (int i = 0; i < clicksToClaim.Count; i++) {
                            Point p = clicksToClaim[i];
                            if (r.Contains(p)) {
                                clicksToClaim.Remove(p);
                                if (movingItem == null) {
                                    movingItem = item;
                                } else {
                                    ItemInInventory item1 = movingItem;
                                    ItemInInventory item2 = item;
                                    foreach (Inventory inv in inventories) {
                                        if (inv.contains(item1)) {
                                            int index = inv.indexOf(item1);
                                            inv.items[inv.indexOf(item1)] = item2;
                                            inv.items[index] = null;
                                        }
                                        if (inv.contains(item2)) {
                                            int index = inv.indexOf(item2);
                                            inv.items[index] = null;
                                            inv.items[index] = item1;
                                        }
                                    }
                                    movingItem = null;
                                }
                            }
                        }
                        if (movingItem == item) {
                            offg.DrawRectangle(createPen(Color.Red), new Rectangle(Util.subtractPoints(location, new Point(1, 1)), new Size(22, 22)));
                        }
                        offg.DrawImage(item.item.getImage(), location);
                        offg.DrawString(item.count.ToString(), getNormalFont(10), createBrush(Color.LightGray), new PointF(location.X + 10, location.Y + 10));
                    }
                }
            }
            //Hotbar
            offg.FillRectangle(createBrush(Color.Gray), new Rectangle(new Point(200, 5), new Size(255, 30)));
            position = 0;
            foreach (ItemInInventory item in player.hotbar.items) {
                position++;
                if (position >= 11) {
                    position = 0;
                }
                if (item != null) {
                    Point location = new Point(position * 25 + 180, 10);
                    Rectangle r = new Rectangle(location, new Size(20, 20));
                    for (int i = 0; i < clicksToClaim.Count; i++) {
                        Point p = clicksToClaim[i];
                        if (r.Contains(p)) {
                            clicksToClaim.Remove(p);
                            if (movingItem == null) {
                                movingItem = item;
                            } else {
                                ItemInInventory item1 = movingItem;
                                ItemInInventory item2 = item;
                                foreach (Inventory inv in inventories) {
                                    if (inv.contains(item1)) {
                                        int index = inv.indexOf(item1);
                                        inv.items[inv.indexOf(item1)] = item2;
                                        inv.items[index] = null;
                                    }
                                    if (inv.contains(item2)) {
                                        int index = inv.indexOf(item2);
                                        inv.items[index] = null;
                                        inv.items[index] = item1;
                                    }
                                }
                                movingItem = null;
                            }
                        }
                    }
                    offg.DrawImage(item.item.getImage(), location);
                    offg.DrawString(item.count.ToString(), getNormalFont(10), createBrush(Color.LightGray), new PointF(location.X + 10, location.Y + 10));
                }
            }
            //Debug Menu
            if (debugMenu) {
                offg.DrawString("OpenTerraria " + player.ToString() + " Ground: " + player.isOnGround + " ViewOffset: " + viewOffset.ToString(), getNormalFont(8), blackBrush, new Point(0, 20));
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
