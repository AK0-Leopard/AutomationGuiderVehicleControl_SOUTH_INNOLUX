using CommonMessage.ProtocolFormat.ShelfFun;
using Grpc.Core;
using ObjectConverterInterface.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ObjectConverter_OHBC_ASE_K21.BLL
{
    public class ShelfBLL : IShelfBLL
    {
        private readonly string ns = "ObjectConverter_OHBC_ASE_K21.BLL" + ".ShelfBLL";

        private static readonly string control_web_address = "ohxcv.ha.ohxc.mirle.com.tw";
        private Channel channel = new Channel(control_web_address, 7002, ChannelCredentials.Insecure);

        private DateTime last_update_time = DateTime.MinValue;

        public ShelfBLL()
        {
        }

        public bool GetAllShelves(ref List<ViewerObject.Shelf> shelves, out string result)
        {
            string ms = System.Reflection.MethodBase.GetCurrentMethod().Name;
            result = "";
            try
            {
                //使用Proto檔並傳入連線通道來建立client端
                var client = new shelfGreeter.shelfGreeterClient(channel);
                //建立提出要求的資料 Empty
                //提出要求並回收server端給予的回應
                var reply = client.getAllShelfInfo(new Empty());
                if (reply == null)
                {
                    result = $"{ns}.{ms} - reply = null";
                    return false;
                }
                if (reply.ShelfInfo == null)
                {
                    result = $"{ns}.{ms} - reply.ShelfInfo = null";
                    return false;
                }
                foreach (var info in reply.ShelfInfo)
                {
                    var shelf = shelves.Where(s => s.SHELF_ID == info.ShelfId.Trim()).FirstOrDefault();
                    if (shelf == null) continue;

                    shelf.ENABLE = info.Enable;
                    shelf.BOX_ID = info.BoxId?.Trim() ?? "";
                    shelf.CST_ID = info.CstId?.Trim() ?? "";
                    shelf.SHELF_STATUS = GetShelfStatus(info.ShelfStatus);
                }
                return true;
            }
            catch (Exception ex)
            {
                result = $"{ns}.{ms} - Failed, Exception: {ex.Message}";
                return false;
            }
        }

        public bool UpdateShelves(ref List<ViewerObject.Shelf> shelves, out string result)
        {
            string ms = System.Reflection.MethodBase.GetCurrentMethod().Name;
            result = "";
            try
            {
                //使用Proto檔並傳入連線通道來建立client端
                var client = new shelfGreeter.shelfGreeterClient(channel);
                //建立提出要求的資料
                lastUpdateTime req = new lastUpdateTime();
                req.Datetime = last_update_time.ToBinary();
                last_update_time = DateTime.Now;
                //提出要求並回收server端給予的回應
                var reply = client.getNeedChangeShelf(req);
                if (reply == null)
                {
                    result = $"{ns}.{ms} - reply = null";
                    return false;
                }
                if (reply.ShelfInfo == null)
                {
                    result = $"{ns}.{ms} - reply.ShelfInfo = null";
                    return false;
                }
                foreach (var info in reply.ShelfInfo)
                {
                    var shelf = shelves.Where(s => s.SHELF_ID == info.ShelfId.Trim()).FirstOrDefault();
                    if (shelf == null) continue;

                    shelf.ENABLE = info.Enable;
                    shelf.BOX_ID = info.BoxId?.Trim() ?? "";
                    shelf.CST_ID = info.CstId?.Trim() ?? "";
                    shelf.SHELF_STATUS = GetShelfStatus(info.ShelfStatus);
                }
                return true;
            }
            catch (Exception ex)
            {
                result = $"{ns}.{ms} - Failed, Exception: {ex.Message}";
                return false;
            }
        }

        public ViewerObject.Definition.ShelfStatus GetShelfStatus(shelfStatus status)
        {
            switch (status)
            {
                case shelfStatus.Empty:
                    return ViewerObject.Definition.ShelfStatus.Empty;
                case shelfStatus.Store:
                    return ViewerObject.Definition.ShelfStatus.Stored;
                case shelfStatus.PreIn:
                    return ViewerObject.Definition.ShelfStatus.PreIn;
                case shelfStatus.PreOut:
                    return ViewerObject.Definition.ShelfStatus.PreOut;
                case shelfStatus.Alternate:
                    return ViewerObject.Definition.ShelfStatus.Alternate;
                default:
                    return ViewerObject.Definition.ShelfStatus.Default;
            }
        }

        //public List<ViewerObject.Shelf> LoadAllShelves(List<ViewerObject.Address> addresses)
        //{
        //    var result = new List<ViewerObject.Shelf>();
        //    var shelfDefs = loadAllShelves();
        //    if (addresses?.Count > 0 && shelfDefs?.Count > 0)
        //    {
        //        // 建立 ViewerObject.Shelf 基本資料
        //        foreach (var shelfdef in shelfDefs)
        //        {
        //            var address = addresses.Where(a => a.ID == shelfdef.ADR_ID?.Trim()).FirstOrDefault();
        //            if (address == null) continue;

        //            result.Add(new ViewerObject.Shelf(shelfdef.ShelfID,
        //                                              shelfdef.ZoneID,
        //                                              address,
        //                                              getShelfStatus(shelfdef)));
        //        }

        //        // 計算 Zone Direction Vector
        //        List<string> zoneIDs = result.Select(s => s.ZONE_ID).Distinct().OrderBy(s => s).ToList();
        //        foreach (var zoneID in zoneIDs)
        //        {
        //            var shelvesInZone = result.Where(s => s.ZONE_ID == zoneID).OrderBy(s => s.SNO).ToList();
        //            if (shelvesInZone.Count < 2) continue; // 需要 2 個點才能求方向

        //            Point zone_dir_vec = new Point(shelvesInZone[shelvesInZone.Count - 1].ADDRESS.X - shelvesInZone[0].ADDRESS.X,
        //                                           shelvesInZone[shelvesInZone.Count - 1].ADDRESS.Y - shelvesInZone[0].ADDRESS.Y);

        //            foreach (var shelf in shelvesInZone)
        //            {
        //                shelf.ZONE_DIR_VEC = zone_dir_vec;
        //            }
        //        }
        //    }
        //    return result;
        //}
        //private List<ShelfDef> loadAllShelves()
        //{
        //    using (DBConnection_EF con = DBConnection_EF.GetUContext())
        //    {
        //        if (!string.IsNullOrWhiteSpace(connectionString))
        //            con.Database.Connection.ConnectionString = connectionString;

        //        var query = from cmd in con.ShelfDef.AsNoTracking()
        //                    select cmd;
        //        return query?.ToList() ?? new List<ShelfDef>();
        //    }
        //}
        //private ViewerObject.Definition.ShelfStatus getShelfStatus(ShelfDef shelfDef)
        //{
        //    if (shelfDef == null) return ViewerObject.Definition.ShelfStatus.Unknown;
        //    if (shelfDef.Enable?.Contains("Y") ?? false)
        //    {
        //        switch (shelfDef.ShelfState?.Trim())
        //        {
        //            case ShelfDef.E_ShelfState.EmptyShelf:
        //                return ViewerObject.Definition.ShelfStatus.Empty;
        //            case ShelfDef.E_ShelfState.Stored:
        //                return ViewerObject.Definition.ShelfStatus.Stored;
        //            case ShelfDef.E_ShelfState.StorageInReserved:
        //                return ViewerObject.Definition.ShelfStatus.PreIn;
        //            case ShelfDef.E_ShelfState.RetrievalReserved:
        //                return ViewerObject.Definition.ShelfStatus.PreOut;
        //            case ShelfDef.E_ShelfState.Alternate:
        //                return ViewerObject.Definition.ShelfStatus.Alternate;
        //            default:
        //                return ViewerObject.Definition.ShelfStatus.Unknown;
        //        }
        //    }
        //    else return ViewerObject.Definition.ShelfStatus.Disabled;
        //}
    }
}
