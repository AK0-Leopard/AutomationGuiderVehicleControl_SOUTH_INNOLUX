using System.Windows;

namespace ViewerObject
{
    public class Address
    {
        public string ID { get; private set; }
        public double X { get; private set; }
        public double Y { get; private set; }
        public Point Point { get; private set; }

        public Address() { }
        public Address(string id, double x, double y)
        {
            ID = id?.Trim() ?? "";
            X = x;
            Y = y;
            Point = new Point(x, y);
        }
    }
}
