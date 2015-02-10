using Feature;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Media;
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
    public sealed partial class AudioPage : Page
    {
        private MediaElement _mediaElement;
        public AudioPage()
        {
            this.InitializeComponent();
            InitList();

            this.DataContext = Global.Current.Globalization;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            Grid gridMedia = MainPage.Current.FindName("GridMedia") as Grid;
            gridMedia.Visibility = Visibility.Visible;

            _mediaElement = MainPage.Current.FindName("Scenario1MediaElement") as MediaElement;
        }

        private void InitList()
        {
            List<ItemData> lists = new List<ItemData>()
            {
                new ItemData(){Name = "陈慧娴", Song = "千千阙歌"},
                new ItemData(){Name = "王菲", Song = "红豆"},
                new ItemData(){Name = "梁静茹", Song = "可惜不是你"},
                new ItemData(){Name = "筷子兄弟", Song = "老男孩"},
                new ItemData(){Name = "林志炫", Song = "你的样子"},
                new ItemData(){Name = "陈奕迅", Song = "稳稳的幸福"},
                new ItemData(){Name = "魏晨", Song = "美丽的谎言"},
                new ItemData(){Name = "付辛博", Song = "为爱放手"},
                new ItemData(){Name = "吴奇隆", Song = "寒冬"},
                new ItemData(){Name = "潘美辰", Song = "你管不着"},
                new ItemData(){Name = "星弟", Song = "同归于尽"},
                new ItemData(){Name = "陈浩民", Song = "起点"},
            };
            gridView.ItemsSource = lists;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MediaElement temp = MainPage.Current.FindName("Scenario1MediaElement") as MediaElement;
            MediaElementState MediaElementState = temp.CurrentState;
            Frame scenarioFrame = MainPage.Current.FindName("ScenarioFrame") as Frame;
            scenarioFrame.Navigate(typeof(Video_Sample.AudioMainPage));
        }

        private void gridView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _mediaElement.Source = new Uri("ms-appx:///Assets/VideoSample/Media/红豆.mp3", UriKind.RelativeOrAbsolute);
            _mediaElement.AutoPlay = true;
        }
    }

    public class ItemData
    {
        public string Name { get; set; }
        public string Song { get; set; }
    }
}
