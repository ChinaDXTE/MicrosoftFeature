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

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=391641 上有介绍

namespace OTOSample
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private NavigationHelper navigationHelper;
        private WalletManage walletManage = new WalletManage();

        public MainPage()
        {
            this.InitializeComponent();
            this.Loaded += MainPage_Loaded;
            this.navigationHelper = new NavigationHelper(this);
            this.NavigationCacheMode = NavigationCacheMode.Required;
        }

        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            this.Loaded -= MainPage_Loaded;
            Global.Current.Globalization.LoadSampleData(Feature.DataModel.Channel.business);
            this.DataContext = Global.Current.Globalization;
            this.walletManage.CreateWallet();
        }

        private void Grid_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Grid grid = sender as Grid;
            Feature.DataModel.Channel channel = Feature.DataModel.Channel.business;
            switch (grid.Tag.ToString())
            {
                case "movie":
                    channel = Feature.DataModel.Channel.movie;
                    break;
                case "food":
                    channel = Feature.DataModel.Channel.food;
                    break;
                case "hotel":
                    channel = Feature.DataModel.Channel.hotel;
                    break;
                case "entertainment":
                    channel = Feature.DataModel.Channel.entertainment;
                    break;
                case "life":
                    channel = Feature.DataModel.Channel.life;
                    break;
                case "beauty":
                    channel = Feature.DataModel.Channel.beauty;
                    break;
                case "traveling":
                    channel = Feature.DataModel.Channel.traveling;
                    break;
                case "shop":
                    channel = Feature.DataModel.Channel.shop;
                    break;
                case "gift":
                    channel = Feature.DataModel.Channel.gift;
                    break;
                case "lottery":
                    channel = Feature.DataModel.Channel.lottery;
                    break;
            }
            Frame.Navigate(typeof(Channel),channel);
        }

        private void list_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (((Grid)sender).DataContext != null && ((Grid)sender).DataContext is Feature.DataModel.SampleItem)
            {
                Feature.DataModel.SampleItem sampleItem = ((Grid)sender).DataContext as Feature.DataModel.SampleItem;
                Frame.Navigate(typeof(DetaileView), sampleItem);
            }
        }

        private async void btnWallet_Click(object sender, RoutedEventArgs e)
        {
            await this.walletManage.ShowWalletItemAsync();
        }

        private void btnClearMessage_Click(object sender, RoutedEventArgs e)
        {
            Global.Current.Notifications.ClearMainTile();
            Global.Current.Notifications.ClearToastNotifier();
        }

        private void btnShareContract_Click(object sender, RoutedEventArgs e)
        {
            "http://msdn.microsoft.com/".RegisterForShare("邀请", "我在这里学习");
        }

        private void MenuFlyoutItem_Click(object sender, RoutedEventArgs e)
        {
            var language = (sender as MenuFlyoutItem).Tag.ToString();
            Global.Current.Globalization.o2o_CurrentRegion = language;
        }

        private void btnOrderTrack_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(OrderTrack));
        }

    }
}
