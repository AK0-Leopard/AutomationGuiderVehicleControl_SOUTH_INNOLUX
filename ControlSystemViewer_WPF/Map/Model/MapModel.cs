using com.mirle.ibg3k0.bc.wpf.App;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Reflection;

namespace Map.Model
{
    public class MapModel
    {
        public double dMinimumScale { get; set; }
        public double dMaximumScale { get; set; }
        public double dScale { get; set; }
        public double dScaleTickFrequency { get; set; }


        public MapModel()
        {
            dMinimumScale = 0.1;
            dMaximumScale = 1;
            dScale = 1;
            dScaleTickFrequency = 0.1;
        }
    }
}
