using Feature;
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
    public sealed partial class DetaileHall : Page
    {
        private NavigationHelper navigationHelper;
        private SampleItems sampleItems = new SampleItems(Feature.DataModel.Channel.hall);
        private List<SampleItem> listInfo = new List<SampleItem>();
        private CalendarManage calendarManage = new CalendarManage();

        public DetaileHall()
        {
            this.InitializeComponent();
            this.navigationHelper = new NavigationHelper(this);
            this.NavigationCacheMode = NavigationCacheMode.Required;
            this.Loaded += DetaileHall_Loaded;
        }

        private void DetaileHall_Loaded(object sender, RoutedEventArgs e)
        {
            this.Loaded -= DetaileHall_Loaded;
            this.sampleItems.List[0].Option = "添加日程";
            this.listBusiness.ItemsSource = this.sampleItems.List;
        }

        /// <summary>
        /// 在此页将要在 Frame 中显示时进行调用。
        /// </summary>
        /// <param name="e">描述如何访问此页的事件数据。
        /// 此参数通常用于配置页。</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedTo(e);
            if (e.Parameter != null && e.Parameter is List<SampleItem> && e.NavigationMode == NavigationMode.New)
            {
                this.listInfo = (List<SampleItem>)e.Parameter;
                if (listInfo != null && listInfo.Count == 2)
                {
                    this.txtTitle.Text = listInfo[1].Title;
                }
            }
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedFrom(e);
        }

        private async void Button_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (((Button)sender).DataContext != null && ((Button)sender).DataContext is Feature.DataModel.SampleItem)
            {
                Feature.DataModel.SampleItem sampleItem = ((Button)sender).DataContext as Feature.DataModel.SampleItem;
                if (sampleItem.Option.Contains("选座"))
                {
                    if (this.listInfo.Count < 3)
                        this.listInfo.Add(sampleItem);
                    else
                        this.listInfo[2] = sampleItem;
                    Frame.Navigate(typeof(SeatSelect), listInfo);
                }
                else
                {
                    await this.calendarManage.CreateNewAppointment(this.listInfo[0].Title, new DateTimeOffset(DateTime.Now.AddSeconds(1)), TimeSpan.FromSeconds(1), "bj");
                }
            }
        }

    }
}
