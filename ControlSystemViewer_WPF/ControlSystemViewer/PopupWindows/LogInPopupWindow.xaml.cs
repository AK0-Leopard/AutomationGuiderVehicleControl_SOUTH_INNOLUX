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
    /// LogInPopupWindow.xaml 的互動邏輯
    /// </summary>
    public partial class LogInPopupWindow : Window
    {
        #region 公用參數設定
        private static Logger logger = LogManager.GetCurrentClassLogger();
        #endregion 公用參數設定

        public LogInPopupWindow()
        {
            InitializeComponent();

            LogIn.CloseEvent += _CloseEvent;
        }

        #region RemoveIcon
        protected override void OnSourceInitialized(EventArgs e)
        {
            UtilsAPI.Tool.IconHelper.RemoveIcon(this);
        }
        #endregion RemoveIcon

        private void _CloseEvent(object sender, EventArgs e)
        {
            try
            {
                LogIn.CloseEvent -= _CloseEvent;
                this.Close();
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
            }
        }
    }
}
