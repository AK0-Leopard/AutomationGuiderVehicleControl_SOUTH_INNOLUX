using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewerObject
{
    public class VSEGMENT : ViewerObjectBase
    {
        public VSEGMENT(string segID, bool isDisabled)
        {
            SEG_ID = segID?.Trim() ?? "";
            IS_DISABLED = isDisabled;
        }

        public string SEG_ID { get; private set; } = "";

        private bool iS_DISABLED = false;
        public bool IS_DISABLED
        {
            get { return iS_DISABLED; }
            set
            {
                if (iS_DISABLED != value)
                {
                    iS_DISABLED = value;
                    OnPropertyChanged();
                }
            }
        }
    }
}
