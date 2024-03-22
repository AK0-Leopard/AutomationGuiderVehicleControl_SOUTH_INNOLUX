using com.mirle.ibg3k0.sc;
using com.mirle.ibg3k0.sc.Data;
using ObjectConverterInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObjectConverter_OHBC_ASE_K21
{
    public class Portdef : IAPortStation
    {
        private readonly string ns = "ObjectConverter_OHBC_ASE_K21" + ".PortDef";

        private string connectionString = "";
        public Portdef(string _connectionString)
        {
            connectionString = _connectionString ?? "";
        }

        public bool SetVPORTSTATIONs(ref List<ViewerObject.VPORTSTATION> ports, string data, out string result)
        {
            string ms = System.Reflection.MethodBase.GetCurrentMethod().Name;
            result = "";

            string _doing = "";
            try
            {
                _doing = "Get portDefs from DB";
                List<PortDef> portDefs = new List<PortDef>();
                using (DBConnection_EF con = DBConnection_EF.GetUContext())
                {
                    if (!string.IsNullOrWhiteSpace(connectionString))
                        con.Database.Connection.ConnectionString = connectionString;

                    var query = from a in con.PortDef.AsNoTracking()
                                where a.ADR_ID!="99999"
                                select a;
                    portDefs.AddRange(query?.ToList() ?? new List<PortDef>());
                }

                _doing = "Init List<VPORTSTATION>";
                if (ports == null) ports = new List<ViewerObject.VPORTSTATION>();
                else ports.Clear();

                _doing = "Set each VPORTSTATION by PortDef";
                foreach (var portDef in portDefs)
                {
                    if (!SetVPORTSTATION(portDef, out ViewerObject.VPORTSTATION port, out result)) return false;
                    ports.Add(port);
                }

                return true;
            }
            catch
            {
                result = $"{ns}.{ms} - Exception occurred while {_doing}";
                return false;
            }
        }
        public bool SetVPORTSTATION(PortDef portDef, out ViewerObject.VPORTSTATION port, out string result)
        {
            string ms = System.Reflection.MethodBase.GetCurrentMethod().Name;
            result = "";
            port = null;

            if (portDef == null)
            {
                result = $"{ns}.{ms} - portDef = null";
                return false;
            }

            string _doing = "";
            try
            {
                _doing = $"new ViewerObject.VPORTSTATION({portDef.PLCPortID}, {portDef.ADR_ID}, {1})";
                port = new ViewerObject.VPORTSTATION(portDef.PLCPortID, portDef.ADR_ID, 1/*aport.stageCount*/);
                return true;
            }
            catch
            {
                result = $"{ns}.{ms} - Exception occurred while {_doing}";
                return false;
            }
        }
    }
}
