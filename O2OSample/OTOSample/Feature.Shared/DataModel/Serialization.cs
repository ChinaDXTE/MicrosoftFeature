using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.IO;

namespace Feature.DataModel
{
    public class Serialization
    {
        private const string fileName = "sampleItems.json";

        public async void SaveOrder(SampleItem sampleItem)
        {
            string content = await fileName.GetContent();
            SampleItems sampleItems = new SampleItems(content);
            sampleItems.List.Add(sampleItem);
            string result = sampleItems.Organizate();
            await result.Save(fileName);
        }

        public async void SaveOrderList(List<SampleItem> listItem)
        {
            SampleItems sampleItems = new SampleItems();
            sampleItems.List = listItem;
            string result = sampleItems.Organizate();
            await result.Save(fileName);
        }

        public async Task<List<SampleItem>> LoadOrder()
        {
            List<SampleItem> listData = new List<SampleItem>();
            string content = await fileName.GetContent();
            SampleItems sampleItems = new SampleItems(content);
            listData.AddRange(sampleItems.List);
            return listData;
        }

    }
}
