using NewsSample.Data;
using NewsSample.Common;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Windows.Input;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace NewsSample
{
    /// <summary>
    /// 显示单个组的概述的页，包括组内各项
    /// 的预览。
    /// </summary>
    public sealed partial class SectionPage : Page
    {
        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();

        public SectionPage()
        {
            this.InitializeComponent();
            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += this.NavigationHelper_LoadState;
        }

        /// <summary>
        /// 获取用于协助导航和进程生命期管理的 NavigationHelper。
        /// </summary>
        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }

        /// <summary>
        /// 获取 DefaultViewModel。可将其更改为强类型视图模型。
        /// </summary>
        public ObservableDictionary DefaultViewModel
        {
            get { return this.defaultViewModel; }
        }

        /// <summary>
        /// 使用在导航过程中传递的内容填充页。在从以前的会话
        /// 重新创建页时，也会提供任何已保存状态。
        /// </summary>
        /// <param name="sender">
        /// 事件的来源; 通常为 <see cref="NavigationHelper"/>
        /// </param>
        /// <param name="e">事件数据，其中既提供在最初请求此页时传递给
        /// <see cref="Frame.Navigate(Type, object)"/> 的导航参数，又提供
        /// 此页在以前会话期间保留的状态的
        /// 字典。 首次访问页面时，该状态将为 null。</param>
        private async void NavigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
            // TODO: 创建适用于问题域的合适数据模型以替换示例数据
            if ((string)e.NavigationParameter == "ReadList")
            {
                var group = await ReadListUtil.GetReadList();
                this.DefaultViewModel["Group"] = group;
                this.DefaultViewModel["Items"] = group.Items;
            }
            else if ((string)e.NavigationParameter == "FavList")
            {
                var group = await ReadListUtil.GetFavList();
                this.DefaultViewModel["Group"] = group;
                this.DefaultViewModel["Items"] = group.Items;
            }
            else
            {
                var group = await SampleDataSource.GetGroupAsync((string)e.NavigationParameter);
                this.DefaultViewModel["Group"] = group;
                this.DefaultViewModel["Items"] = group.Items;
            }
        }

        /// <summary>
        /// 在单击某个项时进行调用。
        /// </summary>
        /// <param name="sender">显示所单击项的 GridView。</param>
        /// <param name="e">描述所单击项的事件数据。</param>
        private void ItemView_ItemClick(object sender, ItemClickEventArgs e)
        {
            // 导航至相应的目标页，并
            // 通过将所需信息作为导航参数传入来配置新页
            var itemId = ((SampleDataItem)e.ClickedItem).UniqueId;

            ((SampleDataItem)e.ClickedItem).Read = true;
            ReadListUtil.SaveReadId(itemId);

            TileUitl.UpdateTile(((SampleDataItem)e.ClickedItem).Title);

            this.Frame.Navigate(typeof(ItemPage), itemId);
        }

        #region NavigationHelper 注册

        /// <summary>
        /// 此部分中提供的方法只是用于使
        /// NavigationHelper 可响应页面的导航方法。
        /// 应将页面特有的逻辑放入用于
        /// <see cref="Common.NavigationHelper.LoadState"/>
        /// 和 <see cref="Common.NavigationHelper.SaveState"/> 的事件处理程序中。
        /// 除了在会话期间保留的页面状态之外
        /// LoadState 方法中还提供导航参数。
        /// </summary>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedFrom(e);
        }

        #endregion
    }
}