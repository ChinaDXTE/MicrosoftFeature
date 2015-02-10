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

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上提供

namespace Video_Sample
{
    /// <summary>
    /// 可独立使用或用于导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public static MainPage Current = null;
        private bool isLoaded = false;
        private static Frame page;

        public MainPage()
        {
            this.InitializeComponent();
            this.DataContext = Global.Current.Globalization;

            if (Current == null)
                Current = this;

            this.Loaded += Page_Loaded;


        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Scenario1MediaElement.CurrentStateChanged += (o, args) => { };
            Scenario1MediaElement.MediaEnded += (o, args) => { };
            Scenario1MediaElement.MediaFailed += (o, args) => { };
            Scenario1MediaElement.MediaOpened += (o, args) => { };

            MediaControl.PlayPauseTogglePressed += (o, o1) => { };
            MediaControl.PlayPressed += (o, o1) => { };
            MediaControl.PausePressed += (o, o1) => { };
            MediaControl.StopPressed += (o, o1) => { };

            if (page == null)
                page = Frame;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            Grid gridMedia = MainPage.Current.FindName("GridMedia") as Grid;
            gridMedia.Visibility = Visibility.Collapsed;

            MediaElement mediaElement = MainPage.Current.FindName("Scenario1MediaElement") as MediaElement;
            mediaElement.Stop();
            //if (SuspensionManager.SessionState.ContainsKey("SelectedScenarioIndex"))
            //{
            //    ScenarioControl.SelectedIndex = Convert.ToInt32(SuspensionManager.SessionState["SelectedScenarioIndex"]);
            //    ScenarioControl.ScrollIntoView(ScenarioControl.SelectedItem);
            //}
            //else
            //{
            //    ScenarioControl.SelectedIndex = 0;//2;//
            //}

            this.GridMedia.Visibility = Visibility.Collapsed;
        }
        private void Grid_Tapped(object sender, TappedRoutedEventArgs e)
        {
            ScenarioFrame.Navigate(typeof(AudioMainPage));
        }

        private void Grid_Tapped_1(object sender, TappedRoutedEventArgs e)
        {
            Frame.Navigate(typeof(Video_Sample.VideoMainPage), null);
        }

        private void gridSetting_Tapped(object sender, TappedRoutedEventArgs e)
        {
            new Setting().ShowIndependent();
        }

        private void Scenario1MediaElement_DoubleTapped(object sender, Windows.UI.Xaml.Input.DoubleTappedRoutedEventArgs e)
        {
            //Scenario1MediaElement.IsFullWindow = !Scenario1MediaElement.IsFullWindow;
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            if (page.CanGoBack)
                page.GoBack();
            //Global.Current.AppType = null;
            Current = null;
        }
    }
}
