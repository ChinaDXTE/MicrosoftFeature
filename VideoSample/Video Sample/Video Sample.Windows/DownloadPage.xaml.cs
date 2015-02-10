using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Networking.BackgroundTransfer;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上有介绍
using Video_Sample.Common;
using Feature.Background;
using Feature;

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

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            DownloadFile(true);
        }

        private void PauseButton_Click(object sender, RoutedEventArgs e)
        {
            DownloadFile(false);
        }

        private void SetButtonVisable(bool isDowloading)
        {
            if (isDowloading)
            {
                StartButton.Visibility = Visibility.Collapsed;
                PauseButton.Visibility = Visibility.Visible;
            }
            else
            {
                PauseButton.Visibility = Visibility.Collapsed;
                StartButton.Visibility = Visibility.Visible;
            }

        }

        private void DownloadFile(bool isBegin)
        {
            SetButtonVisable(isBegin);

            if (!isBegin)
            {
                StateTextBlock.Text = Global.Current.Globalization.Video_Pause;
                _download.PauseAll();
            }
            else
            {
                if (_download == null)
                {
                    var url = CommonString.URL;

                    _download = new BackgroundDownload();

                    _download.GetPercent += (current, total) =>
                    {
                        PBar.Value = current;

                        PercentTextBlock.Text = current + "%";
                        if (current >= 100)
                        {
                            SetButtonVisable(false);
                            StateTextBlock.Text = Global.Current.Globalization.Video_DownloadMessage;
                            _download = null;
                            StartButton.IsEnabled = false;
                        }
                    };

                    _download.ErrorException += (sender, s) =>
                    {
                        StateTextBlock.Text = Global.Current.Globalization.Video_DownloadError + "：" + s;
                    };

                    StateTextBlock.Text = Global.Current.Globalization.Video_Downloading;
                    PercentTextBlock.Text = 0 + "%";
                    PBar.Value = 0;
                    _download.StartDownload(url);
                }
                else
                {
                    StateTextBlock.Text = Global.Current.Globalization.Video_Downloading;
                    _download.ResumeAll();
                }
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            if (Frame.CanGoBack)
                Frame.GoBack();
        }
    }
}
