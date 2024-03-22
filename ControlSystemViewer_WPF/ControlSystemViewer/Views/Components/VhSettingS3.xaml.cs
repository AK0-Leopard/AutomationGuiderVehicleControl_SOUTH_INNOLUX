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
    /// VhSettingS3.xaml 的互動邏輯
    /// </summary>
    public partial class VhSettingS3 : UserControl
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public VhSettingS3()
        {
            InitializeComponent();
        }

        public void SetTXBTitleName(string titleName1, string titleName2, string titleName3, string titleName4, string titleName5)
        {
            try
            {
                txb_Title1.Text = titleName1 + " : ";
                txb_Title2.Text = titleName2 + " : ";
                txb_Title3.Text = titleName3 + " : ";
                txb_Title4.Text = titleName4 + " : ";
                txb_Title5.Text = titleName5 + " : ";
                //txb_Title6.Text = titleName6 + " : ";
                //combo_Content2.IsReadOnly = true;

                //txt_Content2.IsReadOnly = true;
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
            }
        }

        public void SetVehicleCommandInfo(string vh_id, string cst, string source)
        {
            try
            {
                if (txt_Content1.IsReadOnly == false)
                {
                    txt_Content1.IsReadOnly = true;
                }


                txt_Content1.Text = vh_id;
                txt_Content2.Text = cst;
                combo_Content2.SelectedItem = source;
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
            }
        }

        public void SetBTNTitleName(string btnName1)
        {
            try
            {
                btn_Title1.Content = btnName1;
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
            }
        }
    }
}
