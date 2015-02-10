using NewsSample.Common;
using NewsSample.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Windows.Input;
using Windows.ApplicationModel.Resources;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Display;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// “通用中心应用程序”项目模板在 http://go.microsoft.com/fwlink/?LinkID=391955 上有介绍

namespace NewsSample
{
    /// <summary>
    /// 显示组内某一项的详细信息的页面。
    /// </summary>
    public sealed partial class ItemPage : Page
    {
        private readonly NavigationHelper navigationHelper;
        private readonly ObservableDictionary defaultViewModel = new ObservableDictionary();

        ContentDialog dialog;

        public ItemPage()
        {
            this.InitializeComponent();

            this.navigationHelper = new NavigationHelper(this);

            this.navigationHelper.LoadState += this.NavigationHelper_LoadState;
            this.navigationHelper.SaveState += this.NavigationHelper_SaveState;

            this.NewsBrowser.NavigationCompleted += NewsBrowser_NavigationCompleted;
            this.NewsBrowser.ScriptNotify += NewsBrowser_ScriptNotify;

            this.Loaded += ItemPage_Loaded;

            
        }

        void dialog_Closed(ContentDialog sender, ContentDialogClosedEventArgs args)
        {
            commandBar.Visibility = Visibility.Visible;
        }

        void dialog_Opened(ContentDialog sender, ContentDialogOpenedEventArgs args)
        {
            commandBar.Visibility = Visibility.Collapsed;
        }


        async void ItemPage_Loaded(object sender, RoutedEventArgs e)
        {
            await System.Threading.Tasks.Task.Delay(1000);
            var resourceLoader = ResourceLoader.GetForCurrentView("Resources");
            string aaa = resourceLoader.GetString("HTMLContent");
            aaa = aaa.Replace("<!DOCTYPE html><html lang='en' xmlns='http://www.w3.org/1999/xhtml'><head>    <meta charset='utf-8' />    <title></title></head><body>", "<!DOCTYPE html><html lang='en' xmlns='http://www.w3.org/1999/xhtml'><head>    <meta charset='utf-8' />    <title></title></head><body bgcolor='#ffffff' text='#000000'><h1>" + (this.DefaultViewModel["Item"] as SampleDataItem).Title + "</h1><br/><br/>");
            
            this.NewsBrowser.NavigateToString(aaa);
        }


        async void NewsBrowser_NavigationCompleted(WebView sender, WebViewNavigationCompletedEventArgs args)
        {
            //await System.Threading.Tasks.Task.Delay(2000);
            
            this.LoadingGrid.Visibility = Visibility.Collapsed;
        }


