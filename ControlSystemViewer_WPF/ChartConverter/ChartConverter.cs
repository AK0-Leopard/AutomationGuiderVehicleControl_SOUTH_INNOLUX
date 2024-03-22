using ChartConverter.ChartDataClass;
using NLog;
using ScottPlot;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChartConverter.Converter
{
    public class ObjChartConverter
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        #region Create Class
        private static ObjChartConverter oChartConverter = null;
        public static ObjChartConverter GetInstance()
        {
            if (oChartConverter == null) oChartConverter = new ObjChartConverter();
           return oChartConverter;

        }
        private ObjChartConverter()
        {

        }
        #endregion

        

        public ScottPlot.WpfPlot GetTimeBarChart(TimeBarChart oTimeBarChart, DataLimit dataLimit = DataLimit.Ypositive,string Title="")
        {
            ScottPlot.WpfPlot wpfPlot = new ScottPlot.WpfPlot();
            try
            {

                if (oTimeBarChart == null) return wpfPlot;
                if (oTimeBarChart.dicBar == null) return wpfPlot;
                if (oTimeBarChart.dicBar.Count() == 0) return wpfPlot;
                if (Title != "") wpfPlot.Plot.Title(Title);

                List<List<double>> lspositions = new List<List<double>>();//X軸座標陣列
                wpfPlot.Plot.XAxis.DateTimeFormat(true);
                int vhNumber = oTimeBarChart.dicBar.Keys.Count();
                double minValue = 0.0;
                int DataRng = 0;
                foreach (var bKey in oTimeBarChart.dicBar.Keys)
                {
                    lspositions.Add(new List<double>());//建立對應空間量
                    if (DataRng == 0)
                    {
                        foreach (var DetailKey in oTimeBarChart.dicBar[bKey].Keys)
                        {
                            DataRng = oTimeBarChart.dicBar[bKey][DetailKey].Count();
                        }
                    }
                }
                DataRng = Math.Min(DataRng, oTimeBarChart.HourCount);//因為資料時間有可能數量小於時間區間量，故取較小值即為顯示範圍

                //切割空間，因為Bar沒辦法擺在X相同值上，所以要對每個資料刻度再針對VH數量進行切割
                int i = 0;
                int j = 0;
                for (i = 0; i < DataRng; i++)
                {
                    for (j = 0; j < lspositions.Count(); j++)
                    {
                        lspositions[j].Add(oTimeBarChart.StartTime.AddHours(i).ToOADate() + ((1.0 / 24) * j / vhNumber * .8));
                    }
                }

                #region Bar建立
                //Add Bar
                i = 0;
                List<List<double>> ValueList = null; //因為疊圖，需要從資料最高的開始印Bar，所以這個空間會先把全部資料儲存起來
                List<string> DetailNameList = new List<string>();//Detail Key值
                List<double> tempList = null;//單次Detail資料陣列
                List<ScottPlot.Plottable.BarPlot> lsBar = new List<ScottPlot.Plottable.BarPlot>();
                int iCount = 0;
                foreach (var bKey in oTimeBarChart.dicBar.Keys)
                {
                    DetailNameList.Clear();
                    ValueList = null;
                    tempList = null;
                    foreach (var BarD in oTimeBarChart.dicBar[bKey].Keys)
                    {
                        if (ValueList == null)
                        {
                            //每一個X座標的最前面資料
                            ValueList = new List<List<double>>();
                            ValueList.Add(new List<double>(oTimeBarChart.dicBar[bKey][BarD]));
                            tempList = oTimeBarChart.dicBar[bKey][BarD];
                        }
                        else
                        {
                            if (tempList.Count() != oTimeBarChart.dicBar[bKey][BarD].Count()) throw new Exception("Data number must be consistent.");//每一筆資料數量必須一致，若不一致直接丟Exception
                            //因為ScottPlot機制是多條Bar疊在同一個X座標，所以後面的資料數值其實是疊在前面的資料+要顯示的資料數值
                            for (int tp = 0; tp < tempList.Count(); tp++)
                            {
                                tempList[tp] += oTimeBarChart.dicBar[bKey][BarD][tp];
                            }
                            ValueList.Add(new List<double>(tempList)); //複製一份到總表
                        }
                        DetailNameList.Add(BarD);
                        minValue = Math.Min(minValue, tempList.Min());
                    }

                    //單次完成，開始列印Bar
                    //因為要從高的資料開始印，故這邊反向列印
                    for (iCount = ValueList.Count - 1; iCount >= 0; iCount--)
                    {
                        var bar = wpfPlot.Plot.AddBar(ValueList[iCount].ToArray(), lspositions[i].ToArray());
                        bar.Label = DetailNameList[iCount] + "(" + oTimeBarChart.Unit + ")";
                        // indicate each bar width should be 1/24 of a day then shrink sligtly to add spacing between bars
                        bar.BarWidth = (1.0 / 24) / vhNumber * .8;
                        bar.YAxisIndex = 0;
                        //bar.FillColor = Color.FromArgb(50, bar.Color); //透明色機制拿掉，因為透明色疊色後會讓原本顏色跑掉，會看不出來是哪筆資料
                        bar.ShowValuesAboveBars = true;//設定Bar顯示數值
                    }
                    i++; //移動至下一個X軸位置
                }

                i = 0;
                #endregion

                if (dataLimit == DataLimit.Ypositive)
                {
                    if (minValue > 0) minValue = 0.0;
                }

                wpfPlot.Plot.Legend(location: Alignment.UpperRight);
                wpfPlot.Plot.SetAxisLimits(yMin: minValue);

                return wpfPlot;

            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
                return wpfPlot;
            }
        }

        public ScottPlot.WpfPlot GetTimeBarChart_WithScatter(TimeBarChart oTimeBarChart, TimeScatter oTimeScatter, DataLimit dataLimit = DataLimit.Ypositive,string Title ="")
        {
            ScottPlot.WpfPlot wpfPlot = new ScottPlot.WpfPlot();
            try
            {

                if (oTimeBarChart == null) return wpfPlot;
                if (oTimeBarChart.dicBar == null) return wpfPlot;
                if (oTimeBarChart.dicBar.Count() == 0) return wpfPlot;
                if (Title != "") wpfPlot.Plot.Title(Title);

                double minValue = 0.0;
                double maxValue = double.MaxValue;
                List<List<double>> lspositions = new List<List<double>>();
                wpfPlot.Plot.XAxis.DateTimeFormat(true);

                #region 時間基底建立(X軸)
                int vhNumber = oTimeBarChart.dicBar.Keys.Count();     
                int DataRng = 0;
                double MinX = 0;
                foreach (var bKey in oTimeBarChart.dicBar.Keys)
                {
                    lspositions.Add(new List<double>());//建立對應空間量
                    if (DataRng == 0)
                    {
                        foreach (var DetailKey in oTimeBarChart.dicBar[bKey].Keys)
                        {
                            DataRng = oTimeBarChart.dicBar[bKey][DetailKey].Count();
                        }
                    }
                }
                DataRng = Math.Min(DataRng, oTimeBarChart.HourCount);//因為資料時間有可能數量小於時間區間量，故取較小值即為顯示範圍

                //切割X軸空間，因為Bar沒辦法擺在X相同值上，所以要對每個資料刻度再針對VH數量進行切割
                int i = 0;
                int j = 0;
                for (i = 0; i < DataRng; i++)
                {
                    for (j = 0; j < lspositions.Count(); j++)
                    {
                        lspositions[j].Add(oTimeBarChart.StartTime.AddHours(i).ToOADate() + ((1.0 / 24) * j / vhNumber * .8));
                    }
                }
                if (lspositions[0] != null)
                {
                    if(lspositions[0].Count >0)
                    {
                        MinX = lspositions[0][0];
                    }
                }

                #endregion

                #region Bar建立
                //Add Bar
                i = 0;
                List<List<double>> ValueList = null; //因為疊圖，需要從資料最高的開始印Bar，所以這個空間會先把全部資料儲存起來
                List<string> DetailNameList = new List<string>();//Detail Key值
                List<double> tempList = null;//單次Detail資料陣列
                List<ScottPlot.Plottable.BarPlot> lsBar =  new List<ScottPlot.Plottable.BarPlot> ();
                int iCount = 0;
                foreach (var bKey in oTimeBarChart.dicBar.Keys)
                {
                    DetailNameList.Clear();
                    ValueList = null;
                    tempList = null;
                    foreach (var BarD in oTimeBarChart.dicBar[bKey].Keys)
                    {
                        if (ValueList == null)
                        {
                            //每一個X座標的最前面資料
                            ValueList = new List<List<double>>();
                            ValueList.Add(new List<double>(oTimeBarChart.dicBar[bKey][BarD]));
                            tempList = oTimeBarChart.dicBar[bKey][BarD];
                        }
                        else
                        {
                            if (tempList.Count() != oTimeBarChart.dicBar[bKey][BarD].Count()) throw new Exception("Data number must be consistent.");//每一筆資料數量必須一致，若不一致直接丟Exception
                            //因為ScottPlot機制是多條Bar疊在同一個X座標，所以後面的資料數值其實是疊在前面的資料+要顯示的資料數值
                            for (int tp = 0; tp < tempList.Count(); tp++)
                            {
                                tempList[tp] += oTimeBarChart.dicBar[bKey][BarD][tp];                         
                            }
                            ValueList.Add(new List<double>(tempList)); //複製一份到總表
                        }
                        DetailNameList.Add(BarD);
                        minValue = Math.Min(minValue, tempList.Min());
                        maxValue = Math.Max(maxValue, tempList.Max());
                    }

                    //單次完成，開始列印Bar
                    //因為要從高的資料開始印，故這邊反向列印
                    for(iCount = ValueList.Count-1; iCount >=0; iCount--)
                    {
                        var bar = wpfPlot.Plot.AddBar(ValueList[iCount].ToArray(), lspositions[i].ToArray());
                        bar.Label = DetailNameList[iCount] + "(" + oTimeBarChart.Unit + ")";
                        // indicate each bar width should be 1/24 of a day then shrink sligtly to add spacing between bars
                        bar.BarWidth = (1.0 / 24) / vhNumber * .8;
                        bar.YAxisIndex = 0;
                        //bar.FillColor = Color.FromArgb(50, bar.Color); //透明色機制拿掉，因為透明色疊色後會讓原本顏色跑掉，會看不出來是哪筆資料
                        bar.ShowValuesAboveBars = true;//設定Bar顯示數值
                    }
                    i++; //移動至下一個X軸位置
                }

                i = 0;
                #endregion

                #region Scatter建立: X軸一律以第一筆為基準繪製Scatter，並繪製在Y軸2
                //Add Scatter
                foreach (var bKey in oTimeScatter.dicScatter.Keys)
                {
                    var scatter = wpfPlot.Plot.AddScatter(ys: oTimeScatter.dicScatter[bKey].ToArray(), xs: lspositions[0].ToArray());
                    scatter.Label = bKey + "(" + oTimeScatter.Unit + ")";
                    scatter.YAxisIndex = 1;

                    if(oTimeScatter.ShowValue)
                    {
                        //因為Scatter沒有支援顯示數值，Marker僅能以第一Y軸繪製，這邊採用生一個透明Bar並Show Value處理
                        var bar = wpfPlot.Plot.AddBar(oTimeScatter.dicScatter[bKey].ToArray(), lspositions[0].ToArray());
                        bar.FillColor = Color.FromArgb(0, 0, 0, 0);
                        bar.Font.Color = scatter.Color;
                        bar.BarWidth = 0;
                        bar.YAxisIndex = 1;
                        bar.ShowValuesAboveBars = true;
                    }
                     
                    // indicate each bar width should be 1/24 of a day then shrink sligtly to add spacing between bars
                    minValue = Math.Min(minValue, oTimeScatter.dicScatter[bKey].Min());
                    maxValue = Math.Max(minValue, oTimeScatter.dicScatter[bKey].Max());
                }
                #endregion

                //Limit設定:如果Y軸資料只有正數，Y軸最小值Limit固定為0
                if (dataLimit == DataLimit.Ypositive)
                {
                    if (minValue > 0) minValue = 0.0;
                }

                //設定Y軸的顯示文字
                if (oTimeBarChart.YAxisTitle != "") wpfPlot.Plot.YAxis.Label(oTimeBarChart.YAxisTitle + "(" + oTimeBarChart.Unit + ")");
                if (oTimeScatter.YAxisTitle != "") wpfPlot.Plot.YAxis2.Label(oTimeScatter.YAxisTitle + "(" + oTimeScatter.Unit + ")");

                wpfPlot.Plot.YAxis2.Ticks(true);
                wpfPlot.Plot.Legend(location: Alignment.UpperRight);
                wpfPlot.Plot.SetAxisLimits(yMin: minValue);
                wpfPlot.Plot.SetOuterViewLimits(yMin: minValue,yMax: maxValue);

                return wpfPlot;

            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
                return wpfPlot;
            }
        }

        public ScottPlot.WpfPlot GetScatterChart(Scatter oScatter, Scatter oScatterY2=null, DataLimit dataLimit = DataLimit.Ypositive,string Title ="")
        {
            ScottPlot.WpfPlot wpfPlot = new ScottPlot.WpfPlot();
            try
            {

                if (oScatter == null) return wpfPlot;
                if (oScatter.XAxisDataString.Count() == 0) return wpfPlot;
                if (oScatter.dicScatter.Count() == 0) return wpfPlot;
                if (Title != "") wpfPlot.Plot.Title(Title);

                double minValue = 0.0;
                double maxValue = double.MaxValue;

                int vhNumber = oScatter.dicScatter.Keys.Count();
                int DataRng = 0;
                double MinX = 0;

                #region Scatter建立: X軸一律以第一筆為基準繪製Scatter，並繪製在Y軸2

                //設定X軸對應文字
                wpfPlot.Plot.XAxis.ManualTickPositions(oScatter.XAxisDataString.Keys.ToArray(), oScatter.XAxisDataString.Values.ToArray());
                string unit = "";
                if(oScatter.Unit!="")
                {
                    unit = "(" + oScatter.Unit + ")";
                }
                //Add Scatter
                foreach (var bKey in oScatter.dicScatter.Keys)
                {
                    var scatter = wpfPlot.Plot.AddScatter(ys: oScatter.dicScatter[bKey].ToArray(), xs: oScatter.XAxisData.ToArray());
                    scatter.Label = bKey + unit;
                    scatter.LineWidth = 2;
                    // indicate each bar width should be 1/24 of a day then shrink sligtly to add spacing between bars
                    minValue = Math.Min(minValue, oScatter.dicScatter[bKey].Min());


                    if(oScatter.ShowValue)
                    {
                        //因為Scatter沒有支援顯示數值，Marker僅能以第一Y軸繪製，這邊採用生一個透明Bar並Show Value處理
                        var bar = wpfPlot.Plot.AddBar(oScatter.dicScatter[bKey].ToArray(), oScatter.XAxisData.ToArray());
                        bar.FillColor = Color.FromArgb(0, 0, 0, 0);
                        bar.Font.Color = scatter.Color;
                        bar.BarWidth = 0;
                        bar.YAxisIndex = 0;
                        bar.ShowValuesAboveBars = true;
                    }
                   

                }
                #endregion

                #region ScatterY2建立: X軸一律以第一筆為基準繪製Scatter，並繪製在Y軸2
                if(oScatterY2 != null)
                {
                    //設定X軸對應文字
                    wpfPlot.Plot.XAxis.ManualTickPositions(oScatterY2.XAxisDataString.Keys.ToArray(), oScatterY2.XAxisDataString.Values.ToArray());
                    string unitY2 = "";
                    if (oScatterY2.Unit != "")
                    {
                        unit = "(" + oScatterY2.Unit + ")";
                    }
                    //Add Scatter
                    foreach (var bKey in oScatterY2.dicScatter.Keys)
                    {
                        var scatter = wpfPlot.Plot.AddScatter(ys: oScatterY2.dicScatter[bKey].ToArray(), xs: oScatterY2.XAxisData.ToArray());
                        scatter.YAxisIndex = 1;
                        scatter.Label = bKey + unit;
                        scatter.LineWidth = 2;

                        if (oScatterY2.ShowValue)
                        {
                            //因為Scatter沒有支援顯示數值，Marker僅能以第一Y軸繪製，這邊採用生一個透明Bar並Show Value處理
                            var bar = wpfPlot.Plot.AddBar(oScatterY2.dicScatter[bKey].ToArray(), oScatterY2.XAxisData.ToArray());
                            bar.FillColor = Color.FromArgb(0, 0, 0, 0);
                            bar.Font.Color = scatter.Color;
                            bar.BarWidth = 0;
                            bar.YAxisIndex = 1;
                            bar.ShowValuesAboveBars = true;
                        }

                        // indicate each bar width should be 1/24 of a day then shrink sligtly to add spacing between bars
                        minValue = Math.Min(minValue, oScatterY2.dicScatter[bKey].Min());
                        //maxValue = Math.Max(minValue, oScatter.dicScatter[bKey].Max());
                    }
                }
              
                #endregion

                //Limit設定:如果Y軸資料只有正數，Y軸最小值Limit固定為0
                if (dataLimit == DataLimit.Ypositive)
                {
                    if (minValue > 0) minValue = 0.0;
                }

                //設定Y軸的顯示文字
                if (oScatter.YAxisTitle != "") wpfPlot.Plot.YAxis.Label(oScatter.YAxisTitle + "(" + oScatter.Unit + ")");
                if (oScatterY2 != null)
                {
                    if (oScatterY2.YAxisTitle != "") wpfPlot.Plot.YAxis2.Label(oScatterY2.YAxisTitle + "(" + oScatterY2.Unit + ")");
                }
                   

                if(oScatterY2!=null)wpfPlot.Plot.YAxis2.Ticks(true);
                wpfPlot.Plot.Legend(location: Alignment.UpperRight);
                wpfPlot.Plot.SetAxisLimits(yMin: minValue);
                wpfPlot.Plot.SetOuterViewLimits(yMin: minValue);

                return wpfPlot;

            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
                return wpfPlot;
            }
        }

        public ScottPlot.WpfPlot GetPieChart(PieChart oPieChart,  string Title = "")
        {
            ScottPlot.WpfPlot wpfPlot = new ScottPlot.WpfPlot();
            try
            {

                if (oPieChart == null) return wpfPlot;
                if (oPieChart.Values.Count() == 0) return wpfPlot;
                if (Title != "") wpfPlot.Plot.Title(Title);

                List<string> lsLabel = new List<string>();
                List<Color> lsLabelColor = new List<Color>();
                #region Pie建立
                foreach(var key in oPieChart.Values.Keys)
                {
                    if (oPieChart.Labels.ContainsKey(key)) lsLabel.Add(oPieChart.Labels[key]);
                    else lsLabel.Add("");

                    lsLabelColor.Add(Color.Black);
                }

                var pie = wpfPlot.Plot.AddPie(oPieChart.Values.Values.ToArray());
                pie.SliceLabels = lsLabel.ToArray();
                pie.SliceLabelColors = lsLabelColor.ToArray();
                pie.ShowPercentages = true;
                pie.ShowValues = true;
                pie.ShowLabels = oPieChart.ShowLabelInChart;
                wpfPlot.Plot.Legend();


                #endregion
                //設定Y軸的顯示文字
                if (oPieChart.YAxisTitle != "") wpfPlot.Plot.YAxis.Label(oPieChart.YAxisTitle + "(" + oPieChart.Unit + ")");
                //if (oScatter.YAxisTitle != "") wpfPlot.Plot.YAxis2.Label(oScatter.YAxisTitle + "(" + oScatter.Unit + ")");

                //wpfPlot.Plot.YAxis2.Ticks(true);
                wpfPlot.Plot.Legend(location: Alignment.UpperRight);

                return wpfPlot;

            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
                return wpfPlot;
            }
        }


        public enum ChartType
        {
            BarChart =0,
            TimeBarChart =1
        }
        public enum DataLimit
        {
            Ypositive = 0
        }
    }
}
