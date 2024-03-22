using ClosedXML.Excel;
using om.mirle.ibg3k0.bc.winform.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtilsAPI.Common;

namespace com.mirle.ibg3k0.bc.winform.Common
{ 
    public class XSLXHelper
    {
        public XSLXHelper(int _max_row_size =10000)
        {
            MAX_ROW_SIZE = _max_row_size;
        }

        public  int MAX_ROW_SIZE = 10000;
        /// <summary>
        /// 產生 excel
        /// </summary>
        /// <typeparam name="T">傳入的物件型別</typeparam>
        /// <param name="data">物件資料集</param>
        /// <param name="oXSLXFormat">Format Class</param>
        /// <returns></returns>
        public XLWorkbook Export<T>(List<T> data, XSLXFormat oXSLXFormat = null)
        {
            if (oXSLXFormat == null) oXSLXFormat = new XSLXFormat();
            //建立 excel 物件
            XLWorkbook workbook = new XLWorkbook();
            try
            {
                //加入 excel 工作表名為 `Report`
                var sheet = workbook.Worksheets.Add("Report");
                //欄位起啟位置
                int rowIdx = 1;
                int colIdx = 1;
                string unit = "";
                if (oXSLXFormat.WKSheetHeader.ContainsKey(sheet.Name))
                {
                    if (oXSLXFormat.WKSheetHeader[sheet.Name] != "")
                    {
                        sheet.Cell(rowIdx, colIdx).Value = oXSLXFormat.WKSheetHeader[sheet.Name];
                        rowIdx++;
                    }
                }
                //使用 reflection 將物件屬性取出當作工作表欄位名稱
                foreach (var item in typeof(T).GetProperties())
                {
                    if (oXSLXFormat.HiddenColumns.Contains(item.Name)) continue;//如果是要隱藏的Column，跳過

                    if (oXSLXFormat.ColumnUnit.ContainsKey(item.Name))
                    {
                        unit = "(" + oXSLXFormat.ColumnUnit[item.Name] + ")";//如果有要顯示單位，增加
                    }
                    else
                    {
                        unit = "";
                    }
                    #region - 可以使用 DescriptionAttribute 設定，找不到 DescriptionAttribute 時改用屬性名稱 -
                    sheet.Cell(rowIdx, colIdx).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    if (oXSLXFormat.ChangeHeaders.ContainsKey(item.Name))
                    {
                        sheet.Cell(rowIdx, colIdx++).Value = oXSLXFormat.ChangeHeaders[item.Name] + unit;
                    }
                    else
                    {
                        //可以使用 DescriptionAttribute 設定，找不到 DescriptionAttribute 時改用屬性名稱
                        DescriptionAttribute description = Attribute.GetCustomAttribute(item, typeof(DescriptionAttribute), false) as DescriptionAttribute;  /*item.GetCustomAttribute(typeof(DescriptionAttribute)) as DescriptionAttribute;*/

                        if (description != null)
                        {
                            sheet.Cell(rowIdx, colIdx++).Value = description.Description + unit;
                            continue;
                        }
                        sheet.Cell(rowIdx, colIdx++).Value = item.Name + unit;
                    }
                    #endregion
                    #region - 直接使用物件屬性名稱 -
                    //或是直接使用物件屬性名稱
                    //sheet.Cell(1, colIdx++).Value = item.Name;
                    #endregion
                }
                //資料起始列位置             
                rowIdx++;
                foreach (var item in data)
                {
                    if (rowIdx > MAX_ROW_SIZE && MAX_ROW_SIZE != -1 ) break;//Print限制
                    //每筆資料欄位起始位置
                    int conlumnIndex = 1;
                    foreach (var jtem in item.GetType().GetProperties())
                    {
                        if (oXSLXFormat.HiddenColumns.Contains(jtem.Name)) continue;//如果是要隱藏的Column，跳過
                        //檢查如果資料是string而且包含'E'的時候，這時候Excel會變成展開的科學符號(1E006 => 100000)，改成Type:Text也無法處理，僅能在前面加上單引號
                        if (jtem.PropertyType == typeof(string))
                        {
                            string temp = (string)(jtem.GetValue(item, null));
                            if (temp != null)
                            {
                                if (temp.Contains('E'))
                                {
                                    temp = "'" + temp;//將資料內容加上 "'" 避免受到 excel 預設格式影響，並依 row 及 column 填入
                                }
                            }
                            sheet.Cell(rowIdx, conlumnIndex).Value = temp;
                        }
                        else if (jtem.PropertyType == typeof(DateTime))
                        {
                            DateTime temp = (DateTime)(jtem.GetValue(item, null));
                            if (temp != DateTime.MinValue) sheet.Cell(rowIdx, conlumnIndex).Value = jtem.GetValue(item, null);
                        }
                        else
                        {
                            sheet.Cell(rowIdx, conlumnIndex).Value = jtem.GetValue(item, null);
                        }

                       
                        if (oXSLXFormat.ColumnFormat.ContainsKey(jtem.Name))
                        {
                            //如果是百分比，值/100
                            if (oXSLXFormat.ColumnFormat[jtem.Name].NumberFormat.Contains("%"))
                            {
                                    sheet.Cell(rowIdx, conlumnIndex).Value = Convert.ToDouble(jtem.GetValue(item, null)) / 100;
                            }
                            sheet.Cell(rowIdx, conlumnIndex).Style = oXSLXFormat.ColumnFormat[jtem.Name].XSLXStyle_ComToIXLStyle(sheet.Cell(rowIdx, conlumnIndex).Style);
                        }
                        else
                        {
                            if (oXSLXFormat.DefaultTypeFormat.ContainsKey(jtem.PropertyType))
                            {
                                sheet.Cell(rowIdx, conlumnIndex).Style = oXSLXFormat.DefaultTypeFormat[jtem.PropertyType].XSLXStyle_ComToIXLStyle(sheet.Cell(rowIdx, conlumnIndex).Style);
                            }
                        }
                        conlumnIndex++;
                    }
                    rowIdx++;
                }
            }
            catch (Exception ex)
            {
                NLog.LogManager.GetCurrentClassLogger().Warn(ex, "Exception");
            }
            return workbook;
        }

