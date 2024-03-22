using System.Windows.Media;

namespace ControlSystemViewer.Views.Components
{
    public class CustomVm
    {
        public int index { get; set; }
        public string VehicleName { get; set; }
        public string Status { get; set; }
        public int Duration { get; set; }
        public string Description { get; set; }
        public Brush Color { get; set; }
    }
}
