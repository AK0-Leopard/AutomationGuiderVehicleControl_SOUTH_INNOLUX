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
    /// StatusVehicle.xaml 的互動邏輯
    /// </summary>
    public partial class StatusVehicle : UserControl
    {
        public StatusVehicle()
        {
            InitializeComponent();
        }

        public void SetNumOfAutoRemote(int num)
        {
            Num_AutoRemote.Content = Convert.ToString(num);
        }

        public void SetNumOfAutoLocal(int num)
        {
            Num_AutoLocal.Content = Convert.ToString(num);
        }

        public void SetNumOfIdle(int num)
        {
            Num_Idle.Content = Convert.ToString(num);
        }

        public void SetNumOfError(int num)
        {
            Num_Error.Content = Convert.ToString(num);
        }
    }
}
