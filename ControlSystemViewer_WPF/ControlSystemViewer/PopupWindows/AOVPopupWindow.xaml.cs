using com.mirle.ibg3k0.ohxc.wpf.App;
using ControlSystemViewer.Views;
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
using System.Windows.Shapes;
using ViewerObject;

namespace ControlSystemViewer.PopupWindows
{
    /// <summary>
    /// AOVPopupWindow.xaml 的互動邏輯
    /// </summary>
    public partial class AOVPopupWindow : Window
    {
        #region 公用參數設定
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private WindownApplication app = null;
        private Settings.AOVPopupWindow aovPopupWindowSettings = null;
        Map_Base map_Base = null;
        #endregion 公用參數設定

        public AOVPopupWindow()
        {
            InitializeComponent();

            this.Loaded += _Loaded;
        }

        private void _Loaded(object sender, EventArgs e)
        {
            app = WindownApplication.getInstance();
            aovPopupWindowSettings = app?.ObjCacheManager.ViewerSettings?.aovPopupWindow;
            map_Base = ((MainWindow)App.Current.MainWindow).MainView.MainLayout.Map_Base;
            var currAOV = map_Base.GetCurrAOV();
            var currFlipH = map_Base.GetcurrFlipH();
            var currFlipV = map_Base.GetcurrFlipV();

            switch (aovPopupWindowSettings?.Style)
            {
                case 1:
                    this.Height = 460;
                    this.Width = 300;
                    var uc_AOV_1 = new Views.Components.AngleOfView_1();
                    uc_AOV_1.SelectAOV(currAOV);
                    uc_AOV_1.SelectFlipH(currFlipH);
                    if (currFlipH) uc_AOV_1.ckb_FlipH.IsChecked = true;
                    uc_AOV_1.SelectFlipV(currFlipV);
                    if (currFlipV) uc_AOV_1.ckb_FlipV.IsChecked = true;
                    uc_AOV_1.AngleOfViewChanged += _AngleOfViewChangedWithFlip;
                    uc_AOV_1.HorizontalFlipChanged += _HorizontalFlipChanged;
                    uc_AOV_1.VerticalFlipChanged += _VerticalFlipChanged;
                    grid.Children.Add(uc_AOV_1);
                    break;
                default:
                    var uc_AOV = new Views.Components.AngleOfView();
                    uc_AOV.SelectAOV(currAOV);
                    uc_AOV.AngleOfViewChanged += _AngleOfViewChanged;
                    uc_AOV.Margin = new Thickness(10);
                    grid.Children.Add(uc_AOV);
                    break;
            }
        }

        private void _AngleOfViewChanged(object sender, Definition.AngleOfViewType aov)
        {
            map_Base?.ChangeAOV(sender, aov);
        }

        #region With Flip
        private void _AngleOfViewChangedWithFlip(object sender, Definition.AngleOfViewType aov)
        {
            map_Base?.ChangeAOVWithFlip(sender, aov);
        }

        private void _HorizontalFlipChanged(object sender, bool FlipH)
        {
            map_Base?.ChangeFlipH(sender, FlipH);
        }

        private void _VerticalFlipChanged(object sender, bool FlipV)
        {
            map_Base?.ChangeFlipV(sender, FlipV);
        }
        #endregion
    }
}
