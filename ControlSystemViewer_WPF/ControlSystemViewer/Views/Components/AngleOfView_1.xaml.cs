using NLog;
using System;
using System.Collections.Generic;
using System.Drawing;
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
    /// AngleOfView_1.xaml 的互動邏輯
    /// </summary>
    public partial class AngleOfView_1 : UserControl
    {
        #region "Internal Variable"
        public Definition.AngleOfViewType selectedAOV = Definition.AngleOfViewType.degree_0;
        public bool selectedFlipH = false;
        public bool selectedFlipV = false;
        public EventHandler<Definition.AngleOfViewType> AngleOfViewChanged;
        public EventHandler<bool> HorizontalFlipChanged;
        public EventHandler<bool> VerticalFlipChanged;
        private static Logger logger = LogManager.GetCurrentClassLogger();
        #endregion	/* Internal Variable */

        public AngleOfView_1()
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

        private async void FlipH_Click(object sender, RoutedEventArgs e)
        {
            setBtnEnable(false);
            //await SelectFlip_Async(ckb_FlipH.IsChecked??false,ckb_FlipV.IsChecked ?? false);
            await SelectFlipH_Async(ckb_FlipH.IsChecked ?? false);
            setBtnEnable(true);
        }

        private async void FlipV_Click(object sender, RoutedEventArgs e)
        {
            setBtnEnable(false);
            await SelectFlipV_Async(ckb_FlipV.IsChecked ?? false);
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

        public void SelectFlipH(bool FlipH)
        {
            try
            {
                if (selectedFlipH != FlipH)
                {
                    selectedFlipH = FlipH;
                    HorizontalFlipChanged?.Invoke(null, FlipH);
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
            }
        }

        public void SelectFlipV(bool FlipV)
        {
            try
            {
                if (selectedFlipV != FlipV)
                {
                    selectedFlipV = FlipV;
                    VerticalFlipChanged?.Invoke(null, FlipV);
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

        public async Task SelectFlipH_Async(bool FlipH)
        {
            try
            {
                SelectFlipH(FlipH);
                await Task.Delay(2000); // 快速切換會在地圖重繪時發生問題，加延遲
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
            }
        }

        public async Task SelectFlipV_Async(bool FlipV)
        {
            try
            {
                SelectFlipV(FlipV);
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
                string sTop = "unselected";
                string sRight = "unselected";
                string sLeft = "unselected";
                string sBottom = "unselected";
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
                img_Top.Source = UtilsAPI.Tool.ImageSetting.ToBitmapImage(new Bitmap(sPath + $"/Resources/AOV/{sTop}ArrowUp.png"));
                img_Right.Source = UtilsAPI.Tool.ImageSetting.ToBitmapImage(new Bitmap(sPath + $"/Resources/AOV/{sRight}ArrowRight.png"));
                img_Left.Source = UtilsAPI.Tool.ImageSetting.ToBitmapImage(new Bitmap(sPath + $"/Resources/AOV/{sLeft}ArrowLeft.png"));
                img_Bottom.Source = UtilsAPI.Tool.ImageSetting.ToBitmapImage(new Bitmap(sPath + $"/Resources/AOV/{sBottom}ArrowDown.png"));
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
