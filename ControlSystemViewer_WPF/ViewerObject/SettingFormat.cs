using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewerObject
{
    public class SettingFormat
    {
        public List<SettingItem> SettingItems { get; set; }
    }
    public class SettingItem
    {
        public string Category { get; set; }
        public List<Item> Items { get; set; }
    }
    public class Item
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }
}
