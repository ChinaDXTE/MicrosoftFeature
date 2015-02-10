using Feature.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace OTOSample
{
    public class TemplateSelector : DataTemplateSelector
    {
        /// <summary>
        /// 电影显示模版
        /// </summary>
        public DataTemplate MovieTemplate { get; set; }
        /// <summary>
        /// 其他频道模版
        /// </summary>
        public DataTemplate OtherTemplate { get; set; }
        /// <summary>
        /// 核心方法：根据不同的数据源类型返回给前台不同的样式模版
        /// </summary>
        /// <param name="item"></param>
        /// <param name="container"></param>
        /// <returns></returns>
        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
        {
            SampleItem model = item as SampleItem;
            if (!string.IsNullOrEmpty(model.Director))
            {
                return MovieTemplate;
            }
            else
            {
                return OtherTemplate;
            }
        }
    }
}
