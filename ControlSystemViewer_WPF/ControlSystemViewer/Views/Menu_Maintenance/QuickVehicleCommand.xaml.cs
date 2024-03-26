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
using static ControlSystemViewer.Views.Menu_Maintenance.VehicleManagement;

namespace ControlSystemViewer.Views.Menu_Maintenance
{
    /// <summary>
    /// QuickVehicleCommand.xaml 的互動邏輯
    /// </summary>
    public partial class QuickVehicleCommand : UserControl
    {
        #region 公用參數設定
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private WindownApplication app = null;
        #endregion 公用參數設定

        public QuickVehicleCommand()
        {
            InitializeComponent();

            this.Loaded += _Load;
        }

        private void _Load(object sender, RoutedEventArgs e)
        {
            try
            {
                combo_CmdType.Focus(); //將游標放置指定位置
                System.Windows.Input.InputMethod.SetIsInputMethodEnabled(txt_CarrierID, false); //設置IME和輸入是否可以是中文
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
            }
        }

        public void Close()
        {
            unregisterEvent();
        }

        public void InitUI(Map_Parts.Vehicle selectedVh = null)
        {
            try
            {
                app = WindownApplication.getInstance();

                registerEvent();

                combo_CmdType.ItemsSource = Enum.GetValues(typeof(E_UICMD_TYPE)).Cast<E_UICMD_TYPE>();
                combo_CmdType.SelectedIndex = combo_CmdType.Items.IndexOf(E_UICMD_TYPE.Move);

                //// 暫時只開放 Move 
                //combo_CmdType.IsEnabled = false;
                //combo_CmdType.Foreground = new SolidColorBrush(Color.FromRgb(150, 150, 150));
                //txt_CarrierID.IsEnabled = false;

                foreach (var adr in app.ObjCacheManager.Addresses)
                {
                    string s_address = adr.ID;
                    try
                    {
                        VPORTSTATION port = app.ObjCacheManager.GetPortStation(s_address);
                        if (port != null)
                        {
                            s_address = s_address + $" ({port.PORT_ID.Trim()})";
                        }
                        combo_Src.Items.Add(s_address);
                        combo_Dest.Items.Add(s_address);
                    }
                    catch (Exception ex)
                    {
                    }
                }
                combo_Src.SelectedIndex = combo_Src.Items.Count > 0 ? 0 : -1;
                combo_Dest.SelectedIndex = combo_Dest.Items.Count > 0 ? 0 : -1;

                combo_VhID.Items.Clear();
                int indexOfSelectedVh = -1;
                for (int i = 0; i < app.ObjCacheManager.GetVEHICLEs().Count; i++)
                {
                    var vh = app.ObjCacheManager.GetVEHICLEs()[i];
                    string sVhID = vh.VEHICLE_ID.Trim();
                    combo_VhID.Items.Add(sVhID);
                    if (selectedVh != null && indexOfSelectedVh == -1)
                    {
                        if (selectedVh.p_ID.Trim() == sVhID)
                        {
                            indexOfSelectedVh = i;
                            string sCstID = vh.CST_ID_L?.Trim();
                            if (!string.IsNullOrEmpty(sCstID))
                            {
                                txt_CarrierID.Text = sCstID;
                                //combo_CmdType.SelectedIndex = combo_CmdType.Items.IndexOf(E_UICMD_TYPE.Unload);
                            }
                            string sCurAdr = vh.CUR_ADR_ID?.Trim();
                            if (!string.IsNullOrEmpty(sCurAdr))
                            {
                                SelectItem(FromAdrID: sCurAdr);
                            }
                        }
                    }
                }
                combo_VhID.SelectedIndex = indexOfSelectedVh;
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
            }
        }

