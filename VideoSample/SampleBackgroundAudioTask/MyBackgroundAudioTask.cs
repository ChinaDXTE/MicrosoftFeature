using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;
using Windows.Foundation.Collections;
using Windows.Media;
using Windows.Media.Playback;
using Windows.Storage;

namespace SampleBackgroundAudioTask
{
    /// <summary>
    /// Enum to identify foreground app state
    /// </summary>
    enum ForegroundAppStatus
    {
        Active,
        Suspended,
        Unknown
    }

    /// <summary>
    /// Impletements IBackgroundTask to provide an entry point for app code to be run in background. 
    /// Also takes care of handling UVC and communication channel with foreground
    /// </summary>
    public sealed class MyBackgroundAudioTask : IBackgroundTask
    {

        #region Private fields, properties
        private SystemMediaTransportControls systemmediatransportcontrol;
        private MyPlaylistManager playlistManager;
        private BackgroundTaskDeferral deferral; // Used to keep task alive
        private ForegroundAppStatus foregroundAppState = ForegroundAppStatus.Unknown;
        private AutoResetEvent BackgroundTaskStarted = new AutoResetEvent(false);
        private bool backgroundtaskrunning = false;

        /// <summary>
        /// Property to hold current playlist
        /// </summary>
        private MyPlaylist Playlist
        {
            get
            {
                if (null == playlistManager)
                {
                    playlistManager = new MyPlaylistManager();
                }
                return playlistManager.Current;
            }
        }
        #endregion

        #region IBackgroundTask and IBackgroundTaskInstance Interface Members and handlers
        /// <summary>
        /// The Run method is the entry point of a background task. 
        /// </summary>
        /// <param name="taskInstance"></param>
        public void Run(IBackgroundTaskInstance taskInstance)
        {
            Debug.WriteLine("Background Audio Task " + taskInstance.Task.Name + " starting...");

            //SystemMediaTransportControls，是按下音量键后顶部弹出的控制音乐播放的控件。
            //设置 SystemMediaTransportControls 的各个按键属性以及事件的订阅
            systemmediatransportcontrol = SystemMediaTransportControls.GetForCurrentView();
            systemmediatransportcontrol.ButtonPressed += systemmediatransportcontrol_ButtonPressed;
            systemmediatransportcontrol.PropertyChanged += systemmediatransportcontrol_PropertyChanged;
            systemmediatransportcontrol.IsEnabled = true;
            systemmediatransportcontrol.IsPauseEnabled = true;
            systemmediatransportcontrol.IsPlayEnabled = true;
            systemmediatransportcontrol.IsNextEnabled = true;
            systemmediatransportcontrol.IsPreviousEnabled = true;

            // 关联取消 后台任务处理完成
            taskInstance.Canceled += new BackgroundTaskCanceledEventHandler(OnCanceled);
            taskInstance.Task.Completed += Taskcompleted;

            //读取应用程序状态
            var value = ApplicationSettingsHelper.ReadResetSettingsValue(Constants.AppState);
            if (value == null)
                foregroundAppState = ForegroundAppStatus.Unknown;
            else
                foregroundAppState = (ForegroundAppStatus)Enum.Parse(typeof(ForegroundAppStatus), value.ToString());

            //添加在媒体播放器状态发生更改时处理函数
            BackgroundMediaPlayer.Current.CurrentStateChanged += Current_CurrentStateChanged;

            //Add handlers for playlist trackchanged
            //添加 后台播放列表变更时处理函数
            Playlist.TrackChanged += playList_TrackChanged;
            Playlist.TrackComplete += playList_TrackComplete;

            //初始化消息通道 前台发往后台的信息
            BackgroundMediaPlayer.MessageReceivedFromForeground += BackgroundMediaPlayer_MessageReceivedFromForeground;

            //Send information to foreground that background task has been started if app is active
            //将信息发送到前景中的后台任务已经启动，如果应用程序被激活
            if (foregroundAppState != ForegroundAppStatus.Suspended)
            {
                ValueSet message = new ValueSet();
                message.Add(Constants.BackgroundTaskStarted, "");
                BackgroundMediaPlayer.SendMessageToForeground(message);
            }
            BackgroundTaskStarted.Set();
            backgroundtaskrunning = true;

            ApplicationSettingsHelper.SaveSettingsValue(Constants.BackgroundTaskState, Constants.BackgroundTaskRunning);
            object value11 = ApplicationSettingsHelper.ReadResetSettingsValue(Constants.BackgroundTaskState);
            deferral = taskInstance.GetDeferral();
        }

