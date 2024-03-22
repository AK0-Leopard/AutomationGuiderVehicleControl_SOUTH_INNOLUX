using BasicFunction;
using com.mirle.ibg3k0.sc;
using com.mirle.ibg3k0.sc.Data;
using com.mirle.ibg3k0.sc.Data.DAO;
using com.mirle.ibg3k0.sc.Data.DAO.EntityFramework;
using ObjectConverterInterface;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Security;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace ObjectConverter_OHBC_ASE_K24
{
    public class DBTableWatcher : IDBTableWatcher
    {
        private static readonly string ns = "ObjectConverter_OHBC_ASE_K24" + ".DBTableWatcher";

        private DBConnection_EF dbCon = new DBConnection_EF();

        #region Dao
        private UserDao userDao = new UserDao();
        private UserGroupDao userGroupDao = new UserGroupDao();
        private UserFuncDao userFuncDao = new UserFuncDao();
        private PortStationDao portStationDao = new PortStationDao();
        private SegmentDao segmentDao = new SegmentDao();
        private CMD_OHTCDao commandDao = new CMD_OHTCDao();
        private CMD_MCSDao transferDao = new CMD_MCSDao();
        //private CassetteDao cassetteDao = new CassetteDao();
        private AlarmDao alarmDao = new AlarmDao();
        private ShelfDefDao shelfDefDao = new ShelfDefDao();
        #endregion Dao

        #region ImmediateNotificationRegister
        private ImmediateNotificationRegister<UASUSR> userNotification = null;
        private ImmediateNotificationRegister<UASUSRGRP> userGroupNotification = null;
        private ImmediateNotificationRegister<UASUFNC> userGroupFuncNotification = null;
        private ImmediateNotificationRegister<UASFNC> funcCodeNotification = null;
        private ImmediateNotificationRegister<APORTSTATION> portStationNotification = null;
        private ImmediateNotificationRegister<ASEGMENT> segmentNotification = null;
        private ImmediateNotificationRegister<ACMD_OHTC> commandNotification = null;
        private ImmediateNotificationRegister<ACMD_MCS> transferNotification = null;
        //private ImmediateNotificationRegister<ACASSETTE> cassetteNotification = null;
        private ImmediateNotificationRegister<ALARM> alarmNotification = null;
        #endregion ImmediateNotificationRegister

        #region EventHandler
        public event EventHandler UasUserChange;
        public event EventHandler UasUserGroupChange;
        public event EventHandler UasGroupFuncChange;
        public event EventHandler UasFuncCodeChange;
        public event EventHandler PortStationChange;
        public event EventHandler SegmentChange;
        public event EventHandler SectionChange;
        public event EventHandler CommandChange;
        public event EventHandler TransferChange;
        public event EventHandler CarrierChange;
        public event EventHandler AlarmChange;
        public event EventHandler ConstantChange;
        #endregion EventHandler

        public DBTableWatcher(string connectionString, out string result)
        {
            string ms = System.Reflection.MethodBase.GetCurrentMethod().Name;
            result = "";
            dbCon.Database.Connection.ConnectionString = connectionString;
            ImmediateNotificationRegister<object>.StartMonitor(dbCon);
            bool isSuccess = canRequestNotifications(out result);
            if (isSuccess) start(out result);

            if (!string.IsNullOrWhiteSpace(result))
                result = $"{ns}.{ms} - {result}";
        }
        private bool canRequestNotifications(out string result)
        {
            result = "";
            // In order to use the callback feature of the
            // SqlDependency, the application must have
            // the SqlClientPermission permission.
            try
            {
                SqlClientPermission perm =
                    new SqlClientPermission(
                    PermissionState.Unrestricted);

                perm.Demand();

                return true;
            }
            //catch (SecurityException se)
            //{
            //    return false;
            //}
            catch (Exception ex)
            {
                result = ex.Message;
                return false;
            }
        }
        private void start(out string result)
        {
            string ms = System.Reflection.MethodBase.GetCurrentMethod().Name;
            result = "";
            string doing = "";
            try
            {
                doing = "setting userNotification";
                userNotification = new ImmediateNotificationRegister<UASUSR>(dbCon, userDao.getQueryAllSQL(dbCon));
                userNotification.OnChanged += NotificationUasUserOnChanged;
                doing = "setting userGroupNotification";
                userGroupNotification = new ImmediateNotificationRegister<UASUSRGRP>(dbCon, userGroupDao.getQueryAllSQL(dbCon));
                userGroupNotification.OnChanged += NotificationUasUserGroupOnChanged;
                doing = "setting userGroupFuncNotification";
                userGroupFuncNotification = new ImmediateNotificationRegister<UASUFNC>(dbCon, userFuncDao.getQueryAllSQL(dbCon));
                userGroupFuncNotification.OnChanged += NotificationUasGroupFuncOnChanged;
                doing = "setting funcCodeNotification";
                funcCodeNotification = new ImmediateNotificationRegister<UASFNC>(dbCon, getQueryAllSQL_UASFNC(dbCon));
                funcCodeNotification.OnChanged += NotificationUasFuncCodeOnChanged;
                doing = "setting portStationNotification";
                portStationNotification = new ImmediateNotificationRegister<APORTSTATION>(dbCon, portStationDao.getQueryAllSQL(dbCon));
                portStationNotification.OnChanged += NotificationPositionStationOnChanged;
                doing = "setting segmentNotification";
                segmentNotification = new ImmediateNotificationRegister<ASEGMENT>(dbCon, getQueryAllSQL_ASEGMENT(dbCon));
                segmentNotification.OnChanged += NotificationSegmentOnChanged;
                doing = "setting commandNotification";
                commandNotification = new ImmediateNotificationRegister<ACMD_OHTC>(dbCon, commandDao.getQueryAllSQL(dbCon));
                commandNotification.OnChanged += NotificationCommandOnChanged;
                commandNotification.OnChanged += NotificationTransferOnChanged;
                doing = "setting transferNotification";
                transferNotification = new ImmediateNotificationRegister<ACMD_MCS>(dbCon, transferDao.getQueryAllSQL(dbCon));
                transferNotification.OnChanged += NotificationTransferOnChanged;
                //doing = "setting cassetteNotification";
                //cassetteNotification = new ImmediateNotificationRegister<ACASSETTE>(dbCon, cassetteDao.getQueryAllSQL(dbCon));
                //cassetteNotification.OnChanged += NotificationCarrierOnChanged;
                doing = "setting alarmNotification";
                alarmNotification = new ImmediateNotificationRegister<ALARM>(dbCon, getQueryAllSQL_ALARM(dbCon));
                alarmNotification.OnChanged += NotificationAlarmOnChanged;
            }
            catch
            {
                result = $"{ns}.{ms} - Exception occurred while {doing}";
            }
        }

        void NotificationUasUserOnChanged(object sender, EventArgs e)
        {
            UasUserChange?.Invoke(this, e);
        }
        void NotificationUasUserGroupOnChanged(object sender, EventArgs e)
        {
            UasUserGroupChange?.Invoke(this, e);
        }
        void NotificationUasGroupFuncOnChanged(object sender, EventArgs e)
        {
            UasGroupFuncChange?.Invoke(this, e);
        }
        void NotificationUasFuncCodeOnChanged(object sender, EventArgs e)
        {
            UasFuncCodeChange?.Invoke(this, e);
        }
        void NotificationPositionStationOnChanged(object sender, EventArgs e)
        {
            PortStationChange?.Invoke(this, e);
        }
        void NotificationSegmentOnChanged(object sender, EventArgs e)
        {
            SegmentChange?.Invoke(this, e);
        }
        void NotificationCommandOnChanged(object sender, EventArgs e)
        {
            CommandChange?.Invoke(this, e);
        }
        void NotificationTransferOnChanged(object sender, EventArgs e)
        {
            TransferChange?.Invoke(this, e);
        }

        //void NotificationCarrierOnChanged(object sender, EventArgs e)
        //{
        //    CarrierChange?.Invoke(this, e);
        //}
        void NotificationAlarmOnChanged(object sender, EventArgs e)
        {
            AlarmChange?.Invoke(this, e);
        }

        #region getQueryAllSQL
        private IQueryable getQueryAllSQL_UASFNC(DBConnection_EF conn)
        {
            return conn.UASFNC.Select((UASFNC f) => f);
        }
        private IQueryable getQueryAllSQL_ALARM(DBConnection_EF conn)
        {
            return conn.ALARM.Select((ALARM a) => a);
        }
        private IQueryable getQueryAllSQL_ASEGMENT(DBConnection_EF conn)
        {
            return conn.ASEGMENT.Select((ASEGMENT seg) => seg);
        }
        #endregion getQueryAllSQL
    }
}
