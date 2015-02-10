using Feature;
using Feature.Background;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Video_Sample.Common;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上有介绍

namespace Video_Sample
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class AudioMainPage : Page
    {
        private MediaElement _mediaElement;
        private BackgroundDownload _download;
        public AudioMainPage()
        {
            this.InitializeComponent();
            this.DataContext = Global.Current.Globalization;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            Grid gridMedia = MainPage.Current.FindName("GridMedia") as Grid;
            gridMedia.Visibility = Visibility.Visible;

            _mediaElement = MainPage.Current.FindName("Scenario1MediaElement") as MediaElement;
        }

        private void ButtonBack_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Frame scenarioFrame = MainPage.Current.FindName("ScenarioFrame") as Frame;
            scenarioFrame.Navigate(typeof(Video_Sample.MainPage));
        }

        private void Scenario1MediaElement_DoubleTapped(object sender, Windows.UI.Xaml.Input.DoubleTappedRoutedEventArgs e)
        {
        }

        private void Grid_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Frame scenarioFrame = MainPage.Current.FindName("ScenarioFrame") as Frame;
            scenarioFrame.Navigate(typeof(AudioPage));
        }

        private async void MyMusicTapped(object sender, TappedRoutedEventArgs e)
        {
            Grid grid = sender as Grid;
            string tag = grid.Tag.ToString();
            Frame scenarioFrame = MainPage.Current.FindName("ScenarioFrame") as Frame;
            if (tag == "Download")
            {
                scenarioFrame.Navigate(typeof(DownloadPage));
            }
            else if (tag == "Local")
            {
                FileOpenPicker fileOpenPicker = new FileOpenPicker();

                // Filter to include a sample subset of file types
                fileOpenPicker.FileTypeFilter.Add(".wmv");
                fileOpenPicker.FileTypeFilter.Add(".mp4");
                fileOpenPicker.FileTypeFilter.Add(".mp3");
                fileOpenPicker.FileTypeFilter.Add(".wma");
                fileOpenPicker.SuggestedStartLocation = PickerLocationId.MusicLibrary;

                // Prompt user to select a file            
                StorageFile file = await fileOpenPicker.PickSingleFileAsync();

                // Ensure a file was selected
                if (file != null)
                {
                    IRandomAccessStream stream = await file.OpenAsync(FileAccessMode.Read);
                    Grid gridMedia = MainPage.Current.FindName("GridMedia") as Grid;
                    gridMedia.Visibility = Visibility.Visible;

                    _mediaElement = MainPage.Current.FindName("Scenario1MediaElement") as MediaElement;
                    _mediaElement.SetSource(stream, file.ContentType);
                }
            }
            else if (tag == "Like")
            {
                scenarioFrame.Navigate(typeof(AudioPage));
            }
        }
    }
}
