using Nokia.Graphics.Imaging;
using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI;

namespace Feature.Filter
{
    public class FilterItems
    {
        private List<FilterItem> listFilter = new List<FilterItem>();
        public List<FilterItem> ListFilter
        {
            get { return this.listFilter; }
        }

        public FilterItems()
        {
            this.listFilter.Add(new FilterItem("Original","原图",null,true));
            this.listFilter.Add(new FilterItem("Antique","怀旧", new AntiqueFilter()));
            this.listFilter.Add(new FilterItem("AutoLevels","现代", new AutoLevelsFilter()));
            this.listFilter.Add(new FilterItem("Sketch","素描", new SketchFilter(SketchMode.Color)));
            this.listFilter.Add(new FilterItem("Sepia","复古", new SepiaFilter()));
        }

        public void Select(int index)
        {
            for (int i = 0; i < this.listFilter.Count; i++)
            {
                if (i == index)
                    this.listFilter[i].IsSelect = true;
                else
                    this.listFilter[i].IsSelect = false;
            }
        }

    }
}
