using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace OpenTerraria {
    public class Util {
        public static Point addPoints(Point p1, Point p2) {
            return new Point(p1.X + p2.X, p1.Y + p2.Y);
        }
        public static PointF addPoints(PointF p1, PointF p2) {
            return new PointF(p1.X + p2.X, p1.Y + p2.Y);
        }
        public static Point convertToPoint(PointF p) {
            return new Point((int)p.X, (int)p.Y);
        }
    }
}
