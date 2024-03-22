using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Mirle.UtilsAPI.Commands;
using Map.Model;
using Mirle.UtilsAPI;
using com.mirle.ibg3k0.bc.wpf.App;
using System.Windows.Threading;
using System.Windows;
using System.Reflection;
using System.IO;
using com.mirle.ibg3k0.ohxc.wpf.App;

namespace Map.ViewModel
{
    public class MapViewModel : ViewModelBase
    {
        #region Properties : 用到哪一些類別
        public WindownApplication app = null;
        private MapModel model = new MapModel();
        private string iconScaleRuler = "";
        #endregion

        #region Command : Button 要 Ref 的命令 (ICommand : 適用Windows 的 WPF 組件)
        public ICommand SideCommand { get; }
        public ICommand HomeCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand SaveCommand { get; }
        public ICommand SetLanguageCommand { get; }
        #endregion

        #region  Constructors 
        public MapViewModel()
        {
            ////指定功能function
            SideCommand = new AnotherCommandImplementation(_ => side());    //AnotherCommandImplementation :  UtilsAPI 的類別
            HomeCommand = new AnotherCommandImplementation(_ => home());
            ////DeleteCommand = new AnotherCommandImplementation(_ => Delete());
            ////SaveCommand = new AnotherCommandImplementation(_ => Save());
            //SetLanguageCommand = new AnotherCommandImplementation(o => SetLanguage(o?.ToString()));
        }
        #endregion Constructors

        #region Display
        public string IconScaleRuler
        {
            get { return iconScaleRuler; }
            set
            {
                if (iconScaleRuler != value)
                {
                    iconScaleRuler = value;
                    OnPropertyChanged();
                }
            }
        }
        public double MinimumScale
        {
            get { return model.dMinimumScale; }
            set
            {
                if (model.dMinimumScale != value)
                {
                    model.dMinimumScale = value;
                    OnPropertyChanged();
                }
            }
        }
        public double MaximumScale
        {
            get { return model.dMaximumScale; }
            set
            {
                if (model.dMaximumScale != value)
                {
                    model.dMaximumScale = value;
                    OnPropertyChanged();
                }
            }
        }
        public double Scale
        {
            get { return model.dScale; }
            set
            {
                if (model.dScale != value)
                {
                    model.dScale = value;
                    OnPropertyChanged();
                }
            }
        }
        public double ScaleTickFrequency
        {
            get { return model.dScaleTickFrequency; }
            set
            {
                if (model.dScaleTickFrequency != value)
                {
                    model.dScaleTickFrequency = value;
                    OnPropertyChanged();
                }
            }
        }
        private double scrollBarWidth = 20;
        public double ScrollBarWidth
        {
            get { return scrollBarWidth; }
            set
            {
                if (scrollBarWidth != value)
                {
                    scrollBarWidth = value;
                    OnPropertyChanged();
                }
            }
        }
        #endregion Display

        #region Funtion
        private void side()
        {
        }

        private void home()
        {
        }

        public void ShowFullMap(double displayArea_Width, double displayArea_Height, double map_Width, double map_Height)
        {
            double scaleW = displayArea_Width / map_Width;
            double scaleH = displayArea_Height / map_Height;
            Scale = scaleW < scaleH ? scaleW : scaleH;
            MinimumScale = Scale;
            //MaximumScale = 0.1;
            ScaleTickFrequency = (MaximumScale - Scale) / 10;
        }
        #endregion Funtion
    }
}
