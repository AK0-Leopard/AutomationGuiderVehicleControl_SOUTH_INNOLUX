using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewerObject
{
    public class ProjectInfo
    {
        public string Customer { get; private set; } = "";
        public string ProductLine { get; private set; } = "";
        public string MapInfoFolder { get; private set; } = "";
        public string ObjectConverter { get; private set; } = "";

        public bool IsSelected { get; set; } = false;

        public ProjectInfo(string customer, string productLine, string mapInfoFolder, string objectConverter, string sIsSelected = "N")
        {
            Customer = customer?.Trim() ?? "";
            ProductLine = productLine?.Trim() ?? "";
            MapInfoFolder = mapInfoFolder?.Trim() ?? "";
            ObjectConverter = objectConverter?.Trim() ?? "";
            IsSelected = string.IsNullOrWhiteSpace(sIsSelected) ? false :
                         sIsSelected.Trim() == "Y";
        }
    }
}
