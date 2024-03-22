using com.mirle.ibg3k0.Utility.uc;
using com.mirle.ibg3k0.bc.wpf.App;
using com.mirle.ibg3k0.ohxc.wpf.App;
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
using NLog;

namespace ControlSystemViewer.Views.Menu_Maintenance
{
    /// <summary>
    /// Equipment_Constant.xaml 的互動邏輯
    /// </summary>
    public partial class Equipment_Constant : UserControl
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private WindownApplication app = null;
        public event EventHandler CloseEvent;

        public Equipment_Constant()
        {
            InitializeComponent();
        }

        public void startUI()
        {
            app = WindownApplication.getInstance();
            registerEvent();
            dgv_EQConstant.ItemsSource = app.ObjCacheManager.CONSTANTs;
        }

        public void Close()
        {
            try
            {
                unRegisterEvent();
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
            }
        }

        private void registerEvent()
        {
            app.ObjCacheManager.ConstantChange += ObjCacheManager_ConstantChange;
        }

        private void unRegisterEvent()
        {
            app.ObjCacheManager.ConstantChange -= ObjCacheManager_ConstantChange;
        }



        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            VCONSTANT vconstant = (VCONSTANT)this.dgv_EQConstant.SelectedItem;
            TB_ECID.Text = vconstant.ECID;
            TB_EC_NAME.Text = vconstant.ECNAME;
            TB_ECValue.Text = vconstant.ECV == null ? "" : vconstant.ECV.Trim();
            TB_MaxValue.Text = vconstant.ECMAX;
            TB_MinValue.Text = vconstant.ECMIN;
        }

        private void BT_Save_MouseClick(object sender, RoutedEventArgs e)
        {
            app.ObjCacheManager.constantControl(TB_ECID.Text, TB_ECValue.Text);
            refreshEQConstant();
        }

        private void ObjCacheManager_ConstantChange(object sender, EventArgs e)
        {

            try
            {
                refreshEQConstant();
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
            }
        }

        private void refreshEQConstant()
        {
            Adapter.Invoke((obj) =>
            {
                this.dgv_EQConstant.ItemsSource = app.ObjCacheManager.CONSTANTs;
            }, null);
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
    }
}
