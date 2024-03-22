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
    /// StatusCST.xaml 的互動邏輯
    /// </summary>
    public partial class StatusCST : UserControl
    {
        public StatusCST()
        {
            InitializeComponent();
        }

        public void SetNumOfTransfer(int num)
        {
            Num_Transfer.Content = Convert.ToString(num);
        }

        public void SetNumOfWaiting(int num)
        {
            Num_Waiting.Content = Convert.ToString(num);
        }
    }
}
