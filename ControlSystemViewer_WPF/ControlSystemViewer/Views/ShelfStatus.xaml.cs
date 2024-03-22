using com.mirle.ibg3k0.ohxc.wpf.App;
using com.mirle.ibg3k0.Utility.uc;
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
    /// ShelfStatus.xaml 的互動邏輯
    /// </summary>
    public partial class ShelfStatus : UserControl
    {
        WindownApplication app = null;
        DispatcherTimer timer = new DispatcherTimer();
        int timerInerval = 3;

        private class zoneData
        {
            public string zoneName { get; set; }
            public string zoneValue 
            {
                get
                {
                    return ( (storeCount + disableCount) / (emptyCount + storeCount + disableCount) * 100).ToString("00.00") + "%" ;
                }
            }

            private double emptyCount;

            private double storeCount;

            private double disableCount;

            public zoneData(string s)
            {
                zoneName = s;
            }
            public void addDisalbeShelf()
            {
                disableCount = disableCount + 1.0;
            }
            public void addStoreShelf()
            {
                storeCount = storeCount + 1.0;
            }
            public void addEmptyShelf()
            {
                emptyCount = emptyCount + 1.0;
            }
        }

        public ShelfStatus()
        {
            InitializeComponent();
            init();

            app = WindownApplication.getInstance();
        }
        public void init()
        {
            initTimer();
        }

        private void initTimer()
        {
            timer.Interval = TimeSpan.FromSeconds(timerInerval);
            timer.Tick += timer_tick;
            timer.Start();
        }


        private void timer_tick(object sender, EventArgs e)
        {
            if (app?.ObjCacheManager?.Shelves == null || app?.ObjCacheManager?.Shelves.Count==0) return;
            var shelfDataList = app.ObjCacheManager.Shelves;
            zoneData zonedata;
            List<zoneData> zoneDataList = new List<zoneData>();
            foreach(var shelf in shelfDataList)
            {
                if (zoneDataList == null || zoneDataList.FirstOrDefault(x => x.zoneName == shelf.ZONE_ID) == null)
                    zoneDataList.Add(new zoneData(shelf.ZONE_ID));
                zonedata = zoneDataList.FirstOrDefault(x => x.zoneName == shelf.ZONE_ID);
                if (shelf.ENABLE == false)
                    zonedata.addDisalbeShelf();
                else if (shelf.SHELF_STATUS == ViewerObject.Definition.ShelfStatus.Empty)
                    zonedata.addEmptyShelf();
                else
                    zonedata.addStoreShelf();
            }
            Adapter.Invoke((obj) =>
            {
                DataGridView.ItemsSource = zoneDataList;
            }, null);
        }
    }
}
