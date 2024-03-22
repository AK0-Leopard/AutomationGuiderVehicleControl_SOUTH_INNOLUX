using com.mirle.ibg3k0.sc;
using com.mirle.ibg3k0.sc.Data;
using com.mirle.ibg3k0.sc.ProtocolFormat.OHTMessage;
using NLog;
using ObjectConverterInterface.BLL;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.SqlServer;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewerObject;

namespace ObjectConverter_OHTC_AT_S_MALASYIA.BLL
{ 
    public class ParkZoneBLL : IParkZoneBLL
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private string connectionString = "";
        private PZConverter pzConverter = new PZConverter();

        public ParkZoneBLL(string _connectionString)
        {
            connectionString = _connectionString ?? "";
        }

        public List<ViewerObject.ParkingZone> GetAllParkingZoneData()
        {
            try
            {
                List<ViewerObject.ParkingZone> rtnlist = new List<ViewerObject.ParkingZone>();
                
                List<APARKZONEMASTER> pzmasters = loadAllParkingZoneMaster();
                Dictionary<string, List<string>> PZaddress = loadAllParkingZoneAddress();
                List<string> pzadrs;
                if(pzmasters != null)
                {
                    foreach (var pzmaster in pzmasters)
                    {
                        if (pzmaster.IS_ACTIVE)
                        {
                            bool getaddress = PZaddress.TryGetValue(pzmaster.PARK_ZONE_ID.Trim(), out pzadrs);
                            if (getaddress)
                            {
                                ViewerObject.ParkingZone pztmp = new ViewerObject.ParkingZone()
                                {
                                    ParkingZoneID = pzmaster.PARK_ZONE_ID.Trim(),
                                    ParkAddressIDs = pzadrs,
                                    AllowedVehicleTypes = new List<E_VH_TYPE>(),
                                    Capacity = pzmaster.TOTAL_BORDER,
                                    LowWaterlevel = pzmaster.LOWER_BORDER,
                                    order = pzmaster.PARK_TYPE,
                                    PullDest = pzmaster.PULL_DEST
                                };
                                pztmp.AllowedVehicleTypes.Add(pzmaster.VEHICLE_TYPE);
                                rtnlist.Add(pztmp);
                            }
                        }
                    }
                }
                
                return rtnlist;
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception:");
                throw;
            }
        }

