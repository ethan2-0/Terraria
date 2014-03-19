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
        public static Point subtractPoints(Point p1, Point p2) {
            return new Point(p1.X - p2.X, p1.Y - p2.Y);
        }
        public static Point convertToPoint(PointF p) {
            return new Point((int)p.X, (int)p.Y);
        }
        public static double distanceBetween(Point p1, Point p2) {
            return Math.Sqrt(Math.Pow(Math.Abs(p1.X - p2.X), 2) + Math.Pow(Math.Abs(p1.Y - p2.Y), 2));
        }
        public static int indexOf(Object o, List<Object> list) {
            for (int i = 0; i < list.Count; i++) {
                if (list[i] == o) {
                    return i;
                }
            }
            return -1;
        }
        public static int indexOf(Object o, Object[] objects) {
            return indexOf(o, new List<Object>(objects));
        }
    }
}
