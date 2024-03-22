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
using static ViewerObject.VVEHICLE_Def;

namespace ControlSystemViewer.Views.Components
{
    /// <summary>
    /// VhSettingS1.xaml 的互動邏輯
    /// </summary>
    public partial class VhSettingS4 : UserControl
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        TextBlock[] txtblocks;
        TextBlock[] values;
        Button[] btns;

        public VhSettingS4()
        {
            InitializeComponent();
            btn_Title1.Tag = ModeStatus.AutoRemote;
            btn_Title2.Tag = ModeStatus.AutoLocal;
            btn_Title3.Tag = ModeStatus.Manual;

            btn_Title5.Tag = InstallStatus.Installed;
            btn_Title6.Tag = InstallStatus.Removed;

            //btn_Title3.Tag = VHModeStatus.AutoMtl;
            //btn_Title4.Tag = VHModeStatus.AutoMts;
            //btn_Title4.Tag = VHModeStatus.Manual;

            txtblocks = new TextBlock[] { txb_Title1, txb_Title2, txb_Title3, txb_Title4, txb_Title5, txb_Title6, txb_Title7 };
            values = new TextBlock[] { txb_Value1, txb_Value2, txb_Value3, txb_Value4, txb_Value5, txb_Value6, txb_Value7, };
            btns = new Button[] { btn_Title1, btn_Title2, btn_Title3, btn_Title4, btn_Title5, btn_Title6 };

            //txtblocks = TitlePanel.Children.AsQueryable().OfType<TextBlock>().ToArray();
        }

        public void SetTXBTitleName(params string[] titleNames)
        {
            try
            {
                for (int i = 0; i < txtblocks.Length; i++)
                {
                    txtblocks[i].Text = titleNames.ElementAtOrDefault(i) ?? "";
                    txtblocks[i].Visibility = titleNames.ElementAtOrDefault(i) == null ? Visibility.Collapsed : Visibility.Visible;
                    values[i].Visibility = titleNames.ElementAtOrDefault(i) == null ? Visibility.Collapsed : Visibility.Visible;
                }

            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
            }
        }

        public void SetBTNTitleName(params string[] btnNames)
        {
            try
            {
                for (int i = 0; i < btns.Length; i++)
                {
                    btns[i].Content = btnNames.ElementAtOrDefault(i) ?? "";
                    btns[i].Visibility = btnNames.ElementAtOrDefault(i) == null ? Visibility.Collapsed : Visibility.Visible;
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
            }
        }

        public void SetVehicleCmdInfo(string action_status, string MCS_cmd_id, string OHxC_cmd_id, string cmd_type, string carrier_id, string source, string destination)
        {
            try
            {
                txb_Value1.Text = action_status;
                txb_Value2.Text = MCS_cmd_id;
                txb_Value3.Text = OHxC_cmd_id;
                txb_Value4.Text = cmd_type;
                txb_Value5.Text = carrier_id;
                txb_Value6.Text = source;
                txb_Value7.Text = destination;
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
            }
        }
        public void SetTXBVehicleInfo(string vh_id, string mode, string curr_adr, string curr_sec, string sec_dis, string alarm_sts, string install_remove)
        {
            try
            {
                txb_Value1.Text = vh_id;
                txb_Value2.Text = mode;
                txb_Value3.Text = curr_adr;
                txb_Value4.Text = curr_sec;
                txb_Value5.Text = sec_dis;
                txb_Value6.Text = alarm_sts;
                txb_Value7.Text = install_remove;
                bool AllEnable = true;
                for (int i = 0; i < btns.Length; i++)
                {
                    if (btns[i].Tag is ModeStatus modeStatus)
                    {
                        if (mode == modeStatus.ToString())
                        {
                            btns[i].IsEnabled = false;
                            AllEnable = false;
                        }
                        else
                        {
                            btns[i].IsEnabled = true;
                        }
                    }
                    if (btns[i].Tag is InstallStatus installStatus)
                    {
                        if (install_remove == installStatus.ToString())
                        {
                            btns[i].IsEnabled = false;
                            AllEnable = false;
                        }
                        else
                        {
                            btns[i].IsEnabled = true;
                        }
                    }
                }
                if (AllEnable == true)
                {
                    btn_Title1.IsEnabled = true;
                    btn_Title2.IsEnabled = true;
                    btn_Title3.IsEnabled = true;
                    btn_Title4.IsEnabled = true;
                    btn_Title5.IsEnabled = true;
                }

                //if(alarm_sts == "0")
                //{
                //    btn_Title5.IsEnabled = false;
                //}
                //else
                //{
                //    btn_Title5.IsEnabled = true;
                //}
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
            }
        }
    }
}
