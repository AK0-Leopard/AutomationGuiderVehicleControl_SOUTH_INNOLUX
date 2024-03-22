using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection;
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

namespace ControlSystemViewer.Views.Menu_TipMessage
{
    /// <summary>
    /// TipMessage.xaml 的互動邏輯
    /// </summary>
    public partial class TipMessage : UserControl
    {
        #region 公用參數設定
        public event EventHandler CloseEvent;
        #endregion 公用參數設定

        public TipMessage()
        {
            InitializeComponent();

            string sPath = Convert.ToString(Directory.GetParent(Assembly.GetExecutingAssembly().Location));
            Img_TipMessage.Source = UtilsAPI.Tool.ImageSetting.ToBitmapImage(new Bitmap(sPath + "\\Resources\\SystemIcon\\info1.png"));
        }

        public void SetTipMessage(List<VTIPMESSAGE> tip_message)
        {
            data.grid_TipMessage.ItemsSource = tip_message ?? new List<VTIPMESSAGE>();
        }

        private void Button_Close_Click(object sender, RoutedEventArgs e)
        {
            CloseEvent?.Invoke(this, e);
        }
    }
}
