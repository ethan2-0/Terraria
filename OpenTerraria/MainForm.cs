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
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

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
        public List<DamageIndicator> damageIndicators;
        Bitmap cursor;
        public EventDispatcher drawEventDispatcher = new EventDispatcher();
        /// <summary>
        /// Obsolete.
        /// </summary>
        [Obsolete]
        public EventDispatcher doneLoadingEventDispatcher = new EventDispatcher();
        public long totalRenders = 0;
        public CraftingManager inventoryCraftingManager;
        public int lastIndex = -1;
        public Inventory lastInventory;
        public static Random random;
        public static Bitmap background;
        public bool craftingShown = false;
        public bool working = false;
        public bool mouseDown = false;
        public bool linux = false;
        public bool waitingForInitialClick = false;
        public Point cursorOffset = Point.Empty;
        public MainForm() {
            random = new Random();
            damageIndicators = new List<DamageIndicator>();
            entities = new List<Entity>();
            inventories = new List<Inventory>();
            instance = this;
            cursor = Reference.getImage("cursor.png");
            viewOffset = new Point(0, 0);
            player = new Player(new Point(9000, 0));
            Zombie zombie = new Zombie(Util.addPoints(player.location, new Point(50, 0)));
            Zombie zombie2 = new Zombie(Util.addPoints(player.location, new Point(100, 0)));
            Zombie zombie3 = new Zombie(Util.addPoints(player.location, new Point(150, 0)));

            //An attempt to fix Wine compatability. My theory as to why not: Using the Windows cursor position method on linux before messagebox is done.
            //Get cursor position, and ask whether or not they use a Unix-like.
            if (false) {//MessageBox.Show("Are you running the game on a non-Windows system using the Wine compatibility layer?", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes) {
                linux = true;
                MessageBox.Show("So that the game knows where your mouse cursor is, please click the top-left of the INSIDE of the window when the game starts. For example, if the following was the window, you would click where the ^ is." +
                    "\n+========+" +
                    "\n|X                   |" +
                    "\n|                       |" +
                    "\n|                       |" +
                    "\n|                       |" +
                    "\n|                       |" +
                    "\n|                       |" +
                    "\n+========+", "Locate your cursor");
                waitingForInitialClick = true;
            }

            //Windows Forms stuff
            InitializeComponent();
            //Generate World
            world = World.createWorld(500, 500);

            //Initialize Recipies
            List<Recepie> recepies = new List<Recepie>();
            Dictionary<InventoryItem, int> input = new Dictionary<InventoryItem, int>();
            input.Add(BlockPrototype.log, 1);
            recepies.Add(new Recepie(input, new KeyValuePair<InventoryItem, int>(BlockPrototype.planks, 5)));

            Dictionary<InventoryItem, int> input2 = new Dictionary<InventoryItem, int>();
            input2.Add(BlockPrototype.planks, 1);
            recepies.Add(new Recepie(input2, new KeyValuePair<InventoryItem, int>(ItemTemplate.stick.createNew(), 2)));

            Dictionary<InventoryItem, int> input3 = new Dictionary<InventoryItem, int>();
            input3.Add(ItemTemplate.stick.createNew(), 6);
            input3.Add(ItemTemplate.wool.createNew(), 2);
            recepies.Add(new Recepie(input3, new KeyValuePair<InventoryItem, int>(new ItemBow(), 1)));

            Dictionary<InventoryItem, int> input4 = new Dictionary<InventoryItem, int>();
            input4.Add(ItemTemplate.stick.createNew(), 4);
            input4.Add(BlockPrototype.oreCoal, 1);
            recepies.Add(new Recepie(input4, new KeyValuePair<InventoryItem, int>(BlockPrototype.torch, 4)));

            Dictionary<InventoryItem, int> input5 = new Dictionary<InventoryItem, int>();
            input5.Add(ItemTemplate.stick.createNew(), 2);
            input5.Add(ItemTemplate.ironBar.createNew(), 4);
            recepies.Add(new Recepie(input5, new KeyValuePair<InventoryItem, int>(ItemTool.createPickaxe(PickaxeType.PICKAXE_IRON), 1)));

            inventoryCraftingManager = new CraftingManager(recepies);

            this.FormClosing += new FormClosingEventHandler(MainForm_FormClosing);
            //this.Paint += new PaintEventHandler(MainForm_Paint);
            this.Resize += new EventHandler(MainForm_Resize);
            this.KeyDown += new KeyEventHandler(MainForm_KeyDown);
            this.KeyPress += new KeyPressEventHandler(MainForm_KeyPress);
            this.MouseClick += new MouseEventHandler(MainForm_MouseClick);
            this.MouseWheel += new MouseEventHandler(MainForm_MouseWheel);
            this.MouseDown += new MouseEventHandler(MainForm_MouseDown);
            this.MouseUp += new MouseEventHandler(MainForm_MouseUp);

            LightingEngine.doFullLightingUpdate();
        }

        void MainForm_MouseUp(object sender, MouseEventArgs e) {
            mouseDown = false;
        }

        void MainForm_MouseDown(object sender, MouseEventArgs e) {
            mouseDown = true;
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
                PhysicsTimer.Enabled = !PhysicsTimer.Enabled;
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
            //See if we're doing the initial locating click
            if(waitingForInitialClick) {
                Point p = Util.subtractPoints(MousePosition, this.DesktopLocation);
                waitingForInitialClick = false;
                cursorOffset = Util.absoluteValueOf(Util.subtractPoints(p, e.Location)); //Get the difference between what we thought it was and what it acutally was
                //Util.absoluteValueOf(Util.subtractPoints(e.Location, this.DesktopLocation));
                return;
            }
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
            if (e.KeyCode == Keys.C) {
                craftingShown = !craftingShown;
                inventory = craftingShown;
            }
            if (e.KeyCode == Keys.E) {
                Point cursorLocation = getCursorBlockLocation();
                world.getBlockAt(cursorLocation.X, cursorLocation.Y).use();
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
            if (LightingEngine.updating) {
                return;
            }
            if (offg == null) {
                return;
            }
            try {
                //Updating components
                PausePanel.Visible = !PhysicsTimer.Enabled;
                PausePanel.Left = this.Width / 2 - PausePanel.Width / 2;
                WorkingBar.Width = this.Width;
                WorkingBar.Visible = working;
                if (working) {
                    return;
                }

                totalRenders++;
                //Bitmap b = new Bitmap(this.Width, this.Height);
                //Graphics g = Graphics.FromImage(b);
                this.DoubleBuffered = true;
                Pen blackPen = createPen(Color.Black);
                Brush blackBrush = createBrush(Color.Black);
                offg.Clear(Color.SkyBlue);
                //offg.DrawImage(background, new Point(0, 0));
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
                if (craftingShown) {
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
                //Entity tooltip
                foreach (Entity e in entities) {
                    if (e is Creature) {
                        if (new Rectangle(e.location, e.hitBox).Contains(getCursorWorldLocation())) {
                            Creature c = (Creature)e;
                            offg.DrawString(c.name + ": " + c.health + "/" + c.getMaxHealth(), getNormalFont(13), createBrush(Color.DarkOrange), getCursorPos());
                        }
                    }
                }
                //Damage indicators
                foreach (DamageIndicator indicator in damageIndicators) {
                    indicator.draw(offg);
                }
                if (!PhysicsTimer.Enabled) {
                    
                }
                //Have all the external drawers do their drawing thing
                drawEventDispatcher.dispatch();
                //Finishing off the double buffering
                //Never mind, this has been moved elsewhere...
                graphics.DrawImage(screen, new Point(0, 0));
                //graphics.DrawImage(b, new Point(0, 0));
                //b.Dispose();
                //graphics.Dispose();
            } catch (ExternalException e) {
                return;
            }
        }
        public Point getCursorPos() {
            //return Util.subtractPoints(MousePosition, Util.addPoints(this.DesktopLocation, new Point(4, 30)));
            if (linux) {
                return Util.subtractPoints(MousePosition, Util.addPoints(this.DesktopLocation, cursorOffset));
            } else {
                Point p = Cursor.Position;
                return this.PointToClient(p);
            }
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
            foreach (Entity en in entities) {
                en.updateMomentumLock();
            }
            for (int i = 0; i < damageIndicators.Count; i++) {
                damageIndicators[i].tick();
            }
            int offsetX = player.location.X - this.Width / 2;
            int offsetY = player.location.Y - this.Height / 2;
            viewOffset = new Point(offsetX, offsetY);
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
            /*Bitmap dirtBackground = Reference.getImage("dirtBackground.png");
            background = new Bitmap(world.width * 20, world.height * 20);
            Graphics backG = Graphics.FromImage(background);
            backG.Clear(Color.LightBlue);
            foreach (Block[] blocks in world.blocks) {
                bool canSeeSky = true;
                for (int i = 0; i < blocks.Count(); i++) {
                    if (blocks[i].prototype.solid) {
                        canSeeSky = false;
                    }
                    if (!canSeeSky) {
                        backG.DrawImage(dirtBackground, blocks[i].location);
                    }
                }
            }*/
        }

        private void button1_Click(object sender, EventArgs e) {

        }

        private void RenderTimer_Tick(object sender, EventArgs e) {
            paint();
        }

        private void label2_Click(object sender, EventArgs e) {
            Application.Exit();
        }
        public void serializeGame() {
            Thread thread = new Thread(new ThreadStart(serializeGameThread));
            thread.Start();
        }
        public void serializeGameThread() {
            working = true;
            String saveFileLocation = Reference.executablePath + "save.bin";
            BinaryFormatter formatter = new BinaryFormatter();
            Object[] objects = new Object[3];
            objects[0] = world;
            objects[1] = entities;
            objects[2] = inventories;
            FileStream fs = null;
            try {
                fs = new FileStream(saveFileLocation, FileMode.Create);
                formatter.Serialize(fs, objects);
            } catch (Exception e) {
                System.Diagnostics.Debug.WriteLine(e.ToString());
            } finally {
                fs.Close();
            }
            working = false;
        }
        public void unserializeGame() {
            Thread thread = new Thread(new ThreadStart(unserializeGameThread));
            thread.Start();
        }
        public void unserializeGameThread() {
            working = true;
            String saveFileLocation = Reference.executablePath + "save.bin";
            FileStream fs = null;
            try {
                fs = new FileStream(saveFileLocation, FileMode.Open);
                BinaryFormatter formatter = new BinaryFormatter();
                Object[] objects = (Object[]) formatter.Deserialize(fs);
                world = (World)objects[0];
                entities = (List<Entity>)objects[1];
                inventories = (List<Inventory>)objects[2];
                foreach (Entity entity in entities) {
                    if (entity is Player) { //It's the player!
                        player = (Player) entity;
                    }
                }
                player.registerHandlers();
                LightingEngine.fullLightingUpdateEventDispatcher.handlers.Clear();
                foreach (Block b in world.blockList) {
                    b.setEmittedLightLevel(b.emittedLightLevel);
                }
                LightingEngine.doFullLightingUpdate();
            } catch (Exception e) {
                System.Diagnostics.Debug.WriteLine(e.ToString());
            } finally {
                fs.Close();
            }
            working = false;
        }

        private void label3_Click(object sender, EventArgs e) {
            serializeGame();
        }

        private void label4_Click(object sender, EventArgs e) {
            unserializeGame();
        }
    }
}