        /// <summary>
        /// 列印帶有Relation的資料使用，可以輸出有Detail Group的Excel
        /// </summary>
        /// <param name="data">主Data，即父資料</param>
        /// <param name="dicDetail">Detail的Dictionary，Key值為父資料，Value為Relation資料的List</param>
        /// <param name="oXSLXFormat">Format Class</param>
        /// <returns> XLWorkbook </returns>
        public XLWorkbook Export_Detail<T, DetailT>(List<T> data, Dictionary<T,List<DetailT>> dicDetail, XSLXFormat oXSLXFormat = null)
        {
            //建立 excel 物件
            XLWorkbook workbook = new XLWorkbook();
            try
            {
                if (oXSLXFormat == null) oXSLXFormat = new XSLXFormat();
                //加入 excel 工作表名為 `Report`
                var sheet = workbook.Worksheets.Add("Report");
                //欄位起啟位置
                int rowIdx = 1;
                int colIdx = 1;
                string unit="";
                if (oXSLXFormat.WKSheetHeader.ContainsKey(sheet.Name))
                {
                    if (oXSLXFormat.WKSheetHeader[sheet.Name] != "")
                    {
                        sheet.Cell(rowIdx, colIdx).Value = oXSLXFormat.WKSheetHeader[sheet.Name];
                        rowIdx++;
                    }
                }
                //使用 reflection 將物件屬性取出當作工作表欄位名稱
                if(typeof(T) != typeof(string))
                {
                    foreach (var item in typeof(T).GetProperties())
                    {
                        if (oXSLXFormat.HiddenColumns.Contains(item.Name)) continue;//如果是要隱藏的Column，跳過

                        if (oXSLXFormat.ColumnUnit.ContainsKey(item.Name))
                        {
                            unit = "(" + oXSLXFormat.ColumnUnit[item.Name] + ")";//如果有要顯示單位，增加
                        }
                        else
                        {
                            unit = "";
                        }
                        #region - 可以使用 DescriptionAttribute 設定，找不到 DescriptionAttribute 時改用屬性名稱 -
                        sheet.Cell(rowIdx, colIdx).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                        if (oXSLXFormat.ChangeHeaders.ContainsKey(item.Name))
                        {
                            sheet.Cell(rowIdx, colIdx++).Value = oXSLXFormat.ChangeHeaders[item.Name] + unit;
                        }
                        else
                        {
                            //可以使用 DescriptionAttribute 設定，找不到 DescriptionAttribute 時改用屬性名稱
                            DescriptionAttribute description = Attribute.GetCustomAttribute(item, typeof(DescriptionAttribute), false) as DescriptionAttribute;  /*item.GetCustomAttribute(typeof(DescriptionAttribute)) as DescriptionAttribute;*/

                            if (description != null)
                            {
                                sheet.Cell(rowIdx, colIdx++).Value = description.Description + unit;
                                continue;
                            }
                            sheet.Cell(rowIdx, colIdx++).Value = item.Name + unit;
                        }


                        #endregion
                        #region - 直接使用物件屬性名稱 -
                        //或是直接使用物件屬性名稱
                        //sheet.Cell(1, colIdx++).Value = item.Name;
                        #endregion
                    }
                }
               
                //資料起始列位置
                rowIdx++;
                var groupStartRow = 0;
                var groupEndRow = 0;
                foreach (var item in data)
                {
                    if (rowIdx > MAX_ROW_SIZE && MAX_ROW_SIZE != -1) break;//Print限制

                    //每筆資料欄位起始位置
                    int conlumnIndex = 1;
                    var lsDetail = dicDetail[item];

                    //非自定義Class Key僅接受string
                    if(item.GetType()==typeof(string))
                    {
                        //如果是string tpye key，直接列印數值，不做properties分析
                        sheet.Cell(rowIdx, conlumnIndex).Value= item.ToString();
                    }
                    else
                    {
                        //如果是自定義Class當Key，把Properties show出來
                        foreach (var jtem in item.GetType().GetProperties())
                        {
                            if (oXSLXFormat.HiddenColumns.Contains(jtem.Name)) continue;//如果是要隱藏的Column，跳過
                                                                                        //將資料內容加上 "'" 避免受到 excel 預設格式影響，並依 row 及 column 填入

                            //檢查如果資料是string而且包含'E'的時候，這時候Excel會變成展開的科學符號(1E006 => 100000)，改成Type:Text也無法處理，僅能在前面加上單引號
                            if (jtem.PropertyType == typeof(string))
                            {
                                string temp = (string)(jtem.GetValue(item, null));
                                if (temp != null)
                                {
                                    if (temp.Contains('E'))
                                    {
                                        temp = "'" + temp;//將資料內容加上 "'" 避免受到 excel 預設格式影響，並依 row 及 column 填入
                                    }
                                }
                                sheet.Cell(rowIdx, conlumnIndex).Value = temp;
                            }
                            else if (jtem.PropertyType == typeof(DateTime))
                            {
                                if(jtem.GetValue(item, null) != null)
                                {
                                    DateTime temp = (DateTime)(jtem.GetValue(item, null));
                                    if (temp != DateTime.MinValue) sheet.Cell(rowIdx, conlumnIndex).Value = jtem.GetValue(item, null);
                                }                              
                            }
                            else
                            {
                                sheet.Cell(rowIdx, conlumnIndex).Value = jtem.GetValue(item, null);
                            }
                            if (oXSLXFormat.ColumnFormat.ContainsKey(jtem.Name))
                            {
                                //如果是百分比，值/100
                                if (oXSLXFormat.ColumnFormat[jtem.Name].NumberFormat.Contains("%"))
                                {
                                    sheet.Cell(rowIdx, conlumnIndex).Value = Convert.ToDouble(jtem.GetValue(item, null)) / 100;
                                }
                                sheet.Cell(rowIdx, conlumnIndex).Style = oXSLXFormat.ColumnFormat[jtem.Name].XSLXStyle_ComToIXLStyle(sheet.Cell(rowIdx, conlumnIndex).Style);
                            }
                            else
                            {
                                if (oXSLXFormat.DefaultTypeFormat.ContainsKey(jtem.PropertyType))
                                {
                                    sheet.Cell(rowIdx, conlumnIndex).Style = oXSLXFormat.DefaultTypeFormat[jtem.PropertyType].XSLXStyle_ComToIXLStyle(sheet.Cell(rowIdx, conlumnIndex).Style);
                                }
                            }
                            conlumnIndex++;
                        }
                    }
                   

                    //Detail 資料起始設定
                    //Detail Header
                    rowIdx++;
                    sheet.Cell(rowIdx, 2).Value = string.Concat("'","Detail:" );
                    groupStartRow = rowIdx; //紀錄Detail 這一組Group的開始Row

                    #region Get Detail Item Name
                    conlumnIndex = 2;// Detail資料從隔行第二列開始印
                    rowIdx++;
                    //使用 reflection 將Detail物件屬性取出當作工作表欄位名稱
                    foreach (var DetailT_Item in typeof(DetailT).GetProperties())
                    {
                        if (oXSLXFormat.HiddenColumns.Contains(DetailT_Item.Name)) continue;//如果是要隱藏的Column，跳過

                        if (oXSLXFormat.ColumnUnit.ContainsKey(DetailT_Item.Name))
                        {
                            unit = "(" + oXSLXFormat.ColumnUnit[DetailT_Item.Name] + ")";//如果有要顯示單位，增加
                        }
                        else
                        {
                            unit = "";
                        }
                        #region - 可以使用 DescriptionAttribute 設定，找不到 DescriptionAttribute 時改用屬性名稱 -
                        sheet.Cell(1, colIdx).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                        if (oXSLXFormat.ChangeHeaders.ContainsKey(DetailT_Item.Name))
                        {
                            sheet.Cell(1, colIdx++).Value = oXSLXFormat.ChangeHeaders[DetailT_Item.Name] + unit;
                        }
                        else
                        {
                            //可以使用 DescriptionAttribute 設定，找不到 DescriptionAttribute 時改用屬性名稱
                            DescriptionAttribute description = Attribute.GetCustomAttribute(DetailT_Item, typeof(DescriptionAttribute), false) as DescriptionAttribute;  /*item.GetCustomAttribute(typeof(DescriptionAttribute)) as DescriptionAttribute;*/
                            if (description != null)
                            {
                                sheet.Cell(rowIdx, conlumnIndex++).Value = description.Description + unit;
                                continue;
                            }
                            sheet.Cell(rowIdx, conlumnIndex++).Value = DetailT_Item.Name + unit;
                        }
                        
                        #endregion
                        #region - 直接使用物件屬性名稱 -
                        //或是直接使用物件屬性名稱
                        //sheet.Cell(1, colIdx++).Value = item.Name;
                        #endregion
                    }
                    #endregion

                   #region Detail Data Print               
                    foreach (var Detail_item in lsDetail)
                    {
                        rowIdx++;
                        conlumnIndex = 2;// Detail資料從隔行第二列開始印
                        foreach (var DetailProperties in Detail_item.GetType().GetProperties())
                        {
                            if (oXSLXFormat.HiddenColumns.Contains(DetailProperties.Name)) continue;//如果是要隱藏的Column，跳過
                             //檢查如果資料是string而且包含'E'的時候，這時候Excel會變成展開的科學符號(1E006 => 100000)，改成Type:Text也無法處理，僅能在前面加上單引號
                            if (DetailProperties.PropertyType == typeof(string))
                            {
                                string temp = (string)(DetailProperties.GetValue(Detail_item, null));
                                if(temp!= null)
                                {
                                    if (temp.Contains('E'))
                                    {
                                        temp = "'" + temp;//將資料內容加上 "'" 避免受到 excel 預設格式影響，並依 row 及 column 填入
                                    }
                                }
                               
                                sheet.Cell(rowIdx, conlumnIndex).Value = temp;
                            }
                            else if (DetailProperties.PropertyType == typeof(DateTime))
                            {
                                DateTime temp = (DateTime)(DetailProperties.GetValue(Detail_item, null));
                                if (temp != DateTime.MinValue) sheet.Cell(rowIdx, conlumnIndex).Value = DetailProperties.GetValue(Detail_item, null);
                            }
                            else
                            {
                                sheet.Cell(rowIdx, conlumnIndex).Value = DetailProperties.GetValue(Detail_item, null); //string.Concat("'", Convert.ToString(DetailProperties.GetValue(Detail_item, null)));
                            }

                            if (oXSLXFormat.ColumnFormat.ContainsKey(DetailProperties.Name))
                            {
                                //如果是百分比，值/100
                                if(oXSLXFormat.ColumnFormat[DetailProperties.Name].NumberFormat.Contains("%"))
                                {

                                        sheet.Cell(rowIdx, conlumnIndex).Value = Convert.ToDouble(DetailProperties.GetValue(Detail_item, null))/100; 
                                }
                                sheet.Cell(rowIdx, conlumnIndex).Style = oXSLXFormat.ColumnFormat[DetailProperties.Name].XSLXStyle_ComToIXLStyle(sheet.Cell(rowIdx, conlumnIndex).Style);
                            }
                            else
                            {
                                if (oXSLXFormat.DefaultTypeFormat.ContainsKey(DetailProperties.PropertyType))
                                {
                                    sheet.Cell(rowIdx, conlumnIndex).Style = oXSLXFormat.DefaultTypeFormat[DetailProperties.PropertyType].XSLXStyle_ComToIXLStyle(sheet.Cell(rowIdx, conlumnIndex).Style);
                                }
                            }
                            conlumnIndex++;
                        }                                
                    }

                    groupEndRow = rowIdx; //紀錄Detail 這一組Group的結束Row
                    sheet.Rows(groupStartRow, groupEndRow).Group(); //將Detail資料Group
                    #endregion

                    rowIdx++;
                }
            }
            catch (Exception ex)
            {
                NLog.LogManager.GetCurrentClassLogger().Warn(ex, "Exception");
            }
            return workbook;
        }

