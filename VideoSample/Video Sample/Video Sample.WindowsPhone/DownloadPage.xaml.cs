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

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkID=390556 上有介绍
using Windows.Networking.BackgroundTransfer;
using Video_Sample.Common;
using Feature;
using Feature.Background;

namespace Video_Sample
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class DownloadPage : Page
    {
        private BackgroundDownload _download; 

        public DownloadPage()
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
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            if (_download != null)
            {
                _download.Cancel();
            }
        }
        

        private void Image_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Image im = sender as Image;
            bool isBegin = false;
            if (im.Name == "ImageStart")
            {
                this.ImageStart.Visibility = Visibility.Collapsed;
                this.ImagePauset.Visibility = Visibility.Visible;
                isBegin = true;
            }
            else
            {
                this.ImageStart.Visibility = Visibility.Visible;
                this.ImagePauset.Visibility = Visibility.Collapsed;
            }

            if (!isBegin)
            {
                _download.PauseAll();
            }
            else
            {
                if (_download == null)
                {
                    string url = CommonString.URL;

                    _download = new BackgroundDownload();

                    _download.GetPercent += (current, total) =>
                    {
                        PBarDownload.Value = current;

                        TextBlockPercent.Text = current + "%";
                        if (current >= 100)
                        {
                            TextBlockState.Text = "下载完成";
                            _download = null;

                            MyToast.ShowToast(" ", Global.Current.Globalization.Video_DownloadMessage);
                        }
                    };

                    _download.ErrorException += (o, s) =>
                    {
                        TextBlockState.Text = "下载错误 " + s;
                    };

                    TextBlockState.Text = "下载进度";
                    TextBlockPercent.Text = 0 + "%";
                    PBarDownload.Value = 0;
                    _download.StartDownload(url);
                }
                else
                {
                    TextBlockState.Text = Global.Current.Globalization.Video_Downloading;
                    _download.ResumeAll();
                }
            }
        }
    }
}
