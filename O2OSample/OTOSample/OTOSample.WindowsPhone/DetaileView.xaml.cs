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
using Feature.DataModel;
using Windows.ApplicationModel.Calls;
using Feature;
using Feature.Common;
// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkID=390556 上有介绍

namespace OTOSample
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class DetaileView : Page
    {
        private NavigationHelper navigationHelper;
        private SampleItem sampleItem = new SampleItem();
        private double price = 0;
        private WalletManage walletManage = new WalletManage();
        private ProximityManage proximityManage = new ProximityManage();

        public DetaileView()
        {
            this.InitializeComponent();
            this.navigationHelper = new NavigationHelper(this);
            this.Loaded += DetaileView_Loaded;
        }

        private void DetaileView_Loaded(object sender, RoutedEventArgs e)
        {
            this.Loaded -= DetaileView_Loaded;
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
                this.price = this.sampleItem.PriceNum;
            }
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedFrom(e);
            this.proximityManage.StopProDevice();
        }

        private void TextBlock_Tapped(object sender, TappedRoutedEventArgs e)
        {
            TextBlock txtBlock = sender as TextBlock;
            string num = txtBlock.Text;
            string name = this.txtTitle.Text;
            PhoneCallManager.ShowPhoneCallUI(num, name);
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

        private void AppbarQuick_Click(object sender, RoutedEventArgs e)
        {
            this.proximityManage.SubscribeForMessage();
        }

        private void AppbarBuy_Click(object sender, RoutedEventArgs e)
        {
            this.walletManage.Transaction(this.sampleItem.Title, this.sampleItem.PriceNum);
        }


    }
}