        /// <summary>
        /// 產生 excel
        /// </summary>
        /// <typeparam name="T">傳入的物件型別</typeparam>
        /// <param name="workbook">工作簿</param>
        /// <param name="SheetName">新的Sheet名稱</param>
        /// <param name="data">物件資料集</param>
        /// <param name="oXSLXFormat">Format Class</param>
        /// <returns></returns>
        public void AddSheet<T>(ref XLWorkbook workbook, string SheetName,List<T> data, XSLXFormat oXSLXFormat = null)
        {
            List<System.Reflection.PropertyInfo> TOnlyProperties = new List<System.Reflection.PropertyInfo>();
            List<System.Reflection.PropertyInfo> BaseTProperties = new List<System.Reflection.PropertyInfo>();
            List<System.Reflection.PropertyInfo> MergeProperties = new List<System.Reflection.PropertyInfo>();

            if (oXSLXFormat == null) oXSLXFormat = new XSLXFormat();
            try
            {
                //因為要先列印BaseType的東西，所以先處理MergeProperties
                BaseTProperties = typeof(T).BaseType.GetProperties().ToList();
                foreach (var item in typeof(T).GetProperties())
                {
                    if (BaseTProperties.Where(x=> x.Name == item.Name).Count() >0 ) break;//因為GetProperties會先列出子Class的Properties，如果找到第一個符合的，後面都是BaseClass的，直接跳出
                    TOnlyProperties.Add(item);
                }
                MergeProperties.AddRange(BaseTProperties);//最低的BaseType是Object，GetProperties為0
                MergeProperties.AddRange(TOnlyProperties);

                //加入 excel 工作表名為 `Report`
                var sheet = workbook.Worksheets.Add(SheetName);
                //欄位起啟位置
                int rowIdx = 1;
                int colIdx = 1;
                string unit = "";
                if(oXSLXFormat.WKSheetHeader.ContainsKey(sheet.Name))
                {
                    if (oXSLXFormat.WKSheetHeader[sheet.Name] != "")
                    {
                        sheet.Cell(rowIdx, colIdx).Value = oXSLXFormat.WKSheetHeader[sheet.Name];
                        rowIdx++;
                    }
                }        
                //使用 reflection 將物件屬性取出當作工作表欄位名稱
                foreach (var item in MergeProperties)
                {
                    if (oXSLXFormat.HiddenColumns.Contains(item.Name)) continue;//如果是要隱藏的Column，跳過

                    if (oXSLXFormat.ColumnUnit.ContainsKey(item.Name))
                    {
                        unit = "(" + oXSLXFormat.ColumnUnit[item.Name] + ")";//如果有要顯示單位，增加
                    }
                    else
                    {
                        unit = "";
                    }
                    #region - 可以使用 DescriptionAttribute 設定，找不到 DescriptionAttribute 時改用屬性名稱 -
                    sheet.Cell(rowIdx, colIdx).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    if (oXSLXFormat.ChangeHeaders.ContainsKey(item.Name))
                    {
                        sheet.Cell(rowIdx, colIdx++).Value = oXSLXFormat.ChangeHeaders[item.Name] + unit;
                    }
                    else
                    {
                        //可以使用 DescriptionAttribute 設定，找不到 DescriptionAttribute 時改用屬性名稱
                        DescriptionAttribute description = Attribute.GetCustomAttribute(item, typeof(DescriptionAttribute), false) as DescriptionAttribute;  /*item.GetCustomAttribute(typeof(DescriptionAttribute)) as DescriptionAttribute;*/

                        if (description != null)
                        {
                            sheet.Cell(rowIdx, colIdx++).Value = description.Description + unit;
                            continue;
                        }
                        sheet.Cell(rowIdx, colIdx++).Value = item.Name + unit;
                        
                    }
                    #endregion
                    #region - 直接使用物件屬性名稱 -
                    //或是直接使用物件屬性名稱
                    //sheet.Cell(1, colIdx++).Value = item.Name;
                    #endregion
                }
                //資料起始列位置
                rowIdx++;
                foreach (var item in data)
                {
                    if (rowIdx > MAX_ROW_SIZE && MAX_ROW_SIZE != -1) break;//Print限制
                    //每筆資料欄位起始位置
                    int conlumnIndex = 1;
                    foreach (var jtem in MergeProperties)
                    {
                        if (oXSLXFormat.HiddenColumns.Contains(jtem.Name)) continue;//如果是要隱藏的Column，跳過

                        //檢查如果資料是string而且包含'E'的時候，這時候Excel會變成展開的科學符號(1E006 => 100000)，改成Type:Text也無法處理，僅能在前面加上單引號
                        if (jtem.PropertyType == typeof(string))
                        {
                            string temp = (string)(jtem.GetValue(item, null));
                            if (temp != null)
                            {
                                if (temp.Contains('E'))
                                {
                                    temp = "'" + temp;//將資料內容加上 "'" 避免受到 excel 預設格式影響，並依 row 及 column 填入
                                }
                            }
                            sheet.Cell(rowIdx, conlumnIndex).Value = temp;
                        }
                        else if (jtem.PropertyType == typeof(DateTime))
                        {
                            DateTime temp = (DateTime)(jtem.GetValue(item, null));
                            if (temp != DateTime.MinValue) sheet.Cell(rowIdx, conlumnIndex).Value = jtem.GetValue(item, null);
                        }
                        else
                        {
                            sheet.Cell(rowIdx, conlumnIndex).Value = jtem.GetValue(item, null);
                        }

                        if ( oXSLXFormat.ColumnFormat.ContainsKey(jtem.Name))
                        {
                            //如果是百分比 值/100，
                            if (oXSLXFormat.ColumnFormat[jtem.Name].NumberFormat.Contains("%"))
                            {
                                sheet.Cell(rowIdx, conlumnIndex).Value = Convert.ToDouble(jtem.GetValue(item, null)) / 100;
                            }
                            sheet.Cell(rowIdx, conlumnIndex).Style = oXSLXFormat.ColumnFormat[jtem.Name].XSLXStyle_ComToIXLStyle(sheet.Cell(rowIdx, conlumnIndex).Style);
                        }
                        else
                        {
                            if (oXSLXFormat.DefaultTypeFormat.ContainsKey(jtem.PropertyType))
                            {                                                        
                                sheet.Cell(rowIdx, conlumnIndex).Style = oXSLXFormat.DefaultTypeFormat[jtem.PropertyType].XSLXStyle_ComToIXLStyle(sheet.Cell(rowIdx, conlumnIndex).Style);
                            }
                        }
                            
                        conlumnIndex++;
                    }
                    rowIdx++;
                }
            }
            catch (Exception ex)
            {
                NLog.LogManager.GetCurrentClassLogger().Warn(ex, "Exception");
            }
        }

