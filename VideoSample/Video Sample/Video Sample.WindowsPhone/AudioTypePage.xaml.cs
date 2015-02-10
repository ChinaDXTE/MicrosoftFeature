using Feature;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Video_Sample.Data;
using Windows.Foundation;
using Windows.Foundation.Collections;
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
    public sealed partial class AudioTypePage : Page
    {
        public AudioTypePage()
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
            string para = e.Parameter.ToString();
            InitList();
        }
        private void InitList()
        {
            List<ItemData> items = new List<ItemData>() { 
                new ItemData(){Name = "邓紫棋", Song = "龙卷风"},
                new ItemData(){Name = "邓紫棋", Song = "你把我灌醉"},
                new ItemData(){Name = "邓紫棋", Song = "泡沫"},
                new ItemData(){Name = "金志文", Song = "我们结婚吧"},
                new ItemData(){Name = "张杰", Song = "夜空中最亮的星"}
            };
            listBox.ItemsSource = items;
        }

        private void listBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListBox list = sender as ListBox;
            ItemData item = list.SelectedItem as ItemData;
            int index = list.SelectedIndex;
            AudioPlayInfo para = new AudioPlayInfo()
            {
                PlayType = PLAYTYPE.PlaySingle,
                Name = item.Name,
                Song = "红豆.mp3",
                Path = "ms-appx:///Assets/VideoSample/Media/红豆.mp3",
                fileToken = null
            };
            Frame.Navigate(typeof(AudioPlay), para);
        }
    }
}
