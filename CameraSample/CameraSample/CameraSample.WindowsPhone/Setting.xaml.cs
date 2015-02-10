using Feature;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkID=390556 上有介绍

namespace CameraSample
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class Setting : Page
    {
        private bool isLoaded = false;
        public Setting()
        {
            this.InitializeComponent();
            this.DataContext = Global.Current.Globalization;
            this.Loaded += Setting_Loaded;
        }

        /// <summary>
        /// 在此页将要在 Frame 中显示时进行调用。
        /// </summary>
        /// <param name="e">描述如何访问此页的事件数据。
        /// 此参数通常用于配置页。</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        private void Setting_Loaded(object sender, RoutedEventArgs e)
        {
            this.Loaded -= Setting_Loaded;
            this.loadOption();
            this.isLoaded = true;
        }

        private async void cmbLanguage_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.isLoaded)
            {
                if (this.cmbLanguage.SelectedIndex == 0)
                {
                    Global.Current.Globalization.camera_CurrentRegion = "EN";
                }
                else
                {
                    Global.Current.Globalization.camera_CurrentRegion = "CN";
                }
                await this.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Low, () =>
                {
                    Global.Current.Globalization.Updata();
                });
                this.loadOption();
            }
        }

        private void cmbPhotoPath_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.isLoaded)
            {
                if (this.cmbPhotoPath.SelectedIndex == 0)
                    Global.Current.Globalization.camera_CurrentLocationValue = "roll";
                else
                    Global.Current.Globalization.camera_CurrentLocationValue = "sd";
            }
        }

        private void menuClearMessage_Click(object sender, RoutedEventArgs e)
        {
            Global.Current.Notifications.ClearMainTile();
            Global.Current.Notifications.ClearToastNotifier();
        }

        private async void loadOption()
        {
            try
            {
                this.isLoaded = false;
                List<string> languages = new List<string>();
                languages.Add(Global.Current.Globalization.camera_English);
                languages.Add(Global.Current.Globalization.camera_Chinese);
                this.cmbLanguage.ItemsSource = languages;
                this.cmbLanguage.SelectedItem = Global.Current.Globalization.camera_CurrentLanguage;
                List<string> paths = new List<string>();
                paths.Add(Global.Current.Globalization.camera_PictureLibrary);
                paths.Add(Global.Current.Globalization.camera_SD);
                this.cmbPhotoPath.ItemsSource = paths;
                await Task.Delay(100);
                await this.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                {
                    this.cmbPhotoPath.SelectedItem = Global.Current.Globalization.camera_CurrentLocation;
                });
                this.isLoaded = true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
    }
}
