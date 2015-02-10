using Feature;
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
    public sealed partial class Setting : SettingsFlyout
    {
        public Setting()
        {
            this.InitializeComponent();
            this.DataContext = Global.Current.Globalization;
        }

        private void btnShard_Tapped(object sender, TappedRoutedEventArgs e)
        {
            "http://msdn.microsoft.com/".RegisterForShare("邀请", "我在这里学习");
        }

        private void btnOrderTrack_Tapped(object sender, TappedRoutedEventArgs e)
        {
            (Window.Current.Content as Frame).Navigate(typeof(OrderTrack));
        }


    }
}
