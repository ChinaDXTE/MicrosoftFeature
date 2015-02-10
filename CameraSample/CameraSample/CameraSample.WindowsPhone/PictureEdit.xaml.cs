using Feature;
using Feature.Filter;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Imaging;
using Windows.Media.Capture;
using Windows.Media.MediaProperties;
using Windows.Phone.UI.Input;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

namespace CameraSample
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public partial class PictureEdit : Page
    {
        #region Field
        /// <summary>
        /// filter
        /// </summary>
        private FilterItems filterItems = new FilterItems();
        /// <summary>
        /// originalstream
        /// </summary>
        private IRandomAccessStream originalPicStream = null;
        /// <summary>
        /// target bitmap
        /// </summary>
        private WriteableBitmap targetBitMap = null;

        #endregion

        #region Constructor

        public PictureEdit()
        {
            this.InitializeComponent();
            this.targetBitMap = new WriteableBitmap((int)(Window.Current.Bounds.Width), (int)(Window.Current.Bounds.Height));
            this.Loaded += PictureEdit_Loaded;
        }
        #endregion

        private void PictureEdit_Loaded(object sender, RoutedEventArgs e)
        {
            this.Loaded -= PictureEdit_Loaded;
            this.listEffect.ItemsSource = this.filterItems.ListFilter;
        }

        /// <summary>
        /// 在此页将要在 Frame 中显示时进行调用。
        /// </summary>
        /// <param name="e">描述如何访问此页的事件数据。
        /// 此参数通常用于配置页。</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            this.progressRing.IsActive = true;
            if (e.Parameter is string)
            {
                var fileToken = e.Parameter.ToString();
                this.loadImage(fileToken);
            }
            else
            {
                this.loadImageByStream(e.Parameter as InMemoryRandomAccessStream);
            }
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
        }

        /// <summary>
        /// loadImage by fileToken:filepicker call this Method
        /// </summary>
        /// <param name="fileToken"></param>
        private async void loadImage(string fileToken)
        {
            if (!string.IsNullOrEmpty(fileToken))
            {
                StorageFile file = await Windows.Storage.AccessCache.StorageApplicationPermissions.FutureAccessList.GetFileAsync(fileToken);
                if (file != null)
                {
                    this.originalPicStream = await file.OpenReadAsync();
                    await this.targetBitMap.SetSourceAsync(this.originalPicStream);
                    this.image.Source = this.targetBitMap;
                    this.progressRing.IsActive = false;
                }
            }
        }

        /// <summary>
        /// loadImageByStream:capture stream call this Method
        /// </summary>
        /// <param name="stream"></param>
        private async void loadImageByStream(InMemoryRandomAccessStream stream)
        {
            await Task.Delay(2000);
            this.originalPicStream = stream;
            await this.targetBitMap.SetSourceAsync(this.originalPicStream);
            this.image.Source = this.targetBitMap;
            this.progressRing.IsActive = false;
        }

        private async void listEffect_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.progressRing.IsActive)
                return;
            var index = this.listEffect.SelectedIndex;
            this.filterItems.Select(index);
            if (index > 0)
            {
                var filter = this.filterItems.ListFilter[index].Filter;
                if (filter != null)
                {
                    this.targetBitMap = await this.originalPicStream.GetImageByFilter(filter);
                    this.image.Source = this.targetBitMap;
                }
            }
            else
            {
                this.originalPicStream.Seek(0);
                await this.targetBitMap.SetSourceAsync(this.originalPicStream);
                this.image.Source = this.targetBitMap;
            }
        }

        private async void AppbarCameraShared_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (this.progressRing.IsActive)
                return;
            if (this.targetBitMap != null && !this.progressRing.IsActive)
            {
                var byteImage = await this.targetBitMap.GetPhotoBytesAsync();
                InMemoryRandomAccessStream randomStream = new InMemoryRandomAccessStream();
                var stream = randomStream.AsStreamForWrite();
                await stream.WriteAsync(byteImage, 0, byteImage.Length);
                await stream.FlushAsync();
                if (stream.Length > 0)
                    randomStream.RegisterForShare("Photo File", "This is Photo File");
            }
        }

        private async void AppbarCameraSave_Tapped(object sender, TappedRoutedEventArgs e)
        {
            try
            {
                if (this.targetBitMap != null && !this.progressRing.IsActive)
                {
                    string fileName = string.Empty;
                    int count = await Notifications.conImageFolderName.GetFileCount();
                    if (count < 5)
                        fileName = "image" + (count + 1).ToString() + ".jpg";
                    else
                        fileName = "image1.jpg";
                    var byteImage = await this.targetBitMap.GetPhotoBytesAsync();
                    var storageTarget = StorageTarget.cameraroll;
                    if (Global.Current.Globalization.camera_CurrentLocationValue == "sd")
                        storageTarget = StorageTarget.SD;
                    await byteImage.Save(DateTime.Now.Ticks.ToString() + ".jpg", storageTarget);
                    //save to folder for update tile
                    await byteImage.Save(Notifications.conImageFolderName, fileName);
                    Global.Current.Notifications.UpdateBadgeWithNumber(1);
                    Global.Current.Notifications.CreateToastNotifier(Global.Current.Globalization.camera_Tip, Global.Current.Globalization.camera_SaveSuccess);
                    Global.Current.Notifications.CreateTileWide310x150PeekImageCollection01(Global.Current.Globalization.camera_TileTitle, Global.Current.Globalization.camera_TileMessage);
                }
            }
            catch (Exception ex)
            {
                Global.Current.Notifications.CreateToastNotifier(Global.Current.Globalization.camera_Tip, Global.Current.Globalization.camera_SaveError);
                Debug.WriteLine(ex.Message);
            }
        }

    }
}
