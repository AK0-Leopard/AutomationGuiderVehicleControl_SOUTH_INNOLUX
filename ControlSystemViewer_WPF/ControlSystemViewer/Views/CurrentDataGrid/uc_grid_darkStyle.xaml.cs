using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
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

namespace com.mirle.ibg3k0.ohxc.winform.UI.Components.WPF_UserControl
{
    /// <summary>
    /// uc_grid_darkStyle.xaml 的互動邏輯
    /// </summary>
    public partial class uc_grid_darkStyle : UserControl
    {
        string sortStr = string.Empty;
        ListSortDirection sortType = ListSortDirection.Ascending;
        List<DataGridRow> gridRows = new List<DataGridRow>();
        public uc_grid_darkStyle()
        {
            InitializeComponent();
            gridData.ColumnWidth = DataGridLength.SizeToHeader;
            gridData.AutoGenerateColumns = false;
            gridData.SelectionMode = DataGridSelectionMode.Single;
            gridData.Sorting += GridData_Sorting;
            //gridData.LoadingRow += GridData_LoadingRow;
        }


        private void GridData_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            var aa = e.Row.Item;
            var a = gridRows.FirstOrDefault()?.Item;
            var rowNum = gridRows.Where(x => x.Item == e.Row.Item)?.FirstOrDefault();
            //if(rowNum > 0)
            //{
            //    e.Row.DetailsVisibility = Visibility.Visible;
            //}
        }

        public void setDataGridSource<T>(IEnumerable<T> obj)
        {
            //Binding binding = new Binding(bindingStr);
            //gridData.SetBinding(DataGrid.ItemsSourceProperty, binding);

            setDataGridColumn(obj);
            gridData.ItemsSource = obj;
        }

        public void setDataContext(object obj)
        {
            try
            {
                DataContext = obj;
            }
            catch
            {

            }
        }

        public void setDataGridColumn<T>(IEnumerable<T> source)
        {
            var type = typeof(T);
            var pro = type.GetProperties();
            foreach (var info in pro.Where(x => x.Name != "Details"))
            {
                DataGridTextColumn column = new DataGridTextColumn();
                column.Header = info.Name;
                column.Binding = new Binding(info.Name);
                column.Width = 200;
                column.ElementStyle = new Style { TargetType = typeof(TextBlock) };
                column.CanUserSort = true;
                gridData.Columns.Add(column);
            }
        }

        public void whenUpdateData()
        {
            if (!string.IsNullOrEmpty(sortStr))
            {
                ICollectionView view = CollectionViewSource.GetDefaultView(gridData.ItemsSource);
                view.SortDescriptions.Clear();
                view.SortDescriptions.Add(new SortDescription(sortStr, sortType));
                gridData.Columns.Where(x => x.SortMemberPath == sortStr).FirstOrDefault().SortDirection = sortType;
            }
        }

        private void RowDoubleClick(object sender, RoutedEventArgs e)
        {
            var row = (DataGridRow)sender;
            var prop = row.Item.GetType().GetProperty("Details");
            var details = prop?.GetValue(row.Item) as string;
            if (!string.IsNullOrEmpty(details))
            {
                row.DetailsVisibility = row.DetailsVisibility == Visibility.Collapsed ?
                    Visibility.Visible : Visibility.Collapsed;

                if (row.DetailsVisibility == Visibility.Visible)
                {
                    gridRows.Add(row);
                }
                else
                {
                    gridRows.Remove(row);
                }
            }
            else
            {
                row.DetailsVisibility = Visibility.Collapsed;
            }
        }
        private void Expander_Expanded(object sender, RoutedEventArgs e)
        {
            for (var vis = sender as Visual; vis != null; vis = VisualTreeHelper.GetParent(vis) as Visual)
                if (vis is DataGridRow)
                {
                    var row = (DataGridRow)vis;
                    row.DetailsVisibility = row.DetailsVisibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
                    break;
                }
        }
        private void Expander_Collapsed(object sender, RoutedEventArgs e)
        {
            for (var vis = sender as Visual; vis != null; vis = VisualTreeHelper.GetParent(vis) as Visual)
                if (vis is DataGridRow)
                {
                    var row = (DataGridRow)vis;
                    row.DetailsVisibility = row.DetailsVisibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
                    break;
                }
        }

        private void GridData_Sorting(object sender, System.Windows.Controls.DataGridSortingEventArgs e)
        {
            sortStr = e.Column.SortMemberPath;
            if (e.Column.SortDirection == null)
            {
                sortType = ListSortDirection.Ascending;
            }
            else
            {
                sortType = e.Column.SortDirection.GetValueOrDefault()
                    == ListSortDirection.Ascending ? ListSortDirection.Descending : ListSortDirection.Ascending;
            }

        }


    }
}
