using Feature;
using System;
using System.Diagnostics;
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

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkID=390556 上有介绍

namespace CameraSample
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class ScreenRecord : Page
    {
        private bool isPlay = false;

        public ScreenRecord()
        {
            this.InitializeComponent();
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
            this.mediaElement.Stop();
        }

        private async void AppbarRecord_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if ((App.Current as App).isRecoding)
            {
                await (App.Current as App).StopRecording();
                this.AppbarRecord.Label = "录屏";
            }
            else
            {
                (App.Current as App).StartRecording();
                this.AppbarRecord.Label = "停止录屏";
            }
        }

        private void Grid_PointerMoved(object sender, PointerRoutedEventArgs e)
        {
            var position = e.GetCurrentPoint(this._canvas).Position;
            position.Y = (position.Y < 0) ? 0 : position.Y;
            this._circle.SetValue(Canvas.LeftProperty, position.X);
            this._circle.SetValue(Canvas.TopProperty, position.Y);
        }

        private async void AppbarPaly_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if ((App.Current as App).isRecoding)
            {
                await (App.Current as App).StopRecording();
                this.AppbarRecord.Label = "录屏";
            }
            if (!this.isPlay)
            {
                string fileName = await "recording.mp4".Exist();
                if (!string.IsNullOrEmpty(fileName))
                {
                    var file = await ApplicationData.Current.LocalFolder.GetFileAsync("recording.mp4");
                    var stream = await file.OpenAsync(Windows.Storage.FileAccessMode.Read);
                    await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                    {
                        mediaElement.SetSource(stream, "");
                        mediaElement.Play();
                    });
                }
            }
            else
            {
                this.mediaElement.Stop();
                this.isPlay = false;
            }
        }

        private void mediaElement_CurrentStateChanged(object sender, RoutedEventArgs e)
        {
            if (this.mediaElement.CurrentState == MediaElementState.Stopped ||
                this.mediaElement.CurrentState == MediaElementState.Closed ||
                this.mediaElement.CurrentState == MediaElementState.Paused)
            {
                this.AppbarPaly.Icon = new SymbolIcon(Symbol.Play);
                this.AppbarPaly.Label = "播放";
                this.isPlay = false;
                this.mediaElement.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                this._canvas.Visibility = Windows.UI.Xaml.Visibility.Visible;
            }
            else if (this.mediaElement.CurrentState == MediaElementState.Opening ||
                this.mediaElement.CurrentState == MediaElementState.Playing)
            {
                this.AppbarPaly.Icon = new SymbolIcon(Symbol.Stop);
                this.AppbarPaly.Label = "停止播放";
                this.isPlay = true;
                this.mediaElement.Visibility = Windows.UI.Xaml.Visibility.Visible;
                this._canvas.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            }
        }

        public void StopPaly()
        {
            if (this.isPlay)
            {
                this.mediaElement.Stop();
                this.mediaElement.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                this._canvas.Visibility = Windows.UI.Xaml.Visibility.Visible;
                this.AppbarPaly.Icon = new SymbolIcon(Symbol.Play);
                this.isPlay = false;
            }
        }

        private async void AppbarSave_Tapped(object sender, TappedRoutedEventArgs e)
        {
            try
            {
                if ((App.Current as App).isRecoding)
                {
                    await (App.Current as App).StopRecording();
                    this.AppbarRecord.Label = "录屏";
                }
                if (this.isPlay)
                    this.StopPaly();
                string filepath = await "recording.mp4".Exist();
                if (!string.IsNullOrEmpty(filepath))
                {
                    var file = await ApplicationData.Current.LocalFolder.GetFileAsync("recording.mp4");
                    var buffer = await FileIO.ReadBufferAsync(file);
                    byte[] bytes = WindowsRuntimeBufferExtensions.ToArray(buffer, 0, (int)buffer.Length);
                    string fileName = DateTime.Now.Ticks.ToString() + ".mp4";
                    await bytes.Save(fileName, StorageTarget.cameraroll);
                    Global.Current.Notifications.CreateToastNotifier("提示", "保存文件成功");
                }
                else
                {
                    Global.Current.Notifications.CreateToastNotifier("提示", "请录屏");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        private async void AppbarShared_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if ((App.Current as App).isRecoding)
            {
                await (App.Current as App).StopRecording();
                this.AppbarRecord.Label = "录屏";
            }
            if (this.isPlay)
                this.StopPaly();
            string filepath = await "recording.mp4".Exist();
            if (!string.IsNullOrEmpty(filepath))
            {
                var file = await ApplicationData.Current.LocalFolder.GetFileAsync("recording.mp4");
                if (file != null)
                    file.RegisterForShare("视频文件", "录屏文件");
            }
            else
            {
                Global.Current.Notifications.CreateToastNotifier("提示", "请录屏");
            }
        }
    }
}
