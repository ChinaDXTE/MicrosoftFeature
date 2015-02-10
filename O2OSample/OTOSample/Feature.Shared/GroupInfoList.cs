using Feature.DataModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Feature
{
    public class GroupInfoList
    {
        public string GroupTitle { get; set; }

        public List<SampleItem> ItemContent { get; set; }

    }
}
