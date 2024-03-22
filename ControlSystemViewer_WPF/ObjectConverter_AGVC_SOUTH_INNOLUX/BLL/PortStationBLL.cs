using com.mirle.ibg3k0.sc.Data;
using NLog;
using ObjectConverterInterface.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewerObject;

namespace ObjectConverter_AGVC_SOUTH_INNOLUX.BLL
{
    public class PortStationBLL : IPortStationBLL
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private string connectionString = "";
        public PortStationBLL(string _connectionString)
        {
            connectionString = _connectionString ?? "";
        }

        public List<VPORTSTATION> GetVPORTSTATION() => GetVPortStations(getvportstation());
        private List<com.mirle.ibg3k0.sc.APORTSTATION> getvportstation()
        {
            List<com.mirle.ibg3k0.sc.APORTSTATION> vportstations = new List<com.mirle.ibg3k0.sc.APORTSTATION>();
            try
            {
                using (DBConnection_EF con = DBConnection_EF.GetUContext())
                {
                    if (!string.IsNullOrWhiteSpace(connectionString))
                        con.Database.Connection.ConnectionString = connectionString;

                    var query = from a in con.APORTSTATION.AsNoTracking()
                                select a;
                    vportstations = query?.ToList();
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
            }

            return vportstations;
        }


        private List<VPORTSTATION> GetVPortStations(List<com.mirle.ibg3k0.sc.APORTSTATION> aPORTSTATIONs)
        {
            List<VPORTSTATION> vPORTSTATIONs = new List<VPORTSTATION>();
            VPORTSTATION vPORTSTATION = null;
            try
            {
                foreach (com.mirle.ibg3k0.sc.APORTSTATION aPORTSTATION in aPORTSTATIONs)
                {
                    vPORTSTATION = GetVPortStation(aPORTSTATION);
                    if (vPORTSTATION != null)
                    {
                        vPORTSTATIONs.Add(vPORTSTATION);
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
            }
            return vPORTSTATIONs;


        }

        private VPORTSTATION GetVPortStation(com.mirle.ibg3k0.sc.APORTSTATION aPORTSTATION)
        {
            VPORTSTATION vPORTSTATION = null;
            try
            {
                vPORTSTATION = new VPORTSTATION
                (
                    portID: aPORTSTATION.PORT_ID,
                    adrID: aPORTSTATION.ADR_ID,
                    stageCount: 0
                );

            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
            }
            return vPORTSTATION;


        }
    }
}
