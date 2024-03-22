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
using System.Windows.Threading;

namespace ControlSystemViewer.Views
{
    /// <summary>
    /// Loading.xaml 的互動邏輯
    /// </summary>
    public partial class Loading : UserControl
    {
        private DispatcherTimer dispatcherTimer = new DispatcherTimer();

        public Loading()
        {
            InitializeComponent();

            dispatcherTimer.Interval = TimeSpan.FromSeconds(1);
            dispatcherTimer.Tick += new EventHandler(changeLabelVisibility);
            Start();
        }

        public void Start(string txt = null)
        {
            if (txt != null)
                lbl_Init.Content = txt;
            this.Visibility = Visibility.Visible;

            dispatcherTimer.Start();
        }

        public void Stop()
        {
            dispatcherTimer.Stop();

            this.Visibility = Visibility.Collapsed;
        }

        private void gif_MediaEnded(object sender, RoutedEventArgs e)
        {
            gif.Position = new TimeSpan(0, 0, 1);
            gif.Play();
        }

        private void changeLabelVisibility(object sender, EventArgs e)
        {
            switch (lbl_Init.Visibility)
            {
                case Visibility.Visible:
                    lbl_Init.Visibility = Visibility.Hidden;
                    break;
                case Visibility.Hidden:
                    lbl_Init.Visibility = Visibility.Visible;
                    break;
                default:
                    break;
            }
        }
    }
}
