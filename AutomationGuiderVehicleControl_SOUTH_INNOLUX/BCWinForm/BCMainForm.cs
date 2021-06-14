﻿//*********************************************************************************
//      BCMainForm.cs
//*********************************************************************************
// File Name: BCMainForm.cs
// Description: BC Main Form
//
//(c) Copyright 2014, MIRLE Automation Corporation
//
// Date               Author       Request No.  Tag     Description
// ------------- -------------  -------------  ------  -----------------------------
//**********************************************************************************
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using com.mirle.ibg3k0.bc.winform.App;
using com.mirle.ibg3k0.bcf.Common;
using com.mirle.ibg3k0.sc.Data.VO;
using com.mirle.ibg3k0.bc.winform.UI;
using NLog;
using com.mirle.ibg3k0.bcf.App;
using com.mirle.ibg3k0.sc.App;
using com.mirle.ibg3k0.sc.Common;
using System.Threading;
using com.mirle.ibg3k0.sc.Data.ValueDefMapAction;
using com.mirle.ibg3k0.sc.Data.SECS;
using System.Reflection;
using com.mirle.ibg3k0.bcf.Controller;
using com.mirle.ibg3k0.sc.BLL;
using com.mirle.ibg3k0.bc.winform.UI.UAS;
using System.Diagnostics;
using com.mirle.ibg3k0.bc.winform.Common;
using com.mirle.ibg3k0.sc.MQTT;
using com.mirle.ibg3k0.sc;

namespace com.mirle.ibg3k0.bc.winform
{//test
    /// <summary>
    /// Class BCMainForm.
    /// </summary>
    /// <seealso cref="System.Windows.Forms.Form" />
    public partial class BCMainForm : Form
    {
        /// <summary>
        /// The MPC tip MSG log
        /// </summary>
        private static Logger mpcTipMsgLog = LogManager.GetLogger("MPCTipMessageLog");
        /// <summary>
        /// The master pc memory log
        /// </summary>
        private static Logger masterPCMemoryLog = LogManager.GetLogger("MasterPCMemory");

        /// <summary>
        /// The bc application
        /// </summary>
        private BCApplication bcApp = null;
        /// <summary>
        /// Gets the bc application.
        /// </summary>
        /// <value>The bc application.</value>
        public BCApplication BCApp
        {
            get { return bcApp; }
        }
        /// <summary>
        /// The logger
        /// </summary>
        private static Logger logger = LogManager.GetCurrentClassLogger();
        /// <summary>
        /// The open forms
        /// </summary>
        Dictionary<String, Form> openForms = new Dictionary<string, Form>();
        public Dictionary<String, Form> OpenForms { get { return openForms; } }
        /// <summary>
        /// The pic_ lock
        /// </summary>
        Image Pic_Lock = null;
        /// <summary>
        /// The pic_ unlock
        /// </summary>
        Image Pic_Unlock = null;

        /// <summary>
        /// The b c_ identifier
        /// </summary>
        string BC_ID = "";
        string ServerName = "";
        /// <summary>
        /// The ci
        /// </summary>
        CommonInfo ci;
        /// <summary>
        /// The line
        /// </summary>
        ALINE line;

        private DateTime lastMove_DT = DateTime.Now;          //A0.40

        public bool isAutoOpenTip = true;
        /// <summary>
        /// Sets the information.
        /// </summary>
        /// <param name="setLable">The set lable.</param>
        /// <param name="setColor">Color of the set.</param>
        /// <param name="setForeColor">Color of the set fore.</param>
        private void setInfo(Label setLable, Color setColor, Color setForeColor)
        {
            setLable.BackColor = setColor;
            setLable.ForeColor = setForeColor;
        }

