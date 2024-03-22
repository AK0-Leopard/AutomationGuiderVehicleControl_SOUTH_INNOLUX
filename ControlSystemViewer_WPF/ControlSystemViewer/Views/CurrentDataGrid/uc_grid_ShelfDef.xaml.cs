using com.mirle.ibg3k0.bc.winform.App;
using com.mirle.ibg3k0.sc;
using MirleGO_UIFrameWork.UI.uc_Button;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace com.mirle.ibg3k0.ohxc.winform.UI.Components.WPF_UserControl
{
    /// <summary>
    /// uc_grid_ShelfDef.xaml 的互動邏輯
    /// </summary>
    public partial class uc_grid_ShelfDef : System.Windows.Controls.UserControl
    {
        public App.WindownApplication Application => App.WindownApplication.getInstance();
        //App.WindownApplication app = null;
        public uc_grid_ShelfDef()
        {
            InitializeComponent();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
           
        }

        private async void Enable_Click(object sender, RoutedEventArgs e)
        {
            var shelfs = grid_ShelfDef.SelectedItems.Cast<ShelfDef>();
            string selectShelfs = string.Empty;
            if (TipMessage_Request_Light.Show($"Update these Shelf\n" +
                $"{shelfs.Aggregate(selectShelfs, (total, next) => total + next.ShelfID + "  ")} \nto Enable ? \n") == DialogResult.Yes)
            {
                string result = string.Empty;
                await Task.Run(() => result = Application.ShelfDefBLL.SendMultiShelfEnableUpdate(shelfs));
                if (result == "OK")
                {
                    TipMessage_Type_Light.Show("Succeed", $"{result}", BCAppConstants.INFO_MSG);
                }
                else
                {
                    TipMessage_Type_Light.Show("Failure", $"{result}", BCAppConstants.INFO_MSG);
                }
            }        
        }

        private async void Disable_Click(object sender, RoutedEventArgs e)
        {
            var shelfs = grid_ShelfDef.SelectedItems.Cast<ShelfDef>();
            string selectShelfs = string.Empty;
            if (TipMessage_Request_Light.Show($"Update these Shelf\n" +
                $"{shelfs.Aggregate(selectShelfs, (total, next) => total + next.ShelfID + "  ")} \nto Enable ? \n") == DialogResult.Yes)
            {
                string result = string.Empty;
                await Task.Run(() => result = Application.ShelfDefBLL.SendMultiShelfDisableUpdate(shelfs));
                if (result == "OK")
                {
                    TipMessage_Type_Light.Show("Succeed", $"{result}", BCAppConstants.INFO_MSG);
                }
                else
                {
                    TipMessage_Type_Light.Show("Failure", $"{result}", BCAppConstants.INFO_MSG);
                }

            }
        }
    }
}
