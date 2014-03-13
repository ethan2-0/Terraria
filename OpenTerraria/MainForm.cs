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
using OpenTerraria.Items;
using OpenTerraria.Blocks;
using OpenTerraria.Entities;

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
        public Graphics graphics, offg;
        Bitmap screen;
        public bool debugMenu = false;
        public bool inventory = false;
        public ItemInInventory movingItem = null;
        public List<Inventory> inventories;
        Bitmap cursor;
        public EventDispatcher drawEventDispatcher = new EventDispatcher();
        public long totalRenders = 0;
        public CraftingManager inventoryCraftingManager;
        public int lastIndex = -1;
        public Inventory lastInventory;
        public MainForm() {
            entities = new List<Entity>();
            inventories = new List<Inventory>();
            instance = this;
            cursor = Reference.getImage("cursor.png");
            viewOffset = new Point(0, 0);
            world = World.createWorld(500, 500);
            player = new Player(new Point(9000, 0));
            //Zombie zombie = new Zombie(Util.addPoints(player.location, new Point(50, 0)));
            //Zombie zombie2 = new Zombie(Util.addPoints(player.location, new Point(-50, 0)));*/
            InitializeComponent();

            List<Recepie> recepies = new List<Recepie>();
            Dictionary<InventoryItem, int> input = new Dictionary<InventoryItem, int>();
            input.Add(BlockPrototype.planks, 1);
            recepies.Add(new Recepie(input, new KeyValuePair<InventoryItem, int>(BlockPrototype.planks, 5)));

            Dictionary<InventoryItem, int> input2 = new Dictionary<InventoryItem, int>();
            input2.Add(BlockPrototype.planks, 1);
            recepies.Add(new Recepie(input, new KeyValuePair<InventoryItem, int>(ItemTemplate.stick.createNew(), 2)));

            inventoryCraftingManager = new CraftingManager(recepies);

            this.FormClosing += new FormClosingEventHandler(MainForm_FormClosing);
            //this.Paint += new PaintEventHandler(MainForm_Paint);
            this.Resize += new EventHandler(MainForm_Resize);
            this.KeyDown += new KeyEventHandler(MainForm_KeyDown);
            this.KeyPress += new KeyPressEventHandler(MainForm_KeyPress);
            this.MouseClick += new MouseEventHandler(MainForm_MouseClick);
            this.MouseWheel += new MouseEventHandler(MainForm_MouseWheel);
        }

        public void MainForm_MouseWheel(object sender, MouseEventArgs e) {
            if (e.Delta != 0) {
                if (e.Delta < 0) {
                    player.hotbarSelectedIndex++;
                } else {
                    player.hotbarSelectedIndex--;
                }
                if (player.hotbarSelectedIndex < 0) {
                    player.hotbarSelectedIndex = 0;
                }
                if (player.hotbarSelectedIndex >= player.hotbar.items.Length) {
                    player.hotbarSelectedIndex = player.hotbarSelectedIndex - 1;
                }
            }
        }
        public Point getCursorWorldLocation() {
            /*Point adjustedPixels = new Point(getCursorPos().X + viewOffset.X, getCursorPos().Y - viewOffset.Y);
            Point adjustedBlocks = new Point((int) Math.Floor((double) adjustedPixels.X / 20) - 1, (int) Math.Floor((double)adjustedPixels.Y / 20) - 1);
            return adjustedBlocks;*/
            /*Point cursorAdjusted = new Point(getCursorPos().X / 20, getCursorPos().Y / 20);
            Point p = Util.subtractPoints(new Point(player.blockX + (Width / 40), player.blockY + (Height / 40)), cursorAdjusted);
            return p;*/
            Point pos = Util.subtractPoints(player.location, MainForm.getInstance().viewOffset);
            Point pos2 = Util.subtractPoints(getCursorPos(), pos);
            Point pos3 = Util.addPoints(pos2, player.location);
            return pos3;
        }
        public Point getCursorBlockLocation() {
            Point p = new Point(getCursorWorldLocation().X / 20, getCursorWorldLocation().Y / 20);
            return p;
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
            //First try to use the item on the hotbar
            
            //See if the owner of the click is an Inventory
            bool foundIt = false;
            foreach (Inventory inventory in inventories) {
                //ItemInInventory item = inventory.drawer.getItemAtLocation(e.Location);
                if (!inventory.drawer.lastRenderedRectangle.Contains(getCursorPos())) {
                    continue;
                }
                int index = inventory.drawer.getItemAtLocation(e.Location);
                if(index != -1) {
                    //We found it
                    if (movingItem == null) {
                        movingItem = inventory.items[index];
                    } else {
                        try {
                            Inventory movingParent = getParentInventory(movingItem);
                            ItemInInventory item = inventory.items[index];
                            inventory.items[index] = movingItem;
                            int slot = movingItem.slot;
                            movingItem = null;
                            movingParent.items[slot] = item;
                        } catch (Exception ex) {
                            //Lose the item
                            movingItem = null;
                        }
                    }
                    foundIt = true;
                    break;
                }
            }
            if (foundIt) {
                return;
            }
            if (movingItem != null) {
                movingItem.use();
                return;
            }
            if (player.hotbar.items[player.hotbarSelectedIndex] != null) {
                player.hotbar.items[player.hotbarSelectedIndex].use();
                return;
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
        public Point getTotalCursorPos() {
            return Util.addPoints(getCursorPos(), viewOffset);
        }
        void MainForm_Paint(object sender, PaintEventArgs e) {
            paint();
        }
        public void paint() {
            try {
                totalRenders++;
                //Bitmap b = new Bitmap(this.Width, this.Height);
                //Graphics g = Graphics.FromImage(b);
                this.DoubleBuffered = true;
                Pen blackPen = createPen(Color.Black);
                Brush blackBrush = createBrush(Color.Black);
                offg.Clear(Color.SkyBlue);
                drawEventDispatcher.dispatch();
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
                try {
                    Rectangle rect = new Rectangle(player.hotbar.drawer.lastRenderedPositions[player.hotbarSelectedIndex], new Size(30, 30));
                    offg.DrawRectangle(createPen(Color.Black), rect);
                } catch (KeyNotFoundException e) {
                    return;
                }
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
                //Crafting Window
                if (inventory) {
                    inventoryCraftingManager.render(offg, new Point(5, 160));
                } else {
                    inventoryCraftingManager.hide();
                }
                //Item tooltip
                foreach (Inventory inv in inventories) {
                    if (!inv.drawer.lastRenderedRectangle.Contains(getCursorPos())) {
                        continue;
                    }
                    //int index = inv.drawer.getItemAtLocation(getCursorPos());
                    int index = lastIndex;
                    if (totalRenders % 3 == 0) {
                        lastIndex = inv.drawer.getItemAtLocation(getCursorPos());
                        index = lastIndex;
                        lastInventory = inv;
                    }
                    if (index != -1) {
                        //Found it
                        try {
                            if (inv.items[index] != null) {
                                offg.DrawString(inv.items[index].item.getName(), getNormalFont(14), createBrush(Color.DarkOrange), getCursorPos());
                            }
                        } catch (IndexOutOfRangeException e) {
                            continue;
                        }
                    }
                }
                //Finishing off the double buffering
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
            return new Font("Comic Sans MS", size);
        }
        private void GameTimer_Tick(object sender, EventArgs e) {
            int j = 0;
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

        private void button1_Click(object sender, EventArgs e) {

        }
    }
}
