using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BasicFunction
{
    public static class MathFormula
    {
        public static double Distance(Point p1, Point p2)
        {
            double dX = p1.X - p2.X;
            double dY = p1.Y - p2.Y;
            return Math.Sqrt(dX * dX + dY * dY);
        }
        public static double Distance(Point p)
        {
            Point p_origin = new Point(0, 0);
            return Distance(p, p_origin);
        }
        public static double Distance(double x1, double y1, double x2, double y2)
        {
            Point p1 = new Point(x1, y1);
            Point p2 = new Point(x2, y2);
            return Distance(p1, p2);
        }
        public static double Distance(double x, double y)
        {
            Point p = new Point(x, y);
            Point p_origin = new Point(0, 0);
            return Distance(p, p_origin);
        }
    }
}
