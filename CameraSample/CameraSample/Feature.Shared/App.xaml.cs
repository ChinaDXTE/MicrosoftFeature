using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;
using Feature.Common;
using CameraSample;
using System.Threading.Tasks;
using Windows.Graphics.Display;
using Windows.Media.Capture;
using Windows.Devices.Enumeration;
using System.Diagnostics;
using Windows.Media.MediaProperties;
using Windows.Storage;
#if WINDOWS_PHONE_APP
using Windows.Phone.UI.Input;
using Windows.UI.Core;
#else
using Windows.UI.ApplicationSettings;
#endif

namespace Feature
{
    /// <summary>
    /// 提供特定于应用程序的行为，以补充默认的应用程序类。
    /// </summary>
    public sealed partial class App : Application
    {
#if WINDOWS_PHONE_APP
        private TransitionCollection transitions;
        private ContinuationManager continuationManager;
        public event EventHandler<BackPressedEventArgs> BackPressed;
#endif
        /// <summary>
        /// Capture Photo
        /// </summary>
        public MediaCapture mediaCapture { get; set; }
        /// <summary>
        /// Capture Screen
        /// </summary>
        public MediaCapture screenRecord { get; set; }
        /// <summary>
        /// device Info
        /// </summary>
        public DeviceInformation deviceInfo { get; set; }
        /// <summary>
        /// is support front camera
        /// </summary>
        public bool isSupportFront { get;set;}
        /// <summary>
        /// is only camera (pc usb)
        /// </summary>
        public bool isExternalCamera { get; set; }
        /// <summary>
        /// is process pic
        /// </summary>
        public bool IsProcessingPicture { get; set; }
        /// <summary>
        /// current Device Panel
        /// </summary>
        private Windows.Devices.Enumeration.Panel currentPanel = Windows.Devices.Enumeration.Panel.Back;
        /// <summary>
        /// screen record maxSecend,you can uapdate value
        /// </summary>
        private int maxSecend = 60;
        /// <summary>
        /// screen record timer
        /// </summary>
        private DispatcherTimer timer = new DispatcherTimer();

        public bool isRecoding { get; private set; }

        /// <summary>
        /// 初始化 <see cref="App"/> 类的单一实例。这是创作的代码的第一行
        /// 逻辑上等同于 main() 或 WinMain()。
        /// </summary>
        public App()
        {
            this.InitializeComponent();
            this.Suspending += this.OnSuspending;
#if WINDOWS_PHONE_APP
            HardwareButtons.BackPressed += this.HardwareButtons_BackPressed;
            this.Resuming += App_Resuming;
#endif
        }

        /// <summary>
        /// 在应用程序由最终用户正常启动时进行调用。
        /// 当启动应用程序以打开特定的文件或显示搜索结果等操作时，
        /// 将使用其他入口点。
        /// </summary>
        /// <param name="e">有关启动请求和过程的详细信息。</param>
        protected async override void OnLaunched(LaunchActivatedEventArgs e)
        {
#if DEBUG
            if (System.Diagnostics.Debugger.IsAttached)
            {
                this.DebugSettings.EnableFrameRateCounter = true;
            }
#endif

            Frame rootFrame = Window.Current.Content as Frame;

            // 不要在窗口已包含内容时重复应用程序初始化，
            // 只需确保窗口处于活动状态
            if (rootFrame == null)
            {
                // 创建要充当导航上下文的框架，并导航到第一页
                rootFrame = new Frame();

                //将框架与 SuspensionManager 键关联
                SuspensionManager.RegisterFrame(rootFrame, "AppFrame");

                // TODO: 将此值更改为适合您的应用程序的缓存大小
                rootFrame.CacheSize = 1;
                rootFrame.Language = Windows.Globalization.ApplicationLanguages.Languages[0];

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    // 仅当合适时才还原保存的会话状态
                    try
                    {
                        await SuspensionManager.RestoreAsync();
                    }
                    catch (SuspensionManagerException)
                    {
                        // 还原状态时出现问题。
                        // 假定没有状态并继续
                    }
                }

                // 将框架放在当前窗口中
                Window.Current.Content = rootFrame;
            }

            if (rootFrame.Content == null)
            {
#if WINDOWS_PHONE_APP
                // 删除用于启动的旋转门导航。
                if (rootFrame.ContentTransitions != null)
                {
                    this.transitions = new TransitionCollection();
                    foreach (var c in rootFrame.ContentTransitions)
                    {
                        this.transitions.Add(c);
                    }
                }

                rootFrame.ContentTransitions = null;
                rootFrame.Navigated += this.RootFrame_FirstNavigated;
                await Windows.UI.ViewManagement.StatusBar.GetForCurrentView().HideAsync();
                //Global.Current.Push.Bind();
#endif
                // 当未还原导航堆栈时，导航到第一页，
                // 并通过将所需信息作为导航参数传入来配置
                // 参数
                if (!rootFrame.Navigate(typeof(MainPage), e.Arguments))
                {
                    throw new Exception("Failed to create initial page");
                }
            }

