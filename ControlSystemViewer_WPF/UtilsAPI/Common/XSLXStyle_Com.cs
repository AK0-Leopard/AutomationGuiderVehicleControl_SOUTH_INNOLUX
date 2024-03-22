using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using ClosedXML.Excel;

namespace UtilsAPI.Common
{
    public  class XSLXStyle_Com
    {
        //因為ClosedXml Style是封閉的，這邊創一支可以針對cell處理Style的Class

        public Color BackgroundColor { get; set; } =Color.Empty;//Cell底色
        public string NumberFormat { get; set; } = ""; //數值Format
        public eFontAlignment FontAlignment { get; set; } = eFontAlignment.Left;//數值Alignment

        public enum eFontAlignment
        {
            Left,
            Center,
            Right
        }


        public IXLStyle XSLXStyle_ComToIXLStyle( IXLStyle dest)
        {
            var destA = dest;
            if(BackgroundColor != Color.Empty)
            {
                destA.Fill.BackgroundColor= XLColor.FromColor(BackgroundColor);
            }

            if (NumberFormat !="")
            {
                destA.NumberFormat.Format = NumberFormat;
            }

            switch(FontAlignment)
            {
                case eFontAlignment.Right:
                    destA.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                    break;
                case eFontAlignment.Center:
                    destA.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    break;
                default:
                    destA.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                    break;

            }
            return destA;
           
        }
    }
}
