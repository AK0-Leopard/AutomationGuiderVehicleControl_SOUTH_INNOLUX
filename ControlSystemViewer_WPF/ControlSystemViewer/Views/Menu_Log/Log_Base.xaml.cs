using com.mirle.ibg3k0.ohxc.wpf.App;
using NLog;
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
using ViewerObject;

namespace ControlSystemViewer.Views.Menu_Log
{
    /// <summary>
    /// Log_Base.xaml 的互動邏輯
    /// </summary>
    public partial class Log_Base : UserControl
    {
        #region 公用參數設定
        public WindownApplication app = null;
        private static Logger logger = LogManager.GetCurrentClassLogger();
        #endregion 公用參數設定

        public Log_Base()
        {
            InitializeComponent();
        }

        public void Show(WindownApplication _app, string sSelectTab = null)
        {
            if (!this.IsVisible)
            {
                this.Visibility = Visibility.Visible;
                startupUI(_app);
            }

            if (TabControl.Items != null)
            {
                if (!string.IsNullOrEmpty(sSelectTab))
                {
                    for (int i = TabControl.Items.Count - 1; i >= 0; i--)
                    {
                        TabItem tab = TabControl.Items[i] as TabItem;
                        if ((i > 0 && sSelectTab == tab.Header?.ToString())
                            || i == 0) // set as default
                        {
                            TabControl.SelectedIndex = i;
                            //lbl_Title.Content = tab.Header?.ToString();
                            break;
                        }
                    }
                }
            }
        }

        public void Close()
        {
            if (this.IsVisible) _CloseEvent(null, null);
        }

        private void startupUI(WindownApplication _app)
        {
            app = _app;

            registerEvents();

            AlarmHistory.StartupUI();
            VehicleCommandHistory.StartupUI();
            TransferCommandHistory.StartupUI();

            if (app?.ObjCacheManager.ViewerSettings?.menuItem_Log.Visible_STATISTICS ?? false)
            {
                TabItem_STATISTICS.Visibility = Visibility.Visible;
                Chart_RunDownIdle.StartupUI();
            }
        }

        private void registerEvents()
        {
            if (app != null)
                app.LanguageChanged += _LanguageChanged;

            TabControl.SelectionChanged += _SelectionChanged;

            AlarmHistory.CloseEvent += _CloseEvent;
            VehicleCommandHistory.CloseEvent += _CloseEvent;
            TransferCommandHistory.CloseEvent += _CloseEvent;
        }

        private void unregisterEvents()
        {
            if (app != null)
                app.LanguageChanged -= _LanguageChanged;

            TabControl.SelectionChanged -= _SelectionChanged;

            AlarmHistory.CloseEvent -= _CloseEvent;
            VehicleCommandHistory.CloseEvent -= _CloseEvent;
            TransferCommandHistory.CloseEvent -= _CloseEvent;
        }

        private void _CloseEvent(object sender, EventArgs e)
        {
            try
            {
                unregisterEvents();
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
            }
            this.Visibility = Visibility.Collapsed;
        }

        private void _SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            resetSubTitle();
        }

        private void _LanguageChanged(object sender, EventArgs e)
        {
            resetSubTitle();
        }

        private void resetSubTitle()
        {
            if (TabControl.SelectedIndex < 0)
            {
                lbl_Title.Content = "";
            }
            else
            {
                lbl_Title.Content = ((TabItem)TabControl.SelectedItem).Header?.ToString() ?? "";
            }
        }
    }
}
