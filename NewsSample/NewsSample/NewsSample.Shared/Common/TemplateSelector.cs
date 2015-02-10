using NewsSample.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace NewsSample.Common
{
    public class TemplateSelector : DataTemplateSelector
    {
        /// <summary>
        /// 目录模板
        /// </summary>
        public DataTemplate CatalogTemplate { get; set; }
        /// <summary>
        /// 新闻模板
        /// </summary>
        public DataTemplate NewsTemplate { get; set; }
        /// <summary>
        /// 关于模板
        /// </summary>
        public DataTemplate AboutTemplate { get; set; }
        /// <summary>
        /// Hero Image模板
        /// </summary>
        public DataTemplate HeroTemplate { get; set; }
        /// <summary>
        /// 核心方法：根据不同的数据源类型返回给前台不同的样式模版
        /// </summary>
        /// <param name="item"></param>
        /// <param name="container"></param>
        /// <returns></returns>
        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
        {
            ZoomItem s = item as ZoomItem;
            if (item is SampleDataItem)
            {
                return NewsTemplate;
            }
            else if (item is SampleDataGroup)
            {
                return CatalogTemplate;
            }
            else if(item is HeroImageItem)
            {
                return HeroTemplate;
            }
            else
            {
                return AboutTemplate;
            }

        }
    }
}
