using Feature;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上提供

namespace CameraSample
{
    /// <summary>
    /// 可独立使用或用于导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            this.DataContext = Global.Current.Globalization;
            this.Loaded += MainPage_Loaded;
        }

        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            this.Loaded -= MainPage_Loaded;
            Global.Current.FilePicker.FilePickerFinish += (o, s) =>
            {
                if (o is StorageFile)
                {
                    var file = o as StorageFile;
                    var fileToken = Windows.Storage.AccessCache.StorageApplicationPermissions.FutureAccessList.Add(file);
                    Frame.Navigate(typeof(PictureEdit), fileToken);
                }
            };
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
             base.OnNavigatedTo(e);
        }

        private void gridCamera_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Frame.Navigate(typeof(PictureCapture));
        }

        private void gridPicker_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Global.Current.FilePicker.OpenFilePicker(FileType.pic);
        }

        private void gridSetting_Tapped(object sender, TappedRoutedEventArgs e)
        {
            (App.Current as App).ShowCustomSettingFlyout();
        }
    }
}
