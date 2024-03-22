using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewerObject
{
    public class Label
    {
        public string ID { get; private set; }
        public string Text { get; private set; }
        public string TextColor { get; private set; }
        public double X1 { get; private set; }
        public double X2 { get; private set; }
        public double Y1 { get; private set; }
        public double Y2 { get; private set; }
        public bool IsVisible { get; private set; }

        public Label() { }

        public Label(string id, string text, string textColor, double x1, double x2, double y1, double y2, string visible)
        {
            ID = id;
            Text = text ?? "";
            TextColor = textColor ?? "FFFFFF";
            X1 = x1;
            X2 = x2;
            Y1 = y1;
            Y2 = y2;
            //X1 = Math.Min(x1, x2);
            //X2 = Math.Max(x1, x2);
            //Y1 = Math.Min(y1, y2);
            //Y2 = Math.Max(y1, y2);
            IsVisible = text.Length > 0
                     && visible != null && visible.Length > 0
                     && visible.ToUpper().Contains("Y");
        }
    }
}
