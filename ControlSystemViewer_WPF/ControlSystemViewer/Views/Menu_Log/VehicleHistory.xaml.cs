//#define IS_FOR_OHTC_NOT_AGVC // 若對應AGVC，則註解此行

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
    /// AGVHistory.xaml 的互動邏輯
    /// </summary>
    public partial class VehicleHistory : UserControl
    {
        #region 公用參數設定
        private static Logger logger = LogManager.GetCurrentClassLogger();
        //SysExcuteQualityQueryService sysExcuteQualityQueryService;
        public event EventHandler CloseEvent;

		private DateTime SearchTimeFrom = DateTime.MinValue;
		private DateTime SearchTimeTo = DateTime.MinValue;
		#endregion 公用參數設定

		public VehicleHistory()
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
				cb_VhID.Items.Clear();
				cb_VhID.Items.Add("");
				var vehicleList = app?.ObjCacheManager.GetVEHICLEs();
				if (vehicleList?.Count > 0)
				{
					foreach (var vehicle in vehicleList)
					{
						cb_VhID.Items.Add(vehicle.VEHICLE_ID);
					}
				}

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
				txt_CstID.Text = "";
				cb_VhID.SelectedIndex = -1;
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
					TipMessage_Type_Light.Show("Failure", String.Format("Please select start time."), BCAppConstants.WARN_MSG);
					return;
				}
				if (string.IsNullOrWhiteSpace(m_EndDTCbx.SelectedDate.ToString()))
				{
					TipMessage_Type_Light.Show("Failure", String.Format("Please select end time."), BCAppConstants.WARN_MSG);
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
				string cst_id = string.Empty;
				string vh_id = string.Empty;
				if (!string.IsNullOrWhiteSpace(txt_CstID.Text))
				{
					cst_id = txt_CstID.Text?.Trim();
				}
				if (!string.IsNullOrWhiteSpace(cb_VhID.Text))
				{
					vh_id = cb_VhID.Text?.Trim();
				}
				var scApp = WindownApplication.getInstance();
				dateTimeFrom = new DateTime(dateTimeFrom.Year, dateTimeFrom.Month, dateTimeFrom.Day, dateTimeFrom.Hour, dateTimeFrom.Minute, dateTimeFrom.Second, DateTimeKind.Local);
				dateTimeTo = new DateTime(dateTimeTo.Year, dateTimeTo.Month, dateTimeTo.Day, dateTimeTo.Hour, dateTimeTo.Minute, dateTimeTo.Second, DateTimeKind.Local);
				//List<com.mirle.ibg3k0.sc.ACMD> ohtc_list = null;
				List<VCMD> hcmd_list = null;
				//await Task.Run(() => system_qualitys = sysExcuteQualityQueryService.loadOHTHHistory(dateTimeFrom, dateTimeTo));
				//await Task.Run(() => ohtc_list = scApp.CmdBLL.GetCmd_OhtcByConditions(dateTimeFrom, dateTimeTo, cst_id, vh_id));
				((MainWindow)App.Current.MainWindow).Loading_Start("Searching");
				isLoading = true;
				await Task.Run(() => hcmd_list = scApp.CmdBLL.GetHCmdByConditions(dateTimeFrom, dateTimeTo, cst_id, vh_id));
				if (hcmd_list == null || hcmd_list.Count <= 0)
				{
					var languageDictionary = App.Current.Resources.MergedDictionaries?[0];
					string sTipMsg = languageDictionary?["TIPMSG_QUERY_NO_DATA"]?.ToString() ?? "There is no matching data for your query.";
					TipMessage_Type_Light.Show("", sTipMsg, BCAppConstants.WARN_MSG);
				}
				else
				{
					//ohtc_list = ohtc_list.OrderBy(info => info.CMD_START_TIME).ToList();
					dgv_log_query.ItemsSource = hcmd_list;
					//uctl_ElasticQuery_CMDExcute_1.setDataItemsSource(system_qualitys);
				}
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
				var data_list = dgv_log_query.ItemsSource as List<VCMD>;
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

		private void Button_Click_1(object sender, RoutedEventArgs e)
		{
			//string url = com.mirle.ibg3k0.ohxc.wpf.Common.ElasticSearchManager.ELASTIC_URL;
			//var node = new Uri($"http://{url}:9200");
			//var settings = new Nest.ConnectionSettings(node).DefaultIndex("default");
			//settings.DisableDirectStreaming();
			//var client = new Nest.ElasticClient(settings);

			//com.mirle.ibg3k0.sc.AVEHICLE aVEHICLE = new AVEHICLE()
			//{
			//	MANT_DATE = DateTime.Now,
			//	GRIP_MANT_DATE = DateTime.Now,
			//	ACC_SEC_DIST = 1,
			//	ACT_STATUS = VHActionStatus.Commanding,
			//	CST_ID = "CST"
			//};
			//sysExcuteQualityQueryService.indexData(aVEHICLE);
			////client.Index(aVEHICLE, index => index.Index($"{Common.ElasticSearchManager.ELASTIC_TABLE_INDEX_OHTHistory}"));
			////client.IndexDocument(aLARM);

		}
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
	}
}
