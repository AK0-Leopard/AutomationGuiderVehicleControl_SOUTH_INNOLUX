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
    /// SystemModeControlPopupWindow.xaml 的互動邏輯
    /// </summary>
    public partial class Chart_BasePopupWindow : Window
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private ScottPlot.WpfPlot gPlot = null;
        private string DefaultFileName ="";
        public Chart_BasePopupWindow( ScottPlot.WpfPlot inplot,string _filename="")
        {
            InitializeComponent();
            Chart_base.CloseEvent += _CloseEvent;
            gPlot = inplot;
            DefaultFileName = _filename;
            Show();
        }

        private void _CloseEvent(object sender, EventArgs e)
        {
            try
            {
                Chart_base.CloseEvent -= _CloseEvent;
                this.Close();
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
            }
        }
        private void Show()
        {
            Chart_base.Show(ref gPlot,DefaultFileName);
        }

        #region RemoveIcon
        protected override void OnSourceInitialized(EventArgs e)
        {
            IconHelper.RemoveIcon(this);
        }
        public static class IconHelper
        {
            [DllImport("user32.dll")]
            static extern int GetWindowLong(IntPtr hwnd, int index);

            [DllImport("user32.dll")]
            static extern int SetWindowLong(IntPtr hwnd, int index, int newStyle);

            [DllImport("user32.dll")]
            static extern bool SetWindowPos(IntPtr hwnd, IntPtr hwndInsertAfter,
                int x, int y, int width, int height, uint flags);

            [DllImport("user32.dll")]
            static extern IntPtr SendMessage(IntPtr hwnd, uint msg,
                IntPtr wParam, IntPtr lParam);

            const int GWL_EXSTYLE = -20;
            const int WS_EX_DLGMODALFRAME = 0x0001;
            const int SWP_NOSIZE = 0x0001;
            const int SWP_NOMOVE = 0x0002;
            const int SWP_NOZORDER = 0x0004;
            const int SWP_FRAMECHANGED = 0x0020;
            const uint WM_SETICON = 0x0080;

            public static void RemoveIcon(Window window)
            {
                // Get this window's handle
                IntPtr hwnd = new WindowInteropHelper(window).Handle;

                // Change the extended window style to not show a window icon
                int extendedStyle = GetWindowLong(hwnd, GWL_EXSTYLE);
                SetWindowLong(hwnd, GWL_EXSTYLE, extendedStyle | WS_EX_DLGMODALFRAME);

                // Update the window's non-client area to reflect the changes
                SetWindowPos(hwnd, IntPtr.Zero, 0, 0, 0, 0, SWP_NOMOVE |
                      SWP_NOSIZE | SWP_NOZORDER | SWP_FRAMECHANGED);
            }
        }
        #endregion RemoveIcon
    }
}
