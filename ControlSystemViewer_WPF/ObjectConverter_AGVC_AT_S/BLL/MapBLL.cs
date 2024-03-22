using com.mirle.ibg3k0.sc;
using com.mirle.ibg3k0.sc.App;
using com.mirle.ibg3k0.sc.Data;
using ObjectConverterInterface.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObjectConverter_AGVC_AT_S.BLL
{
    public class MapBLL : IMapBLL
    {
        private readonly string ns = "ObjectConverter_AGVC_AT_S.BLL" + ".MapBLL";

        private string connectionString = "";
        public MapBLL(string _connectionString)
        {
            connectionString = _connectionString ?? "";
        }

        //public List<ViewerObject.?> loadAllUsingBlockQueue() => ?Converter.GetVBLOCKZONEQUEUEs(loadAllUsingBlockQueue());
        private List<BLOCKZONEQUEUE> loadAllUsingBlockQueue()
        {
            List<BLOCKZONEQUEUE> result = new List<BLOCKZONEQUEUE>();
            using (DBConnection_EF con = DBConnection_EF.GetUContext())
            {
                if (!string.IsNullOrWhiteSpace(connectionString))
                    con.Database.Connection.ConnectionString = connectionString;

                var query = from block in con.BLOCKZONEQUEUE.AsNoTracking()
                            where (block.STATUS.CompareTo(SCAppConstants.BlockQueueState.Request) >= 0) &&
                            (block.STATUS.CompareTo(SCAppConstants.BlockQueueState.Release) < 0)
                            orderby block.REQ_TIME
                            select block;
                result.AddRange(query?.ToList() ?? new List<BLOCKZONEQUEUE>());
            }
            return result;
        }
    }
}
