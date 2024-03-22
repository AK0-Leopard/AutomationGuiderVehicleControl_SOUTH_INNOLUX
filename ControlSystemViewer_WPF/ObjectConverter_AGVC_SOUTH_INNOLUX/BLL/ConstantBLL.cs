using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.mirle.ibg3k0.sc.Data;
using ObjectConverterInterface;
using ViewerObject;
using CommonMessage.ProtocolFormat.ControllerSettingFun;
using Grpc.Core;

namespace ObjectConverter_AGVC_SOUTH_INNOLUX.BLL
{
    public class ConstantBLL : IConstant
    {
        //private DBConnection_EF con;
        private static readonly string grpcServer = "ohxcv.ha.ohxc.mirle.com.tw";
        //private static readonly string segControlServer = "127.0.0.1";
        private Channel channel = new Channel(grpcServer, 7004, ChannelCredentials.Insecure);
        private string connectionString = "";
        public ConstantBLL(string _conntectionString)
        {
            connectionString = _conntectionString;


        }
        public List<VCONSTANT> getAllVConstant()
        {
            var constants = getAllConstant();
            List<VCONSTANT> result;
            if (constants == null)
                return null;
            else
                result = new List<VCONSTANT>();
            foreach (var constant in constants)
            {
                VCONSTANT vconstant = new VCONSTANT();
                vconstant.ECID = constant.ECID;
                vconstant.EQPT_REAL_ID = constant.EQPT_REAL_ID;
                vconstant.ECNAME = constant.ECNAME;
                vconstant.ECMIN = constant.ECMIN;
                vconstant.ECMAX = constant.ECMAX;
                vconstant.ECV = constant.ECV;
                result.Add(vconstant);
            }

            return result;
        }

        public (bool, ControllerParameterSettingReply) constantControl(ControllerParameterSettingRequest request)
        {
            bool isSuccess = false;
            ControllerParameterSettingReply reply;
            ControllerSettingFunGreeter.ControllerSettingFunGreeterClient client;
            client = new ControllerSettingFunGreeter.ControllerSettingFunGreeterClient(channel);
            try
            {
                reply = client.ControllerParameterSetting(
                    request: request,
                    deadline: DateTime.UtcNow.AddSeconds(5));
                isSuccess = true;
            }
            catch (Exception ex)
            {
                reply = new ControllerParameterSettingReply();
                reply.Result = ex.Message;
                isSuccess = false;
            }
            return (isSuccess, reply);
        }

        private List<com.mirle.ibg3k0.sc.AECDATAMAP> getAllConstant()
        {
            List<com.mirle.ibg3k0.sc.AECDATAMAP> result;
            try
            {
                using (DBConnection_EF con = DBConnection_EF.GetUContext())
                {
                    if (!string.IsNullOrWhiteSpace(connectionString))
                        con.Database.Connection.ConnectionString = connectionString;
                    result = con.AECDATAMAP.ToList();
                }
            }
            catch (Exception ex)
            {
                result = null;
            }
            return result;
            //return con.AECDATAMAP.ToList<com.mirle.ibg3k0.sc.AECDATAMAP>();
        }

        public void resetBuzzer()
        {
            var client = new ControllerSettingFunGreeter.ControllerSettingFunGreeterClient(channel);
            try
            {
                var reply = client.resetBuzzer(
                     request: new Empty(),
                     deadline: DateTime.UtcNow.AddSeconds(5));
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
