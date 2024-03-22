using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChartConverter.ChartDataClass
{
    public class Scatter
    {

        public bool ShowValue { get; set; } = false;
        public Dictionary<string, List<double>> dicScatter
        {
            get
            {
                return dicscatter;
            }
        }
        Dictionary<string, List<double>> dicscatter = new Dictionary<string, List<double>>();

        public Dictionary<double,string> XAxisDataString
        {
            get { return xaxisdatastring; }
        }
        public Dictionary<double, string> xaxisdatastring = new Dictionary<double, string>();


        public List<double> XAxisData
        {
            get { return xaxisdata; }
        }
        public List<double> xaxisdata = new List<double>();



        public Scatter(List<double> _XAxisData, Dictionary<string, List<double>> _dicScatter, string _Unit = "count", Dictionary<double, string> _XAxisDataString = null)
        {
           
            xaxisdata = _XAxisData;
            dicscatter = _dicScatter;
            Unit = _Unit;
            if(_XAxisDataString != null)
            {
                xaxisdatastring = _XAxisDataString;
            }        
        }


        public string Unit { get; set; } = "count";
        public string YAxisTitle { get; set; } = "";
    }
}
