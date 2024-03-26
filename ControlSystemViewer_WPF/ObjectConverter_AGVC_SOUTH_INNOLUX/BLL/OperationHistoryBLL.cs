using com.mirle.ibg3k0.bcf.Common;
using com.mirle.ibg3k0.sc;
using com.mirle.ibg3k0.sc.App;
using com.mirle.ibg3k0.sc.Data;
using NLog;
using ObjectConverterInterface.BLL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using ViewerObject;

namespace ObjectConverter_AGVC_SOUTH_INNOLUX.BLL
{
    public class OperationHistoryBLL : IOperationHistoryBLL
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        private OperationHistoryConverter alarmConverter = new OperationHistoryConverter();

        private string connectionString = "";
        public OperationHistoryBLL(string _connectionString)
        {
            connectionString = _connectionString ?? "";
        }


        public void InsertOperation(ViewerObject.VOPERATION valarm)
        {
            insertAlarm(alarmConverter.GetOperation(valarm));
        }
        private void insertAlarm(HOPERATION alarm)
        {
            using (DBConnection_EF con = DBConnection_EF.GetUContext())
            {
                if (!string.IsNullOrWhiteSpace(connectionString))
                    con.Database.Connection.ConnectionString = connectionString;

                con.HOPERATION.Add(alarm);
                con.SaveChanges();
            }
        }

        public List<ViewerObject.VOPERATION> LoadOperationsByConditions(DateTime startDatetime, DateTime endDatetime)
                                 => alarmConverter.GetVOperations(getOperationsByConditions(startDatetime, endDatetime));
        private List<HOPERATION> getOperationsByConditions(DateTime startDatetime, DateTime endDatetime)
        {
            List<HOPERATION> alarms = new List<HOPERATION>();
            using (DBConnection_EF con = DBConnection_EF.GetUContext())
            {
                if (!string.IsNullOrWhiteSpace(connectionString))
                    con.Database.Connection.ConnectionString = connectionString;

                var query = from a in con.HOPERATION.AsNoTracking()
                            where a.INSERT_TIME >= startDatetime && a.INSERT_TIME <= endDatetime
                            select a;

                alarms.AddRange(query?.ToList() ?? new List<HOPERATION>());
            }
            return alarms;
        }
    }


    public class OperationHistoryConverter
    {
        public ViewerObject.VOPERATION GetVOperation(HOPERATION input)
        {
            return new ViewerObject.VOPERATION()
            {
                SEQ_NO = input.SEQ_NO,
                T_STAMP = input.T_STAMP,
                USER_ID = input.USER_ID,
                FORM_NAME = input.FORM_NAME,
                ACTION = input.ACTION,
                BUTTON_NAME = "",
                BUTTON_CONTENT = "",
                INSERT_TIME = input.INSERT_TIME
            };
        }
        public List<ViewerObject.VOPERATION> GetVOperations(List<HOPERATION> input)
        {
            List<ViewerObject.VOPERATION> output = new List<ViewerObject.VOPERATION>();
            if (input?.Count > 0)
            {
                foreach (var i in input)
                {
                    var o = GetVOperation(i);
                    if (o != null) output.Add(o);
                }
            }
            return output;
        }

        internal HOPERATION GetOperation(VOPERATION valarm)
        {
            string timeStamp = BCFUtility.formatDateTime(DateTime.Now, SCAppConstants.TimestampFormat_19);

            return new HOPERATION()
            {
                SEQ_NO = Guid.NewGuid().ToString(),
                T_STAMP = timeStamp,
                USER_ID = valarm.USER_ID,
                FORM_NAME = valarm.FORM_NAME,
                ACTION = valarm.ACTION,
                INSERT_TIME = valarm.INSERT_TIME
            };
        }
    }
}