            // 确保当前窗口处于活动状态
            Window.Current.Activate();
        }

#if WINDOWS_PHONE_APP
        private void HardwareButtons_BackPressed(object sender, BackPressedEventArgs e)
        {
            Frame frame = Window.Current.Content as Frame;
            if (frame == null)
            {
                return;
            }

            var handler = this.BackPressed;
            if (handler != null)
            {
                handler(sender, e);
            }

            if (frame.CanGoBack && !e.Handled)
            {
                frame.GoBack();
                e.Handled = true;
            }
        }
        /// <summary>
        /// 启动应用程序后还原内容转换。
        /// </summary>
        /// <param name="sender">附加了处理程序的对象。</param>
        /// <param name="e">有关导航事件的详细信息。</param>
        private void RootFrame_FirstNavigated(object sender, NavigationEventArgs e)
        {
            var rootFrame = sender as Frame;
            rootFrame.ContentTransitions = this.transitions ?? new TransitionCollection() { new NavigationThemeTransition() };
            rootFrame.Navigated -= this.RootFrame_FirstNavigated;
        }

        /// <summary>
        /// Handle OnActivated event to deal with File Open/Save continuation activation kinds
        /// </summary>
        /// <param name="e">Application activated event arguments, it can be casted to proper sub-type based on ActivationKind</param>
        protected async override void OnActivated(IActivatedEventArgs e)
        {
            base.OnActivated(e);
            continuationManager = new ContinuationManager();
            Frame rootFrame = Window.Current.Content as Frame;

            // Do not repeat app initialization when the Window already has content,
            // just ensure that the window is active
            if (rootFrame == null)
            {
                // Create a Frame to act as the navigation context and navigate to the first page
                rootFrame = new Frame();

                // Set the default language
                rootFrame.Language = Windows.Globalization.ApplicationLanguages.Languages[0];

                // Place the frame in the current Window
                Window.Current.Content = rootFrame;
            }
            if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
            {
                // Restore the saved session state only when appropriate
                try
                {
                    await SuspensionManager.RestoreAsync();
                }
                catch (SuspensionManagerException)
                {
                    //Something went wrong restoring state.
                    //Assume there is no state and continue
                }
            }

            if (rootFrame.Content == null)
            {
                rootFrame.Navigate(typeof(MainPage));
            }

            var continuationEventArgs = e as IContinuationActivatedEventArgs;
            if (continuationEventArgs != null)
            {
                continuationManager.Continue(continuationEventArgs);
            }

            Window.Current.Activate();
        }

        private async void App_Resuming(object sender, object e)
        {
            Frame root = Window.Current.Content as Frame;
            if (root != null)
            {
                if (root.Content is PictureCapture)
                {
                    var pictureCapture = root.Content as PictureCapture;
                    await pictureCapture.StartCapturePreviewAsync();
                }
            }
        }
#else
        protected override void OnWindowCreated(WindowCreatedEventArgs args)
        {
            SettingsPane.GetForCurrentView().CommandsRequested += OnCommandsRequested;
        }

        private void OnCommandsRequested(SettingsPane sender, SettingsPaneCommandsRequestedEventArgs args)
        {
            var cameraSetting = Global.Current.Globalization.camera_CameraSetting;
            args.Request.ApplicationCommands.Add(new SettingsCommand(
                cameraSetting, cameraSetting, (handler) => ShowCustomSettingFlyout()));
        }

        public void ShowCustomSettingFlyout()
        {
            Setting CustomSettingFlyout = new Setting();
            CustomSettingFlyout.Show();
        }
#endif

        /// <summary>
        /// 在将要挂起应用程序执行时调用。    将保存应用程序状态
        /// 将被终止还是恢复的情况下保存应用程序状态，
        /// 并让内存内容保持不变。
        /// </summary>
        private async void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();

