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

namespace GeofenceApp
{
    /// <summary>
    /// 可独立使用或用于导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public static MainPage Current;

        public Frame ContentFrame
        {
            get
            {
                return this.FindName("ScenarioFrame") as Frame;
            }
        }

        public MainPage()
        {
            this.InitializeComponent();
            Current = this;
            this.DataContext = Global.Current.Globalization;
            ScenarioFrame.Navigated += ScenarioFrame_Navigated;
        }

        /// <summary>
        /// 在此页将要在 Frame 中显示时进行调用。
        /// </summary>
        /// <param name="e">描述如何访问此页的事件数据。
        /// 此参数通常用于配置页。</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            ScenarioFrame.Navigate(typeof(MapPage));
        }

        private void showGeofence_Click(object sender, RoutedEventArgs e)
        {
            //TODO:显示地理围栏列表页
            if (ScenarioFrame.CurrentSourcePageType.Equals(typeof(GeofenceListPage))) return;
            ScenarioFrame.Navigate(typeof(GeofenceListPage));
        }

        private void setting_Click(object sender, RoutedEventArgs e)
        {
            hideAppbar();
            new SettingPage().ShowIndependent();
        }

        void ScenarioFrame_Navigated(object sender, NavigationEventArgs e)
        {
            hideAppbar();
            if (ScenarioFrame.CurrentSourcePageType.Equals(typeof(MapPage)))
            {
                this.BottomAppBar.Visibility = Visibility.Visible;
                this.TopAppBar.Visibility = Visibility.Collapsed;
            }
            else
            {
                this.TopAppBar.Visibility = Visibility.Visible;
                this.BottomAppBar.Visibility = Visibility.Collapsed;
            }
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            if (ScenarioFrame.CanGoBack)
            {
                ScenarioFrame.GoBack();
            }
        }

        private void hideAppbar()
        {
            this.TopAppBar.IsOpen = false;
            this.BottomAppBar.IsOpen = false;
        }
    }
}
