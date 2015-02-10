using Feature;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Activation;
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
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkID=390556 上有介绍

namespace Video_Sample
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class VideoPage : Page
    {
        public VideoPage()
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
            List<VideoIndo> list = new List<VideoIndo>()
            {
                new VideoIndo(){name = "变形金刚",StarringName = "马克·沃尔伯格",DirectName="迈克尔·贝", num = "150.000", 
                    ImageSource = new BitmapImage(new Uri("ms-appx:///Assets/VideoSample/Video/变形金刚.png")),
                VideoName = Global.Current.Globalization.Video_VideoName,
                Direct = Global.Current.Globalization.Video_Direct,
                Starring = Global.Current.Globalization.Video_Starring,
                PlayCount = Global.Current.Globalization.Video_PlayCount,
                },

                new VideoIndo(){name = "哈利波特",StarringName = "Alfonso",DirectName="Daniel", num = "150.000", 
                    ImageSource = new BitmapImage(new Uri("ms-appx:///Assets/VideoSample/Video/哈利波特.png")),
                VideoName = Global.Current.Globalization.Video_VideoName,
                Direct = Global.Current.Globalization.Video_Direct,
                Starring = Global.Current.Globalization.Video_Starring,
                PlayCount = Global.Current.Globalization.Video_PlayCount,
                },
                
                new VideoIndo(){name = "少林寺",StarringName = "刘德华",DirectName="陈木胜", num = "150.000", 
                    ImageSource = new BitmapImage(new Uri("ms-appx:///Assets/VideoSample/Video/少林寺.png")),
                VideoName = Global.Current.Globalization.Video_VideoName,
                Direct = Global.Current.Globalization.Video_Direct,
                Starring = Global.Current.Globalization.Video_Starring,
                PlayCount = Global.Current.Globalization.Video_PlayCount,
                },
            };

            ListBox.ItemsSource = list;
        }

        private void Grid_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Frame.Navigate(typeof(VideoInfoPage));
        }

    }

    public class VideoIndo
    {
        public string num { get; set; }
        public string name { get; set; }
        public string DirectName { get; set; }
        public string StarringName { get; set; }

        public string VideoName { get; set; }
        public string Direct { get; set; }
        public string Starring { get; set; }
        public string PlayCount { get; set; }
        public BitmapImage ImageSource { get; set; }
    }
}
