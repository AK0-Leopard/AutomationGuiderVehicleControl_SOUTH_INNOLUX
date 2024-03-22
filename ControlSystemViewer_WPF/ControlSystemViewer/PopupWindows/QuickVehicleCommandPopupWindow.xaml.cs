using ControlSystemViewer.Views;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ControlSystemViewer.PopupWindows
{
    /// <summary>
    /// QuickVehicleCommandPopupWindow.xaml 的互動邏輯
    /// </summary>
    public partial class QuickVehicleCommandPopupWindow : Window
    {
        #region 公用參數設定
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private Map_Base map_Base = null;
        private Views.Map_Parts.Vehicle selectedVh = null;
        #endregion 公用參數設定

        public QuickVehicleCommandPopupWindow(Map_Base _map_Base = null, Views.Map_Parts.Vehicle _selectedVh = null)
        {
            InitializeComponent();

            map_Base = _map_Base;
            selectedVh = _selectedVh;

            this.Loaded += _Load;
            this.Closed += _Closed;
        }

        private void _Load(object sender, EventArgs e)
        {
            QuickVehicleCommand.InitUI(selectedVh);
        }

        private void _Closed(object sender, EventArgs e)
        {
            QuickVehicleCommand.Close();
            map_Base?.Closed_QuickVehicleCommandForm();
        }

        public void BringToFront()
        {
            if (!this.IsVisible)
            {
                this.Show();
            }

            if (this.WindowState == WindowState.Minimized)
            {
                this.WindowState = WindowState.Normal;
            }

            this.Activate();
            this.Focus();
        }

        public void SelectVehicle(string VhID)
        {
            QuickVehicleCommand.SelectItem(VhID: VhID);
        }

        public void SelectFromAdr(string AdrID)
        {
            QuickVehicleCommand.SelectItem(FromAdrID: AdrID);
        }

        public void SelectToAdr(string AdrID)
        {
            QuickVehicleCommand.SelectItem(ToAdrID: AdrID);
        }

        public void SelectFromAdr(int AdrNo)
        {
            QuickVehicleCommand.SelectItem(FromAdrNo: AdrNo);
        }

        public void SelectToAdr(int AdrNo)
        {
            QuickVehicleCommand.SelectItem(ToAdrNo: AdrNo);
        }

        #region RemoveIcon
        protected override void OnSourceInitialized(EventArgs e)
        {
            UtilsAPI.Tool.IconHelper.RemoveIcon(this);
        }
        #endregion RemoveIcon
    }
}
