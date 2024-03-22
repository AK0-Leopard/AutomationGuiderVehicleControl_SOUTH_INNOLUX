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
    /// ToggleButton.xaml 的互動邏輯
    /// </summary>
    public partial class ToggleButton : UserControl
    {
        Thickness LeftSide = new Thickness(-25, 0, 0, 0);
        Thickness RightSide = new Thickness(0, 0, -25, 0);
        SolidColorBrush Off = new SolidColorBrush(Color.FromRgb(255, 0, 0)); //紅色
        SolidColorBrush On = new SolidColorBrush(Color.FromRgb(0, 204, 0));  //綠色
        private bool Toggled = false;

        public ToggleButton()
        {
            InitializeComponent();
            Back.Fill = Off;
            Toggled = false;
            Dot.Margin = LeftSide;
        }

        public bool Toggled1 { get => Toggled; set => Toggled = value; }

        private void Dot_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (!Toggled)
            {
                Back.Fill = On;
                Toggled = true;
                Dot.Margin = RightSide;
            }
            else
            {
                Back.Fill = Off;
                Toggled = false;
                Dot.Margin = LeftSide;
            }
        }

        private void Back_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (!Toggled)
            {
                Back.Fill = On;
                Toggled = true;
                Dot.Margin = RightSide;
            }
            else
            {
                Back.Fill = Off;
                Toggled = false;
                Dot.Margin = LeftSide;
            }
        }

        public void set_ToggleButton(bool toggled)
        {
            Toggled = toggled;
            if (Toggled)
            {
                Back.Fill = On;
                Dot.Margin = RightSide;
            }
            else
            {
                Back.Fill = Off;
                Dot.Margin = LeftSide;
            }
        }
    }
}
