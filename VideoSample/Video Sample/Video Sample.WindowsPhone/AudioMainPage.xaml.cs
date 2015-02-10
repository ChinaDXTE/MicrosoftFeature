using Feature;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Video_Sample.Data;
using Windows.ApplicationModel.Activation;
using Windows.Data.Xml.Dom;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Media.Playback;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Core;
using Windows.UI.Notifications;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkID=390556 上有介绍

namespace Video_Sample
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class AudioMainPage : Page, IFileOpenPickerContinuable
    {
        public AudioMainPage()
        {
            this.InitializeComponent();
            this.DataContext = Global.Current.Globalization;
            InitList();
            InitRanking();
        }


        protected override void OnNavigatedTo(NavigationEventArgs e)
        {

            PlayStatus();
            BackgroundMediaPlayer.Current.CurrentStateChanged -= this.MediaPlayer_CurrentStateChanged;
            BackgroundMediaPlayer.Current.CurrentStateChanged += this.MediaPlayer_CurrentStateChanged;
        }

        private void InitList()
        {
            List<ItemData> list = new List<ItemData>() { 
                new ItemData(){Name = "魏晨", Song = "美丽的谎言"},
                new ItemData(){Name = "付辛博", Song = "为爱放手"},
                new ItemData(){Name = "吴奇隆", Song = "寒冬"},
                new ItemData(){Name = "潘美辰", Song = "你管不着"},
                new ItemData(){Name = "星弟", Song = "同归于尽"},
                new ItemData(){Name = "陈浩民", Song = "起点"},
            };
            listView.ItemsSource = list;
        }

        public void InitRanking()
        {
            List<RrankingItemData> items = new List<RrankingItemData>() { 
                new RrankingItemData(){Title = "热歌排行榜", Intro = "热门歌曲权威发布", ImageSource = new BitmapImage(new Uri("ms-appx:///Assets/VideoSample/MediaIcon/bangdan7.png",
                UriKind.Absolute))},
                new RrankingItemData(){Title = "下载榜", Intro = "歌曲下载量排行榜", ImageSource = new BitmapImage(new Uri("ms-appx:///Assets/VideoSample/MediaIcon/bangdan10.png",
                UriKind.Absolute))},
                new RrankingItemData(){Title = "新歌榜", Intro = "引领新歌潮流风向标", ImageSource = new BitmapImage(new Uri("ms-appx:///Assets/VideoSample/MediaIcon/bangdan11.png",
                UriKind.Absolute))},
                new RrankingItemData(){Title = "黑马榜", Intro = "飙升最快的歌曲排行榜", ImageSource = new BitmapImage(new Uri("ms-appx:///Assets/VideoSample/MediaIcon/bangdan8.png",
                UriKind.Absolute))},
                new RrankingItemData(){Title = "欧美榜", Intro = "欧美流行音乐", ImageSource = new BitmapImage(new Uri("ms-appx:///Assets/VideoSample/MediaIcon/bangdan12.png",
                UriKind.Absolute))},
            };
            listViewRanking.ItemsSource = items;
        }
        private void listBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Windows.UI.Xaml.Controls.ListView list = sender as Windows.UI.Xaml.Controls.ListView;
            ItemData item = list.SelectedItem as ItemData;
            if (item == null)
                return;

            AudioPlayInfo para = new AudioPlayInfo()
            {
                PlayType = PLAYTYPE.PlaySingle,
                Name = item.Name,
                Song = item.Song + ".mp3",
                Path = "ms-appx:///Assets/VideoSample/Media/红豆.mp3",
                fileToken = null
            };
            Frame.Navigate(typeof(AudioPlay), para);
        }

        private void Loacl_Tapped(object sender, TappedRoutedEventArgs e)
        {
            FileOpenPicker openPicker = new FileOpenPicker();
            openPicker.ViewMode = PickerViewMode.Thumbnail;
            openPicker.SuggestedStartLocation = PickerLocationId.MusicLibrary;
            openPicker.FileTypeFilter.Add(".mp3");
            openPicker.FileTypeFilter.Add(".wma");
            openPicker.FileTypeFilter.Add(".wav");

            openPicker.PickSingleFileAndContinue();
        }

        public async void ContinueFileOpenPicker(FileOpenPickerContinuationEventArgs args)
        {
            if (args.Files.Count > 0)
            {
                var file = args.Files[0] as StorageFile;
                await this.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                {
                    var value = new ValueSet();
                    var fileToken = Windows.Storage.AccessCache.StorageApplicationPermissions.FutureAccessList.Add(file);
                    AudioPlayInfo para = new AudioPlayInfo()
                    {
                        PlayType = PLAYTYPE.PlayLoacl,
                        Name = "",
                        Song = args.Files[0].DisplayName,
                        Path = null,
                        fileToken = fileToken
                    };
                    Frame.Navigate(typeof(AudioPlay), para);
                });
            }
        }

        private void Download_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Frame.Navigate(typeof(DownloadPage));
        }

        private void MyLike_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Frame.Navigate(typeof(AudioMyLikePage));
        }

        private void Grid_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Grid grid = sender as Grid;
            string index = grid.Tag.ToString();
            Global.Current.Globalization.AudioTypeIndex = index;
            Frame.Navigate(typeof(AudioTypePage), index);
        }

        private void GridRecommend_Tapped(object sender, TappedRoutedEventArgs e)
        {
            AudioPlayInfo para = new AudioPlayInfo()
            {
                PlayType = PLAYTYPE.PlaySingle,
                Name = "",
                Song = "红豆" + ".mp3",
                Path = "ms-appx:///Assets/VideoSample/Media/红豆.mp3",
                fileToken = null
            };
            Frame.Navigate(typeof(AudioPlay), para);
        }

        private void GridRanking_Tapped(object sender, TappedRoutedEventArgs e)
        {
            string index = "1";
            Frame.Navigate(typeof(AudioTypePage), index);
        }

        private void AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            AppBarButton abb = sender as AppBarButton;
            var value = new ValueSet();
            switch (abb.Tag.ToString())
            {
                case "previous":
                    value.Add("skipprevious", "");
                    BackgroundMediaPlayer.SendMessageToBackground(value);
                    break;
                case "next":
                    value.Add("skipnext", "");
                    BackgroundMediaPlayer.SendMessageToBackground(value);
                    break;
                case "PlayPage":
                    Frame.Navigate(typeof(AudioPlay));
                    break;
            }
        }

        private void PlayStatus()
        {
            var temp = BackgroundMediaPlayer.Current;

            if (AudioPlay.IsMyBackgroundTaskRunning)
            {
                if (MediaPlayerState.Playing == BackgroundMediaPlayer.Current.CurrentState)
                {
                    //正在播放，
                    SetPlayButtonState(true);
                }
                else
                {
                    SetPlayButtonState(false);
                }
            }
            else
            {
                SetPlayButtonState(false);
            }
        }

        private void AppBarButtonPlay_Click(object sender, RoutedEventArgs e)
        {
            if (AudioPlay.IsMyBackgroundTaskRunning)
            {
                if (MediaPlayerState.Playing == BackgroundMediaPlayer.Current.CurrentState)
                {
                    //正在播放，播放暂停
                    BackgroundMediaPlayer.Current.Pause();
                    //控件状态 显示播放
                    SetPlayButtonState(false);
                }
                else if (MediaPlayerState.Paused == BackgroundMediaPlayer.Current.CurrentState)
                {
                    BackgroundMediaPlayer.Current.Play();
                    //控件状态 显示暂停
                    SetPlayButtonState(true);
                }
                else if (MediaPlayerState.Closed == BackgroundMediaPlayer.Current.CurrentState)
                {
                    var message = new ValueSet();
                    message.Add("playsinglefile", "ms-appx:///Assets/VideoSample/Media/红豆.mp3");
                    BackgroundMediaPlayer.SendMessageToBackground(message);
                    //控件状态 显示暂停
                    SetPlayButtonState(true);
                }
            }
            else
            {
                MyToast.ShowToast("提示", "暂无音乐播放，请先选一首歌曲");
            }
        }
        private void SetPlayButtonState(bool flag)
        {
            BitmapIcon bIcon = new BitmapIcon();
            if (flag)
            {
                Global.Current.Globalization.IPlaying = true;
                bIcon.UriSource = new Uri("ms-appx:///Assets/VideoSample/Play/pause.png", UriKind.Absolute);
                AppBarButtonPlay.Icon = bIcon;
            }
            else
            {
                Global.Current.Globalization.IPlaying = false;
                bIcon.UriSource = new Uri("ms-appx:///Assets/VideoSample/Play/play.png", UriKind.Absolute);
                AppBarButtonPlay.Icon = bIcon;
            }
        }

        public void App_Resuming()
        {
            PlayStatus();
        }

        async void MediaPlayer_CurrentStateChanged(MediaPlayer sender, object args)
        {
            switch (sender.CurrentState)
            {
                case MediaPlayerState.Playing:
                    await this.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                    {
                        SetPlayButtonState(true);
                        AppBarButtonPrevious.IsEnabled = true;
                        AppBarButtonNext.IsEnabled = true;
                    }
                        );
                    break;
                case MediaPlayerState.Paused:
                    await this.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                    {
                        SetPlayButtonState(false);
                    }
                    );
                    break;
            }
        }
    }
    public class RrankingItemData
    {
        public string Intro { get; set; }
        public string Title { get; set; }

        public BitmapImage ImageSource { get; set; }
    }
}
