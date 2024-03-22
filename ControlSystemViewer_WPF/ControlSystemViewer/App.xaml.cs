using com.mirle.ibg3k0.bc.wpf.App;
using MirleGO_UIFrameWork.UI.uc_Button;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace ControlSystemViewer
{
    /// <summary>
    /// App.xaml 的互動邏輯
    /// </summary>
    public partial class App : Application
    {
        private static Mutex mutex;
        protected override void OnStartup(StartupEventArgs e)
        {
            string control_system_viewer_name = "ControlSystemViewer";
            if (e.Args != null && e.Args.Length > 0)
            {
                control_system_viewer_name = e.Args[0];
            }
            //當有設定參數時，使用設定的參數來帶入，使其可以達到同一台電腦開多個Viewer
            //mutex = new Mutex(true, "ControlSystemViewer");
            mutex = new Mutex(true, control_system_viewer_name);
            if (mutex.WaitOne(0, false))
            {
                base.OnStartup(e);
            }
            else
            {
                //如果是允許多系統模式，一樣打開，但會強制進入OffLineMode，僅能使用少數功能如報表
                if (ConfigurationManager.AppSettings["MulitySystem"].ToString().Contains("Y"))
                {
                    base.OnStartup(e);
                    return;
                }
                //TipMessage_Type_Light_woBtn.Show("Closing", "Can Not Execute Multiple Control System Viewer!!", BCAppConstants.ERROR_MSG);
                MessageBox.Show("Can Not Execute Multiple Control System Viewer!!", "Closing");
                Environment.Exit(Environment.ExitCode);
                //this.Shutdown();
            }
        }
    }
}