        /// <summary>
        /// Indicate that the background task is completed.
        /// </summary>       
        void Taskcompleted(BackgroundTaskRegistration sender, BackgroundTaskCompletedEventArgs args)
        {
            Debug.WriteLine("MyBackgroundAudioTask " + sender.TaskId + " Completed...");
            deferral.Complete();
        }

        /// <summary>
        /// Handles background task cancellation. Task cancellation happens due to :
        /// 1. Another Media app comes into foreground and starts playing music 
        /// 2. Resource pressure. Your task is consuming more CPU and memory than allowed.
        /// In either case, save state so that if foreground app resumes it can know where to start.
        /// </summary>
        private void OnCanceled(IBackgroundTaskInstance sender, BackgroundTaskCancellationReason reason)
        {
            // You get some time here to save your state before process and resources are reclaimed
            Debug.WriteLine("MyBackgroundAudioTask " + sender.Task.TaskId + " Cancel Requested...");
            try
            {
                //save state
                ApplicationSettingsHelper.SaveSettingsValue(Constants.CurrentTrack, Playlist.CurrentTrackName);
                ApplicationSettingsHelper.SaveSettingsValue(Constants.Position, BackgroundMediaPlayer.Current.Position.ToString());
                ApplicationSettingsHelper.SaveSettingsValue(Constants.BackgroundTaskState, Constants.BackgroundTaskCancelled);
                
                ApplicationSettingsHelper.SaveSettingsValue(Constants.AppState, Enum.GetName(typeof(ForegroundAppStatus), foregroundAppState));
                backgroundtaskrunning = false;
                //unsubscribe event handlers
                systemmediatransportcontrol.ButtonPressed -= systemmediatransportcontrol_ButtonPressed;
                systemmediatransportcontrol.PropertyChanged -= systemmediatransportcontrol_PropertyChanged;
                Playlist.TrackChanged -= playList_TrackChanged;
                Playlist.TrackComplete -= playList_TrackComplete;

                //clear objects task cancellation can happen uninterrupted
                playlistManager.ClearPlaylist();
                playlistManager = null;
                BackgroundMediaPlayer.Shutdown(); // shutdown media pipeline
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
            deferral.Complete(); // signals task completion. 
            Debug.WriteLine("MyBackgroundAudioTask Cancel complete...");
        }
        #endregion

        #region SysteMediaTransportControls related functions and handlers
        /// <summary>
        /// Update UVC using SystemMediaTransPortControl apis
        /// </summary>
        private void UpdateUVCOnNewTrack(string title)
        {
            systemmediatransportcontrol.PlaybackStatus = MediaPlaybackStatus.Playing;
            systemmediatransportcontrol.DisplayUpdater.Type = MediaPlaybackType.Music;
            systemmediatransportcontrol.DisplayUpdater.MusicProperties.Title = title;// Playlist.CurrentTrackName;
            systemmediatransportcontrol.DisplayUpdater.Update();
        }

        /// <summary>
        /// Fires when any SystemMediaTransportControl property is changed by system or user
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        void systemmediatransportcontrol_PropertyChanged(SystemMediaTransportControls sender, SystemMediaTransportControlsPropertyChangedEventArgs args)
        {
            //TODO: If soundlevel turns to muted, app can choose to pause the music
        }

        /// <summary>
        /// This function controls the button events from UVC.
        /// This code if not run in background process, will not be able to handle button pressed events when app is suspended.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void systemmediatransportcontrol_ButtonPressed(SystemMediaTransportControls sender, SystemMediaTransportControlsButtonPressedEventArgs args)
        {
            switch (args.Button)
            {
                case SystemMediaTransportControlsButton.Play:
                    Debug.WriteLine("UVC play button pressed");
                    // If music is in paused state, for a period of more than 5 minutes, 
                    //app will get task cancellation and it cannot run code. 
                    //However, user can still play music by pressing play via UVC unless a new app comes in clears UVC.
                    //When this happens, the task gets re-initialized and that is asynchronous and hence the wait
                    if (!backgroundtaskrunning)
                    {
                        bool result = BackgroundTaskStarted.WaitOne(2000);
                        if (!result)
                            throw new Exception("Background Task didnt initialize in time");
                    }
                    StartPlayback();
                    break;
                case SystemMediaTransportControlsButton.Pause:
                    Debug.WriteLine("UVC pause button pressed");
                    try
                    {
                        BackgroundMediaPlayer.Current.Pause();
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex.ToString());
                    }
                    break;
                case SystemMediaTransportControlsButton.Next:
                    Debug.WriteLine("UVC next button pressed");
                    SkipToNext();
                    break;
                case SystemMediaTransportControlsButton.Previous:
                    Debug.WriteLine("UVC previous button pressed");
                    SkipToPrevious();
                    break;
            }
        }



