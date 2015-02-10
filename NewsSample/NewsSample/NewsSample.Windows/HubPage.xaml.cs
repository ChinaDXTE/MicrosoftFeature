using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Collections.ObjectModel;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using NewsSample.Data;
using NewsSample.Common;
using Windows.ApplicationModel.Resources;
using System.ComponentModel;
using Windows.UI.Xaml.Media.Imaging;

// “通用中心应用程序”项目模板在 http://go.microsoft.com/fwlink/?LinkID=391955 上有介绍

namespace NewsSample
{
    /// <summary>
    /// 显示分组的项集合的页。
    /// </summary>
    public sealed partial class HubPage : Page
    {
        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();
        private readonly ResourceLoader resourceLoader = ResourceLoader.GetForCurrentView("Resources");


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

        public HubPage()
        {
            this.InitializeComponent();
            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += this.NavigationHelper_LoadState;

        }

        /// <summary>
        /// 使用在导航过程中传递的内容填充页。  在从以前的会话
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
            // TODO:  创建适用于问题域的合适数据模型以替换示例数据
            var sampleDataGroups = await SampleDataSource.GetGroupsAsync();
            this.DefaultViewModel["Groups"] = sampleDataGroups;

            ObservableCollection<ZoomItem> zoomList = new ObservableCollection<ZoomItem>();

            var hero = new List<HeroImageItem>();
            hero.Add(new HeroImageItem() { HeroTitle = this.resourceLoader.GetString("HeroTitle") });
            var about = new List<AboutItem>();
            about.Add(new AboutItem() { AboutString = this.resourceLoader.GetString("ReadListTitle"), tag = "ReadList" });
            about.Add(new AboutItem() { AboutString = this.resourceLoader.GetString("FavListTitle"), tag = "FavList" });

            zoomList.Add(new ZoomItem() { Key = "", ItemContent = hero });
            zoomList.Add(new ZoomItem() { Key = sampleDataGroups[0].Title, ItemContent = sampleDataGroups[0].Items });
            zoomList.Add(new ZoomItem() { Key = sampleDataGroups[1].Title, ItemContent = sampleDataGroups[1].Items });
            zoomList.Add(new ZoomItem() { Key = this.resourceLoader.GetString("HubSection3HeaderText"), ItemContent = sampleDataGroups });
            zoomList.Add(new ZoomItem() { Key = this.resourceLoader.GetString("HubSection4HeaderText"), ItemContent = about });
            this.itemcollectSource.Source = zoomList;

            //跳过封面图片的header
            outView.ItemsSource = itemcollectSource.View.CollectionGroups.Skip(1);
            
        }

        /// <summary>
        /// 在单击 HubSection 标题时调用。
        /// </summary>
        /// <param name="sender">包含单击了其标题的 HubSection 的中心。</param>
        /// <param name="e">描述如何启动单击的事件数据。</param>
        void Hub_SectionHeaderClick(object sender, HubSectionHeaderClickEventArgs e)
        {
            HubSection section = e.Section;
            var group = section.DataContext;
            if (group is System.Collections.ObjectModel.ObservableCollection<NewsSample.Data.SampleDataGroup>)
            {
                return;
            }
            this.Frame.Navigate(typeof(SectionPage), ((SampleDataGroup)group).UniqueId);
        }

        void OnHeaderClick(object sender, RoutedEventArgs e)
        {
            semanticZoom.IsZoomedInViewActive = false;
        }

        private void CatalogView_ItemClick(object sender, ItemClickEventArgs e)
        {
            var groupId = ((SampleDataGroup)e.ClickedItem).UniqueId;
            if (!Frame.Navigate(typeof(SectionPage), groupId))
            {
                throw new Exception(this.resourceLoader.GetString("NavigationFailedExceptionMessage"));
            }
        }

        /// <summary>
        /// 在单击节内的项时调用。
        /// </summary>
        /// <param name="sender">GridView 或 ListView
        /// 为 ListView)。</param>
        /// <param name="e">描述所单击项的事件数据。</param>
        void ItemView_ItemClick(object sender, ItemClickEventArgs e)
        {
            // 导航至相应的目标页，并
            // 通过将所需信息作为导航参数传入来配置新页
            if (e.ClickedItem is SampleDataItem)
            {
                var itemId = ((SampleDataItem)e.ClickedItem).UniqueId;

                ((SampleDataItem)e.ClickedItem).Read = true;
                ReadListUtil.SaveReadId(itemId);

                TileUitl.UpdateTile(((SampleDataItem)e.ClickedItem).Title);

                this.Frame.Navigate(typeof(ItemPage), itemId);
            }
            else if (e.ClickedItem is SampleDataGroup)
            {
                var groupId = ((SampleDataGroup)e.ClickedItem).UniqueId;
                if (!Frame.Navigate(typeof(SectionPage), groupId))
                {
                    throw new Exception(this.resourceLoader.GetString("NavigationFailedExceptionMessage"));
                }
            }
            else if (e.ClickedItem is HeroImageItem)
            {
                Frame.Navigate(typeof(ItemPage), "Group-3-Item-4");
            }
            else if (e.ClickedItem is AboutItem)
            {
                var tag = ((AboutItem)e.ClickedItem).tag;
                if (tag == "ReadList") 
                {
                    Frame.Navigate(typeof(SectionPage), "ReadList");
                }else if(tag=="FavList")
                {
                    Frame.Navigate(typeof(SectionPage), "FavList");
                }
            }
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