            while (this.IsProcessingPicture)
            {
                await Task.Delay(500);
            }
             //stop preview
            this.CleanUpCapture();
#if WINDOWS_PHONE_APP
            Frame root = Window.Current.Content as Frame;
            if (root != null)
            {
                if (root.Content is ScreenRecord)
                {
                    var ScreenRecord = root.Content as ScreenRecord;
                    ScreenRecord.StopPaly();
                }
            }
            // stop record
            if(this.isRecoding)
                await this.StopRecording();
#endif
            deferral.Complete();
        }

        #region Capture
        /// <summary>
        /// Initialze MediaCapture
        /// </summary>
        /// <param name="panel"></param>
        /// <returns></returns>
        public async Task InitializeCapture(Windows.Devices.Enumeration.Panel panel = Windows.Devices.Enumeration.Panel.Back)
        {
            mediaCapture = new MediaCapture();
            this.currentPanel = panel;
            MediaCaptureInitializationSettings setting = await InitializeSettings(panel);
            if (setting != null)
            {
                await mediaCapture.InitializeAsync(setting);
            }
            else
            {
                await mediaCapture.InitializeAsync();
            }
            var resolutions = this.mediaCapture.VideoDeviceController.GetAvailableMediaStreamProperties(MediaStreamType.Photo);
            if (resolutions.Count >= 1)
            {
                var hires = resolutions.OrderByDescending(item => ((VideoEncodingProperties)item).Width).First();
                await mediaCapture.VideoDeviceController.SetMediaStreamPropertiesAsync(MediaStreamType.Photo, hires);
            }
            if (!this.isExternalCamera)
            {
                var previewRotation = (this.currentPanel == Windows.Devices.Enumeration.Panel.Back) ? VideoRotation.Clockwise90Degrees : VideoRotation.Clockwise270Degrees;
                try
                {
                    mediaCapture.SetPreviewRotation(previewRotation);
                    mediaCapture.SetRecordRotation(previewRotation);
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.Message);
                }
            }
        }
        /// <summary>
        /// dispose MediaCapture
        /// </summary>
        public async void CleanUpCapture()
        {
            try
            {
                if (this.mediaCapture != null)
                {
                    await this.mediaCapture.StopPreviewAsync();
                    this.mediaCapture.Dispose();
                    this.mediaCapture = null;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
        /// <summary>
        /// Get MediaCaptureInitializationSettings
        /// </summary>
        /// <param name="panel"></param>
        /// <returns></returns>
        private async Task<MediaCaptureInitializationSettings> InitializeSettings(Windows.Devices.Enumeration.Panel panel)
        {
            MediaCaptureInitializationSettings setting = null;
            DeviceInformation deviceFront = null;
            var deviceCollection = await DeviceInformation.FindAllAsync(DeviceClass.VideoCapture);
            if (deviceCollection == null || deviceCollection.Count == 0)
                return null;
            if (deviceCollection.Count >= 2)
            {
                this.deviceInfo = deviceCollection.FirstOrDefault(d => d.EnclosureLocation.Panel == panel);
                deviceFront = deviceCollection.FirstOrDefault(d => d.EnclosureLocation.Panel == Windows.Devices.Enumeration.Panel.Front);
            }
            if (deviceFront != null)
                this.isSupportFront = true;
            else
                this.isSupportFront = false;
            if (this.deviceInfo != null)
            {
                setting = new MediaCaptureInitializationSettings();
                setting.AudioDeviceId = "";
                setting.VideoDeviceId = this.deviceInfo.Id;
            }
            else if (deviceCollection.Count == 1 && deviceCollection[0].EnclosureLocation == null)
            {
                setting = new MediaCaptureInitializationSettings();
                setting.AudioDeviceId = "";
                setting.VideoDeviceId = deviceCollection[0].Id;
                this.isExternalCamera = true;
            }
            return setting;
        }
        #endregion

#if WINDOWS_PHONE_APP
        #region Screen Record
        public async Task StopRecording()
        {
            if (this.screenRecord != null)
            {
                await this.screenRecord.StopRecordAsync();
            }
            if (this.screenRecord != null)
            {
                this.screenRecord.Dispose();
            }
            this.isRecoding = false;
        }

        public async void StartRecording()
        {
            try
            {
                var screenCapture = Windows.Media.Capture.ScreenCapture.GetForCurrentView();
                // Set the MediaCaptureInitializationSettings to use the ScreenCapture as the
                // audio and video source.
                var mcis = new Windows.Media.Capture.MediaCaptureInitializationSettings();
                mcis.VideoSource = screenCapture.VideoSource;
                mcis.AudioSource = screenCapture.AudioSource;
                mcis.StreamingCaptureMode = Windows.Media.Capture.StreamingCaptureMode.AudioAndVideo;
                // Initialize the MediaCapture with the initialization settings.
                this.screenRecord = new MediaCapture();
                await this.screenRecord.InitializeAsync(mcis);
                // Create a file to record to.
                StorageFile videoFile = await ApplicationData.Current.LocalFolder.CreateFileAsync("recording.mp4", CreationCollisionOption.ReplaceExisting);
                // Create an encoding profile to use.
                var profile = Windows.Media.MediaProperties.MediaEncodingProfile.CreateMp4(Windows.Media.MediaProperties.VideoEncodingQuality.Auto);
                // Start recording
                await this.screenRecord.StartRecordToStorageFileAsync(profile, videoFile);
                this.isRecoding = true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                Global.Current.Notifications.CreateToastNotifier("test", "Starterror");
            }
        }

        #endregion
#endif

    }
}