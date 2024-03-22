using com.mirle.ibg3k0.sc.Data;
using NLog;
using ObjectConverterInterface.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewerObject.REPORT;

namespace ObjectConverter_OHTC_M4.BLL
{
    public class PLCBLL : IPLCBLL
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private string connectionString = "";
        public PLCBLL(string _connectionString)
        {
            connectionString = _connectionString ?? "";
        }

        public List<VHIDINFO> GetHidinfoByDate(DateTime startDatetime, DateTime endDatetime, string eqptID = null) 
                        => GetVHidinfo(getHidinfoByDate(startDatetime, endDatetime, eqptID));
        private List<com.mirle.ibg3k0.sc.HIDINFO> getHidinfoByDate(DateTime startDatetime, DateTime endDatetime, string eqptID = null)
        {
            List<com.mirle.ibg3k0.sc.HIDINFO> vhidinfos = new List<com.mirle.ibg3k0.sc.HIDINFO>();
            try
            {
                using (DBConnection_EF con = DBConnection_EF.GetUContext())
                {
                    if (!string.IsNullOrWhiteSpace(connectionString))
                        con.Database.Connection.ConnectionString = connectionString;

                    var query = from a in con.HIDINFO.AsNoTracking()
                                where a.UPD_TIME >= startDatetime && a.UPD_TIME <= endDatetime 
                                select a;
                    var list = query?.ToList().Where(t =>
                    {
                        bool b = true;
                        if (eqptID != null)
                        {
                            b = b & (t.HID_ID?.Trim() == eqptID.Trim());
                        }
                        return b;
                    })?.ToList() ?? new List<com.mirle.ibg3k0.sc.HIDINFO>();
                    vhidinfos.AddRange(list);
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
            }
            return vhidinfos;
        }

        private List<VHIDINFO> GetVHidinfo( List<com.mirle.ibg3k0.sc.HIDINFO> hIDINFOs)
        {
            List<VHIDINFO> vHIDINFOs = new List<VHIDINFO>();
            VHIDINFO vHIDINFO = null;
            try
            {
                foreach(com.mirle.ibg3k0.sc.HIDINFO hIDINFO in hIDINFOs)
                {
                    vHIDINFO = GetVHidinfo(hIDINFO);
                    if (vHIDINFO != null)
                    {
                        vHIDINFOs.Add(vHIDINFO);
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
            }
            return vHIDINFOs;

        }

        private VHIDINFO GetVHidinfo( com.mirle.ibg3k0.sc.HIDINFO hIDINFO)
        {
            VHIDINFO vHIDINFO = null;
            try
            {
                vHIDINFO = new VHIDINFO()
                {
                    HID_ID = hIDINFO.HID_ID,
                    Sigma_W_Converted = hIDINFO.Hour_Sigma_Converted.HasValue ? Math.Round(hIDINFO.Hour_Sigma_Converted.Value / 1000) : 0,
                    W_Converted = hIDINFO.Sigma_W_Converted,
                    V_Converted = hIDINFO.Sigma_V_Converted,
                    A_Converted = hIDINFO.Sigma_A_Converted,
                    UPD_TIME = hIDINFO.UPD_TIME
                };
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
            }
            return vHIDINFO;
        }
    }
}
