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

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上有介绍

namespace OTOSample
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            this.Loaded += MainPage_Loaded;
            this.NavigationCacheMode = NavigationCacheMode.Required;
        }

        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            this.Loaded -= MainPage_Loaded;
            this.cvs.Source = Global.Current.SampleItems.Data;
        }


        private void Grid_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (((Grid)sender).DataContext != null && ((Grid)sender).DataContext is SampleItem)
            {
                Frame.Navigate(typeof(DetailePage), ((Grid)sender).DataContext as SampleItem);
            }
        }

        private void Grid_Tapped_movie(object sender, TappedRoutedEventArgs e)
        {
            if (((Grid)sender).DataContext != null && ((Grid)sender).DataContext is SampleItem)
            {
                Frame.Navigate(typeof(DetaileMoviePage), ((Grid)sender).DataContext as SampleItem);
            }
        }

    }
}
