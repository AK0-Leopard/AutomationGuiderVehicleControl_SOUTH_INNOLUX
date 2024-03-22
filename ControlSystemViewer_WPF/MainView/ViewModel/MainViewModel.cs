using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Mirle.UtilsAPI.Commands;
using MainView.Model;
using Mirle.UtilsAPI;
using com.mirle.ibg3k0.bc.wpf.App;
using System.Windows.Threading;
using System.Windows;
using System.Reflection;
using System.IO;
using com.mirle.ibg3k0.ohxc.wpf.App;

namespace MainView.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        #region Properties : 用到哪一些類別
        public WindownApplication app = null;
        private MainModel model = new MainModel();
        private DispatcherTimer dispatcherTimer_1min = new DispatcherTimer();
        #endregion

        #region Command : Button 要 Ref 的命令 (ICommand : 適用Windows 的 WPF 組件)
        public ICommand LogInCommand { get; }
        public ICommand AddModCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand SaveCommand { get; }
        public ICommand SetLanguageCommand { get; }
        #endregion

        #region  Constructors 
        public MainViewModel()
        {
            //指定功能function
            //LogInCommand = new AnotherCommandImplementation(_ => LogIn());    //AnotherCommandImplementation :  UtilsAPI 的類別
            //AddModCommand = new AnotherCommandImplementation(_ => AddMod());
            //DeleteCommand = new AnotherCommandImplementation(_ => Delete());
            //SaveCommand = new AnotherCommandImplementation(_ => Save());
            //SetLanguageCommand = new AnotherCommandImplementation(o => SetLanguage(o?.ToString()));

            refresh_CMDCount(null, null);
            dispatcherTimer_1min.Interval = TimeSpan.FromMinutes(1);
            dispatcherTimer_1min.Tick += new EventHandler(refresh_CMDCount);
            dispatcherTimer_1min.Tick += new EventHandler(changeRunningTime);
            //dispatcherTimer_1min.Start();
        }
        #endregion Constructors

        public void Start()
        {
            dispatcherTimer_1min.Start();
        }

        public void SetLogoCustomer(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
            {
                Logo_Customer = null;
                return;
            }

            string sPath = Convert.ToString(Directory.GetParent(Assembly.GetExecutingAssembly().Location));
            Logo_Customer = $"{sPath}\\Resources\\{fileName}.png";
        }

        #region System Info Bar (Buttom)
        public string System_HourlyProcess
        {
            get { return model.hourlyProcess; }
            set
            {
                if (model.hourlyProcess != value)
                {
                    model.hourlyProcess = value;
                    OnPropertyChanged();
                }
            }
        }
        public string System_TodayProcess
        {
            get { return model.todayProcess; }
            set
            {
                if (model.todayProcess != value)
                {
                    model.todayProcess = value;
                    OnPropertyChanged();
                }
            }
        }
        public string System_RunTime
        {
            get { return model.runTime; }
            set
            {
                if (model.runTime != value)
                {
                    model.runTime = value;
                    OnPropertyChanged();
                }
            }
        }
        public string System_BuildDate
        {
            get { return model.buildDate; }
            set
            {
                if (model.buildDate != value)
                {
                    model.buildDate = value;
                    OnPropertyChanged();
                }
            }
        }
        public string System_Version
        {
            get { return model.version; }
        }
        public string Logo_Customer
        {
            get { return model.logo_Customer; }
            private set
            {
                if (model.logo_Customer != value)
                {
                    model.logo_Customer = value;
                    OnPropertyChanged();
                }
            }
        }
        public string Logo_Mirle
        {
            get { return model.logo_Mirle; }
        }
        private void refresh_CMDCount(Object myObject, EventArgs myEventArgs)
        {
            try
            {
                if (app != null)
                {
                    System_HourlyProcess = app.CmdBLL.GetHTransferHourlyCount().ToString();
                    System_TodayProcess = app.CmdBLL.GetHTransferTodayCount().ToString();
                }
            }
            catch //(Exception ex)
            {
                System_HourlyProcess = "0";
                System_TodayProcess = "0";
            }
        }
        private DateTime runTimeStart = DateTime.Now;
        private void changeRunningTime(Object myObject, EventArgs myEventArgs)
        {
            try
            {
                String m_minute;
                String m_hour;
                String m_day;

                DateTime now = DateTime.Now;

                TimeSpan diffSpan = now.Subtract(runTimeStart);
                if (diffSpan.Minutes < 10)
                {
                    m_minute = "0" + diffSpan.Minutes.ToString();
                }
                else
                {
                    m_minute = diffSpan.Minutes.ToString();
                }

                if (diffSpan.Hours < 10)
                {
                    m_hour = "0" + diffSpan.Hours.ToString();
                }
                else
                {
                    m_hour = diffSpan.Hours.ToString();
                }

                if (diffSpan.Days < 10)
                {
                    m_day = "0" + diffSpan.Days.ToString();
                }
                else
                {
                    m_day = diffSpan.Days.ToString();
                }

                System_RunTime = $"{m_day}d {m_hour}h {m_minute}m";
            }
            catch //(Exception ex)
            {
                System_RunTime = "00d 00h 00m";
            }
        }
        #endregion System Info Bar (Buttom)
    }
}
