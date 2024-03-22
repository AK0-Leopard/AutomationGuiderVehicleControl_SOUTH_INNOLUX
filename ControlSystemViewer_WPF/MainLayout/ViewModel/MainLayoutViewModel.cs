using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Mirle.UtilsAPI.Commands;
using MainLayout.Model;
using Mirle.UtilsAPI;
using com.mirle.ibg3k0.bc.wpf.App;
using System.Windows.Threading;
using System.Windows;
using System.Reflection;
using System.IO;
using com.mirle.ibg3k0.ohxc.wpf.App;

namespace MainLayout.ViewModel
{
    public class MainLayoutViewModel : ViewModelBase
    {
        #region Properties : 用到哪一些類別
        public WindownApplication app = null;
        private MainLayoutModel model = new MainLayoutModel();
        private DispatcherTimer dispatcherTimer = new DispatcherTimer();
        private int iWidth_Side_Rec = 0;
        private int iWidth_Side = 300;
        private int iHeight_Data = 216;
        private bool isConnectedControlSystem = false;
        public EventHandler<bool> ControlSystemConnectionChanged;
        #endregion

        #region Command : Button 要 Ref 的命令 (ICommand : 適用Windows 的 WPF 組件)
        public ICommand SideCommand { get; }
        public ICommand HomeCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand SaveCommand { get; }
        public ICommand SetLanguageCommand { get; }
        #endregion

        #region  Constructors 
        public MainLayoutViewModel()
        {
            ////指定功能function
            SideCommand = new AnotherCommandImplementation(_ => Side());    //AnotherCommandImplementation :  UtilsAPI 的類別
            HomeCommand = new AnotherCommandImplementation(_ => Home());
            ////DeleteCommand = new AnotherCommandImplementation(_ => Delete());
            ////SaveCommand = new AnotherCommandImplementation(_ => Save());
            //SetLanguageCommand = new AnotherCommandImplementation(o => SetLanguage(o?.ToString()));

            dispatcherTimer.Interval = TimeSpan.FromSeconds(6);
            dispatcherTimer.Tick += new EventHandler(AskControlExist);
        }
        #endregion Constructors

        #region Display
        public string Str_ProductLine
        {
            get { return model.str_ProductLine; }
            set
            {
                if (model.str_ProductLine != value)
                {
                    model.str_ProductLine = value;
                    OnPropertyChanged();
                }
            }
        }
        public string Img_Side => model.img_Side;
        public string Img_Home => model.img_Home;
        public string Img_Signal_Control
        {
            get { return model.img_Signal_Control; }
            set
            {
                if (model.img_Signal_Control != value)
                {
                    model.img_Signal_Control = value;
                    OnPropertyChanged();
                }
            }
        }
        public string Img_Signal_Host
        {
            get { return model.img_Signal_Host; }
            set
            {
                if (model.img_Signal_Host != value)
                {
                    model.img_Signal_Host = value;
                    OnPropertyChanged();
                }
            }
        }
        public string Img_Signal_Alarm
        {
            get { return model.img_Signal_Alarm; }
            set
            {
                if (model.img_Signal_Alarm != value)
                {
                    model.img_Signal_Alarm = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Img_LogInOut => model.img_LogInOut;

        public int Width_Side
        {
            get { return iWidth_Side; }
            set
            {
                if (iWidth_Side != value)
                {
                    iWidth_Side = value;
                    OnPropertyChanged();
                }
            }
        }
        public int Height_Data
        {
            get { return iHeight_Data; }
            set
            {
                if (iHeight_Data != value)
                {
                    iHeight_Data = value;
                    OnPropertyChanged();
                }
            }
        }
        public string Img_Data_Size_Up => model.img_Data_Size_Up;
        public string Img_Data_Size_Default => model.img_Data_Size_Default;
        public string Img_Data_Size_Dowm => model.img_Data_Size_Dowm;
        #endregion Display

        #region Funtion
        public void Start()
        {
            dispatcherTimer.Start();
        }

        public void Side()
        {
            if (Width_Side > 0)
            {
                iWidth_Side_Rec = Width_Side;
                Width_Side = 0;
            }
            else
            {
                Width_Side = iWidth_Side_Rec;
            }
        }

        public void Home()
        {
        }

        public void SetSignal(string target, bool isOn)
        {
            switch (target?.Trim()?.ToUpper())
            {
                case "CONTROL":
                    Img_Signal_Control = isOn ? model.img_SignalOn : model.img_SignalOff;
                    break;
                case "HOST":
                    Img_Signal_Host = isOn ? model.img_SignalOn : model.img_SignalOff;
                    break;
                case "ALARM":
                    Img_Signal_Alarm = isOn ? model.img_SignalOn : model.img_SignalOff;
                    break;
                default:
                    break;
            }
        }

        public async void AskControlExist(Object myObject, EventArgs e)
        {
            app = app ?? WindownApplication.getInstance();
            if (!app.IsControlSystemConnentable) return;
            string result = string.Empty;
            bool isConnected_Rec = isConnectedControlSystem;
            await Task.Run(() => isConnectedControlSystem = app.LineBLL.SendControlIsExist("", out result));
            if (isConnectedControlSystem != isConnected_Rec)
            {
                SetSignal("Control", isConnectedControlSystem);
                ControlSystemConnectionChanged?.Invoke(this, isConnectedControlSystem);
            }
        }
        #endregion Funtion
    }
}
