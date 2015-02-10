using Feature;
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
using System.Collections.ObjectModel;
using Windows.UI;
using System.Diagnostics;
// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上有介绍

namespace OTOSample
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class DetaileMoviePage : Page
    {
        private SampleItem currentSampleItem = null;
        private SampleItem sampleItem = new SampleItem();
        private SampleItems sampleHall = new SampleItems(Feature.DataModel.Channel.hall);
        private ObservableCollection<SampleItem> listHallData = new ObservableCollection<SampleItem>();
        private ObservableCollection<string> listSeat = new ObservableCollection<string>();
        private double price = 0;

        public DetaileMoviePage()
        {
            this.InitializeComponent();
            this.NavigationCacheMode = Windows.UI.Xaml.Navigation.NavigationCacheMode.Required;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (e.NavigationMode == NavigationMode.New && e.Parameter != null && e.Parameter is SampleItem)
            {
                this.sampleItem = e.Parameter as SampleItem;
                this.pageTitle.Text = this.sampleItem.TitleGroup;
                this.DataContext = this.sampleItem;
                SampleItems sampleItems = new SampleItems(Feature.DataModel.Channel.cinema);
                this.listBusiness.ItemsSource = sampleItems.List;
                this.sampleHall.List[0].Option = "添加日程";
                this.listBusiness.SelectedIndex = 0;
                this.listHall.ItemsSource = this.listHallData;
                this.listViewSeat.ItemsSource = this.listSeat;
                this.getSeat();
            }
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            if (Frame.CanGoBack)
                Frame.GoBack();
        }

        private async void Button_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (((Button)sender).DataContext != null && ((Button)sender).DataContext is Feature.DataModel.SampleItem)
            {
                Feature.DataModel.SampleItem sampleItem = ((Button)sender).DataContext as Feature.DataModel.SampleItem;
                this.currentSampleItem = sampleItem;
                if (sampleItem.Option.Contains("选座"))
                {
                    this.clearSelect();
                    this.StoryexpansionSeat.Begin();
                }
                else
                {
                    var appointment = new Windows.ApplicationModel.Appointments.Appointment();
                    appointment.Subject = this.sampleItem.Title;
                    appointment.StartTime = new DateTimeOffset(DateTime.Now.AddSeconds(1));
                    appointment.Duration = TimeSpan.FromSeconds(1);
                    appointment.Location = "beijing";
                    Windows.UI.Xaml.Media.GeneralTransform buttonTransform = (sender as FrameworkElement).TransformToVisual(null);
                    Windows.Foundation.Point point = buttonTransform.TransformPoint(new Windows.Foundation.Point());
                    var rect = new Windows.Foundation.Rect(point, new Windows.Foundation.Size((sender as FrameworkElement).ActualWidth, (sender as FrameworkElement).ActualHeight));
                    await Windows.ApplicationModel.Appointments.AppointmentManager.ShowAddAppointmentAsync(appointment, rect, Windows.UI.Popups.Placement.Default);
                }
            }
        }

        private void listBusiness_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.listHallData.Clear();
            Random ra = new Random();
            int count = ra.Next(3,11);
            for (int i = 0; i < count; i++)
                this.listHallData.Add(this.sampleHall.List[i]);
        }

        private void AppbarBack_Click(object sender, RoutedEventArgs e)
        {
            this.currentSampleItem = null;
            this.txtPrice.Text = string.Empty;
            this.txtPrice.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            this.StorycollapsedSeat.Begin();
        }

        private void getSeat()
        {
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
                    Grid.SetColumn(grid, i - 1);
                    Grid.SetRow(grid, j - 1);
                    grid.Tag = j.ToString() + "排" + i.ToString() + "座";
                    this.gridSeat.Children.Add(grid);
                }
            }
        }

        private void clearSelect()
        {
            this.listSeat.Clear();
            foreach (UIElement element in this.gridSeat.Children)
            {
                if (element is Grid)
                {
                    (element as Grid).Background = Application.Current.Resources["blocktNumLineBackground"] as SolidColorBrush;
                }
            }
        }

        private void grid_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Grid grid = sender as Grid;
            if (this.listSeat.Contains(grid.Tag.ToString()))
            {
                this.listSeat.Remove(grid.Tag.ToString());
                this.price = this.currentSampleItem.PriceNum * this.listSeat.Count;
                this.txtPrice.Text = this.price + "元";
                grid.Background = Application.Current.Resources["blocktNumLineBackground"] as SolidColorBrush;
                if (this.listSeat.Count == 0)
                {
                    this.txtPrice.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                }
            }
            else
            {
                if (this.listSeat.Count < 4)
                {
                    this.listSeat.Add(grid.Tag.ToString());
                    this.txtPrice.Visibility = Windows.UI.Xaml.Visibility.Visible;
                    this.price = this.currentSampleItem.PriceNum * this.listSeat.Count;
                    this.txtPrice.Text = price.ToString() + "元";
                    grid.Background = new SolidColorBrush(Color.FromArgb(255, 3, 238, 25));
                }
                else
                {
                    Global.Current.Notifications.CreateToastNotifier("提示", "最多可选4个座位");
                }
            }
        }

        private void GridPay_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (this.listSeat == null || this.listSeat.Count == 0)
            {
                Global.Current.Notifications.CreateToastNotifier("提示", "请选择座位");
                return;
            }
            try
            {
                if (this.listBusiness.SelectedItem != null)
                {
                    SampleItem item = this.listBusiness.SelectedItem as SampleItem;
                    item.PriceNum = this.price;
                    Global.Current.Serialization.SaveOrder(item);
                    Global.Current.Notifications.CreateToastNotifier("提示", "支付成功");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

    }
}
