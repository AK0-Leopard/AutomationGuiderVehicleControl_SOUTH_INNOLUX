using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewerObject
{
    public class VTIPMESSAGE
    {
        public string XID { get; set; } = "";
        public string Time { get; set; } = "";
        public VTIPMESSAGE_Def.MsgLevel MsgLevel { get; set; } = VTIPMESSAGE_Def.MsgLevel.Info;
        public string Msg { get; set; } = "";
        public bool IsConfirm { get; set; } = false;

        public VTIPMESSAGE()
        {
        }
    }

    public static class VTIPMESSAGE_Def
    {
        public enum MsgLevel
        {
            Info = 0,
            Warn = 1,
            Error = 2
        }
    }
}