        public List<ViewerObject.ParkZoneMaster> LoadParkingZoneMaster() => pzConverter.GetPZMaster(loadAllParkingZoneMaster());
        public List<APARKZONEMASTER> loadAllParkingZoneMaster()
        {
            List<APARKZONEMASTER> rtnlist = null;

            try
            {
                using (DBConnection_EF con = DBConnection_EF.GetUContext())
                {
                    var query = from obj in con.APARKZONEMASTER
                                orderby obj.PARK_ZONE_ID
                                select obj;

                    rtnlist = query.ToList();
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
            }

            return rtnlist;
        }

        public Dictionary<string, List<string>> loadAllParkingZoneAddress()
        {
            Dictionary<string, List<string>> rtnlist = new Dictionary<string, List<string>>();

            try
            {
                using (DBConnection_EF con = DBConnection_EF.GetUContext())
                {
                    var query = from aparkzonedetail in con.APARKZONEDETAIL
                                group aparkzonedetail by aparkzonedetail.PARK_ZONE_ID into g
                                select new
                                {
                                    g.Key,
                                    ADRs = from adrs in g
                                           orderby adrs.PRIO
                                           select adrs
                                };

                    foreach (var q in query)
                    {
                        string pzid = q.Key.Trim();
                        List<string> pzaddresses = new List<string>();

                        foreach (var addr in q.ADRs)
                        {
                            pzaddresses.Add(addr.ADR_ID.Trim());
                        }
                        rtnlist.Add(pzid, pzaddresses);
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
            }

            return rtnlist;
        }

        public List<ViewerObject.ParkZoneDetail> LoadParkingZoneDetail(string address) => pzConverter.GetPZDetails(loadParkingZoneDetail(address));
        public List<APARKZONEDETAIL> loadParkingZoneDetail(string address)
        {
            List<APARKZONEDETAIL> rtnlist = null;

            try
            {
                using (DBConnection_EF con = DBConnection_EF.GetUContext())
                {
                    var query = from obj in con.APARKZONEDETAIL
                                where obj.ADR_ID.Trim() == address.Trim()
                                orderby obj.PARK_ZONE_ID
                                select obj;

                    rtnlist = query.ToList();
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
            }

            return rtnlist;
        }

    }

    public class PZConverter
    {
        public ViewerObject.ParkZoneMaster GetPZMaster(APARKZONEMASTER input)
        {
            ViewerObject.ParkZoneMaster output = null;
            if (input != null)
            {
                output = new ViewerObject.ParkZoneMaster();

                output.PARK_TYPE_ID = input.PARK_TYPE_ID?.Trim() ?? "";
                output.PARK_ZONE_ID = input.PARK_ZONE_ID?.Trim() ?? "";
                output.ENTRY_ADR_ID = input.ENTRY_ADR_ID?.Trim() ?? "";
                output.TOTAL_BORDER = input.TOTAL_BORDER;
                output.LOWER_BORDER = input.LOWER_BORDER;
                output.IS_ACTIVE = input.IS_ACTIVE;
                output.PULL_DEST = input.PULL_DEST;

                switch(input.VEHICLE_TYPE)
                {
                    case E_VH_TYPE.None:
                        output.VEHICLE_TYPE = 0;
                        break;
                    case E_VH_TYPE.Type1:
                        output.VEHICLE_TYPE = 1;
                        break;
                    case E_VH_TYPE.Type2:
                        output.VEHICLE_TYPE = 2;
                        break;
                    case E_VH_TYPE.Type3:
                        output.VEHICLE_TYPE = 3;
                        break;
                    case E_VH_TYPE.Type4:
                        output.VEHICLE_TYPE = 4;
                        break;
                    default:
                        break;
                }

                switch(input.PARK_TYPE)
                {
                    case E_PARK_TYPE.OrderByAsc:
                        output.PARK_TYPE = 1;
                        break;
                    case E_PARK_TYPE.OrderByDes:
                        output.PARK_TYPE = 2;
                        break;
                    default:
                        break;
                }
            }
            return output;
        }

        public List<ViewerObject.ParkZoneMaster> GetPZMaster(List<APARKZONEMASTER> input)
        {
            List<ViewerObject.ParkZoneMaster> output = new List<ViewerObject.ParkZoneMaster>();
            if (input?.Count > 0)
            {
                foreach (var i in input)
                {
                    var o = GetPZMaster(i);
                    if (o != null) output.Add(o);
                }
            }
            return output;
        }

        public ViewerObject.ParkZoneDetail GetPZDetail(APARKZONEDETAIL input)
        {
            ViewerObject.ParkZoneDetail output = null;
            if (input != null)
            {
                output = new ViewerObject.ParkZoneDetail()
                {
                    PARK_ZONE_ID = input.PARK_ZONE_ID?.Trim() ?? "",
                    ADR_ID = input.ADR_ID?.Trim() ?? "",
                    PRIO = input.PRIO,
                    CAR_ID = input.CAR_ID?.Trim() ?? ""
                };
            }
            return output;
        }

        public List<ViewerObject.ParkZoneDetail> GetPZDetails(List<APARKZONEDETAIL> input)
        {
            List<ViewerObject.ParkZoneDetail> output = new List<ViewerObject.ParkZoneDetail>();
            if (input?.Count > 0)
            {
                foreach (var i in input)
                {
                    var o = GetPZDetail(i);
                    if (o != null) output.Add(o);
                }
            }
            return output;
        }
    }
}
