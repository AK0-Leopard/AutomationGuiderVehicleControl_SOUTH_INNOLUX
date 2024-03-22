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
using ViewerObject;
using static ViewerObject.VTIPMESSAGE_Def;

namespace ControlSystemViewer.PopupWindows
{
    /// <summary>
    /// TipMessagePopupWindow.xaml 的互動邏輯
    /// </summary>
    public partial class TipMessagePopupWindow : Window
    {
        #region 公用參數設定
        private static Logger logger = LogManager.GetCurrentClassLogger();
        public EventHandler CloseEvent;
        #endregion 公用參數設定

        public TipMessagePopupWindow()
        {
            InitializeComponent();

            this.Closed += _Closed;
            TipMessage.CloseEvent += _CloseEvent;
        }

        public void SetTipMessage(List<VTIPMESSAGE> tip_message)
        {
            try
            {
                TipMessage.SetTipMessage(tip_message);

                if (tip_message != null && tip_message.Count > 0)
                {
                    if (tip_message[0].MsgLevel == MsgLevel.Error)
                    {
                        BringToFront();
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
            }
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
            this.Topmost = true;
            this.Topmost = false;
            this.Focus();
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
                TipMessage.CloseEvent -= _CloseEvent;
                this.Close();
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
            }
        }

        private void _Closed(object sender, EventArgs e)
        {
            try
            {
                this.CloseEvent?.Invoke(this, null);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
            }
        }
    }
}
