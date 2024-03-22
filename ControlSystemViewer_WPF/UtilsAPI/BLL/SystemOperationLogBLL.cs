using com.mirle.ibg3k0.ohxc.wpf.App;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using ViewerObject;

namespace UtilsAPI.BLL
{
    public class SystemOperationLogBLL
    {
        private static Logger logger = LogManager.GetLogger("SystemOperationLogger");
        WindownApplication app = null;
        private  Dictionary<string, string> Data = new Dictionary<string, string>();
        private Dictionary<string, string> NewData = new Dictionary<string, string>();

        public SystemOperationLogBLL(WindownApplication _app)
        {
            app = _app;
        }

        public void addData_KeyValue(string key,string value_Data,string value_NewData =null)
        {
            try
            {
                Data.Add(key, value_Data);
                if(value_NewData != null)
                {
                    NewData.Add(key, value_NewData);
                }
                
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Execption:");
            }
        }


        public void addSystemOperationHis(string result, Dictionary<string, string> _Data = null, Dictionary<string, string> _NewData = null, [CallerMemberName] string Method = "")
        {
            try
            {
                if (_Data == null) _Data = Data;
                if (_NewData == null) _NewData = NewData;
                SYSTEMVOPERATION opLog = new SYSTEMVOPERATION()
                {
                    TIME = DateTime.Now.ToString("yyyyMMddHHmmssfffff"),
                    USER_ID = app.LoginUserID,
                    FUNCTION = $"({Method})",
                    RESULT = result,
                    input = _Data,
                    input2 = _NewData
                };
                PrintOperationLog(opLog);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Execption:");
            }
        }

        public  void PrintOperationLog(SYSTEMVOPERATION opLog)
        {
            try
            {
                if (opLog == null) return;
                StringBuilder sb = new StringBuilder();
                sb.AppendLine();
                sb.AppendLine(string.Format("{0}Time: {1}", new string(' ', 5), DateTime.Now.ToString("yyyyMMddHHmmssfffff")));
                sb.AppendLine(string.Format("{0}User: {1}", new string(' ', 5), opLog.USER_ID));
                sb.AppendLine(string.Format("{0}Function: {1}", new string(' ', 5), opLog.FUNCTION));
                sb.AppendLine(string.Format("{0}Result: {1}", new string(' ', 5), opLog.RESULT));
                sb.AppendLine(string.Format("{0}Data:", new string(' ', 5)));

                if(opLog.input2.Count == 0)
                {
                    if ( opLog.input.Count > 0)
                    {
                        foreach (var item in opLog.input)
                        {
                            sb.AppendLine(string.Format("{0}{1}: {2}", new string(' ', 7), item.Key.ToString(), item.Value.ToString()));
                            //sb.AppendLine(string.Format("{0}         {1} => {2}", new string(' ', 7), item.Key.ToString(), item.Value.ToString()));
                        }
                    }
                }
                else
                {
                    if (opLog.input.Count > 0 && (opLog.input.Count ==opLog.input2.Count))
                    {
                        foreach (var item in opLog.input)
                        {
                            sb.AppendLine(string.Format("{0}{1}: {2} => {3}", new string(' ', 7), item.Key.ToString(), item.Value.ToString(),opLog.input2[item.Key.ToString()]));
                        }
                    }
                }              
                logger.Info(sb.ToString());
                sb.Clear();
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Execption:");
            }
            finally
            {
                if (Data.Count > 0) Data.Clear();
                if (NewData.Count > 0) NewData.Clear();
            }
        }
    }
}