        /// <summary>
        /// Sets the information.
        /// </summary>
        /// <param name="setLable">The set lable.</param>
        /// <param name="setText">The set text.</param>
        /// <param name="setColor">Color of the set.</param>
        /// <param name="setForeColor">Color of the set fore.</param>
        private void setInfo(Label setLable, string setText, Color setColor, Color setForeColor)
        {
            setLable.Text = setText;
            setLable.BackColor = setColor;
            setLable.ForeColor = setForeColor;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BCMainForm"/> class.
        /// </summary>
        /// <param name="bcid">The bcid.</param>
        public BCMainForm(string bcid, string server_name)
        {
            InitializeComponent();
            Adapter.Initialize();
            BC_ID = bcid;
            ServerName = server_name;
            Pic_Lock = global::com.mirle.ibg3k0.bc.winform.Properties.Resources.lock1;
            Pic_Unlock = global::com.mirle.ibg3k0.bc.winform.Properties.Resources.unlock;

            ErrorOriginalColor = tssl_Error_Value.BackColor;
            WarnOriginalColor = tssl_Warn_Value.BackColor;

            logger.Error("Error Test");
            //Application
            BCFApplication.addErrorMsgHandler(errorLogHandler);
            BCFApplication.addWarningMsgHandler(warnLogHandler);
            BCFApplication.addInfoMsgHandler(infoLogHandler);

            GlobalMouseHandler.MouseMovedEvent += GlobalMouseHandler_MouseMovedEvent;
            Application.AddMessageFilter(new GlobalMouseHandler());
        }
        private void GlobalMouseHandler_MouseMovedEvent(object sender, MouseEventArgs e)
        {
            lastMove_DT = DateTime.Now;
        }


        #region Tip Message
        /// <summary>
        /// Errors the log handler.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="LogEventArgs"/> instance containing the event data.</param>
        private void errorLogHandler(Object sender, LogEventArgs args)
        {
            mpcTipMsgLog.Error(args.Message);
            Adapter.BeginInvoke(new SendOrPostCallback((o1) =>
            {
                MPCTipMessage tipMsg = new MPCTipMessage()
                {
                    MsgLevel = sc.ProtocolFormat.OHTMessage.MsgLevel.Error,
                    Msg = args.Message,
                    XID = args.XID
                };
                ci.addMPCTipMsg(tipMsg);
                popUpMPCTipMessageDialog(true);
            }), null);
        }

        /// <summary>
        /// Warns the log handler.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="LogEventArgs"/> instance containing the event data.</param>
        private void warnLogHandler(Object sender, LogEventArgs args)
        {
            mpcTipMsgLog.Warn(args.Message);
            Adapter.BeginInvoke(new SendOrPostCallback((o1) =>
            {
                MPCTipMessage tipMsg = new MPCTipMessage()
                {
                    MsgLevel = sc.ProtocolFormat.OHTMessage.MsgLevel.Warn,
                    Msg = args.Message,
                    XID = args.XID
                };
                ci.addMPCTipMsg(tipMsg);
                popUpMPCTipMessageDialog();
            }), null);
        }

        /// <summary>
        /// Informations the log handler.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="LogEventArgs"/> instance containing the event data.</param>
        private void infoLogHandler(Object sender, LogEventArgs args)
        {
            mpcTipMsgLog.Info(args.Message);
            Adapter.BeginInvoke(new SendOrPostCallback((o1) =>
            {
                MPCTipMessage tipMsg = new MPCTipMessage()
                {
                    MsgLevel = sc.ProtocolFormat.OHTMessage.MsgLevel.Info,
                    Msg = args.Message,
                    XID = args.XID
                };
                ci.addMPCTipMsg(tipMsg);
                popUpMPCTipMessageDialog(true);
            }), null);
        }

        /// <summary>
        /// MPLCs the handshake timeout.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="ErrorEventArgs"/> instance containing the event data.</param>
        private void mplcHandshakeTimeout(object sender, ErrorEventArgs e)
        {
            BCFApplication.onErrorMsg(String.Format("MPLC Handshake Timeout: {0}", e.ErrorMsg));
        }

        /// <summary>
        /// Pops up MPC tip message dialog.
        /// </summary>
        private void popUpMPCTipMessageDialog(bool isNeedOpen = false)
        {
            if (isNeedOpen && isAutoOpenTip)
                openForm(typeof(MPCInfoMsgDialog).Name, true, false);
        }
        #endregion Tip Message

        #region Initialze
        /// <summary>
        /// Handles the Load event of the BCMainForm control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void BCMainForm_Load(object sender, EventArgs e)
        {
            try
            {
                ProgressBarDialog progress = new ProgressBarDialog(bcApp);
                System.Threading.ThreadPool.QueueUserWorkItem(new WaitCallback(callBackDoInitialize), progress);
                if (progress != null && !progress.IsDisposed)
                {
                    progress.ShowDialog();
                }
                //#if DEBUG
                openForm(typeof(OHT_Form).Name);
                //#endif
                timer1.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, String.Format("Exception: {0}", ex));
                logger.Error(ex, "Exception");
            }
        }
        /// <summary>
        /// Calls the back do initialize.
        /// </summary>
        /// <param name="status">The status.</param>
        private void callBackDoInitialize(Object status)
        {
            ProgressBarDialog progress = status as ProgressBarDialog;
            Adapter.Invoke(new SendOrPostCallback((o1) =>
            {
                progress.Begin();
                progress.SetText("Initialize...");
            }), null);
            bcApp = BCApplication.getInstance(ServerName);

            try
            {

                bcApp.addUserToolStripStatusLabel(tssl_LoginUser_Value);
                bcApp.addRefreshUIDisplayFun(delegate (object o) { UpdateUIDisplayByAuthority(); });
                line = bcApp.SCApplication.getEQObjCacheManager().getLine();
                ci = bcApp.SCApplication.getEQObjCacheManager().CommonInfo;

                Adapter.Invoke(new SendOrPostCallback((o1) =>
                {
                    registerEvent();
                    initUI();
                }), null);

                //必須等到UI Event註冊完成後，才可以開啟通訊界面
                //bcApp.startProcess();
                bcApp.SCApplication.ParkBLL.setCurrentParkType();
                bcApp.SCApplication.CycleBLL.setCurrentCycleType();
                //line.addEventHandler(this.Name
                //, BCFUtility.getPropertyName(() => line.ServiceMode)
                //, (s1, e1) => { bcApp.SCApplication.FailOverService.ListenOrShutdownServerPort(); });


            }
            catch (Exception ex)
            {
                Adapter.Invoke(new SendOrPostCallback((o1) =>
                {
                    MessageBox.Show(this, ex.ToString());
                }), null);
                logger.Error(ex, "Exception");
            }
            finally
            {
                Adapter.Invoke(new SendOrPostCallback((o1) =>
                {
                    if (progress != null) { progress.End(); }
                }), null);
            }
        }

