using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;

namespace com.mirle.ibg3k0.ohxc.wpf.ObjectRelay
{
    public class ViewModel : INotifyPropertyChanged
    {
        public CollectionViewSource collectionView = new CollectionViewSource();
        public event EventHandler propChanged;
        //private ObservableCollection<PortDefViewObj> _portDefViewObjs;
        //public ObservableCollection<PortDefViewObj> portDefViewObjs
        //{
        //    get
        //    {
        //        return _portDefViewObjs;
        //    }
        //    set
        //    {
        //        _portDefViewObjs = value;
        //        collectionView.Source = portDefViewObjs;
        //        NotifyPropertyChanged("portDefViewObjs");
        //    }
        //}
        //private ObservableCollection<CarrierViewObj> _carrierViewObjs;
        //public ObservableCollection<CarrierViewObj> carrierViewObjs
        //{
        //    get
        //    {
        //        return _carrierViewObjs;
        //    }
        //    set
        //    {
        //        _carrierViewObjs = value;
        //        NotifyPropertyChanged("carrierViewObjs");
        //    }
        //}
        //private ObservableCollection<ZoneShelfViewObj> _zoneShelfViewObjs;
        //public ObservableCollection<ZoneShelfViewObj> zoneShelfViewObjs
        //{
        //    get
        //    {
        //        return _zoneShelfViewObjs;
        //    }
        //    set
        //    {
        //        _zoneShelfViewObjs = value;
        //        NotifyPropertyChanged("zoneShelfViewObjs");
        //    }
        //}

        private ObservableCollection<AlarmMapViewObj>  _alarmMapViewObjs;
        public ObservableCollection<AlarmMapViewObj>  AlarmMapViewObjs
        {
            get
            {
                return _alarmMapViewObjs;
            }
            set
            {
                _alarmMapViewObjs = value;
                NotifyPropertyChanged("AlarmMapViewObjs");
            }
        }

        public ViewModel()
        {
            //portDefViewObjs.CollectionChanged += _portDefViewObjs_CollectionChanged;
        }

        private void _portDefViewObjs_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            var aa = e.Action;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged(String propertyName)
        {
            try
            {
                if (PropertyChanged != null)
                {
                    PropertyChanged?.Invoke(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
                }
            }
            catch
            {

            }
        }
    }
}
