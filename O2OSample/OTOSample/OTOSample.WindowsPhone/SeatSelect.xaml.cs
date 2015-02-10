using Feature;
using Feature.Common;
using Feature.DataModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
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
    public sealed partial class SeatSelect : Page
    {
        private NavigationHelper navigationHelper;
        private List<SampleItem> listInfo = new List<SampleItem>();
        private ObservableCollection<string> listSeat = new ObservableCollection<string>();
        private double price = 0;
        private WalletManage walletManage = new WalletManage();
        private ProximityManage proximityManage = new ProximityManage();

        public SeatSelect()
        {
            this.InitializeComponent();
            this.navigationHelper = new NavigationHelper(this);
            this.NavigationCacheMode = NavigationCacheMode.Required;
            this.Loaded += SeatSelect_Loaded;
        }

        private void SeatSelect_Loaded(object sender, RoutedEventArgs e)
        {
            this.Loaded -= SeatSelect_Loaded;
            this.listViewSeat.ItemsSource = this.listSeat;
            this.scrollView.ChangeView(300, null, null);
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
                if (listInfo != null && listInfo.Count == 3)
                {
                    this.txtTitle.Text = listInfo[1].Title;
                    this.txtMovie.Text = listInfo[0].Title;
                    this.txtMovieTime.Text = DateTime.Now.ToString("yyyy-MM-dd ") + listInfo[2].BeginTime;
                }
            }
            this.getSeat();
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedFrom(e);
            this.proximityManage.StopProDevice();
        }

        private void AppbarQuick_Click(object sender, RoutedEventArgs e)
        {
            this.proximityManage.SubscribeForMessage();
        }

        private void AppbarBuy_Click(object sender, RoutedEventArgs e)
        {
            if (this.price > 0)
            {
                string des = this.txtTitle.Text;
                this.walletManage.Transaction(des, this.price);
            }
        }

        private void getSeat()
        {
            this.listSeat.Clear();
            this.gridSeat.Children.Clear();
            int colNum = 20;
            int rowNum = 20;
            for (int i = 1; i < colNum; i++)
            {
                this.gridSeat.ColumnDefinitions.Add(new ColumnDefinition());
                for (int j = 1; j < rowNum; j++)
                {
                    this.gridSeat.RowDefinitions.Add(new RowDefinition());
                    Grid grid = new Grid();
                    grid.Margin = new Thickness(10);
                    grid.Width = 38;
                    grid.Height = 38;
                    grid.Background = Application.Current.Resources["blocktNumLineBackground"] as SolidColorBrush;
                    grid.Tapped += grid_Tapped;
                    Grid.SetColumn(grid, i-1);
                    Grid.SetRow(grid,j-1);
                    grid.Tag = j.ToString() + "排" + i.ToString() + "座";
                    this.gridSeat.Children.Add(grid);
                }
            }
        }

        private void grid_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Grid grid = sender as Grid;
            if (this.listSeat.Contains(grid.Tag.ToString()))
            {
                this.listSeat.Remove(grid.Tag.ToString());
                this.price = this.listInfo[1].PriceNum * this.listSeat.Count;
                this.txtPrice.Text = price.ToString() + "元";
                grid.Background = Application.Current.Resources["blocktNumLineBackground"] as SolidColorBrush;
                if (this.listSeat.Count == 0)
                {
                    this.gridPriceInfo.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                    this.gridTip.Visibility = Windows.UI.Xaml.Visibility.Visible;
                }
            }
            else
            {
                if (this.listSeat.Count < 4)
                {
                    this.listSeat.Add(grid.Tag.ToString());
                    this.price = this.listInfo[1].PriceNum * this.listSeat.Count;
                    this.txtPrice.Text = price.ToString() + "元";
                    grid.Background = new SolidColorBrush(Color.FromArgb(255,3,238,25));
                    this.gridPriceInfo.Visibility = Windows.UI.Xaml.Visibility.Visible;
                    this.gridTip.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                }
                else
                {
                    Global.Current.Notifications.CreateToastNotifier("提示","最多可选4个座位");
                }
            }
        }

    }
}