        string EventHandleId = "";
        /// <summary>
        /// Registers the event.
        /// </summary>
        private void registerEvent()
        {
            try
            {
                EventHandleId = this.Name;
                line.addEventHandler(EventHandleId, nameof(line.SegmentPreDisableExcuting), SegmentPreDisableExcute);
                ISMControl.addHandshakeTimeoutErrorHandler(mplcHandshakeTimeout);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception:");
            }
        }

        private void SegmentPreDisableExcute(object sender, bcf.Common.PropertyChangedEventArgs e)
        {
            string RoadControlFormName = typeof(RoadControlForm).Name;
            if (!openForms.ContainsKey(RoadControlFormName))
            {
                Adapter.Invoke((obj) => openForm(RoadControlFormName, true, false), null);
            }
        }

        /// <summary>
        /// Initializes the UI.
        /// </summary>
        private void initUI()
        {
            //設定 Form Title 顯示的字
            tssl_Version_Value.Text = SCAppConstants.getMainFormVersion("");
            tssl_Build_Date_Value.Text = SCAppConstants.GetBuildDateTime().ToString();

            Login_DefaultUser();
        }
        #endregion Initialze

        /// <summary>
        /// Cpus the memory monitor.
        /// </summary>
        /// <param name="obj">The object.</param>
        private void cpuMemoryMonitor(object obj)
        {
            while (true)
            {
                System.Diagnostics.Process ps = System.Diagnostics.Process.GetCurrentProcess();
                try
                {
                    PerformanceCounter pf1 = new PerformanceCounter("Process", "Working Set - Private", ps.ProcessName);
                    PerformanceCounter pf2 = new PerformanceCounter("Process", "Working Set", ps.ProcessName);

                    masterPCMemoryLog.Debug("{0}:{1}  {2:N}KB", ps.ProcessName, "工作集(Process)", ps.WorkingSet64 / 1024);
                    masterPCMemoryLog.Debug("{0}:{1}  {2:N}KB", ps.ProcessName, "工作集        ", pf2.NextValue() / 1024);
                    masterPCMemoryLog.Debug("{0}:{1}  {2:N}KB", ps.ProcessName, "私有工作集    ", pf1.NextValue() / 1024);
                }
                catch (Exception ex)
                {
                    logger.Error(ex, "Exception:");
                }

                Thread.Sleep(10000);
            }
        }




        /// <summary>
        /// Login_s the default user.
        /// </summary>
        private void Login_DefaultUser()
        {
            bcApp.login(BCAppConstants.LOGIN_USER_DEFAULT);
        }

        bool islogin;
        bool IsLogIn
        {
            get { return islogin; }
            set
            {
                islogin = value;
                if (value)
                {
                    logInToolStripMenuItem.Enabled = false;
                    logOutToolStripMenuItem.Enabled = true;
                }
                else
                {
                    logInToolStripMenuItem.Enabled = true;
                    logOutToolStripMenuItem.Enabled = false;
                }
            }
        }

