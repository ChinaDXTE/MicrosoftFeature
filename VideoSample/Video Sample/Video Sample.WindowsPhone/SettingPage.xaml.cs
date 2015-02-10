using Feature;
using System;
using System.Collections.Generic;
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

namespace Video_Sample
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class SettingPage : Page
    {
        private bool isLoaded = false;
        public SettingPage()
        {
            this.InitializeComponent();
            this.DataContext = Global.Current.Globalization;
        }

        /// <summary>
        /// 在此页将要在 Frame 中显示时进行调用。
        /// </summary>
        /// <param name="e">描述如何访问此页的事件数据。
        /// 此参数通常用于配置页。</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            SetComdoBox();
        }

        private async void SetComdoBox()
        {
            this.isLoaded = false;
            List<string> languages = new List<string>();
            languages.Add(Global.Current.Globalization.Video_English);
            languages.Add(Global.Current.Globalization.Video_Chinese);
            cmbLanguage.ItemsSource = languages;
            cmbLanguage.SelectedItem = Global.Current.Globalization.Video_CurrentLanguage;
            List<string> paths = new List<string>();
            paths.Add(Global.Current.Globalization.Video_MusicLibrary);
            paths.Add(Global.Current.Globalization.Video_SDCard);
            this.cmbPhotoPath.ItemsSource = paths;
            await this.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () => 
            {
                this.cmbPhotoPath.SelectedItem = Global.Current.Globalization.Video_DownloadPath;
            });            
            this.isLoaded = true;
        }
        private void cmbLanguage_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (isLoaded)
            {
                if (this.cmbLanguage.SelectedIndex == 0)
                {
                    Global.Current.Globalization.Video_CurrentRegion = "EN";
                }
                else
                {
                    Global.Current.Globalization.Video_CurrentRegion = "CN";
                }
                SetComdoBox();
            }
        }

        private void cmbPhotoPath_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string path = "ML";
            if (this.isLoaded)
            {
                if (this.cmbPhotoPath.SelectedIndex == 0)
                    path = "ML";
                else
                    path = "SD";

                Global.Current.LocalSettings.SaveData("DownloadPath", path);
            }
        }
    }
}
