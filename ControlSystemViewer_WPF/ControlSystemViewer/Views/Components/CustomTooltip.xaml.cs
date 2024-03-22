using System.ComponentModel;
using LiveCharts;
using LiveCharts.Wpf;

namespace ControlSystemViewer.Views.Components
{
    /// <summary>
    /// CustomTooltip.xaml 的互動邏輯
    /// </summary>
    public partial class CustomTooltip : IChartTooltip
    {
        private TooltipData _data;

        public CustomTooltip()
        {
            InitializeComponent();

            //LiveCharts will inject the tooltip data in the Data property
            //your job is only to display this data as required
            SelectionMode = TooltipSelectionMode.OnlySender;
            DataContext = this;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public TooltipData Data
        {
            get { return _data; }
            set
            {
                _data = value;
                OnPropertyChanged("Data");
            }
        }

        public TooltipSelectionMode? SelectionMode { get; set; }

        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            if (PropertyChanged != null)
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
