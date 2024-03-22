using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChartConverter.ChartDataClass
{
    public class PieChart
    {
        public Dictionary<string,double> Values
        {
            get { return values; }
        }
         Dictionary<string, double> values = new Dictionary<string, double>();

        public Dictionary<string, string> Labels
        {
            get { return labels; }
        }
         Dictionary<string,string> labels = new Dictionary<string,string>();

        public string Unit { get; set; } = "count";
        public string YAxisTitle { get; set; } = "";

        public PieChart(Dictionary<string, double> _Values, Dictionary<string, string>  _Labels= null)
        {
            if (_Labels != null) labels = _Labels;
            values = _Values;
        }

        public bool ShowLabelInChart = true;
    }
}
