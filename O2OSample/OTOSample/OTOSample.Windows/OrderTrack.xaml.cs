using Feature;
using Feature.DataModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上有介绍

namespace OTOSample
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class OrderTrack : Page
    {
        private ObservableCollection<SampleItem> ListData = new ObservableCollection<SampleItem>();
        private SampleItem currentSampleItem = null;

        public OrderTrack()
        {
            this.InitializeComponent();
            this.listBusiness.ItemsSource = this.ListData;
            this.Loaded += OrderTrack_Loaded;
        }

        public async void OrderTrack_Loaded(object sender, RoutedEventArgs e)
        {
            this.Loaded -= OrderTrack_Loaded;
            List<SampleItem> listData = await Global.Current.Serialization.LoadOrder();
            this.ListData.Clear();
            for (int i = 0; i < listData.Count; i++)
            {
                this.ListData.Add(listData[i]);
            }
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            Global.Current.Serialization.SaveOrderList(this.ListData.ToList<SampleItem>());
        }

        private void listBusiness_Holding(object sender, HoldingRoutedEventArgs e)
        {
            if (e.HoldingState != HoldingState.Started) return;
            FrameworkElement element = sender as FrameworkElement;
            if (element == null) return;
            FrameworkElement senderElement = sender as FrameworkElement;
            FlyoutBase flyoutBase = FlyoutBase.GetAttachedFlyout(senderElement);
            if (flyoutBase != null)
            {
                this.currentSampleItem = (senderElement as Grid).DataContext as SampleItem;
                flyoutBase.ShowAt(senderElement);
            }
        }

        private void MenuFlyoutItem_Click(object sender, RoutedEventArgs e)
        {
            if (this.currentSampleItem != null)
                this.ListData.Remove(this.currentSampleItem);
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            if (Frame.CanGoBack)
                Frame.GoBack();
        }


    }
}
