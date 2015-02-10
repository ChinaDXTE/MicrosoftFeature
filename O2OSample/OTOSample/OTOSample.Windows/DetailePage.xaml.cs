using Feature;
using Feature.DataModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上有介绍

namespace OTOSample
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class DetailePage : Page
    {
        private SampleItem sampleItem = new SampleItem();
        private double price = 0;

        public DetailePage()
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
                this.price = this.sampleItem.PriceNum;
            }
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            if (Frame.CanGoBack)
                Frame.GoBack();
        }

        private void sub_Tapped(object sender, TappedRoutedEventArgs e)
        {
            int num = int.Parse(this.txtBoxNum.Text);
            if (num > 1)
            {
                this.updateNum(-1);
                int number = int.Parse(this.txtBoxNum.Text);
                if (number > 1)
                {
                    this.sub.Background = Application.Current.Resources["blocktNumBtnBackground"] as SolidColorBrush;
                }
                else
                {
                    this.sub.Background = Application.Current.Resources["blocktNumBtnNoBackground"] as SolidColorBrush;
                }
            }
        }

        private void add_Tapped(object sender, TappedRoutedEventArgs e)
        {
            int number = int.Parse(this.txtBoxNum.Text);
            if (number == 10)
            {
                Global.Current.Notifications.CreateToastNotifier("提示", "您最多可以买10份哦");
                return;
            }
            this.updateNum(1);
            this.sub.Background = Application.Current.Resources["blocktNumBtnBackground"] as SolidColorBrush;
        }

        private void updateNum(int num)
        {
            int number = int.Parse(this.txtBoxNum.Text);
            this.txtBoxNum.Text = (number + num).ToString();
            this.sampleItem.PriceNum += this.price * num;
        }

        private void Grid_Tapped(object sender, TappedRoutedEventArgs e)
        {
            try
            {
                Global.Current.Serialization.SaveOrder(this.sampleItem);
                Global.Current.Notifications.CreateToastNotifier("提示", "支付成功");
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

    }
}