        /// <summary>
        /// Updates the UI display by authority.
        /// </summary>
        private void UpdateUIDisplayByAuthority()
        {

            BindingFlags flag = BindingFlags.Instance | BindingFlags.NonPublic;
            MemberInfo[] memberInfos = typeof(BCMainForm).GetMembers(flag);

            var typeSwitch = new TypeSwitch()
                .Case((System.Windows.Forms.ToolStripMenuItem tsm, bool tf) => { tsm.Enabled = tf; })
                .Case((System.Windows.Forms.ComboBox cb, bool tf) => { cb.Enabled = tf; });

            foreach (MemberInfo memberInfo in memberInfos)
            {
                Attribute AuthorityCheck = memberInfo.GetCustomAttribute(typeof(AuthorityCheck));
                if (AuthorityCheck != null)
                {
                    string attribute_FUNName = ((AuthorityCheck)AuthorityCheck).FUNCode;
                    FieldInfo info = (FieldInfo)memberInfo;
                    if (bcApp.SCApplication.UserBLL.checkUserAuthority(bcApp.LoginUserID, attribute_FUNName))
                    {
                        typeSwitch.Switch(info.GetValue(this), true);
                    }
                    else
                    {
                        typeSwitch.Switch(info.GetValue(this), false);
                    }
                }
            }
            IsLogIn = !BCFUtility.isEmpty(bcApp.LoginUserID);
        }


        /// <summary>
        /// Class TypeSwitch.
        /// </summary>
        public class TypeSwitch
        {
            /// <summary>
            /// The matches
            /// </summary>
            Dictionary<Type, Action<object, bool>> matches = new Dictionary<Type, Action<object, bool>>();
            /// <summary>
            /// Cases the specified action.
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <param name="action">The action.</param>
            /// <returns>TypeSwitch.</returns>
            public TypeSwitch Case<T>(Action<T, bool> action) { matches.Add(typeof(T), (x, enabled) => action((T)x, enabled)); return this; }
            /// <summary>
            /// Switches the specified x.
            /// </summary>
            /// <param name="x">The x.</param>
            /// <param name="enabled">if set to <c>true</c> [enabled].</param>
            public void Switch(object x, bool enabled)
            {
                if (matches.ContainsKey(x.GetType()))
                    matches[x.GetType()](x, enabled);
                else
                    logger.Warn("Switch Type:[{0}], Not exist!!!", x.GetType().Name);
            }
        }

        /// <summary>
        /// Does the start connection.
        /// </summary>
        /// <param name="status">The status.</param>
        private void doStartConnection(object status)
        {
            ProgressBarDialog progress = status as ProgressBarDialog;
            progress.Begin();
            progress.SetText(BCApplication.getMessageString("START_CONNECTING"));
            bcApp.startProcess();

            //Do something...

            progress.End();
        }

        /// <summary>
        /// Does the stop connection.
        /// </summary>
        /// <param name="status">The status.</param>
        private void doStopConnection(object status)
        {
            ProgressBarDialog progress = status as ProgressBarDialog;
            progress.Begin();
            progress.SetText(BCApplication.getMessageString("STOP_CONNECTING"));

            bcApp.stopProcess();

            //Do something...

            progress.End();
        }

        #region Sub Form Open Event



