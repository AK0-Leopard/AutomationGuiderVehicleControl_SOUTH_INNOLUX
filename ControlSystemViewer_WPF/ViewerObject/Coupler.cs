using System;
using System.Collections.Generic;

using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace ViewerObject
{
    public static class Coupler_Def
    {
        public enum CouplerStatus
        {
            None = 0,
            Disable,
            Enable,
            Auto,
            Manual,
            Charging,
            Error
        }

        public enum CouplerHPSafety
        {
            NonSafety = 0,
            Safyte = 1
        }
    }
    public class Coupler
    {
        public virtual string ShowStatus { get { return Status.ToString(); } set { } }
        public Coupler_Def.CouplerStatus Status { get; set; }
        public Coupler_Def.CouplerHPSafety HPSafety { get; set; }
        public int ID { get; private set; }
        public string ChargerID { get; private set; }
        public string Name
        {
            get { return name; }
        }
        private string name = "";

        public Coupler(int _id, string _Name, string _ChargerID)
        {
            ID = _id;
            name = _Name;
            ChargerID = _ChargerID;
        }

        public DateTime ChargingStartTime { get; set; }
        public DateTime ChargingEndTime { get; set; }
        public double ChargingAh { get; set; }
        public string AddressID { get; set; }

        public Brush StatusBrush
        {
            get
            {
                switch (Status)
                {
                    case Coupler_Def.CouplerStatus.Charging:
                        return Brushes.Gold;

                    case Coupler_Def.CouplerStatus.Enable:
                    case Coupler_Def.CouplerStatus.Auto:
                        return Brushes.SkyBlue;

                    case Coupler_Def.CouplerStatus.Manual:
                        return Brushes.YellowGreen;

                    case Coupler_Def.CouplerStatus.Error:
                        return Brushes.Red;

                    case Coupler_Def.CouplerStatus.None:
                    case Coupler_Def.CouplerStatus.Disable:
                    default:
                        return Brushes.Gray;
                }
            }
        }
    }
}
