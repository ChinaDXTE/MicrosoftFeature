using Feature;
using SampleBackgroundAudioTask;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using Video_Sample.Data;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Media.Playback;
using Windows.Phone.UI.Input;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkID=390556 上有介绍

namespace Video_Sample
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class AudioPlay : Page
    {
        private double _dTimeNUm = 0;                           //统计已播放时间
        private double _dPlaySum = 100;                         //统计播放总时间
        private static string _playName;                        //当前播放歌曲的名称
        private PLAYTYPE _playType = PLAYTYPE.Default;          //当前播放类型
        private static string _stSong = "红豆.mp3";
        private DispatcherTimer _timer;
        private string _fileToken;
        private string _filePath;
        private AutoResetEvent SererInitialized;
        private bool _isFirstTap = true;
        private static bool _isMyBackgroundTaskRunning;
        /// <summary>
        /// 获取有关后台任务的信息，正在运行或不读保存后台任务的设置
        /// </summary>
        public static bool IsMyBackgroundTaskRunning
        {
            get
            {
                if (_isMyBackgroundTaskRunning)
                    return true;

                object value = ApplicationSettingsHelper.ReadResetSettingsValue(SampleBackgroundAudioTask.Constants.BackgroundTaskState);
                if (value == null)
                {
                    return false;
                }
                else
                {
                    _isMyBackgroundTaskRunning = ((String)value).Equals(SampleBackgroundAudioTask.Constants.BackgroundTaskRunning);
                    return _isMyBackgroundTaskRunning;
                }
            }
        }

        public AudioPlay()
        {
            this.InitializeComponent();

            SererInitialized = new AutoResetEvent(false);

            InitItme();

            AddMediaPlayerEventHandlers();

            this.DataContext = Global.Current.Globalization;
        }
        /// <summary>
        /// 在此页将要在 Frame 中显示时进行调用。
        /// </summary>
        /// <param name="e">描述如何访问此页的事件数据。
        /// 此参数通常用于配置页。</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            AudioPlayInfo para = e.Parameter as AudioPlayInfo;
            if (para == null)
            {
                bool isPlayButton = false;
                //保持原有播放状态
                if (IsMyBackgroundTaskRunning)
                {
                    ChangePlayTimeNum();

                    ChangePlayTimeCurrent();

                    double value = 0;
                    if (_dPlaySum != 0)
                    {
                        value = _dTimeNUm / _dPlaySum * 100;
                    }
                    PlayProgress.Value = value;

                    if (MediaPlayerState.Playing == BackgroundMediaPlayer.Current.CurrentState)
                    {
                        isPlayButton = true;
                        _timer.Start();
                    }
                }

                SetPlayButtonState(isPlayButton);
            }
            else
            {
                _fileToken = para.fileToken;
                _playType = para.PlayType;
                _filePath = para.Path;
                _stSong = para.Song;
                Play();
            }

            textBlockSong.Text = _stSong;

        }
        private void InitItme()
        {
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(1);
            _timer.Tick += OnTimerTick;
        }

        private void OnTimerTick(object sender, object e)
        {
            _dTimeNUm += 1;
            PlayProgress.Value = _dTimeNUm / _dPlaySum * 100;
            playTime.Text = CalcPlayTime(_dTimeNUm);
        }

        #region 订阅 / 退订MediaPlayer事件
        /// <summary>
        /// 订阅的MediaPlayer事件
        /// </summary>
        private void AddMediaPlayerEventHandlers()
        {
            RemoveMediaPlayerEventHandlers();
            BackgroundMediaPlayer.Current.CurrentStateChanged += this.MediaPlayer_CurrentStateChanged;
            BackgroundMediaPlayer.MessageReceivedFromBackground += this.BackgroundMediaPlayer_MessageReceivedFromBackground;
        }
        /// <summary>
        /// 退订到MediaPlayer的事件。只在暂停时执行
        /// </summary>
        private void RemoveMediaPlayerEventHandlers()
        {
            BackgroundMediaPlayer.Current.CurrentStateChanged -= this.MediaPlayer_CurrentStateChanged;
            BackgroundMediaPlayer.MessageReceivedFromBackground -= this.BackgroundMediaPlayer_MessageReceivedFromBackground;
        }
        #endregion

        #region 后台 MediaPlayer 事件处理
        /// <summary>
        /// MediaPlayer的状态改变的事件处理。
        /// 注意，我们可以订阅事件，即使Media Player正在播放的媒体背景
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        async void MediaPlayer_CurrentStateChanged(MediaPlayer sender, object args)
        {
            switch (sender.CurrentState)
            {
                case MediaPlayerState.Playing:
                    await this.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                    {
                        SetPlayButtonState(true);
                        AppBarButtonPrevious.IsEnabled = true;
                        AppBarButtonNext.IsEnabled = true;
                        TimeSpan NaturalDuration = BackgroundMediaPlayer.Current.NaturalDuration;
                        playNumTime.Text = CalcPlayTime(NaturalDuration.TotalSeconds);
                        _dPlaySum = NaturalDuration.TotalSeconds;
                        _timer.Start();

                    }
                        );

                    break;
                case MediaPlayerState.Paused:
                    await this.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                    {
                        SetPlayButtonState(false);
                        _timer.Stop();
                    }
                    );

                    break;
            }
        }

        /// <summary>
        /// This event fired when a message is recieved from Background Process
        /// </summary>
        async void BackgroundMediaPlayer_MessageReceivedFromBackground(object sender, MediaPlayerDataReceivedEventArgs e)
        {
            foreach (string key in e.Data.Keys)
            {
                switch (key)
                {
                    case SampleBackgroundAudioTask.Constants.Trackchanged:
                        //When foreground app is active change track based on background message
                        await this.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                        {
                            playTime.Text = "00:00";
                            PlayProgress.Value = 0;
                            textBlockSong.Text = (string)e.Data[key];
                            _playName = textBlockSong.Text;
                        }
                        );
                        break;
                    case SampleBackgroundAudioTask.Constants.Trackcomplete:
                        await this.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                        {
                            Debug.WriteLine("###Trackcomplete### ");
                            PlayProgress.Value = 100;
                            TimeSpan NaturalDuration = BackgroundMediaPlayer.Current.NaturalDuration;
                            playTime.Text = CalcPlayTime(NaturalDuration.Seconds);
                            _dPlaySum = NaturalDuration.Seconds;
                            _dTimeNUm = 0;
                            _timer.Stop();
                        }
                        );
                        break;
                    case SampleBackgroundAudioTask.Constants.BackgroundTaskStarted:
                        //Wait for Background Task to be initialized before starting playback
                        Debug.WriteLine("Background Task started");
                        SererInitialized.Set();
                        break;
                }
            }
        }
        #endregion

        #region button 处理函数
        private void AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            AppBarButton abb = sender as AppBarButton;
            var value = new ValueSet();
            switch (abb.Tag.ToString())
            {
                case "previous":
                    value.Add(SampleBackgroundAudioTask.Constants.SkipPrevious, "");
                    BackgroundMediaPlayer.SendMessageToBackground(value);
                    _dTimeNUm = 0;
                    AppBarButtonPrevious.IsEnabled = false;
                    break;
                case "next":
                    value.Add(SampleBackgroundAudioTask.Constants.SkipNext, "");
                    BackgroundMediaPlayer.SendMessageToBackground(value);
                    _dTimeNUm = 0;
                    AppBarButtonNext.IsEnabled = false;
                    break;
            }
        }
        private void AppBarButtonPlay_Click(object sender, RoutedEventArgs e)
        {

            if (IsMyBackgroundTaskRunning)
            {
                if (MediaPlayerState.Playing == BackgroundMediaPlayer.Current.CurrentState)
                {
                    //正在播放，播放暂停
                    BackgroundMediaPlayer.Current.Pause();
                    //控件状态 显示播放
                    SetPlayButtonState(false);
                    _timer.Stop();
                }
                else if (MediaPlayerState.Paused == BackgroundMediaPlayer.Current.CurrentState)
                {
                    ChangePlayTimeNum();

                    ChangePlayTimeCurrent();

                    if (_dTimeNUm == 0)
                    {
                        playTime.Text = "00:00";
                        PlayProgress.Value = 0;
                    }
                    BackgroundMediaPlayer.Current.Play();
                    //控件状态 显示暂停
                    SetPlayButtonState(true);
                    _timer.Start();
                }
                else if (MediaPlayerState.Closed == BackgroundMediaPlayer.Current.CurrentState)
                {
                    Play();
                }
            }
            else
            {
                StartBackgroundAudioTask();
            }
        }

        #endregion

        /// <summary>
        /// Initialize Background Media Player Handlers and starts playback
        /// </summary>
        private void StartBackgroundAudioTask()
        {
            AddMediaPlayerEventHandlers();
            var backgroundtaskinitializationresult = this.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                Debug.WriteLine("###BackgroundMediaPlayer.SendMessageToBackground000000000");
                bool result = SererInitialized.WaitOne(2000);
                //Send message to initiate playback
                if (result == true)
                {
                    Debug.WriteLine("###BackgroundMediaPlayer.SendMessageToBackground11111111111111111");
                    PlayByType();
                    Debug.WriteLine("###BackgroundMediaPlayer.SendMessageToBackground");
                }
                else
                {
                    throw new Exception("Background Audio Task didn't start in expected time");
                }
            }
            );
            backgroundtaskinitializationresult.Completed = new AsyncActionCompletedHandler(BackgroundTaskInitializationCompleted);
        }
        private void BackgroundTaskInitializationCompleted(IAsyncAction action, AsyncStatus status)
        {
            if (status == AsyncStatus.Completed)
            {
                Debug.WriteLine("Background Audio Task initialized");
            }
            else if (status == AsyncStatus.Error)
            {
                Debug.WriteLine("Background Audio Task could not initialized due to an error ::" + action.ErrorCode.ToString());
            }
        }

        private void PlayByType()
        {
            var message = new ValueSet();
            switch (_playType)
            {
                case PLAYTYPE.PlayLoacl:
                    message.Add(SampleBackgroundAudioTask.Constants.conCurrentFile, _fileToken);
                    break;
                case PLAYTYPE.PlaySingle:
                    message.Add(SampleBackgroundAudioTask.Constants.conPlaySingleFile, _filePath);
                    break;
                case PLAYTYPE.PlayList:
                    message.Add(SampleBackgroundAudioTask.Constants.conPlayByIndex, _filePath);
                    break;
                default:
                    message.Add(SampleBackgroundAudioTask.Constants.conPlaySingleFile, "ms-appx:///Assets/Media/红豆.mp3");
                    break;
            }
            BackgroundMediaPlayer.SendMessageToBackground(message);
        }

        private void Play()
        {
            if (IsMyBackgroundTaskRunning)
            {
                PlayByType();
            }
            else
            {
                StartBackgroundAudioTask();
            }
            SetPlayButtonState(true);
        }

        /// <summary>
        /// 设置 播放按钮状态
        /// </summary>
        /// <param name="flag">true 表示当前是播放状态 按钮显示暂停图标</param>
        private void SetPlayButtonState(bool flag)
        {
            BitmapIcon bIcon = new BitmapIcon();
            if (flag)
            {
                bIcon.UriSource = new Uri("ms-appx:///Assets/VideoSample/Play/pause.png", UriKind.Absolute);
                AppBarButtonPlay.Icon = bIcon;
            }
            else
            {
                bIcon.UriSource = new Uri("ms-appx:///Assets/VideoSample/Play/play.png", UriKind.Absolute);
                AppBarButtonPlay.Icon = bIcon;
            }
        }

        private void ChangePlayTimeNum()
        {
            TimeSpan NaturalDuration = BackgroundMediaPlayer.Current.NaturalDuration;
            playNumTime.Text = CalcPlayTime(NaturalDuration.TotalSeconds);
            _dPlaySum = NaturalDuration.TotalSeconds;
        }

        private void ChangePlayTimeCurrent()
        {
            TimeSpan Position = BackgroundMediaPlayer.Current.Position;
            playTime.Text = CalcPlayTime(Position.TotalSeconds);
            _dTimeNUm = Position.TotalSeconds;
        }
        /// <summary>
        /// 计算播放时间
        /// </summary>
        /// <param name="iTimeNum"></param>
        /// <returns></returns>
        private string CalcPlayTime(double iTimeNum)
        {
            if (iTimeNum < 0)
                return "00:00";
            int minute = (int)iTimeNum / 60;
            int second = (int)iTimeNum % 60;
            return minute.ToString("D2") + ":" + second.ToString("D2");
        }

        public void App_Suspending()
        {
            _timer.Stop();
        }

        public void App_Resuming()
        {
            AddMediaPlayerEventHandlers();
            bool isPlayButton = false;
            //保持原有播放状态
            if (IsMyBackgroundTaskRunning)
            {
                ChangePlayTimeNum();

                ChangePlayTimeCurrent();

                double value = 0;
                if (_dPlaySum != 0)
                {
                    value = _dTimeNUm / _dPlaySum * 100;
                }
                PlayProgress.Value = value;
                if (MediaPlayerState.Playing == BackgroundMediaPlayer.Current.CurrentState)
                {
                    isPlayButton = true;
                    _timer.Start();
                }
            }

            SetPlayButtonState(isPlayButton);
        }
    }
}
