using com.mirle.ibg3k0.bc.wpf.App;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Reflection;

namespace Loading.Model
{
    public class LoadingModel
    {
        public string img_Loading { get; private set; }


        public LoadingModel()
        {
            string sPath = Convert.ToString(Directory.GetParent(Assembly.GetExecutingAssembly().Location));
            img_Loading = sPath + "\\Resources\\Mirle_Loading_9.gif";
        }
    }
}
