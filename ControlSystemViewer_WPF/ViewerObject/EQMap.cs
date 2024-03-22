using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewerObject
{
    public class EQMap
    {
        public string id;
        public string eqName;
        public string eqNumber;

        public EQMap(string ID, string EQName, string EQNumber)
        {
            id = ID;
            eqName = EQName;
            eqNumber = EQNumber;
        }

    }
}
