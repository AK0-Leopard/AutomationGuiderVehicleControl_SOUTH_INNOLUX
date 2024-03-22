using com.mirle.ibg3k0.ohxc.wpf.App;
using PortStatus;
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

namespace ControlSystemViewer.Views
{
    /// <summary>
    /// PortStatus.xaml 的互動邏輯
    /// </summary>
    public partial class PortStatus : UserControl
    {
        public PortStatus()
        {
            InitializeComponent();
        }

        public void Start(WindownApplication _app)
        {
            var vm = DataContext as PortStatusViewModel;
            if (vm != null)
            {
                vm.Start(_app);
            }
        }
    }
}
