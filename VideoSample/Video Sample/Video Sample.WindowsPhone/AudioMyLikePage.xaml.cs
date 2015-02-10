using Feature;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Video_Sample.Data;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
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
    public sealed partial class AudioMyLikePage : Page
    {
        private bool _isPressed = false;
        public AudioMyLikePage()
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
            InitList();
        }

        private void InitList()
        {
            List<ItemData> items = new List<ItemData>() { 
                new ItemData(){Name = "陈慧娴", Song = "千千阙歌", ImageSource = new BitmapImage(new Uri("ms-appx:///Assets/VideoSample/MediaIcon/千千阙歌.png",
                UriKind.Absolute))},
                new ItemData(){Name = "王菲", Song = "红豆", ImageSource = new BitmapImage(new Uri("ms-appx:///Assets/VideoSample/MediaIcon/红豆.png",
                UriKind.Absolute))},
                new ItemData(){Name = "梁静茹", Song = "可惜不是你", ImageSource = new BitmapImage(new Uri("ms-appx:///Assets/VideoSample/MediaIcon/可惜不是你.png",
                UriKind.Absolute))},
            };
            listBoxWoman.ItemsSource = items;

            List<ItemData> Womanitems = new List<ItemData>() { 
                new ItemData(){Name = "筷子兄弟", Song = "老男孩", ImageSource = new BitmapImage(new Uri("ms-appx:///Assets/VideoSample/MediaIcon/老男孩.png",
                UriKind.Absolute))},
                new ItemData(){Name = "林志炫", Song = "你的样子", ImageSource = new BitmapImage(new Uri("ms-appx:///Assets/VideoSample/MediaIcon/你的样子.png",
                UriKind.Absolute))},
                new ItemData(){Name = "陈奕迅", Song = "稳稳的幸福", ImageSource = new BitmapImage(new Uri("ms-appx:///Assets/VideoSample/MediaIcon/稳稳的幸福.png",
                UriKind.Absolute))},
            };
            listBox.ItemsSource = Womanitems;
        }

        private void Grid_Tapped(object sender, TappedRoutedEventArgs e)
        {
            
            AudioPlayInfo para = new AudioPlayInfo()
            {
                PlayType = PLAYTYPE.PlaySingle,
                Name = "",
                Song = "红豆.mp3",
                Path = "ms-appx:///Assets/VideoSample/Media/红豆.mp3",
                fileToken = null
            };
            Frame.Navigate(typeof(AudioPlay), para);
        }
    }

    public class ItemData
    {
        public string Name { get; set; }
        public string Song { get; set; }

        public BitmapImage ImageSource { get; set; }
    }
}
