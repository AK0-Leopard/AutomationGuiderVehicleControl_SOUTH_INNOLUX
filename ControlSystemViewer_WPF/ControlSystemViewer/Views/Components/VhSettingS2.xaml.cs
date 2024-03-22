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

namespace ControlSystemViewer.Views.Components
{
    /// <summary>
    /// VhSettingS2.xaml 的互動邏輯
    /// </summary>
    public partial class VhSettingS2 : UserControl
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public VhSettingS2()
        {
            InitializeComponent();
        }

        public void SetTXBTitleName(string titleName1, string titleName2, string titleName3, string titleName4,
            string titleName5, string titleName6, string titleName7, string titleName8)
        {
            try
            {
                txb_Title1.Text = titleName1;
                txb_Title2.Text = titleName2;
                txb_Title3.Text = titleName3;
                txb_Title4.Text = titleName4;
                txb_Title5.Text = titleName5;
                txb_Title6.Text = titleName6;
                txb_Title7.Text = titleName7;
                txb_Title8.Text = titleName8;
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
            }
        }

        public void SetBTNTitleName(string btnName1, string btnName2)
        {
            try
            {
                btn_Title1.Content = btnName1;
                btn_Title2.Content = btnName2;
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
            }
        }

        public void SetTXBPauseInfo(string err_sts, string normal_pause, string block_pause, string obs_pause, string HID_pause, string safety_pause, string earthquake_pause)
        {
            try
            {
                txb_Value1.Text = err_sts;
                txb_Value2.Text = normal_pause;
                txb_Value3.Text = block_pause;
                txb_Value4.Text = obs_pause;
                txb_Value5.Text = HID_pause;
                txb_Value6.Text = safety_pause;
                txb_Value7.Text = earthquake_pause;
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
            }
        }
    }
}
