using Feature;
using Feature.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkID=390556 上有介绍

namespace OTOSample
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class Channel : Page
    {
        private NavigationHelper navigationHelper;
        private Feature.DataModel.Channel channel = Feature.DataModel.Channel.business;

        public Channel()
        {
            this.InitializeComponent();
            this.navigationHelper = new NavigationHelper(this);
            this.NavigationCacheMode = NavigationCacheMode.Required;
            this.Loaded += Channel_Loaded;
        }

        private void Channel_Loaded(object sender, RoutedEventArgs e)
        {
            this.Loaded -= Channel_Loaded;
            this.DataContext = Global.Current.Globalization;
        }

        /// <summary>
        /// 在此页将要在 Frame 中显示时进行调用。
        /// </summary>
        /// <param name="e">描述如何访问此页的事件数据。
        /// 此参数通常用于配置页。</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedTo(e);
            if (e.Parameter != null && e.Parameter is Feature.DataModel.Channel && e.NavigationMode == NavigationMode.New)
            {
                this.listBusiness.ItemTemplate = this.Resources["listDataTemplate"] as DataTemplate;
                this.channel = (Feature.DataModel.Channel)e.Parameter;
                string title = string.Empty;
                switch (channel)
                {
                    case Feature.DataModel.Channel.movie:
                        this.listBusiness.ItemTemplate = this.Resources["listMovieDataTemplate"] as DataTemplate;
                        title = "电影";
                        break;
                    case Feature.DataModel.Channel.food:
                        title = "美食";
                        break;
                    case Feature.DataModel.Channel.hotel:
                        title = "酒店";
                        break;
                    case Feature.DataModel.Channel.entertainment:
                        title = "休闲娱乐";
                        break;
                    case Feature.DataModel.Channel.life:
                        title = "生活服务";
                        break;
                    case Feature.DataModel.Channel.beauty:
                        title = "丽人";
                        break;
                    case Feature.DataModel.Channel.traveling:
                        title = "旅游";
                        break;
                    case Feature.DataModel.Channel.shop:
                        title = "购物";
                        break;
                    case Feature.DataModel.Channel.gift:
                        title = "礼物";
                        break;
                    case Feature.DataModel.Channel.lottery:
                        title = "抽奖";
                        break;
                }
                this.txtTitle.Text = title;
                Global.Current.Globalization.LoadSampleData(channel);
                this.listBusiness.ScrollIntoView(Global.Current.Globalization.ListBusiness[0]);
            }
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedFrom(e);
        }

        private void listBusiness_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (this.listBusiness.SelectedItem != null && this.listBusiness.SelectedItem is Feature.DataModel.SampleItem)
            {
                Feature.DataModel.SampleItem sampleItem = this.listBusiness.SelectedItem as Feature.DataModel.SampleItem;
                if (this.channel == Feature.DataModel.Channel.movie)
                    Frame.Navigate(typeof(DetaileMovie), sampleItem);
                else
                    Frame.Navigate(typeof(DetaileView), sampleItem);
            }
        }
    }
}