        public void SelectItem(string VhID = null, string FromAdrID = null, string ToAdrID = null, int FromAdrNo = -1, int ToAdrNo = -1)
        {
            try
            {
                app = app ?? WindownApplication.getInstance();

                if (VhID != null)
                {
                    int newSelectedIndex = combo_VhID.Items.IndexOf(VhID);
                    if (newSelectedIndex >= 0)
                    {
                        combo_VhID.SelectedIndex = newSelectedIndex;
                        combo_VhID.Focus();
                    }
                    var vh = app.ObjCacheManager.GetVEHICLE(VhID);
                    string sCstID = vh?.CST_ID_L?.Trim();
                    if (!string.IsNullOrEmpty(sCstID))
                    {
                        txt_CarrierID.Text = sCstID;
                        //combo_CmdType.SelectedIndex = combo_CmdType.Items.IndexOf(E_UICMD_TYPE.Unload);
                    }
                    string sCurAdr = vh?.CUR_ADR_ID?.Trim();
                    if (!string.IsNullOrEmpty(sCurAdr))
                    {
                        FromAdrID = sCurAdr;
                    }
                }

                if (FromAdrID != null)
                {
                    string s_address = FromAdrID;
                    VPORTSTATION port = app.ObjCacheManager.GetPortStation(s_address);
                    if (port != null)
                    {
                        s_address = s_address + $" ({port.PORT_ID.Trim()})";
                    }
                    int newSelectedIndex = combo_Src.Items.IndexOf(s_address);
                    if (newSelectedIndex >= 0)
                    {
                        combo_Src.SelectedIndex = newSelectedIndex;
                        combo_Src.Focus();
                    }
                }

                if (ToAdrID != null)
                {
                    string s_address = ToAdrID;
                    VPORTSTATION port = app.ObjCacheManager.GetPortStation(s_address);
                    if (port != null)
                    {
                        s_address = s_address + $" ({port.PORT_ID.Trim()})";
                    }
                    int newSelectedIndex = combo_Dest.Items.IndexOf(s_address);
                    if (newSelectedIndex >= 0)
                    {
                        combo_Dest.SelectedIndex = newSelectedIndex;
                        combo_Dest.Focus();
                    }
                }

                if (FromAdrNo >= 0)
                {
                    combo_Src.SelectedIndex = FromAdrNo;
                    combo_Src.Focus();
                }

                if (ToAdrNo >= 0)
                {
                    combo_Dest.SelectedIndex = ToAdrNo;
                    combo_Dest.Focus();
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
            }
        }

        private void registerEvent()
        {
            try
            {
                combo_CmdType.SelectionChanged += commandTypeSelectionChanged;
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
            }
        }

        private void unregisterEvent()
        {
            try
            {
                combo_CmdType.SelectionChanged -= commandTypeSelectionChanged;
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
            }
        }

        private void commandTypeSelectionChanged(object sender, EventArgs e)
        {
            try
            {
                switch ((E_UICMD_TYPE)combo_CmdType.SelectedItem)
                {
                    case E_UICMD_TYPE.Move:
                    case E_UICMD_TYPE.Unload:
                        combo_Src.IsEnabled = false;
                        title_Src.Foreground = new SolidColorBrush(Color.FromRgb(150, 150, 150));
                        combo_Src.Foreground = new SolidColorBrush(Color.FromRgb(150, 150, 150));

                        combo_Dest.IsEnabled = true;
                        title_Dest.Foreground = new SolidColorBrush(Color.FromRgb(27, 35, 56));
                        combo_Dest.Foreground = new SolidColorBrush(Color.FromRgb(27, 35, 56));
                        break;

                    case E_UICMD_TYPE.Load:
                        combo_Src.IsEnabled = true;
                        title_Src.Foreground = new SolidColorBrush(Color.FromRgb(27, 35, 56));
                        combo_Src.Foreground = new SolidColorBrush(Color.FromRgb(27, 35, 56));

                        combo_Dest.IsEnabled = false;
                        title_Dest.Foreground = new SolidColorBrush(Color.FromRgb(150, 150, 150));
                        combo_Dest.Foreground = new SolidColorBrush(Color.FromRgb(150, 150, 150));
                        break;

                    case E_UICMD_TYPE.LoadUnload:
                        combo_Src.IsEnabled = true;
                        title_Src.Foreground = new SolidColorBrush(Color.FromRgb(27, 35, 56));
                        combo_Src.Foreground = new SolidColorBrush(Color.FromRgb(27, 35, 56));

                        combo_Dest.IsEnabled = true;
                        title_Dest.Foreground = new SolidColorBrush(Color.FromRgb(27, 35, 56));
                        combo_Dest.Foreground = new SolidColorBrush(Color.FromRgb(27, 35, 56));
                        break;

                    //case E_UICMD_TYPE.Scan:
                    //	combo_Src.IsEnabled = false;
                    //	combo_Dest.IsEnabled = true;
                    //	break;

                    default:
                        combo_Src.IsEnabled = true;
                        title_Src.Foreground = new SolidColorBrush(Color.FromRgb(27, 35, 56));
                        combo_Src.Foreground = new SolidColorBrush(Color.FromRgb(27, 35, 56));

                        combo_Dest.IsEnabled = true;
                        title_Dest.Foreground = new SolidColorBrush(Color.FromRgb(27, 35, 56));
                        combo_Dest.Foreground = new SolidColorBrush(Color.FromRgb(27, 35, 56));
                        break;
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
            }
        }

        private async void btn_Confirm_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string select_vh_id = combo_VhID.Text;
                string cmd_type = combo_CmdType.Text;
                string carrier = txt_CarrierID.Text;
                string source = ((string)combo_Src.SelectedItem).Split(' ')[0];
                string destination = ((string)combo_Dest.SelectedItem).Split(' ')[0];

                bool isSuccess = false;
                string result = "";
                await Task.Run(() => isSuccess = app.VehicleBLL.SendCmdToControl(select_vh_id, cmd_type, carrier, source, destination, ref result));
                if (!isSuccess)
                {
                    TipMessage_Type_Light.Show("Send command failed", result, BCAppConstants.INFO_MSG);
                }
                else
                {
                    TipMessage_Type_Light_woBtn.Show("", "Send command succeeded", BCAppConstants.INFO_MSG);
                }
                app.OperationHistoryBLL.
                    addOperationHis(app.LoginUserID,
                                    this.GetType().Name,
                                    $"Excute vh:{select_vh_id} send command type:{cmd_type},cst id:{carrier},source:{source} dest:{destination}, is success:{isSuccess}");
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
            }
        }
    }
}
