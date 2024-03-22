using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewerObject
{
    public class Section
    {
        public string ID;
        public string StartAdr, EndAdr;
        public SecType Type;

        public Address StartAddress;
        public Address EndAddress;

        public bool enable = false;

        public Section(List<Address> adrs, string id, string startAdr, string endAdr, string type = "StraightLine")
        {
            ID = id.Trim();
            StartAdr = startAdr.Trim();
            EndAdr = endAdr.Trim();
            Type = GetType(type);
            StartAddress = adrs.Where(adr => adr.ID.Trim() == StartAdr).FirstOrDefault();
            if (StartAddress == null)
                Console.WriteLine($"adr id:{StartAdr} not exist");

            EndAddress = adrs.Where(adr => adr.ID.Trim() == EndAdr).FirstOrDefault();
            if (EndAddress == null)
                Console.WriteLine($"adr id:{EndAdr} not exist");
        }
        public Section(string id, Address startAdr, Address endAdr, string type)
        {
            ID = id;
            StartAddress = startAdr;
            EndAddress = endAdr;
            Type = GetType(type);
        }

        public Section(string id, bool enable)
        {
            //這個建構函數是專屬於群創用來更新section List的enable專用
            this.ID = id;
            this.enable = enable;
        }

        public SecType GetType(string sType)
        {
            SecType result = SecType.StraightLine;
            switch (sType)
            {
                case "Curve_0to90":
                    result = SecType.Curve_0to90;
                    break;
                case "Curve_90to180":
                    result = SecType.Curve_90to180;
                    break;
                case "Curve_180to270":
                    result = SecType.Curve_180to270;
                    break;
                case "Curve_270to360":
                    result = SecType.Curve_270to360;
                    break;
                case "Curve_0to180":
                    result = SecType.Curve_0to180;
                    break;
                case "Curve_90to270":
                    result = SecType.Curve_90to270;
                    break;
                case "Curve_180to0":
                    result = SecType.Curve_180to0;
                    break;
                case "Curve_270to90":
                    result = SecType.Curve_270to90;
                    break;
                case "Curve_90to0":
                    result = SecType.Curve_90to0;
                    break;
                case "Curve_180to90":
                    result = SecType.Curve_180to90;
                    break;
                case "Curve_270to180":
                    result = SecType.Curve_270to180;
                    break;
                case "Curve_360to270":
                    result = SecType.Curve_360to270;
                    break;

                default:
                    break;
            }
            return result;
        }
    }

    public enum SecType
    {
        StraightLine,
        Curve_0to90,
        Curve_90to180,
        Curve_180to270,
        Curve_270to360,
        Curve_0to180,
        Curve_90to270,
        Curve_180to0,
        Curve_270to90,
        Curve_90to0,
        Curve_180to90,
        Curve_270to180,
        Curve_360to270,
    }
}
