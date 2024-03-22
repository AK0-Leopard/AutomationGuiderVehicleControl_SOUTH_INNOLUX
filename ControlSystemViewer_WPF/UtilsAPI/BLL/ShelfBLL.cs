using com.mirle.ibg3k0.ohxc.wpf.App;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ViewerObject;

namespace UtilsAPI.BLL
{
    public class ShelfBLL
    {
        private Logger logger = LogManager.GetCurrentClassLogger();
        private WindownApplication app = null;

        public ShelfBLL(WindownApplication _app)
        {
            app = _app;
        }
    }
}
