using com.mirle.ibg3k0.bc.winform.Common;
using com.mirle.ibg3k0.bc.wpf.App;
using com.mirle.ibg3k0.ohxc.wpf.App;
using MirleGO_UIFrameWork.UI.uc_Button;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ViewerObject;

namespace ControlSystemViewer.Views.Menu_Log
{
    /// <summary>
    /// TransferCommandHistory.xaml 的互動邏輯
    /// </summary>
    public partial class TransferCommandHistory : UserControl
    {
        #region 公用參數設定
        private static Logger logger = LogManager.GetCurrentClassLogger();
        //SysExcuteQualityQueryService sysExcuteQualityQueryService;
        public event EventHandler CloseEvent;
		private DateTime SearchTimeFrom = DateTime.MinValue;
		private DateTime SearchTimeTo = DateTime.MinValue;
		#endregion 公用參數設定

		public TransferCommandHistory()
		{
			try
			{
				InitializeComponent();

				cb_HrsInterval.Items.Add("");
				for (int i = 1; i <= 24; i++)
				{
					cb_HrsInterval.Items.Add("Last " + i + " Hours");
				}

				//設定日期的最大與最小值
				m_StartDTCbx.DisplayDateStart = DateTime.Now.AddYears(-3);
				m_StartDTCbx.DisplayDateEnd = DateTime.Today.AddDays(1).AddMilliseconds(-1);
				m_EndDTCbx.DisplayDateStart = DateTime.Now.AddYears(-3);
				m_EndDTCbx.DisplayDateEnd = DateTime.Today.AddDays(1).AddMilliseconds(-1);

				//預設起訖日期
				m_StartDTCbx.SelectedDate = DateTime.Now.AddDays(-1);
				m_EndDTCbx.SelectedDate = DateTime.Now.AddMilliseconds(-1);
			}
			catch (Exception ex)
			{
				logger.Error(ex, "Exception");
			}
		}

		public void StartupUI()
		{
			try
			{
				var app = WindownApplication.getInstance();
				if (!(app.ObjCacheManager.GetSelectedProject().ObjectConverter == "OHBC_ASE_K21"))
				{
					int index = dgv_log_query.Columns.IndexOf(dgtc_BoxID);
					dgv_log_query.Columns[index].Visibility = Visibility.Collapsed;
				}

				//預設起訖日期
				m_StartDTCbx.SelectedDate = DateTime.Now.AddDays(-1);
				m_EndDTCbx.SelectedDate = DateTime.Now.AddMilliseconds(-1);
				cbo_StartH.Text = "00:00";
				cbo_EndH.Text = DateTime.Now.Hour.ToString("00") + ":" + DateTime.Now.Minute.ToString("00");
				//sysExcuteQualityQueryService = WindownApplication.getInstance().GetSysExcuteQualityQueryService();
				cb_HrsInterval.MouseWheel += new MouseWheelEventHandler(cb_HrsInterval_MouseWheel);
				//this.frmquery = _frmquery;
			}
			catch (Exception ex)
			{
				logger.Error(ex, "Exception");
			}
		}

		private void cb_HrsInterval_MouseWheel(object sender, MouseWheelEventArgs e)
		{
			try
			{
				e.Handled = true;
			}
			catch (Exception ex)
			{
				logger.Error(ex, "Exception");
			}
		}

