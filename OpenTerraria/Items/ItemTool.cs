using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using OpenTerraria.Blocks;

namespace OpenTerraria.Items {
    [Serializable]
    public class ItemTool : Item {
        public String toolType;
        public PickaxeType type;
        public ItemTool(String name, String imagename, String toolType, PickaxeType type) : base(type.name + " " + name, imagename) {
            this.toolType = toolType;
            this.type = type;
            MainForm.getInstance().Load += new EventHandler(ItemTool_Load);
        }

        void ItemTool_Load(object sender, EventArgs e) {
            MainForm.getInstance().GameTimer.Tick += new EventHandler(GameTimer_Tick);
        }

        void GameTimer_Tick(object sender, EventArgs e) {
            if (MainForm.getInstance().mouseDown) {
                if (MainForm.getInstance().movingItem != null && this == MainForm.getInstance().movingItem.item) {
                    MainForm.getInstance().movingItem.use();
                }
                if (MainForm.getInstance().player.hotbar.items[MainForm.getInstance().player.hotbarSelectedIndex].item != null && this == MainForm.getInstance().player.hotbar.items[MainForm.getInstance().player.hotbarSelectedIndex].item) {
                    MainForm.getInstance().player.hotbar.items[MainForm.getInstance().player.hotbarSelectedIndex].use();
                }
            }
        }
        public override int getMaxStackSize() {
            return 1;
        }
        public override void use(ItemInInventory item) {
            base.use(item);
            MainForm instance = MainForm.getInstance();
            Point cursorLocation = MainForm.getInstance().getCursorBlockLocation();
            if(cursorLocation.X < instance.world.blocks.Count() && cursorLocation.Y < instance.world.blocks[5].Count()) {
                if (instance.world.getBlockAt(cursorLocation.X, cursorLocation.Y) != null && instance.world.getBlockAt(cursorLocation.X, cursorLocation.Y).prototype.breakableBy == toolType) {
                    if(Util.distanceBetween(MainForm.getInstance().getCursorWorldLocation(), MainForm.getInstance().player.location) > 200) {
                        return;
                    }
                    if (instance.world.getBlockAt(cursorLocation.X, cursorLocation.Y).prototype.hardness > instance.world.getBlockAt(cursorLocation.X, cursorLocation.Y).brokenness) {
                        instance.world.getBlockAt(cursorLocation.X, cursorLocation.Y).brokenness += type.hardness;
                        Particle.spawnParticlesAround(Util.addPoints(instance.world.blocks[cursorLocation.X][cursorLocation.Y].location, new Point(10, 10)), instance.world.blocks[cursorLocation.X][cursorLocation.Y].prototype.color, 1);
                        return;
                    }
                    instance.player.inventory.addItem(instance.world.blocks[cursorLocation.X][cursorLocation.Y].prototype, 1);
                    bool shouldDoFullUpdate = false;
                    shouldDoFullUpdate = true;
                    instance.world.blocks[cursorLocation.X][cursorLocation.Y].prepareForRemoval();
                    DamageIndicator indicator = new DamageIndicator(instance.world.blocks[cursorLocation.X][cursorLocation.Y].location, instance.world.blocks[cursorLocation.X][cursorLocation.Y].getName());
                    //DamageIndicator indicator2 = new DamageIndicator(instance.world.blocks[cursorLocation.X][cursorLocation.Y].location, "■", Color.Brown);
                    Particle.spawnParticlesAround(Util.addPoints(instance.world.blocks[cursorLocation.X][cursorLocation.Y].location, new Point(10, 10)), instance.world.blocks[cursorLocation.X][cursorLocation.Y].prototype.color, 30);
                    instance.world.blocks[cursorLocation.X][cursorLocation.Y] = Block.createNewBlock(BlockPrototype.air, new Point(cursorLocation.X * 20, cursorLocation.Y * 20));
                    if (shouldDoFullUpdate) {
                        LightingEngine.doFullLightingUpdate(false);
                    } else {
                        instance.world.updateSkyLightForColumn(cursorLocation.X);
                    }
                }
            }
        }
        public static ItemTool createPickaxe(PickaxeType type) {
            return new ItemTool("Pickaxe", "pickaxe.png", "pickaxe", type);
        }
    }
}