        async void NewsBrowser_ScriptNotify(object sender, NotifyEventArgs e)
        {
            string input = e.Value.ToString();
            if (input.Contains("ms-appx-web"))
            {
                input = input.Replace("ms-appx-web", "ms-appx");
            }
            ImageSource source = new BitmapImage(new Uri(input, UriKind.RelativeOrAbsolute));
            dialog = new ContentDialog() { FullSizeDesired = true, Background = new SolidColorBrush(Windows.UI.Colors.White) };
            Grid grid = new Grid
            {
                Height = LayoutRoot.ActualHeight,
                Width = LayoutRoot.ActualWidth,
                Background = new SolidColorBrush(Windows.UI.Colors.White),
                VerticalAlignment = VerticalAlignment.Stretch,
                HorizontalAlignment = HorizontalAlignment.Stretch
            };
            grid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) });
            grid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
            grid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) });
            Image img = new Image() { Source = source, VerticalAlignment = VerticalAlignment.Center, HorizontalAlignment = HorizontalAlignment.Center };
            grid.Children.Add(img);
            img.SetValue(Grid.RowProperty, 1);
            dialog.Content = grid;
            dialog.Opened += dialog_Opened;
            dialog.Closed += dialog_Closed;

            await dialog.ShowAsync();
        }

        void NewsBrowser_Loaded(object sender, RoutedEventArgs e)
        {
            var resourceLoader = ResourceLoader.GetForCurrentView("Resources");
            string aaa = resourceLoader.GetString("HTMLContent");
            NewsBrowser.NavigateToString(aaa);
        } 

        /// <summary>
        /// 获取与此 <see cref="Page"/> 关联的 <see cref="NavigationHelper"/>。
        /// </summary>
        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }

        /// <summary>
        /// 获取此 <see cref="Page"/> 的视图模型。可将其更改为强类型视图模型。
        /// </summary>
        public ObservableDictionary DefaultViewModel
        {
            get { return this.defaultViewModel; }
        }

        /// <summary>
        /// 使用在导航过程中传递的内容填充页。  在从以前的会话
        /// 重新创建页时，也会提供任何已保存状态。
        /// </summary>
        /// <param name="sender">
        /// 事件的来源；通常为 <see cref="NavigationHelper"/>
        /// </param>
        /// <param name="e">事件数据，其中既提供在最初请求此页时传递给
        /// <see cref="Frame.Navigate(Type, object)"/> 的导航参数，又提供
        /// 此页在以前会话期间保留的状态的
        /// 字典。首次访问页面时，该状态将为 null。</param>
        private async void NavigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
            // TODO: 创建适用于问题域的合适数据模型以替换示例数据
            if ((string)e.NavigationParameter == "ReadList")
            {
                this.DefaultViewModel["Item"] = await ReadListUtil.GetReadList();
            }
            else if ((string)e.NavigationParameter == "FavList")
            {
                this.DefaultViewModel["Item"] = ReadListUtil.GetFavList();
            }
            else
            {
                var item = await SampleDataSource.GetItemAsync((string)e.NavigationParameter);
                this.DefaultViewModel["Item"] = item;
                this.AddFav.Visibility = (ReadListUtil.IsFav(item.UniqueId)) ? Visibility.Collapsed : Visibility.Visible;
                this.RemoveFav.Visibility = (ReadListUtil.IsFav(item.UniqueId)) ? Visibility.Visible : Visibility.Collapsed;
            }
            
        }

        /// <summary>
        /// 保留与此页关联的状态，以防挂起应用程序或
        /// 从导航缓存中放弃此页。值必须符合
        /// <see cref="SuspensionManager.SessionState"/> 的序列化要求。
        /// </summary>
        /// <param name="sender">事件的来源；通常为 <see cref="NavigationHelper"/></param>
        ///<param name="e">提供要使用可序列化状态填充的空字典
        ///的事件数据。</param>
        private void NavigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
            // TODO: 在此处保存页面的唯一状态。
        }

        private async void AppBarClick(object sender, RoutedEventArgs e)
        {
            string id = (DefaultViewModel["Item"] as SampleDataItem).UniqueId;
            var resourceLoader = ResourceLoader.GetForCurrentView("Resources");
            switch ((sender as AppBarButton).Tag as string)
            {
                case "AddFav":
                    ReadListUtil.AddFavId(id);
                    //Lock Screen Badge
                    Global.Current.Notifications.UpdateBadgeWithNumber(1);
                    //Toast Notification
                    ToastUtil.ShowToast(resourceLoader.GetString("HubTitleText"), resourceLoader.GetString("AddFavSuccessText"));
                    this.AddFav.Visibility = Visibility.Collapsed;
                    this.RemoveFav.Visibility = Visibility.Visible;
                    break;
                case "RemoveFav":
                    ReadListUtil.RemoveFavId(id);
                    //Lock Screen Badge
                    Global.Current.Notifications.UpdateBadgeWithNumber(2);
                    //Toast Notification
                    ToastUtil.ShowToast(resourceLoader.GetString("HubTitleText"), resourceLoader.GetString("RemoveFavSuccessText"));
                    this.AddFav.Visibility = Visibility.Visible;
                    this.RemoveFav.Visibility = Visibility.Collapsed;
                    break;
                case "Share":
                    (DefaultViewModel["Item"] as SampleDataItem).Title.RegisterForShare(resourceLoader.GetString("ShareTitle"), resourceLoader.GetString("ShareDesc"));
                    break;
                case "Save":
                    {
                        //检测是否有sd卡，如果没有，就弹出toast提示
                        if (!await FileEx.IsAvailableRemoveDevice("SD"))
                        {
                            ToastUtil.ShowToast(resourceLoader.GetString("HubTitleText"), resourceLoader.GetString("NoSD"));
                            return;
                        }
                        //保存文件到sd卡
                        string fileName = string.Empty;
                        foreach (StorageTarget item in Enum.GetValues(typeof(StorageTarget)))
                        {
                            if (item.ToString() == "SD")
                            {
                                await resourceLoader.GetString("HTMLContent").Save("NewsSample", "saved.html", item);
                                ToastUtil.ShowToast(resourceLoader.GetString("HubTitleText"), resourceLoader.GetString("SaveSDSuccessText"));
                            }
                        }
                    }
                    break;
                default:
                    break;
            }
        }

        #region NavigationHelper 注册

        /// <summary>
        /// 此部分中提供的方法只是用于使
        /// NavigationHelper 可响应页面的导航方法。
        /// <para>
        /// 应将页面特有的逻辑放入
        /// <see cref="NavigationHelper.LoadState"/>
        /// 和 <see cref="NavigationHelper.SaveState"/> 的事件处理程序中。
        /// 除了在早期会话期间保留的页面状态之外，
        /// LoadState 方法中还提供导航参数。
        /// </para>
        /// </summary>
        /// <param name="e">描述如何访问此页的事件数据。</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedFrom(e);
        }

        #endregion
    }
}