		private async void btn_Search_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				dgv_log_query.ItemsSource = null;
				dgv_log_query.Items.Refresh();
				await search();
			}
			catch (Exception ex)
			{
				logger.Error(ex, "Exception");
			}
		}

		private void btn_Clear_Click(object sender, EventArgs e)
		{
			try
			{
				cb_HrsInterval.SelectedIndex = -1;
				txtCstID.Text = "";
				txtLotID.Text = "";
				txtCmdID.Text = "";
				dgv_log_query.ItemsSource = null;
				dgv_log_query.Items.Refresh();
			}
			catch (Exception ex)
			{
				logger.Error(ex, "Exception");
			}
		}

		private async void btn_Export_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				await export();
			}
			catch (Exception ex)
			{
				logger.Error(ex, "Exception");
			}
		}

		private async Task search()
		{
			bool isLoading = false;
			try
			{
				SearchTimeFrom = new DateTime(m_StartDTCbx.SelectedDate.Value.Year, m_StartDTCbx.SelectedDate.Value.Month, m_StartDTCbx.SelectedDate.Value.Day, Convert.ToInt32(cbo_StartH.Text.Split(':')[0]), Convert.ToInt32(cbo_StartH.Text.Split(':')[1]), 0);
				SearchTimeTo = new DateTime(m_EndDTCbx.SelectedDate.Value.Year, m_EndDTCbx.SelectedDate.Value.Month, m_EndDTCbx.SelectedDate.Value.Day, Convert.ToInt32(cbo_EndH.Text.Split(':')[0]), Convert.ToInt32(cbo_EndH.Text.Split(':')[1]), 0);

				if (string.IsNullOrWhiteSpace(m_StartDTCbx.SelectedDate.ToString()))
				{
					TipMessage_Type_Light.Show("Failure", string.Format("Please select start time."), BCAppConstants.WARN_MSG);
					return;
				}
				if (string.IsNullOrWhiteSpace(m_EndDTCbx.SelectedDate.ToString()))
				{
					TipMessage_Type_Light.Show("Failure", string.Format("Please select end time."), BCAppConstants.WARN_MSG);
					return;
				}
				if (SearchTimeFrom > SearchTimeTo)
				{
					DateTime dtTmp = SearchTimeFrom;
					m_StartDTCbx.SelectedDate = m_EndDTCbx.SelectedDate;
					cbo_StartH.Text = cbo_EndH.Text;
					m_EndDTCbx.SelectedDate = SearchTimeFrom;
					cbo_EndH.Text = SearchTimeFrom.Hour.ToString("00");
				}
				btn_Search.IsEnabled = false;
				DateTime dateTimeFrom = SearchTimeFrom;
				DateTime dateTimeTo = SearchTimeTo;
				//string cst_id = string.Empty;
				//string cmd_id = string.Empty;
				//if (SCUtility.isMatche(txtCstID.Text, ""))
				//{
				//	cst_id = null;
				//}
				//else
				//{
				//	cst_id = txtCstID.Text.Trim();
				//}
				//if (SCUtility.isMatche(txtCmdID.Text, ""))
				//{
				//    cmd_id = null;
				//}
				//else
				//{
				//    cmd_id = txtCmdID.Text.Trim();
				//}

				var scApp = WindownApplication.getInstance();
				dateTimeFrom = new DateTime(dateTimeFrom.Year, dateTimeFrom.Month, dateTimeFrom.Day, dateTimeFrom.Hour, dateTimeFrom.Minute, dateTimeFrom.Second, DateTimeKind.Local);
				dateTimeTo = new DateTime(dateTimeTo.Year, dateTimeTo.Month, dateTimeTo.Day, dateTimeTo.Hour, dateTimeTo.Minute, dateTimeTo.Second, DateTimeKind.Local);
				//List<sc.VACMD_MCS> transfer_list = null;
				//List<sc.ACMD_MCS> cmd_Mcs_list =   null;
				//List<TransferCommand> transferCommands = null;
				List<VTRANSFER> transferCommands = null;
				//await Task.Run(() => system_qualitys = sysExcuteQualityQueryService.loadSysexcutequalities(dateTimeFrom, dateTimeTo, keyword));

				string txtcmdid = txtCmdID.Text;
				string txtlotid = txtLotID.Text.Trim();
				string txtcstid = txtCstID.Text.Trim();
				((MainWindow)App.Current.MainWindow).Loading_Start("Searching");
				isLoading = true;
				await Task.Run(() =>
				{
					//List<sc.VTRANSFER> transfer_list = scApp.CmdBLL.GetVCmd_McsByConditions(dateTimeFrom, dateTimeTo);
					//List<sc.ATRANSFER> cmd_Mcs_list = scApp.CmdBLL.GetCmd_McsByConditions(dateTimeFrom, dateTimeTo);
					//var tmp = transfer_list.Select(t =>
					//	new TransferCommand(t, cmd_Mcs_list.FirstOrDefault(c => c.ID.Trim().Equals(t.ID.Trim())))); // add Trim()
					//List<HCMD_MCS> transfer_list = scApp.CmdBLL.GetHCmd_McsByConditions(dateTimeFrom, dateTimeTo);
					//List<HCMD_OHTC> hcmd_list = scApp.CmdBLL.GetHCmd_CtlByConditions(dateTimeFrom, transfer_list?.Max(t => t.CMD_FINISH_TIME) ?? dateTimeTo);
					//var tmp = transfer_list.Select(t =>
					//	new HTransferCommand(t, hcmd_list.FirstOrDefault(c => c.CMD_ID_MCS.Trim() == t.CMD_ID.Trim()))); // add Trim()
					var tmp = scApp.CmdBLL.GetHTransferByConditions(dateTimeFrom, dateTimeTo);
					transferCommands = tmp.Where(t =>
					{
						bool b = true;
						if (!string.IsNullOrWhiteSpace(txtcmdid))
						{
							b = b & t.TRANSFER_ID?.Contains(txtcmdid) ?? false;
						}
						if (!string.IsNullOrWhiteSpace(txtcstid))
						{
							b = b & t.CARRIER_ID?.Contains(txtcstid) ?? false;
						}

						return b;
					}).ToList();
				});
				if (transferCommands == null || transferCommands.Count <= 0)
				{
					var languageDictionary = App.Current.Resources.MergedDictionaries?[0];
					string sTipMsg = languageDictionary?["TIPMSG_QUERY_NO_DATA"]?.ToString() ?? "There is no matching data for your query.";
					TipMessage_Type_Light.Show("", sTipMsg, BCAppConstants.WARN_MSG);
				}
				else
				{
					dgv_log_query.ItemsSource = transferCommands.OrderBy(info => info.INSERT_TIME).ToList();
				}
			}
			catch (System.Data.SqlClient.SqlException sqlex)
			{
				logger.Error(sqlex, "SqlException");
				TipMessage_Type_Light.Show("", sqlex.Message, BCAppConstants.WARN_MSG);
			}
			catch (System.Data.Entity.Core.EntityCommandExecutionException eceex)
			{
				logger.Error(eceex, "EntityCommandExecutionException");
				TipMessage_Type_Light.Show("", eceex.Message, BCAppConstants.WARN_MSG);
			}
			catch (Exception ex)
			{
				logger.Error(ex, "Exception");
			}
			finally
			{
				btn_Search.IsEnabled = true;
				if (isLoading) ((MainWindow)App.Current.MainWindow).Loading_Stop();
			}
		}

		private async Task export()
		{
			try
			{
				var data_list = dgv_log_query.ItemsSource as List<VTRANSFER>;
				if (data_list == null || data_list.Count == 0)
				{
					var languageDictionary = App.Current.Resources.MergedDictionaries?[0];
					string sTipMsg = languageDictionary?["TIPMSG_EXPORT_NO_DATA"]?.ToString() ?? "There is no data to export, please search first.";
					TipMessage_Type_Light.Show("", sTipMsg, BCAppConstants.WARN_MSG);
					return;
				}

				System.Windows.Forms.SaveFileDialog dlg = new System.Windows.Forms.SaveFileDialog();
				dlg.Filter = "Files (*.xlsx)|*.xlsx";
				if (dlg.ShowDialog() != System.Windows.Forms.DialogResult.OK || string.IsNullOrWhiteSpace(dlg.FileName))
				{
					return;
				}
				string filename = dlg.FileName;
				//建立 xlxs 轉換物件
				XSLXHelper helper = new XSLXHelper(WindownApplication.getInstance().ObjCacheManager.ViewerSettings.report.ExportRowLimit);
				//取得轉為 xlsx 的物件
				ClosedXML.Excel.XLWorkbook xlsx = null;
				await Task.Run(() => xlsx = helper.Export(data_list));
				if (xlsx != null)
					xlsx.SaveAs(filename);
			}
			catch (Exception ex)
			{
				logger.Error(ex, "Exception");
			}
		}

		private async void Button_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				cb_HrsInterval.SelectedIndex = -1;
				m_EndDTCbx.SelectedDate = DateTime.Now;
				cbo_EndH.Text = DateTime.Now.Hour.ToString("00") + ":" + DateTime.Now.Minute.ToString("00");

				if (sender.Equals(HypL30mins))
				{
					m_StartDTCbx.SelectedDate = DateTime.Now.AddMinutes(-30);
					cbo_StartH.Text = DateTime.Now.AddMinutes(-30).Hour.ToString("00") + ":" + DateTime.Now.AddMinutes(-30).Minute.ToString("00");
					await search();
				}
				else if (sender.Equals(HypL1hours))
				{
					m_StartDTCbx.SelectedDate = DateTime.Now.AddHours(-1);
					cbo_StartH.Text = DateTime.Now.AddHours(-1).Hour.ToString("00") + ":" + DateTime.Now.AddHours(-1).Minute.ToString("00");
					await search();
				}
				else if (sender.Equals(HypL4hours))
				{
					m_StartDTCbx.SelectedDate = DateTime.Now.AddHours(-4);
					cbo_StartH.Text = DateTime.Now.AddHours(-4).Hour.ToString("00") + ":" + DateTime.Now.AddHours(-4).Minute.ToString("00");
					await search();
				}
				else if (sender.Equals(HypL12hours))
				{
					m_StartDTCbx.SelectedDate = DateTime.Now.AddHours(-12);
					cbo_StartH.Text = DateTime.Now.AddHours(-12).Hour.ToString("00") + ":" + DateTime.Now.AddHours(-12).Minute.ToString("00");
					await search();
				}
				else if (sender.Equals(HypL24hours))
				{
					m_StartDTCbx.SelectedDate = DateTime.Now.AddDays(-1);
					cbo_StartH.Text = DateTime.Now.AddDays(-1).Hour.ToString("00") + ":" + DateTime.Now.AddDays(-1).Minute.ToString("00");
					await search();
				}
				else if (sender.Equals(HypL2days))
				{
					m_StartDTCbx.SelectedDate = DateTime.Now.AddDays(-2);
					cbo_StartH.Text = DateTime.Now.AddDays(-2).Hour.ToString("00") + ":" + DateTime.Now.AddDays(-2).Minute.ToString("00");
					await search();
				}
				else if (sender.Equals(HypL3days))
				{
					m_StartDTCbx.SelectedDate = DateTime.Now.AddDays(-3);
					cbo_StartH.Text = DateTime.Now.AddDays(-3).Hour.ToString("00") + ":" + DateTime.Now.AddDays(-3).Minute.ToString("00");
					await search();
				}
			}
			catch (Exception ex)
			{
				logger.Error(ex, "Exception");
			}
		}

		private async void cb_HrsInterval_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			try
			{
				m_EndDTCbx.SelectedDate = DateTime.Now;
				cbo_EndH.Text = DateTime.Now.Hour.ToString("00") + ":" + DateTime.Now.Minute.ToString("00");
				switch (cb_HrsInterval.SelectedIndex)
				{
					case 0:
						break;

					case 1:
						m_StartDTCbx.SelectedDate = DateTime.Now.AddHours(-1);
						cbo_StartH.Text = DateTime.Now.AddHours(-1).Hour.ToString("00") + ":" + DateTime.Now.AddHours(-1).Minute.ToString("00");
						break;

					case 2:
						m_StartDTCbx.SelectedDate = DateTime.Now.AddHours(-2);
						cbo_StartH.Text = DateTime.Now.AddHours(-2).Hour.ToString("00") + ":" + DateTime.Now.AddHours(-2).Minute.ToString("00");
						break;

					case 3:
						m_StartDTCbx.SelectedDate = DateTime.Now.AddHours(-3);
						cbo_StartH.Text = DateTime.Now.AddHours(-3).Hour.ToString("00") + ":" + DateTime.Now.AddHours(-3).Minute.ToString("00");
						break;

					case 4:
						m_StartDTCbx.SelectedDate = DateTime.Now.AddHours(-4);
						cbo_StartH.Text = DateTime.Now.AddHours(-4).Hour.ToString("00") + ":" + DateTime.Now.AddHours(-4).Minute.ToString("00");
						break;

					case 5:
						m_StartDTCbx.SelectedDate = DateTime.Now.AddHours(-5);
						cbo_StartH.Text = DateTime.Now.AddHours(-5).Hour.ToString("00") + ":" + DateTime.Now.AddHours(-5).Minute.ToString("00");
						break;

					case 6:
						m_StartDTCbx.SelectedDate = DateTime.Now.AddHours(-6);
						cbo_StartH.Text = DateTime.Now.AddHours(-6).Hour.ToString("00") + ":" + DateTime.Now.AddHours(-6).Minute.ToString("00");
						break;

					case 7:
						m_StartDTCbx.SelectedDate = DateTime.Now.AddHours(-7);
						cbo_StartH.Text = DateTime.Now.AddHours(-7).Hour.ToString("00") + ":" + DateTime.Now.AddHours(-7).Minute.ToString("00");
						break;

					case 8:
						m_StartDTCbx.SelectedDate = DateTime.Now.AddHours(-8);
						cbo_StartH.Text = DateTime.Now.AddHours(-8).Hour.ToString("00") + ":" + DateTime.Now.AddHours(-8).Minute.ToString("00");
						break;

					case 9:
						m_StartDTCbx.SelectedDate = DateTime.Now.AddHours(-9);
						cbo_StartH.Text = DateTime.Now.AddHours(-9).Hour.ToString("00") + ":" + DateTime.Now.AddHours(-9).Minute.ToString("00");
						break;

					case 10:
						m_StartDTCbx.SelectedDate = DateTime.Now.AddHours(-10);
						cbo_StartH.Text = DateTime.Now.AddHours(-10).Hour.ToString("00") + ":" + DateTime.Now.AddHours(-10).Minute.ToString("00");
						break;

					case 11:
						m_StartDTCbx.SelectedDate = DateTime.Now.AddHours(-11);
						cbo_StartH.Text = DateTime.Now.AddHours(-11).Hour.ToString("00") + ":" + DateTime.Now.AddHours(-11).Minute.ToString("00");
						break;

					case 12:
						m_StartDTCbx.SelectedDate = DateTime.Now.AddHours(-12);
						cbo_StartH.Text = DateTime.Now.AddHours(-12).Hour.ToString("00") + ":" + DateTime.Now.AddHours(-12).Minute.ToString("00");
						break;

					case 13:
						m_StartDTCbx.SelectedDate = DateTime.Now.AddHours(-13);
						cbo_StartH.Text = DateTime.Now.AddHours(-13).Hour.ToString("00") + ":" + DateTime.Now.AddHours(-13).Minute.ToString("00");
						break;

					case 14:
						m_StartDTCbx.SelectedDate = DateTime.Now.AddHours(-14);
						cbo_StartH.Text = DateTime.Now.AddHours(-14).Hour.ToString("00") + ":" + DateTime.Now.AddHours(-14).Minute.ToString("00");
						break;

					case 15:
						m_StartDTCbx.SelectedDate = DateTime.Now.AddHours(-15);
						cbo_StartH.Text = DateTime.Now.AddHours(-15).Hour.ToString("00") + ":" + DateTime.Now.AddHours(-15).Minute.ToString("00");
						break;

					case 16:
						m_StartDTCbx.SelectedDate = DateTime.Now.AddHours(-16);
						cbo_StartH.Text = DateTime.Now.AddHours(-16).Hour.ToString("00") + ":" + DateTime.Now.AddHours(-16).Minute.ToString("00");
						break;

					case 17:
						m_StartDTCbx.SelectedDate = DateTime.Now.AddHours(-17);
						cbo_StartH.Text = DateTime.Now.AddHours(-17).Hour.ToString("00") + ":" + DateTime.Now.AddHours(-17).Minute.ToString("00");
						break;

					case 18:
						m_StartDTCbx.SelectedDate = DateTime.Now.AddHours(-18);
						cbo_StartH.Text = DateTime.Now.AddHours(-18).Hour.ToString("00") + ":" + DateTime.Now.AddHours(-18).Minute.ToString("00");
						break;

					case 19:
						m_StartDTCbx.SelectedDate = DateTime.Now.AddHours(-19);
						cbo_StartH.Text = DateTime.Now.AddHours(-19).Hour.ToString("00") + ":" + DateTime.Now.AddHours(-19).Minute.ToString("00");
						break;

					case 20:
						m_StartDTCbx.SelectedDate = DateTime.Now.AddHours(-20);
						cbo_StartH.Text = DateTime.Now.AddHours(-20).Hour.ToString("00") + ":" + DateTime.Now.AddHours(-20).Minute.ToString("00");
						break;

					case 21:
						m_StartDTCbx.SelectedDate = DateTime.Now.AddHours(-21);
						cbo_StartH.Text = DateTime.Now.AddHours(-21).Hour.ToString("00") + ":" + DateTime.Now.AddHours(-21).Minute.ToString("00");
						break;

					case 22:
						m_StartDTCbx.SelectedDate = DateTime.Now.AddHours(-22);
						cbo_StartH.Text = DateTime.Now.AddHours(-22).Hour.ToString("00") + ":" + DateTime.Now.AddHours(-22).Minute.ToString("00");
						break;

					case 23:
						m_StartDTCbx.SelectedDate = DateTime.Now.AddHours(-23);
						cbo_StartH.Text = DateTime.Now.AddHours(-23).Hour.ToString("00") + ":" + DateTime.Now.AddHours(-23).Minute.ToString("00");
						break;

					case 24:
						m_StartDTCbx.SelectedDate = DateTime.Now.AddHours(-24);
						cbo_StartH.Text = DateTime.Now.AddHours(-24).Hour.ToString("00") + ":" + DateTime.Now.AddHours(-24).Minute.ToString("00");
						break;

					default:
						break;
				}
			}
			catch (Exception ex)
			{
				logger.Error(ex, "Exception");
			}
		}

		private void btn_Close_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				CloseEvent?.Invoke(this, e);
			}
			catch (Exception ex)
			{
				logger.Error(ex, "Exception");
			}
		}

		private int test = 0;
		private void SelectedDateChanged(object sender, SelectionChangedEventArgs e)
		{
			try
			{
				//若當前時間已超過最大值，重新設定最大值
				if (DateTime.Now > m_EndDTCbx.DisplayDateEnd)
				{
					m_StartDTCbx.DisplayDateEnd = DateTime.Today.AddDays(1).AddMilliseconds(-1);
					m_EndDTCbx.DisplayDateEnd = DateTime.Today.AddDays(1).AddMilliseconds(-1);
				}
			}
			catch (Exception ex)
			{
				logger.Error(ex, "Exception");
			}
		}
		private void Button_Click_1(object sender, RoutedEventArgs e)
		{
			////ElasticSearchManager.ELASTIC_TABLE_INDEX_RECODEREPORTINFO;
			//string url = com.mirle.ibg3k0.ohxc.wpf.Common.ElasticSearchManager.ELASTIC_URL;
			//var node = new Uri($"http://{url}:9200");
			//var settings = new Nest.ConnectionSettings(node).DefaultIndex("default");
			//settings.DisableDirectStreaming();
			//var client = new Nest.ElasticClient(settings);

   //         com.mirle.ibg3k0.ohxc.wpf.Service.ASYSEXCUTEQUALITY aSYSEXCUTEQUALITY = new com.mirle.ibg3k0.ohxc.wpf.Service.ASYSEXCUTEQUALITY()
			//{
			//	CMDQUEUE_TIME = 1,
			//	CMD_FINISH_STATUS = E_CMD_STATUS.AbnormalEndByMCS,
			//	CMD_FINISH_TIME = DateTime.Now.AddDays(1),
			//	CMD_INSERT_TIME = DateTime.Now,
			//	CMD_START_TIME = DateTime.Now,
			//	CMD_TOTAL_EXCUTION_TIME = 1,
			//	CST_ID = (test++).ToString(),
			//	CYCLERUN_VH_COUNT = 1,
			//	DESTINATION_ADR = "SS",
			//	MCS_CMD_ID = "A",
			//	MOVE_TO_DESTN_TIME = 1,
			//	MOVE_TO_SOURCE_TIME = 1,
			//	PARKING_VH_COUNT = 1,
			//	SEC_CNT_TO_DESTN = 1,
			//	SEC_CNT_TO_SOURCE = 1,
			//	SEC_DIS_TO_DESTN = 1,
			//	SEC_DIS_TO_SOURCE = 1,
			//	SOURCE_ADR = "A",
			//	TOTALPAUSE_TIME = 1,
			//	TOTAL_ACT_VH_COUNT = 1,
			//	TOTAL_BLOCK_COUNT_TO_DESTN = 1,
			//	TOTAL_BLOCK_COUNT_TO_SOURCE = 1,
			//	TOTAL_BLOCK_TIME_TO_DESTN = 1,
			//	TOTAL_BLOCK_TIME_TO_SOURCE = 1,
			//	TOTAL_IDLE_VH_COUNT = 1,
			//	TOTAL_OCS_COUNT_TO_DESTN = 1,
			//	TOTAL_OCS_COUNT_TO_SOURCE = 1,
			//	TOTAL_OCS_TIME_TO_DESTN = 2,
			//	TOTAL_OCS_TIME_TO_SOURCE = 1,
			//	VH_ID = "S",
			//	VH_START_SEC_ID = "A"
			//};
			//sysExcuteQualityQueryService.indexData(aSYSEXCUTEQUALITY);
			////client.Index(aSYSEXCUTEQUALITY, index => index.Index($"{Common.ElasticSearchManager.ELASTIC_TABLE_INDEX_SYSEXCUTEQUALITY}"));
		}
	}
}