        #endregion

        #region Playlist management functions and handlers
        /// <summary>
        /// Start playlist and change UVC state
        /// </summary>

        private void Trackat(string songName)
        {
            Playlist.StartTrackAt(songName);
        }

        private void StartPlayback(int index)
        {
            systemmediatransportcontrol.PlaybackStatus = MediaPlaybackStatus.Playing;
            Playlist.StartByIndex(index);
        }
        private void StartPlayback()
        {
            try
            {
                if (Playlist.CurrentTrackName == string.Empty)
                {
                    //If the task was cancelled we would have saved the current track and its position. We will try playback from there
                    var currenttrackname = ApplicationSettingsHelper.ReadResetSettingsValue(Constants.CurrentTrack);
                    var currenttrackposition = ApplicationSettingsHelper.ReadResetSettingsValue(Constants.Position);
                    if (currenttrackname != null)
                    {

                        if (currenttrackposition == null)
                        {
                            // play from start if we dont have position
                            Playlist.StartTrackAt((string)currenttrackname);
                        }
                        else
                        {
                            // play from exact position otherwise
                            Playlist.StartTrackAt((string)currenttrackname, TimeSpan.Parse((string)currenttrackposition));
                        }
                    }
                    else
                    {
                        //If we dont have anything, play from beginning of playlist.
                        Playlist.PlayAllTracks(); //start playback
                    }
                }
                else
                {
                    BackgroundMediaPlayer.Current.Play();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
        }

        /// <summary>
        /// Fires when playlist changes to a new track
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        void playList_TrackChanged(MyPlaylist sender, object args)
        {
            string title;
            if (args is string)
                title = args.ToString();
            else
                title = Playlist.CurrentTrackName;


            UpdateUVCOnNewTrack(title);
            ApplicationSettingsHelper.SaveSettingsValue(Constants.CurrentTrack, sender.CurrentTrackName);

            if (foregroundAppState == ForegroundAppStatus.Active)
            {
                //Message channel that can be used to send messages to foreground
                ValueSet message = new ValueSet();
                message.Add(Constants.Trackchanged, title);
                BackgroundMediaPlayer.SendMessageToForeground(message);
            }
        }

        void playList_TrackComplete(MyPlaylist sender, object args)
        {
            if (args is string)
                UpdateUVCOnNewTrack(args.ToString());
            else
                UpdateUVCOnNewTrack(Playlist.CurrentTrackName);

            ApplicationSettingsHelper.SaveSettingsValue(Constants.CurrentTrack, sender.CurrentTrackName);

            if (foregroundAppState == ForegroundAppStatus.Active)
            {
                //Message channel that can be used to send messages to foreground
                ValueSet message = new ValueSet();
                message.Add(Constants.Trackcomplete, null);
                BackgroundMediaPlayer.SendMessageToForeground(message);
            }
        }


        /// <summary>
        /// Skip track and update UVC via SMTC
        /// </summary>
        private void SkipToPrevious()
        {
            systemmediatransportcontrol.PlaybackStatus = MediaPlaybackStatus.Changing;
            Playlist.SkipToPrevious();
        }

        /// <summary>
        /// Skip track and update UVC via SMTC
        /// </summary>
        private void SkipToNext()
        {
            systemmediatransportcontrol.PlaybackStatus = MediaPlaybackStatus.Changing;
            Playlist.SkipToNext();
        }

        private async void StartFile(string fileToken)
        {
            StorageFile file = await Windows.Storage.AccessCache.StorageApplicationPermissions.FutureAccessList.GetFileAsync(fileToken);
            if (file != null)
            {
                Debug.WriteLine("Play file");
                systemmediatransportcontrol.PlaybackStatus = MediaPlaybackStatus.Playing;
                Playlist.StartFile(file);
            }
        }

        private async void StartSingleFile(string path)
        {
            Debug.WriteLine("Play file");
            systemmediatransportcontrol.PlaybackStatus = MediaPlaybackStatus.Playing;
            Playlist.StartSingleFile(path);
        }
        #endregion

        #region Background Media Player Handlers
        void Current_CurrentStateChanged(MediaPlayer sender, object args)
        {
            if (sender.CurrentState == MediaPlayerState.Playing)
            {
                systemmediatransportcontrol.PlaybackStatus = MediaPlaybackStatus.Playing;
            }
            else if (sender.CurrentState == MediaPlayerState.Paused)
            {
                systemmediatransportcontrol.PlaybackStatus = MediaPlaybackStatus.Paused;
            }
        }


        /// <summary>
        /// Fires when a message is recieved from the foreground app
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void BackgroundMediaPlayer_MessageReceivedFromForeground(object sender, MediaPlayerDataReceivedEventArgs e)
        {
            foreach (var one in e.Data)
            {
                string key = one.Key;
                switch (key.ToLower())
                {
                    case Constants.AppSuspended:
                        Debug.WriteLine("App suspending"); // App is suspended, you can save your task state at this point
                        foregroundAppState = ForegroundAppStatus.Suspended;
                        ApplicationSettingsHelper.SaveSettingsValue(Constants.CurrentTrack, Playlist.CurrentTrackName);
                        break;
                    case Constants.AppResumed:
                        Debug.WriteLine("App resuming"); // App is resumed, now subscribe to message channel
                        foregroundAppState = ForegroundAppStatus.Active;
                        break;
                    case Constants.Trackat: // User has chosen to skip track from app context.
                        foregroundAppState = ForegroundAppStatus.Active;
                        Debug.WriteLine("play to Trackat");
                        Trackat(one.Value.ToString());
                        break;
                    case Constants.StartPlayback: //Foreground App process has signalled that it is ready for playback
                        foregroundAppState = ForegroundAppStatus.Active;
                        Debug.WriteLine("Starting Playback");
                        StartPlayback();
                        break;
                    case Constants.SkipNext: // User has chosen to skip track from app context.
                        foregroundAppState = ForegroundAppStatus.Active;
                        Debug.WriteLine("Skipping to next");
                        SkipToNext();
                        break;
                    case Constants.SkipPrevious: // User has chosen to skip track from app context.
                        foregroundAppState = ForegroundAppStatus.Active;
                        Debug.WriteLine("Skipping to previous");
                        SkipToPrevious();
                        break;
                    case Constants.conCurrentFile:
                        Debug.WriteLine("Play current File");
                        object value = string.Empty;
                        bool rev = e.Data.TryGetValue(Constants.conCurrentFile, out value);
                        if (rev)
                        {
                            string fileToken = value.ToString();
                            StartFile(fileToken);
                        }
                        break;
                    case Constants.conPlaySingleFile:
                        object v = string.Empty;
                        bool bl = e.Data.TryGetValue(Constants.conPlaySingleFile, out v);
                        if (bl)
                        {
                            string fileToken = v.ToString();
                            StartSingleFile(fileToken);
                        }
                        break;
                    case Constants.conPlayByIndex:
                        object vByindex = string.Empty;
                        bool bplay = e.Data.TryGetValue(Constants.conPlayByIndex, out vByindex);
                        if (bplay)
                        {
                            int index = Convert.ToInt32(vByindex);
                            StartPlayback(index);
                        }
                        break;
                }
            }
        }
        #endregion

    }
}