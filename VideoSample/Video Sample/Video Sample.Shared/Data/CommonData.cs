using System;
using System.Collections.Generic;
using System.Text;

namespace Video_Sample.Data
{
    public class CommonData
    {
        public static List<ItemData> HomeList = new List<ItemData>()
            {
                new ItemData(){itemName = "音频"},
                new ItemData(){itemName = "视频"},
                new ItemData(){itemName = "下载"}
            };
    }

    public class ItemData
    {
        public string itemName { get; set; }
    }
}
