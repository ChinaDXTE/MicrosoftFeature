using NewsSample.Data;
using NewsSample.Common;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Windows.Input;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.ApplicationModel.Resources;
using System.Diagnostics;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Documents;

// “通用中心应用程序”项目模板在 http://go.microsoft.com/fwlink/?LinkID=391955 上有介绍

namespace NewsSample
{
    /// <summary>
    /// 显示组内某一项的详细信息的页面。
    /// </summary>
    public sealed partial class ItemPage : Page
    {
        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();

        public ItemPage()
        {
            this.InitializeComponent();
            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += this.NavigationHelper_LoadState;

            //this.NewsBrowser.Loaded += NewsBrowser_Loaded;
            this.NewsBrowser.ScriptNotify += NewsBrowser_ScriptNotify;

            this.SizeChanged += ItemPage_SizeChanged;
            var resourceLoader = ResourceLoader.GetForCurrentView("Resources");
            string aaa = resourceLoader.GetString("HTMLContent");
            this.NewsBrowser.NavigateToString(aaa);
            
            Run run1 = new Run() { Text = resourceLoader.GetString("NewsContentPara1") };
            Run run2 = new Run() { Text = resourceLoader.GetString("NewsContentPara2") };
            Run run3 = new Run() { Text = resourceLoader.GetString("NewsContentPara3") };
            Run run4 = new Run() { Text = resourceLoader.GetString("NewsContentPara4") };
            Run run5 = new Run() { Text = resourceLoader.GetString("NewsContentPara5") };
            Run run6 = new Run() { Text = resourceLoader.GetString("NewsContentPara6") };
            Run run7 = new Run() { Text = resourceLoader.GetString("NewsContentPara7") };
            Run run8 = new Run() { Text = resourceLoader.GetString("NewsContentPara8") };
            Run run9 = new Run() { Text = resourceLoader.GetString("NewsContentPara9") };
            Run run10 = new Run() { Text = resourceLoader.GetString("NewsContentPara10") };

            Image image1 = new Image() { Source = new BitmapImage() { UriSource = new Uri("ms-appx:///Assets/WebImg.png") } };
            Image image2 = new Image() { Source = new BitmapImage() { UriSource = new Uri("ms-appx:///Assets/WebImg2.png") } };

            image1.Tapped += image1_Tapped;
            image2.Tapped += image2_Tapped;

            Paragraph para1 = new Paragraph() { TextIndent = 40.0 };
            Paragraph para2 = new Paragraph() { TextIndent = 40.0 };
            Paragraph para3 = new Paragraph() { TextIndent = 40.0 };
            Paragraph para4 = new Paragraph() { TextIndent = 40.0 };
            Paragraph para5 = new Paragraph() { TextIndent = 40.0 };
            Paragraph para6 = new Paragraph() { TextIndent = 40.0 };
            Paragraph para7 = new Paragraph() { TextIndent = 40.0 };
            Paragraph para8 = new Paragraph() { TextIndent = 40.0 };
            Paragraph para9 = new Paragraph() { TextIndent = 40.0 };
            Paragraph para10 = new Paragraph() { TextIndent = 40.0 };

            InlineUIContainer con1 = new InlineUIContainer() { Child = image1 };
            InlineUIContainer con2 = new InlineUIContainer() { Child = image2 };

            para1.Inlines.Add(run1);
            
            para2.Inlines.Add(run2);
            
            para3.Inlines.Add(run3);
            para3.Inlines.Add(new LineBreak());
            para3.Inlines.Add(con2);
            para3.Inlines.Add(new LineBreak());
            
            para4.Inlines.Add(run4);

            para5.Inlines.Add(run5);

            para6.Inlines.Add(run6);
            para6.Inlines.Add(new LineBreak());
            para6.Inlines.Add(con1);
            para6.Inlines.Add(new LineBreak());

            para7.Inlines.Add(run7);

            para8.Inlines.Add(run8);

            para9.Inlines.Add(run9);

            para10.Inlines.Add(run10);

            

            contentBlock.Blocks.Clear();
            contentBlock.Blocks.Add(para1);
            contentBlock.Blocks.Add(new Paragraph());
            contentBlock.Blocks.Add(para2);
            contentBlock.Blocks.Add(new Paragraph());
            contentBlock.Blocks.Add(para3);
            contentBlock.Blocks.Add(new Paragraph());
            contentBlock.Blocks.Add(para4);
            contentBlock.Blocks.Add(new Paragraph());
            contentBlock.Blocks.Add(para5);
            contentBlock.Blocks.Add(new Paragraph());
            contentBlock.Blocks.Add(para6);
            contentBlock.Blocks.Add(new Paragraph());
            contentBlock.Blocks.Add(para7);
            contentBlock.Blocks.Add(new Paragraph());
            contentBlock.Blocks.Add(para8);
            contentBlock.Blocks.Add(new Paragraph());
            contentBlock.Blocks.Add(para9);
            contentBlock.Blocks.Add(new Paragraph());
            contentBlock.Blocks.Add(para10);
        }

