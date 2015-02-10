using Feature;
using Feature.Common;
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
using Windows.Storage;
// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上提供

namespace CameraSample
{
    /// <summary>
    /// 可独立使用或用于导航至 Frame 内部的空白页。
    /// </summary>
    public partial class MainPage : Page
    {
        protected NavigationHelper navigationHelper;
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
            this.DataContext = Global.Current.Globalization;
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

        /// <summary>
        /// 在此页将要在 Frame 中显示时进行调用。
        /// </summary>
        /// <param name="e">描述如何访问此页的事件数据。
        /// 此参数通常用于配置页。</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {

        }

        private void gridEdit_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Global.Current.FilePicker.OpenFilePicker(FileType.pic);
        }

        private void gridCamera_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Frame.Navigate(typeof(PictureCapture));
        }

        private void gridSetting_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Frame.Navigate(typeof(Setting));
        }

        private void gridScreenCapture_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Frame.Navigate(typeof(ScreenRecord));
        }


    }
}
