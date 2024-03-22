using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

using UtilsAPI.Common;

namespace om.mirle.ibg3k0.bc.winform.Common
{
    public  class XSLXFormat
    {
        #region Property

        //隱藏欄位的List
        public List<string> HiddenColumns { get; set; }

        //要更名的Dictionary，Key為原Column名稱，Value為更名的Header(不含Unit)
        //其他有用到欄位名稱的項目，一律用舊名為參數，而不使用新Header
        public Dictionary<string, string> ChangeHeaders { get; set; }

        //增加顯示欄位的Unit,Key為欄位名稱，Value為括弧內容
        public Dictionary<string, string> ColumnUnit { get; set; }

        //預設的Type欄位使用的Format，Key為Type型別，Value為Format
        public Dictionary<Type, XSLXStyle_Com> DefaultTypeFormat { get; set; }

        //指定欄位使用的format，給客製使用(如需要顯示百分比資料等)，「優先度高於DefaultTypeList」，Key為欄位名稱，Value為Format
        public Dictionary<string, XSLXStyle_Com> ColumnFormat { get; set; }

        //顯示在WorkSheet第一欄的Header內容 Key:SheetName，Value: Header內容
        public Dictionary<string, string> WKSheetHeader { get; set; }

        //預留給樣板使用
        public enum Mode
        {
            Default =0,
            Customer =1
        }
        #endregion


        public XSLXFormat(Mode mode = Mode.Default)
        {
            HiddenColumns = new List<string>();
            ChangeHeaders = new Dictionary<string, string>();
            ColumnUnit = new Dictionary<string, string>();

            DefaultTypeFormat = new Dictionary<Type, XSLXStyle_Com>();
            ColumnFormat = new Dictionary<string, XSLXStyle_Com>();
            WKSheetHeader = new Dictionary<string, string>();


            if (mode == Mode.Default)
            {
                DefaultFormat();
            }
        }


        private void DefaultFormat()
        {
            #region Default Style: int
            XSLXStyle_Com iXSLXStyle = new XSLXStyle_Com();
            iXSLXStyle.FontAlignment = XSLXStyle_Com.eFontAlignment.Right;
            iXSLXStyle.NumberFormat = "@";
            DefaultTypeFormat.Add(typeof(int), iXSLXStyle);
            #endregion

            #region Default Style: Datetiime

            XSLXStyle_Com dtXSLXStyle = new XSLXStyle_Com();
            dtXSLXStyle.NumberFormat = "yyyy/mm/dd HH:MM:ss";
            DefaultTypeFormat.Add(typeof(DateTime), dtXSLXStyle);
            DefaultTypeFormat.Add(typeof(DateTime?), dtXSLXStyle);

            #endregion

            #region Default Style: double
            XSLXStyle_Com dbXSLXStyle = new XSLXStyle_Com();
            dbXSLXStyle.FontAlignment = XSLXStyle_Com.eFontAlignment.Right;
            DefaultTypeFormat.Add(typeof(double), dbXSLXStyle);
            DefaultTypeFormat.Add(typeof(double?), dbXSLXStyle);
            #endregion

            #region Default Style: string
            XSLXStyle_Com strXSLXStyle = new XSLXStyle_Com();
            strXSLXStyle.NumberFormat = "0";
            strXSLXStyle.FontAlignment = XSLXStyle_Com.eFontAlignment.Left;
            DefaultTypeFormat.Add(typeof(string), strXSLXStyle);
            #endregion

        }
    }
}
