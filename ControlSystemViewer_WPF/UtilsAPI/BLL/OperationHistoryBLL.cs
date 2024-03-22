using com.mirle.ibg3k0.ohxc.wpf.App;
using NLog;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using ViewerObject;

namespace UtilsAPI.BLL
{
    public class OperationHistoryBLL
    {
        private static Logger logger = LogManager.GetLogger("OperationLogger");
        WindownApplication app = null;

        public OperationHistoryBLL(WindownApplication _app)
        {
            app = _app;
        }

        /// 紀錄Operation操作Log，各參數可看Summary
        /// </summary>
        /// <param name="user_id"> UserID</param>
        /// <param name="formName"> Form名稱</param>
        /// <param name="action">自定義訊息</param>
        /// <param name="result">執行結果(ActionType=None時不啟用)</param>
        /// <param name="ActionType">Action種類，預設為None</param>
        /// <param name="input1">紀錄資料用Dictionary，EditData時為Before資料</param>
        /// <param name="input2">只有ActionType為EditData時才會啟用的Dictionary，紀錄After資料</param>
        /// <param name="Method">Method，預設不代入由系統處理</param>
        public void addOperationHis(string user_id, string formName, string action, string ButtonName = "",string ButtonContent ="",Dictionary<string,string> input=null, [CallerMemberName] string Method = "")
        {
            try
            {
                VOPERATION opLog = new VOPERATION()
                {
                    T_STAMP = DateTime.Now.ToString("yyyyMMddHHmmssfffff"),
                    USER_ID = user_id,
                    FORM_NAME = $"{formName}-({Method})",
                    ACTION = action,
                    BUTTON_NAME = ButtonName,
                    BUTTON_CONTENT=ButtonContent,
                    input = input
                };
                PrintOperationLog(opLog);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Execption:");
            }
        }
        public static void PrintOperationLog(VOPERATION opLog)
        {
            try
            {
                if (opLog == null) return;
                StringBuilder sb = new StringBuilder();
                sb.AppendLine();
                sb.AppendLine(string.Format("{0}Time: {1}", new string(' ', 5), opLog.T_STAMP));
                sb.AppendLine(string.Format("{0}User: {1}", new string(' ', 5), opLog.USER_ID));
                sb.AppendLine(string.Format("{0}UI Name: {1}", new string(' ', 5), opLog.FORM_NAME));
                sb.AppendLine(string.Format("{0}Button Name: {1}", new string(' ', 5), opLog.BUTTON_NAME));
                sb.AppendLine(string.Format("{0}Button Content: {1}", new string(' ', 5), opLog.BUTTON_CONTENT));
                sb.AppendLine(string.Format("{0}Action: ", new string(' ', 5)));

                if(opLog.ACTION != "")
                {
                    sb.AppendLine(string.Format("{0}         {1}", new string(' ', 5), opLog.ACTION));
                }
                
                if (opLog.input != null && opLog.input.Count > 0)
                {        
                    foreach (var item in opLog.input)
                    {
                        sb.AppendLine(string.Format("{0}         {1} => {2}", new string(' ', 7), item.Key.ToString(),item.Value.ToString()));
                    }
                }

                logger.Info(sb.ToString());
                sb.Clear();
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Execption:");
            }
        }
    }
}
