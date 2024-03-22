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

namespace ControlSystemViewer.Views
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
        }

        public void Start()
        {
            btn_ResetBuzzer.Visibility = Visibility.Collapsed; //由於僅有南群使用到，暫時將其隱藏
            app = WindownApplication.getInstance();

            System.Windows.Input.InputMethod.SetIsInputMethodEnabled(txt_CarrierID, false); //設置IME和輸入是否可以是中文

            registerEvent();

            combo_VhID.SelectedIndex = -1;
            combo_VhID.Items.Clear();
            var vhs = app.ObjCacheManager.GetVEHICLEs();
            if (vhs?.Count > 0)
            {
                foreach (var vh in vhs)
                {
                    combo_VhID.Items.Add(vh.VEHICLE_ID);
                }
            }

            combo_CmdType.SelectedIndex = -1;
            combo_CmdType.Items.Clear();
            foreach (var cmdType in Enum.GetValues(typeof(E_UICMD_TYPE)).Cast<E_UICMD_TYPE>())
            {
                combo_CmdType.Items.Add(cmdType.ToString());
            }
            //combo_CmdType.ItemsSource = Enum.GetValues(typeof(E_UICMD_TYPE)).Cast<E_UICMD_TYPE>(); // 直接用字會變白色的
            combo_CmdType.SelectedIndex = combo_CmdType.Items.IndexOf(E_UICMD_TYPE.Move.ToString());



            if (app.ObjCacheManager.ViewerSettings.system.HasCarrierType)
            {
                title_CarrierType.Visibility = Visibility.Visible;
                combo_CarrierType.Visibility = Visibility.Visible;
                combo_CarrierType.Items.Clear();
                foreach (var carriertype in Enum.GetValues(typeof(VCMD_Def.CarrierType)).Cast<VCMD_Def.CarrierType>())
                {
                    combo_CarrierType.Items.Add(carriertype.ToString());
                }
                combo_CarrierType.SelectedIndex = combo_CarrierType.Items.IndexOf(VCMD_Def.CarrierType.Type1.ToString());
            }
            else
            {
                GridRow15.Height = new GridLength(0);
                GridRow16.Height = new GridLength(0);
                GridRow17.Height = new GridLength(0);
            }


            HashSet<string> port_shelf_ids = new HashSet<string>();
            foreach (var adr in app.ObjCacheManager.Addresses)
            {
                string s_address = adr.ID;
                try
                {
                    VPORTSTATION port = app.ObjCacheManager.GetPortStation(s_address);
                    if (port != null)
                    {
                        s_address = s_address + $" ({port.PORT_ID.Trim()})";
                        port_shelf_ids.Add(port.PORT_ID.Trim());
                    }
                    combo_Src.Items.Add(s_address);
                    combo_Dest.Items.Add(s_address);
                }
                catch (Exception ex)
                {
                }
            }
            foreach (var port_station in app.ObjCacheManager.GetPortStations())
            {

                if (!port_shelf_ids.Contains(port_station.PORT_ID.Trim()))
                {
                    string s_port = "*" + $" ({port_station.PORT_ID.Trim()})";
                    combo_Src.Items.Add(s_port);
                    combo_Dest.Items.Add(s_port);
                }
            }
            foreach (var shelf in app.ObjCacheManager.Shelves)
            {

                if (!port_shelf_ids.Contains(shelf.SHELF_ID.Trim()))
                {
                    string s_shelf = "*" + $" ({shelf.SHELF_ID.Trim()})";
                    combo_Src.Items.Add(s_shelf);
                    combo_Dest.Items.Add(s_shelf);
                }
            }

            combo_Src.SelectedIndex = combo_Src.Items.Count > 0 ? 0 : -1;
            combo_Dest.SelectedIndex = combo_Dest.Items.Count > 0 ? 0 : -1;
        }


        public void SelectVehicle(string VhID)
        {
            SelectItem(VhID: VhID);
        }
        public void SelectFromAdr(string AdrID)
        {
            SelectItem(FromAdrID: AdrID);
        }
        public void SelectToAdr(string AdrID)
        {
            SelectItem(ToAdrID: AdrID);
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
            unregisterEvent();
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
                switch (GetE_UICMD_TYPE(combo_CmdType.SelectedItem?.ToString()))
                {
                    case E_UICMD_TYPE.Move:
                    case E_UICMD_TYPE.Unload:
                        combo_Src.IsEnabled = false;
                        title_Src.Foreground = new SolidColorBrush(Color.FromRgb(150, 150, 150));
                        combo_Src.Foreground = new SolidColorBrush(Color.FromRgb(150, 150, 150));

                        combo_Dest.IsEnabled = true;
                        title_Dest.Foreground = Brushes.White;
                        combo_Dest.Foreground = Brushes.Black;
                        break;

                    case E_UICMD_TYPE.Load:
                        combo_Src.IsEnabled = true;
                        title_Src.Foreground = Brushes.White;
                        combo_Src.Foreground = Brushes.Black;

                        combo_Dest.IsEnabled = false;
                        title_Dest.Foreground = new SolidColorBrush(Color.FromRgb(150, 150, 150));
                        combo_Dest.Foreground = new SolidColorBrush(Color.FromRgb(150, 150, 150));
                        break;

                    case E_UICMD_TYPE.LoadUnload:
                        combo_Src.IsEnabled = true;
                        title_Src.Foreground = Brushes.White;
                        combo_Src.Foreground = Brushes.Black;

                        combo_Dest.IsEnabled = true;
                        title_Dest.Foreground = Brushes.White;
                        combo_Dest.Foreground = Brushes.Black;
                        break;

                    //case E_UICMD_TYPE.Scan:
                    //	combo_Src.IsEnabled = false;
                    //	combo_Dest.IsEnabled = true;
                    //	break;

                    default:
                        combo_Src.IsEnabled = true;
                        title_Src.Foreground = Brushes.White;
                        combo_Src.Foreground = Brushes.Black;

                        combo_Dest.IsEnabled = true;
                        title_Dest.Foreground = Brushes.White;
                        combo_Dest.Foreground = Brushes.Black;
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
                string carrier_type = "";
                if (source == "*")
                    source = "";
                if (destination == "*")
                    destination = "";
                string source_port = ((string)combo_Src.SelectedItem).Split(' ').Count() > 1 ?
                                     ((string)combo_Src.SelectedItem).Split(' ')[1].Substring(1, ((string)combo_Src.SelectedItem).Split(' ')[1].Length - 2) : "";
                string destination_port = ((string)combo_Dest.SelectedItem).Split(' ').Count() > 1 ? ((string)combo_Dest.SelectedItem).Split(' ')[1].Substring(1, ((string)combo_Dest.SelectedItem).Split(' ')[1].Length - 2) : "";

                if (app.ObjCacheManager.ViewerSettings.system.HasCarrierType)
                {
                    carrier_type = GetCarrierType((string)combo_CarrierType.SelectedItem);
                }

                bool isSuccess = false;
                string result = "";
                await Task.Run(() => isSuccess = app.VehicleBLL.SendCmdToControl(select_vh_id, cmd_type, carrier, source, destination, ref result, carrier_type, source_port, destination_port));
                if (!isSuccess)
                {
                    TipMessage_Type_Light.Show("Send command failed", result, BCAppConstants.INFO_MSG);
                }
                else
                {
                    TipMessage_Type_Light_woBtn.Show("", "Send command succeeded", BCAppConstants.INFO_MSG);
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
            }
        }

        private string GetCarrierType(string CarrierType)
        {
            try
            {
                switch (CarrierType)
                {
                    case nameof(VCMD_Def.CarrierType.Type1):
                        return "1";
                    case nameof(VCMD_Def.CarrierType.Type2):
                        return "2";
                    case nameof(VCMD_Def.CarrierType.Type3):
                        return "3";
                    case nameof(VCMD_Def.CarrierType.Type4):
                        return "4";
                    default: return "";
                }
            }
            catch (Exception ex)
            {
                logger.Warn(ex.StackTrace, "Exception");
                return "";
            }
        }


        public enum E_UICMD_TYPE
        {
            Move = 0,
            Load = 3,
            Unload = 4,
            LoadUnload = 5
            //Scan = 15
        }
        public E_UICMD_TYPE GetE_UICMD_TYPE(string sType)
        {
            switch (sType)
            {
                case "Load":
                    return E_UICMD_TYPE.Load;

                case "Unload":
                    return E_UICMD_TYPE.Unload;

                case "LoadUnload":
                    return E_UICMD_TYPE.LoadUnload;

                //case E_UICMD_TYPE.Scan:
                //    return E_UICMD_TYPE.Scan;

                case "Move":
                default:
                    return E_UICMD_TYPE.Move;
            }
        }

        private async void btn_ResetBuzzer_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                await Task.Run(() =>  app.ObjCacheManager.resetBuzzer());
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
            }
        }
    }
}