        /// <summary>
        /// Opens the form.
        /// </summary>
        /// <param name="formID">The form identifier.</param>
        public void openForm(String formID)
        {
            openForm(formID, false, false);
        }
        /// <summary>
        /// 開啟一般視窗 所使用
        /// </summary>
        /// <param name="formID">The form identifier.</param>
        /// <param name="isPopUp">The is pop up.</param>
        /// <param name="forceConfirm">The force confirm.</param>
        public void openForm(String formID, Boolean isPopUp, Boolean forceConfirm)
        {
            Form form;
            //string FormName = formID.Split('.')[1];
            if (openForms.ContainsKey(formID))
            {
                form = (Form)openForms[formID];
                if (isPopUp)
                {
                    form.Activate();
                    if (forceConfirm)
                    {
                        form.Close();
                        if (form != null && !form.IsDisposed) { form.Dispose(); }
                        removeForm(formID);
                        openForm(formID, isPopUp, forceConfirm);
                        return;
                    }
                    else
                    {
                        form.Show();
                    }
                    form.Focus();
                }
                else
                {
                    form.Activate();
                    form.Show();
                    form.Focus();
                    form.AutoScroll = true;
                    //form.WindowState = FormWindowState.Normal;
                    form.WindowState = FormWindowState.Maximized;
                }
            }
            else
            {
                try
                {
                    Type t = Type.GetType(String.Format("com.mirle.ibg3k0.bc.winform.UI.{0}", formID));
                    Object[] args = { this };
                    form = (Form)Activator.CreateInstance(t, args);
                    openForms.Add(formID, form);
                    if (isPopUp)
                    {
                        if (forceConfirm)
                        {
                            form.ShowDialog();
                        }
                        else
                        {
                            form.Show();
                        }
                        form.Focus();
                    }
                    else
                    {
                        if (!form.IsMdiContainer)
                        {
                            form.MdiParent = this;
                        }
                        form.Show();
                        form.Focus();
                        form.AutoScroll = true;
                        form.WindowState = FormWindowState.Normal;
                        form.WindowState = FormWindowState.Maximized;
                    }
                }
                catch (Exception ex)
                {
                    logger.Error(ex, "Exception:");
                    MessageBox.Show(this, "This Fuction Not Enable", "Important Message");
                }
            }
        }


        /// <summary>
        /// 用來開啟SECS Massage Log視窗 所使用
        /// </summary>
        /// <param name="formID">The form identifier.</param>
        /// <param name="Stringparameter1">The stringparameter1.</param>
        public void openForm(String formID, string Stringparameter1)
        {
            Form form;
            //string FormName = formID.Split('.')[1];
            if (openForms.ContainsKey(formID))
            {
                form = (Form)openForms[formID];
                form.Activate();
                form.Show();
                form.Focus();
            }
            else
            {
                try
                {
                    Type t = Type.GetType(String.Format("com.mirle.ibg3k0.bc.winform.UI.{0}", formID));
                    Object[] args = { this, Stringparameter1 };
                    form = (Form)Activator.CreateInstance(t, args);
                    openForms.Add(formID, form);
                    if (!form.IsMdiContainer)
                    {
                        form.MdiParent = this;
                    }
                    form.Show();
                    form.Focus();
                }
                catch (Exception ex)
                {
                    logger.Error(ex, "Exception:");
                    MessageBox.Show(this, "This Fuction Not Enable", "Important Message");
                }
            }
        }

        /// <summary>
        /// Removes the form.
        /// </summary>
        /// <param name="formID">The form identifier.</param>
        public void removeForm(String formID)
        {
            if (openForms.ContainsKey(formID))
            {
                openForms.Remove(formID);
            }
        }
        #endregion Sub Form Open Event

        /// <summary>
        /// Gets the message string.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="args">The arguments.</param>
        /// <returns>System.String.</returns>
        public string getMessageString(string key, params object[] args)
        {
            return SCApplication.getMessageString(key, args);
        }

        #region Start Connection & Stop Connection
        /// <summary>
        /// Handles the Click event of the startConnectionToolStripMenuItem1 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void startConnectionToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            //if (!BCUtility.doLogin(this, bcApp, BCAppConstants.FUNC_CONNECTION_MANAGEMENT))
            //{
            //    return;
            //}
            try
            {
                ProgressBarDialog progress = new ProgressBarDialog(bcApp);
                System.Threading.ThreadPool.QueueUserWorkItem(
                    new System.Threading.WaitCallback(doStartConnection), progress);
                if (progress != null && !progress.IsDisposed)
                {
                    progress.ShowDialog();
                }
                return;
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception:");
            }
        }

