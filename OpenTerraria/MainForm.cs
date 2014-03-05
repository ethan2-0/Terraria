using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Runtime.InteropServices;

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
        public ItemInInventory movingItem = null;
        public List<Inventory> inventories;
        Bitmap cursor;
        public MainForm() {
            entities = new List<Entity>();
            inventories = new List<Inventory>();
            instance = this;
            cursor = Reference.getImage("cursor.png");
            viewOffset = new Point(0, 0);
            world = World.createWorld(500, 500);
            player = new Player(new Point(9000, 150));
            //Zombie zombie = new Zombie(Util.addPoints(player.location, new Point(50, 0)));
            InitializeComponent();
            this.FormClosing += new FormClosingEventHandler(MainForm_FormClosing);
            //this.Paint += new PaintEventHandler(MainForm_Paint);
            this.Resize += new EventHandler(MainForm_Resize);
            this.KeyDown += new KeyEventHandler(MainForm_KeyDown);
            this.KeyPress += new KeyPressEventHandler(MainForm_KeyPress);
            this.MouseClick += new MouseEventHandler(MainForm_MouseClick);
        }
        public Point getCursorBlockLocation() {
            Point adjustedPixels = Util.addPoints(getCursorPos(), viewOffset);
            Point adjustedBlocks = new Point((int) Math.Floor((double) adjustedPixels.X / 20) - 1, (int) Math.Floor((double)adjustedPixels.Y / 20) - 1);
            return adjustedBlocks;
        }
        void MainForm_KeyPress(object sender, KeyPressEventArgs e) {
            if (e.KeyChar == (char)Keys.Escape) {
                GameTimer.Enabled = !GameTimer.Enabled;
            }
        }
        public Inventory getParentInventory(ItemInInventory item) {
            foreach (Inventory inventory in inventories) {
                if (inventory.items.Contains(item)) {
                    return inventory;
                }
            }
            return null;
        }
        void MainForm_MouseClick(object sender, MouseEventArgs e) {
            //See if the owner of the click is an Inventory
            foreach (Inventory inventory in inventories) {
                //ItemInInventory item = inventory.drawer.getItemAtLocation(e.Location);
                int index = inventory.drawer.getItemAtLocation(e.Location);
                if(index != -1) {
                    //We found it
                    if (movingItem == null) {
                        movingItem = inventory.items[index];
                    } else {
                        Inventory movingParent = getParentInventory(movingItem);
                        ItemInInventory item = inventory.items[index];
                        inventory.items[index] = movingItem;
                        int slot = movingItem.slot;
                        movingItem = null;
                        movingParent.items[slot] = item;
                    }
                    break;
                }
            }
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
            //player = new Player(new Point(9000, 150));
            Close();
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
            try {
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
                    player.inventory.draw(new Point(5, 50), offg);
                }
                //Hotbar
                player.hotbar.draw(new Point(200, 5), offg);
                //Debug Menu
                if (debugMenu) {
                    offg.DrawString("OpenTerraria " + player.ToString() + " Ground: " + player.isOnGround + " ViewOffset: " + viewOffset.ToString() + " CursorPos:" + getCursorPos().ToString(), getNormalFont(8), blackBrush, new Point(0, 20));
                    //offg.FillRectangle(createBrush(Color.Black), new Rectangle(getCursorPos(), new Size(20, 20)));
                    //offg.DrawImage(cursor, getCursorPos());
                    //Cursor.Hide();
                } else {
                    //Cursor.Show();
                }
                if (movingItem != null) {
                    player.inventory.drawer.renderItem(movingItem, getCursorPos(), offg, true, 400);
                }
                //Cursor
                graphics.DrawImage(screen, new Point(0, 0));
                //graphics.DrawImage(b, new Point(0, 0));
                //b.Dispose();
                //graphics.Dispose();
            } catch (ExternalException e) {
                return;
            }
        }
        public Point getCursorPos() {
            return Util.subtractPoints(MousePosition, Util.addPoints(this.DesktopLocation, new Point(4, 30)));
        }
        public static Pen createPen(Color color) {
            return new Pen(createBrush(color));
        }
        public static Brush createBrush(Color color) {
            return new SolidBrush(color);
        }
        public static Font getNormalFont() {
            return getNormalFont(10);
        }
        public static Font getNormalFont(float size) {
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