        void image2_Tapped(object sender, TappedRoutedEventArgs e)
        {
            image.Source = ((Image)sender).Source;
            imageGrid.Visibility = Visibility.Visible;
        }

        void image1_Tapped(object sender, TappedRoutedEventArgs e)
        {
            image.Source = ((Image)sender).Source;
            imageGrid.Visibility = Visibility.Visible;
        }

        void ItemPage_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            contentBlock.Height = contentScrollViewer.ActualHeight;
        }


        void NewsBrowser_ScriptNotify(object sender, NotifyEventArgs e)
        {
            string input = e.Value.ToString();
            if (input.Contains("ms-appx-web"))
            {
                input = input.Replace("ms-appx-web", "ms-appx");
            }
            ImageSource source = new BitmapImage(new Uri(input, UriKind.RelativeOrAbsolute));

            image.Source = source;
            imageGrid.Visibility = Visibility.Visible;
            
        }

        void OnGridBackClick(object sender, RoutedEventArgs e)
        {
            imageGrid.Visibility = Visibility.Collapsed;
        }

        void NewsBrowser_Loaded(object sender, RoutedEventArgs e)
        {
            var resourceLoader = ResourceLoader.GetForCurrentView("Resources");
            string aaa = resourceLoader.GetString("HTMLContent");
            NewsBrowser.NavigateToString(aaa);
        }

        /// <summary>
        /// 获取用于协助导航和进程生命期管理的 NavigationHelper。
        /// </summary>
        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }

        /// <summary>
        /// 获取 DefaultViewModel。可将其更改为强类型视图模型。
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
            var item = await SampleDataSource.GetItemAsync((string)e.NavigationParameter);
            this.DefaultViewModel["Item"] = item;

            this.AddFav.Visibility = (ReadListUtil.IsFav(item.UniqueId)) ? Visibility.Collapsed : Visibility.Visible;
            this.RemoveFav.Visibility = (ReadListUtil.IsFav(item.UniqueId)) ? Visibility.Visible : Visibility.Collapsed;

            var resourceLoader = ResourceLoader.GetForCurrentView("Resources");
            string aaa = resourceLoader.GetString("HTMLContent");
            aaa = aaa.Replace("<!DOCTYPE html><html lang='en' xmlns='http://www.w3.org/1999/xhtml'><head>    <meta charset='utf-8' />    <title></title></head><body>", "<!DOCTYPE html><html lang='en' xmlns='http://www.w3.org/1999/xhtml'><head>    <meta charset='utf-8' />    <title></title></head><body bgcolor='#1f1f1f' text='#ffffff'>");
            this.NewsBrowser.NavigateToString(aaa);
        }

        //CommandBar处理
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
        /// 应将页面特有的逻辑放入用于
        /// <see cref="Common.NavigationHelper.LoadState"/>
        /// 和 <see cref="Common.NavigationHelper.SaveState"/> 的事件处理程序中。
        /// 除了在会话期间保留的页面状态之外
        /// LoadState 方法中还提供导航参数。
        /// </summary>
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