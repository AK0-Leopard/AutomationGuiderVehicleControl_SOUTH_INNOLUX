using NLog;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection;
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
using ViewerObject;

namespace ControlSystemViewer.Views.Components
{
    /// <summary>
    /// AngleOfView.xaml 的互動邏輯
    /// </summary>
    public partial class AngleOfView : UserControl
    {
        #region "Internal Variable"
        public Definition.AngleOfViewType selectedAOV = Definition.AngleOfViewType.degree_0;
        public EventHandler<Definition.AngleOfViewType> AngleOfViewChanged;
        private static Logger logger = LogManager.GetCurrentClassLogger();
        #endregion	/* Internal Variable */

        public AngleOfView()
        {
            InitializeComponent();
        }

        private async void Button_Top_Click(object sender, RoutedEventArgs e)
        {
            setBtnEnable(false);
            await SelectAOV_Async(Definition.AngleOfViewType.degree_180);
            setBtnEnable(true);
        }

        private async void Button_Right_Click(object sender, RoutedEventArgs e)
        {
            setBtnEnable(false);
            await SelectAOV_Async(Definition.AngleOfViewType.degree_90);
            setBtnEnable(true);
        }

        private async void Button_Left_Click(object sender, RoutedEventArgs e)
        {
            setBtnEnable(false);
            await SelectAOV_Async(Definition.AngleOfViewType.degree_270);
            setBtnEnable(true);
        }

        private async void Button_Bottom_Click(object sender, RoutedEventArgs e)
        {
            setBtnEnable(false);
            await SelectAOV_Async(Definition.AngleOfViewType.degree_0);
            setBtnEnable(true);
        }

        public void SelectAOV(Definition.AngleOfViewType selection)
        {
            try
            {
                setImg(selection);
                if (selection != selectedAOV)
                {
                    selectedAOV = selection;
                    AngleOfViewChanged?.Invoke(null, selection);
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
            }
        }

        public async Task SelectAOV_Async(Definition.AngleOfViewType selection)
        {
            try
            {
                SelectAOV(selection);
                await Task.Delay(2000); // 快速切換會在地圖重繪時發生問題，加延遲
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
            }
        }

        private void setImg(Definition.AngleOfViewType selection)
        {
            try
            {
                string sTop = "un";
                string sRight = "un";
                string sLeft = "un";
                string sBottom = "un";
                switch (selection)
                {
                    case Definition.AngleOfViewType.degree_180:
                        sTop = "";
                        break;
                    case Definition.AngleOfViewType.degree_90:
                        sRight = "";
                        break;
                    case Definition.AngleOfViewType.degree_270:
                        sLeft = "";
                        break;
                    case Definition.AngleOfViewType.degree_0:
                        sBottom = "";
                        break;
                }
                string sPath = Convert.ToString(Directory.GetParent(Assembly.GetExecutingAssembly().Location));
                img_Top.Source = UtilsAPI.Tool.ImageSetting.ToBitmapImage(new Bitmap(sPath + $"/Resources/AOV/{sTop}selected_1.png"));
                img_Right.Source = UtilsAPI.Tool.ImageSetting.ToBitmapImage(new Bitmap(sPath + $"/Resources/AOV/{sRight}selected_2.png"));
                img_Left.Source = UtilsAPI.Tool.ImageSetting.ToBitmapImage(new Bitmap(sPath + $"/Resources/AOV/{sLeft}selected_3.png"));
                img_Bottom.Source = UtilsAPI.Tool.ImageSetting.ToBitmapImage(new Bitmap(sPath + $"/Resources/AOV/{sBottom}selected_4.png"));
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
            }
        }

        private void setBtnEnable(bool isEnable)
        {
            try
            {
                lbl_Cover.Visibility = isEnable ? Visibility.Collapsed : Visibility.Visible;
                //btn_Top.IsEnabled = isEnable;
                //btn_Right.IsEnabled = isEnable;
                //btn_Left.IsEnabled = isEnable;
                //btn_Bottom.IsEnabled = isEnable;
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
            }
        }
    }
}