        /// <summary>
        /// 產生 excel
        /// </summary>
        /// <param name="data">物件資料集</param>
        /// <param name="oXSLXFormat">Format Class</param>
        /// <returns></returns>
        public XLWorkbook Export_DataTable(DataTable data, XSLXFormat oXSLXFormat = null)
        {
            if (oXSLXFormat == null) oXSLXFormat = new XSLXFormat();
            //建立 excel 物件
            XLWorkbook workbook = new XLWorkbook();
            try
            {
                //加入 excel 工作表名為 `Report`
                var sheet = workbook.Worksheets.Add("Report");
                //欄位起啟位置
                int rowIdx = 1;
                int colIdx = 1;
                string unit = "";
                List<int> HiddenIndexs = new List<int>();
                Dictionary<int,XSLXStyle_Com> ColumnFormatIndexs = new Dictionary<int, XSLXStyle_Com>();
                if (oXSLXFormat.WKSheetHeader.ContainsKey(sheet.Name))
                {
                    if (oXSLXFormat.WKSheetHeader[sheet.Name] != "")
                    {
                        sheet.Cell(rowIdx, colIdx).Value = oXSLXFormat.WKSheetHeader[sheet.Name];
                        rowIdx++;
                    }
                }
                //使用 reflection 將物件屬性取出當作工作表欄位名稱
                foreach (DataColumn item in data.Columns)
                {
                    if (oXSLXFormat.HiddenColumns.Contains(item.Caption))
                    {
                        HiddenIndexs.Add(data.Columns.IndexOf(item));
                        continue;//如果是要隱藏的Column，跳過
                    }
                    if (oXSLXFormat.ColumnFormat.ContainsKey(item.Caption))
                    {
                        ColumnFormatIndexs.Add(data.Columns.IndexOf(item), oXSLXFormat.ColumnFormat[item.Caption]);
                        continue;//如果是要隱藏的Column，跳過
                    }

                    if (oXSLXFormat.ColumnUnit.ContainsKey(item.Caption))
                    {
                        unit = "(" + oXSLXFormat.ColumnUnit[item.Caption] + ")";//如果有要顯示單位，增加
                    }
                    else
                    {
                        unit = "";
                    }
                    #region - 可以使用 DescriptionAttribute 設定，找不到 DescriptionAttribute 時改用屬性名稱 -
                    sheet.Cell(rowIdx, colIdx).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    if (oXSLXFormat.ChangeHeaders.ContainsKey(item.Caption))
                    {
                        sheet.Cell(rowIdx, colIdx++).Value = oXSLXFormat.ChangeHeaders[item.Caption] + unit;
                    }
                    else
                    {
                        sheet.Cell(rowIdx, colIdx++).Value = item.Caption + unit;
                    }
                    #endregion
                    #region - 直接使用物件屬性名稱 -
                    //或是直接使用物件屬性名稱
                    //sheet.Cell(1, colIdx++).Value = item.Name;
                    #endregion
                }
                //資料起始列位置             
                rowIdx++;
                foreach (DataRow item in data.Rows)
                {

                    if (rowIdx > MAX_ROW_SIZE && MAX_ROW_SIZE != -1) break;//Print限制
                    //每筆資料欄位起始位置
                    int conlumnIndex = 1;
                    int rindex = 0;
                    foreach (var jtem in item.ItemArray)
                    {
                        if (HiddenIndexs.Contains(rindex)) continue;//如果是要隱藏的Column，跳過
                        //將資料內容加上 "'" 避免受到 excel 預設格式影響，並依 row 及 column 填入
                        sheet.Cell(rowIdx, conlumnIndex).Value = jtem;// string.Concat("'", Convert.ToString(jtem.GetValue(item, null)));
                        if (ColumnFormatIndexs.ContainsKey(rindex))
                        {
                            //如果是百分比，值/100
                            if (ColumnFormatIndexs[rindex].NumberFormat.Contains("%"))
                            {
                                sheet.Cell(rowIdx, conlumnIndex).Value = Convert.ToDouble(jtem) / 100;
                            }
                            sheet.Cell(rowIdx, conlumnIndex).Style = ColumnFormatIndexs[rindex].XSLXStyle_ComToIXLStyle(sheet.Cell(rowIdx, conlumnIndex).Style);
                        }
                        else
                        {
                            if (oXSLXFormat.DefaultTypeFormat.ContainsKey(jtem.GetType()))
                            {
                                sheet.Cell(rowIdx, conlumnIndex).Style = oXSLXFormat.DefaultTypeFormat[jtem.GetType()].XSLXStyle_ComToIXLStyle(sheet.Cell(rowIdx, conlumnIndex).Style);
                            }
                        }
                        rindex++;
                        conlumnIndex++;
                    }
                    rowIdx++;
                }
            }
            catch (Exception ex)
            {
                NLog.LogManager.GetCurrentClassLogger().Warn(ex, "Exception");
            }
            return workbook;
        }
    }



}
