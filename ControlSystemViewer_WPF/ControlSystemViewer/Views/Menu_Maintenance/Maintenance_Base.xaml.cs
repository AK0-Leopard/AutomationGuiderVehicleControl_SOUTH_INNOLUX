﻿using com.mirle.ibg3k0.ohxc.wpf.App;
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

namespace ControlSystemViewer.Views.Menu_Maintenance
{
    /// <summary>
    /// Maintenance_Base.xaml 的互動邏輯
    /// </summary>
    public partial class Maintenance_Base : UserControl
    {
        #region 公用參數設定
        public WindownApplication app = null;
        private static Logger logger = LogManager.GetCurrentClassLogger();

        #endregion 公用參數設定

        public Maintenance_Base()
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

            VehicleManagement.StartupUI();
            Equipment_Constant.startUI();
            ParkZoneManagement.startUI();
            //CarrierMaintenance.StartupUI();
            if (!app?.ObjCacheManager.ViewerSettings?.menuItem_Maintenance.Visible_EquipmentConstant ?? false)
            {
                TabEquipmentConstant.Visibility = Visibility.Collapsed;
            }
            if (!app?.ObjCacheManager.ViewerSettings?.menuItem_Maintenance.Visible_ParkZoneManagement ?? false)
            {
                TabParkZoneManagement.Visibility = Visibility.Collapsed;
            }

        }

        private void registerEvents()
        {
            if (app != null)
                app.LanguageChanged += _LanguageChanged;

            TabControl.SelectionChanged += _SelectionChanged;

            VehicleManagement.CloseEvent += _CloseEvent;
            Equipment_Constant.CloseEvent += _CloseEvent;
            ParkZoneManagement.CloseEvent += _CloseEvent;
            //CarrierMaintenance.CloseEvent += _CloseEvent;
        }

        private void unregisterEvents()
        {
            if (app != null)
                app.LanguageChanged -= _LanguageChanged;

            TabControl.SelectionChanged -= _SelectionChanged;

            VehicleManagement.CloseEvent -= _CloseEvent;
            Equipment_Constant.CloseEvent -= _CloseEvent;
            ParkZoneManagement.CloseEvent -= _CloseEvent;
            //CarrierMaintenance.CloseEvent -= _CloseEvent;
        }

        private void _CloseEvent(object sender, EventArgs e)
        {
            try
            {
                unregisterEvents();
                VehicleManagement.Close();
                Equipment_Constant.Close();
                ParkZoneManagement.Close();
                //CarrierMaintenance.Close();
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
