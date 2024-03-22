using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Mirle.UtilsAPI.Commands;
using Loading.Model;
using Mirle.UtilsAPI;
using com.mirle.ibg3k0.bc.wpf.App;
using System.Windows;
using System.Reflection;
using System.IO;

namespace Loading.ViewModel
{
    public class LoadingViewModel : ViewModelBase
    {
        #region Properties : 用到哪一些類別
        private LoadingModel model = new LoadingModel();
        #endregion

        #region Command : Button 要 Ref 的命令 (ICommand : 適用Windows 的 WPF 組件)
        public ICommand LogInCommand { get; }
        public ICommand AddModCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand SaveCommand { get; }
        public ICommand SetLanguageCommand { get; }
        #endregion

        #region  Constructors 
        public LoadingViewModel()
        {
            //指定功能function
            //LogInCommand = new AnotherCommandImplementation(_ => LogIn());    //AnotherCommandImplementation :  UtilsAPI 的類別
            //AddModCommand = new AnotherCommandImplementation(_ => AddMod());
            //DeleteCommand = new AnotherCommandImplementation(_ => Delete());
            //SaveCommand = new AnotherCommandImplementation(_ => Save());
            //SetLanguageCommand = new AnotherCommandImplementation(o => SetLanguage(o?.ToString()));
        }
        #endregion Constructors

        public string Img_Loading => model.img_Loading;
    }
}