        /// <summary>
        /// Handles the Click event of the stopConnectionToolStripMenuItem1 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void stopConnectionToolStripMenuItem1_Click(object sender, EventArgs e)
        {

            try
            {
                DialogResult confirmResult = MessageBox.Show(this, BCApplication.getMessageString("Confirm_STOP_CONNECTING"),
                    BCApplication.getMessageString("CONFIRM"), MessageBoxButtons.YesNo);
                if (confirmResult != System.Windows.Forms.DialogResult.Yes)
                {
                    return;
                }

                ProgressBarDialog progress = new ProgressBarDialog(bcApp);
                System.Threading.ThreadPool.QueueUserWorkItem(
                    new System.Threading.WaitCallback(doStopConnection), progress);
                if (progress != null && !progress.IsDisposed)
                {
                    progress.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception:");
            }
        }
        #endregion Start Connection & Stop Connection



        /// <summary>
        /// Handles the FormClosing event of the BCMainForm control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="FormClosingEventArgs"/> instance containing the event data.</param>
        private void BCMainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            //1.初步詢問是否要關閉OHBC
            DialogResult confirmResult = MessageBox.Show(this, "Do you want to close AGVC?",
                BCApplication.getMessageString("CONFIRM"), MessageBoxButtons.YesNo);
            recordAction("Do you want to close AGVC?", confirmResult.ToString());
            if (confirmResult != System.Windows.Forms.DialogResult.Yes)
            {
                e.Cancel = true;
                return;
            }
            if (!BCUtility.doLogin(this, bcApp, BCAppConstants.FUNC_CLOSE_SYSTEM, true))
            {
                e.Cancel = true;
                recordAction("Close Master PC, Authority Check...", "Failed !!");
                return;
            }
            recordAction("Close Master PC, Authority Check...", "Success !!");

            if (e.Cancel == false)
            {
                try
                {
                    ProgressBarDialog progress = new ProgressBarDialog(bcApp);
                    System.Threading.ThreadPool.QueueUserWorkItem(
                        new System.Threading.WaitCallback(doStopConnection), progress);
                    if (progress != null && !progress.IsDisposed)
                    {
                        progress.ShowDialog();
                    }
                }
                catch (Exception ex)
                {
                    logger.Error(ex, "Exception");
                }
            }
        }
        private void recordAction(string tipMessage, string confirmResult)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(tipMessage);
            sb.AppendLine(string.Format("{0}         ConfirmResult:{1}", new string(' ', 5), confirmResult));
            bcApp.SCApplication.BCSystemBLL.addOperationHis(bcApp.LoginUserID, this.Name, sb.ToString());
        }


        #region UAS
        /// <summary>
        /// The uas main form
        /// </summary>
        private UASMainForm uasMainForm = null;

        /// <summary>
        /// Handles the Click event of the uASToolStripMenuItem control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void uASToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!BCUtility.doLogin(this, bcApp, BCAppConstants.FUNC_USER_MANAGEMENT))
            {
                return;
            }
            if (uasMainForm == null || uasMainForm.IsDisposed)
            {
                uasMainForm = new UASMainForm();
                uasMainForm.Show();
                uasMainForm.Focus();
            }
            else
            {
                uasMainForm.rework();
                uasMainForm.Focus();
            }
        }

        /// <summary>
        /// Handles the Click event of the changePasswordToolStripMenuItem control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void changePasswordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!BCUtility.isLogin(bcApp))
            {
                if (!BCUtility.doLogin(this, bcApp))
                {
                    return;
                }
            }
            openForm("ChangePwdForm");
        }

        /// <summary>
        /// A0.19
        /// </summary>
        private void closeUasMainForm()
        {
            if (uasMainForm != null && !uasMainForm.IsDisposed)
            {
                uasMainForm.Close();
            }
        }
        #endregion


        /// <summary>
        /// Handles the Click event of the pic_Logout control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void pic_Logout_Click(object sender, EventArgs e)
        {
            BCUtility.doLogout(bcApp);
        }

        /// <summary>
        /// Handles the MouseDoubleClick event of the menuStrip1 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="MouseEventArgs"/> instance containing the event data.</param>
        private void menuStrip1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && (ModifierKeys & Keys.Shift) == Keys.Shift && (ModifierKeys & Keys.Alt) == Keys.Alt)
            {
                MessageBox.Show(this, "UAS Fail.");
            }
        }

        private void logInToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                BCUtility.doLogin(this, bcApp);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception:");
            }
        }

        private void logOutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                BCUtility.doLogout(bcApp);
                MessageBox.Show(this, BCApplication.getMessageString("LOGOUT_SUCCESS")
                                , BCApplication.getMessageString("INFO")
                                , MessageBoxButtons.OK
                                , MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception:");
            }
        }

        private void englishToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Localization.LCH.getLanguage() != "en-US")
            {
                Localization.LCH.changeConfigLanguage("en-US");
                MessageBox.Show("Language will be changed after restart the program.");
            }

        }

        private void zh_twToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Localization.LCH.getLanguage() != "zh-TW")
            {
                Localization.LCH.changeConfigLanguage("zh-TW");
                MessageBox.Show("Language will be changed after restart the program.");
            }

        }

        private void zh_chToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Localization.LCH.getLanguage() != "zh-CN")
            {
                Localization.LCH.changeConfigLanguage("zh-CN");
                MessageBox.Show("Language will be changed after restart the program.");
            }

        }

        private void tipMessageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openForm(typeof(MPCInfoMsgDialog).Name, true, false);
        }


        private void communectionStatusToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openForm("MonitorForm", true, true);
        }

        public void entryMonitorMode()
        {
            (openForms["OHT_Form"] as OHT_Form).entryMonitorMode();
        }
        public void LeaveMonitorMode()
        {
            (openForms["OHT_Form"] as OHT_Form).LeaveMonitorMode();
        }
        public void setSpecifyRail(string[] secs)
        {
            (openForms["OHT_Form"] as OHT_Form).setSpecifyRail(secs);
        }

        private void engineerToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void BCMainForm_FormClosed(object sender, FormClosedEventArgs e)
        {

            AEQPT fourColorLight = bcApp.SCApplication.getEQObjCacheManager().getEquipmentByEQPTID("ColorLight");
            if (fourColorLight != null)
            {
                fourColorLight.setColorLight(false, false, false, false, false, true);
            }
            AEQPT trafficLight1 = bcApp.SCApplication.getEQObjCacheManager().getEquipmentByEQPTID("TrafficLight1");
            if (trafficLight1 != null)
            {
                trafficLight1.setTrafficLight(false, true, false, false, false, true);
            }
            AEQPT trafficLight2 = bcApp.SCApplication.getEQObjCacheManager().getEquipmentByEQPTID("TrafficLight2");
            if (trafficLight2 != null)
            {
                trafficLight2.setTrafficLight(false, true, false, false, false, true);
            }

            //bcApp.SCApplication.getEQObjCacheManager().getEquipmentByEQPTID("Fire");
            List<AUNIT> units = bcApp.SCApplication.getEQObjCacheManager().getAllUnit();
            foreach (AUNIT unit in units)
            {
                if (unit.UNIT_ID.StartsWith("FireDoor"))
                {
                    FireDoorDefaultValueDefMapAction mapAction = unit.getMapActionByIdentityKey(nameof(FireDoorDefaultValueDefMapAction)) as FireDoorDefaultValueDefMapAction;
                    mapAction.sendFireDoorCrossSignal(true);
                }
            }

            System.Environment.Exit(0);
        }

        private void debugToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            openForm(typeof(DebugForm).Name, true, false);
        }

        private void engineeringModeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openForm(typeof(EngineerFormForAGVC).Name, true, false);
        }

        private void roadControlToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (BCFUtility.isMatche(bcApp.SCApplication.BC_ID, SCAppConstants.WorkVersion.VERSION_NAME_SOUTH_INNOLUX))
            {
                openForm(typeof(RoadControlBySectionForm).Name, true, false);
            }
            else
            {
                openForm(typeof(RoadControlForm).Name, true, false);
            }
        }

        private void zhTwToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.UICulture = new System.Globalization.CultureInfo("zh-TW");
            Properties.Settings.Default.Save();
            Thread.CurrentThread.CurrentUICulture = Properties.Settings.Default.UICulture;
        }

        private void enUSToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.UICulture = new System.Globalization.CultureInfo("en-US");
            Properties.Settings.Default.Save();
            Thread.CurrentThread.CurrentUICulture = Properties.Settings.Default.UICulture;
        }

        private void sectionDataToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openForm(typeof(VehicleDataSettingForm).Name);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            var msgs = ci.MPCTipMsgList;
            int error_happend_count = msgs.Where(msg => !msg.IsConfirm && msg.MsgLevel == sc.ProtocolFormat.OHTMessage.MsgLevel.Error).Count();
            int warn_happend_count = msgs.Where(msg => !msg.IsConfirm && msg.MsgLevel == sc.ProtocolFormat.OHTMessage.MsgLevel.Warn).Count();
            IsErrorHappendNotConfirm = error_happend_count > 0;
            IsWarnHappendNotConfirm = warn_happend_count > 0;

        }
        Color ErrorOriginalColor = Color.Empty;
        Color WarnOriginalColor = Color.Empty;
        Color ErrorTipColor = Color.Red;
        Color WarnTipColor = Color.Yellow;

        bool IsErrorHappendNotConfirm
        {
            set
            {
                if (value)
                {
                    if (tssl_Error_Value.BackColor == ErrorOriginalColor)
                    {
                        tssl_Error_Value.BackColor = ErrorTipColor;
                    }
                    else
                    {
                        tssl_Error_Value.BackColor = ErrorOriginalColor;
                    }
                    if (!tssl_Error_Value.Enabled) tssl_Error_Value.Enabled = true;
                }
                else
                {
                    if (tssl_Error_Value.BackColor != ErrorOriginalColor)
                    {
                        tssl_Error_Value.BackColor = ErrorOriginalColor;
                    }
                    if (tssl_Error_Value.Enabled) tssl_Error_Value.Enabled = false;
                }
            }
        }

        bool IsWarnHappendNotConfirm
        {
            set
            {
                if (value)
                {
                    if (tssl_Warn_Value.BackColor == WarnOriginalColor)
                    {
                        tssl_Warn_Value.BackColor = WarnTipColor;
                    }
                    else
                    {
                        tssl_Warn_Value.BackColor = WarnOriginalColor;
                    }
                    if (!tssl_Warn_Value.Enabled) tssl_Warn_Value.Enabled = true;
                }
                else
                {
                    if (tssl_Warn_Value.BackColor != WarnOriginalColor)
                    {
                        tssl_Warn_Value.BackColor = WarnOriginalColor;
                    }
                    if (!tssl_Warn_Value.Enabled) tssl_Warn_Value.Enabled = true;
                }
            }
        }

        private void tssl_Warn_Value_Click(object sender, EventArgs e)
        {
            openForm(typeof(MPCInfoMsgDialog).Name, true, false);
        }

        private void tssl_Error_Value_Click(object sender, EventArgs e)
        {
            openForm(typeof(MPCInfoMsgDialog).Name, true, false);
        }

        //private void hostConnectionToolStripMenuItem_Click(object sender, EventArgs e)
        //{
        //    openForm(typeof(HostModeChg_Form).Name);
        //}

        private void transferCommandToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openForm(typeof(TransferCommandQureyListForm).Name, true, false);
        }

        private void transferCommandHistoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openForm(typeof(HistoryTransferCommandQureyListForm).Name);

        }

        private void alarmToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openForm(typeof(HistoryAlarmsForm).Name);
        }

        private void reserveInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void reserveSectionInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openForm(typeof(ReserveSectionInfoForm).Name, true, false);
        }

        private void mataToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void hostConnectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openForm(typeof(HostModeChg_Form).Name);

        }

        private void staticsChartToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openForm(typeof(ChartPopupForm).Name, true, false);

        }

        private void carrierInstalledRemoveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openForm(typeof(CarrierMaintenanceForm).Name, true, false);
        }

        private void cycleRunToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openForm(typeof(CycleRun).Name, true, false);
        }

        private void alarmListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openForm(typeof(AlarmListForm).Name, true, false);
        }

        const int AUTO_LOG_OUT_TIME_SECONDS = 300;
        private void logout_timer_Tick(object sender, EventArgs e)
        {
            if (IsLogIn)
            {
                if (lastMove_DT.AddSeconds(AUTO_LOG_OUT_TIME_SECONDS) < DateTime.Now)
                {
                    BCUtility.doLogout(bcApp);
                    closeAllOpenForm();        //B0.03
                    closeUasMainForm();      //B0.03
                }
            }
        }
        private void closeAllOpenForm()
        {
            lock (openForms)
            {
                Dictionary<String, Form> openformsTemp = new Dictionary<string, Form>(openForms);
                foreach (KeyValuePair<string, Form> item in openformsTemp)     //A0.19
                {                                                              //A0.19
                    if (SCUtility.isMatche(item.Key, nameof(OHT_Form)))        //B0.35
                        continue;                                              //A0.19
                    item.Value.Close();                                        //A0.19
                }                                                              //A0.19
            }
        }
    }

    public delegate void MouseMovedEvent();

    public class GlobalMouseHandler : IMessageFilter
    {
        private const int WM_MOUSEMOVE = 0x0200;
        private System.Drawing.Point previousMousePosition = new System.Drawing.Point();
        public static event EventHandler<MouseEventArgs> MouseMovedEvent = delegate { };

        #region IMessageFilter Members

        public bool PreFilterMessage(ref System.Windows.Forms.Message m)
        {
            if (m.Msg == WM_MOUSEMOVE)
            {
                System.Drawing.Point currentMousePoint = Control.MousePosition;
                if (previousMousePosition != currentMousePoint)
                {
                    previousMousePosition = currentMousePoint;
                    MouseMovedEvent(this, new MouseEventArgs(MouseButtons.None, 0, currentMousePoint.X, currentMousePoint.Y, 0));
                }
            }
            // Always allow message to continue to the next filter control
            return false;
        }

        #endregion
    }

}