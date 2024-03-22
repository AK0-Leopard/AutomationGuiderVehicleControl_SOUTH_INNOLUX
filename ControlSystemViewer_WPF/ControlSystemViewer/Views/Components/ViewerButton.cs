using com.mirle.ibg3k0.ohxc.wpf.App;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ControlSystemViewer.Views.Components.ViewerButton
{
    public class ViewerButton : Button 
    {
        private UserControl Owner_UserControl = null;
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private WindownApplication app = null;
        private Dictionary<string, string> DictionaryItem = new Dictionary<string, string>();

        public ViewerButton()
        {
            this.Click += Button_Click;
            //this.Owner_UserControl = ;
            System.Windows.DependencyObject Parent = this.Parent;
        }


        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //Window window = Window.GetWindow(this);
                UserControl userControl_Parent = null;
                var ParentButton = ((Button)sender).Parent;
                while (true)
                {
                    userControl_Parent = ParentButton as UserControl;
                    if (userControl_Parent != null) break;
                    ParentButton = ((FrameworkElement)ParentButton).Parent;
                }
               

                DictionaryItem.Clear();
                foreach (TextBox tb in FindVisualChildren<TextBox>(userControl_Parent))
                {
                    if(! DictionaryItem.ContainsKey(tb.Name))
                    {
                        DictionaryItem.Add(tb.Name, tb.Text);
                    }
                    
                }
                foreach (ComboBox cb in FindVisualChildren<ComboBox>(userControl_Parent))
                {
                    if (!DictionaryItem.ContainsKey(cb.Name))
                    {
                        DictionaryItem.Add(cb.Name, cb.Text);
                    }
                }

                app = WindownApplication.getInstance();
                app.OperationHistoryBLL.addOperationHis(app.LoginUserID, userControl_Parent.Name,"", this.Name, ((Button)sender).Content.ToString(), DictionaryItem);
            }
            catch(Exception ex)
            {
                logger.Error(ex, "Exception");
            }
        }

        public static IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
                    if (child != null && child is T)
                    {
                        yield return (T)child;
                    }

                    foreach (T childOfChild in FindVisualChildren<T>(child))
                    {
                        yield return childOfChild;
                    }
                }
            }
        }
    }
}
