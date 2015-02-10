using Feature;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Pickers;
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
    public sealed partial class VideoMainPage : Page
    {
        public VideoMainPage()
        {
            this.InitializeComponent();
            this.DataContext = Global.Current.Globalization;
        }

        private void Grid_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Frame.Navigate(typeof(Video_Sample.VideoInfoPage), null);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            Grid gridMedia = MainPage.Current.FindName("GridMedia") as Grid;
            gridMedia.Visibility = Visibility.Collapsed;

            MediaElement mediaElement = MainPage.Current.FindName("Scenario1MediaElement") as MediaElement;
            MediaElementState temp = mediaElement.CurrentState;
            mediaElement.Pause();
            MediaElementState temp1 = mediaElement.CurrentState;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (Frame.CanGoBack)
                Frame.GoBack();
            
        }

        private async void Button_Click_1(object sender, RoutedEventArgs e)
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
                Frame.Navigate(typeof(Video_Sample.VideoPlayPage), file);
            }
        }
    }
}
