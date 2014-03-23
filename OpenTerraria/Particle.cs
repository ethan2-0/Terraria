using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTerraria.Entities;
using System.Drawing;
using OpenTerraria.Cave;

namespace OpenTerraria {
    public class Particle : DamageIndicator {
        Color color;
        public Particle(Point location, Color color) : base(location, "Particle") {
            this.color = color;
        }
        public override void draw(Graphics g) {
            g.FillRectangle(MainForm.createBrush(color), new Rectangle(Util.subtractPoints(location, MainForm.getInstance().viewOffset), new Size(3, 3)));
        }
        public override void tick() {
            base.tick();
            //if(!MainForm.getInstance().world.isInsideBlock(location.X, location.Y + 3)) {
                location.Y += 2;
            //}
        }
        public static void spawnParticlesAround(Point p, Color color, int amount) {
            for (int i = 0; i < amount; i++) {
                /*Point location = Util.addPoints(p, new Point(CaveGenerator.random.Next(40) - 20, CaveGenerator.random.Next(40) - 20));*/
                /*int red = (int) (color.R * (CaveGenerator.random.NextDouble() / 4 + 0.75));
                int blue = (int)(color.B * (CaveGenerator.random.NextDouble() / 4 + 0.75));
                int green = (int)(color.G * (CaveGenerator.random.NextDouble() / 4 + 0.75));*/
                Color c = color;
                Particle particle = new Particle(p, c);
            }
        }
    }
}
