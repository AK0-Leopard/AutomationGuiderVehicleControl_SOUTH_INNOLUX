using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewerObject
{
    public class AlarmModule
    {
        public int Number { get; set; }
        public string Module_EN { get; set; }
        public string Module_TW { get; set; }

        public AlarmModule(int number, string moduleEn, string moduleTw)
        {
            Number = number;
            Module_EN = moduleEn;
            Module_TW = moduleTw;
        }

    }
}
    