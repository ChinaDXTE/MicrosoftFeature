using Feature.Common;
using Feature.DataModel;
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
    public sealed partial class DetaileMovie : Page
    {
        private NavigationHelper navigationHelper;
        private SampleItem sampleItem = new SampleItem();
        private List<SampleItem> listInfo = new List<SampleItem>();

        public DetaileMovie()
        {
            this.InitializeComponent();
            this.navigationHelper = new NavigationHelper(this);
            this.NavigationCacheMode = NavigationCacheMode.Required;
            this.Loaded += DetaileMovie_Loaded;
        }

        private void DetaileMovie_Loaded(object sender, RoutedEventArgs e)
        {
            this.Loaded -= DetaileMovie_Loaded;
            this.DataContext = this.sampleItem;
        }

        /// <summary>
        /// 在此页将要在 Frame 中显示时进行调用。
        /// </summary>
        /// <param name="e">描述如何访问此页的事件数据。
        /// 此参数通常用于配置页。</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedTo(e);
            if (e.Parameter != null && e.Parameter is SampleItem && e.NavigationMode == NavigationMode.New)
            {
                this.sampleItem.Update(e.Parameter as SampleItem);

                TimeSpan period = TimeSpan.FromMilliseconds(10);

                Windows.System.Threading.ThreadPoolTimer.CreateTimer(async (source) =>
                {
                    await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, new Windows.UI.Core.DispatchedHandler(() =>
                    {
                        this.scrollViewer.ChangeView(null, 0.0, null);
                    }));
                }, period);

                SampleItems sampleItems = new SampleItems(Feature.DataModel.Channel.cinema);
                this.listBusiness.ItemsSource = sampleItems.List;
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
                this.listInfo.Clear();
                listInfo.Add(this.sampleItem);
                listInfo.Add(sampleItem);
                Frame.Navigate(typeof(DetaileHall),listInfo);
            }
        }
    }
}
