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
    /// StatusMCSQueue.xaml 的互動邏輯
    /// </summary>
    public partial class StatusMCSQueue : UserControl
    {
        public StatusMCSQueue()
        {
            InitializeComponent();
        }

        public void SetNumOfAssigned(int num)
        {
            Num_Assigned.Content = Convert.ToString(num);
        }

        public void SetNumOfWaitingAssigned(int num)
        {
            Num_WaitingAssigned.Content = Convert.ToString(num);
        }
    }
}
