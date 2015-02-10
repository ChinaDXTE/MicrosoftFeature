using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using System.Threading;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.Foundation;
using System.Diagnostics;
using Windows.UI.Xaml.Media;
using Windows.Media;
using Windows.Storage.Streams;
using Windows.Storage;
#if WINDOWS_PHONE_APP
using Windows.Media.Playback;
using System.IO;
using System.Threading.Tasks;
#endif

namespace Feature
{
    public class AudioTaskManager
    {
        private Page _page = null;
        protected Page page
        {
            get { return _page ?? (_page = Window.Current.Content.GetVisualDescendants().OfType<Page>().FirstOrDefault()); }
        }

#if WINDOWS_PHONE_APP
        #region Field
        private bool isMyBackgroundTaskRunning = false;
        private bool isFirstRunTask = false;
        private AutoResetEvent SererInitialized;
        public event TypedEventHandler<MediaPlayer, object> CurrentStateChanged;
        public event EventHandler<MediaPlayerDataReceivedEventArgs> MessageReceivedFromBackground;

        /// <summary>
        /// Gets the information about background task is running or not by reading the setting saved by background task
        /// </summary>
        private bool IsMyBackgroundTaskRunning
        {
            get
            {
                if (isMyBackgroundTaskRunning)
                    return true;
                object value = Global.Current.LocalSettings.LoadData(Constants.conBackgroundTaskState);
                if (value == null)
                {
                    return false;
                }
                else
                {
                    isMyBackgroundTaskRunning = ((String)value).Equals(Constants.conBackgroundTaskRunning);
                    return isMyBackgroundTaskRunning;
                }
            }
        }
        /// <summary>
        /// Read current track information from application settings
        /// </summary>
        private string CurrentTrack
        {
            get
            {
                object value = Global.Current.LocalSettings.LoadData(Constants.conCurrentTrack);
                if (value != null)
                {
                    return (String)value;
                }
                else
                    return String.Empty;
            }
        }

        public MediaPlayer CurrentMediaPlayer
        {
            get { return BackgroundMediaPlayer.Current; }
        }
        #endregion

        public AudioTaskManager()
        {
            SererInitialized = new AutoResetEvent(false);
            App.Current.Suspending += ForegroundApp_Suspending;
            App.Current.Resuming += ForegroundApp_Resuming;
            Global.Current.LocalSettings.SaveData(Constants.conAppState, Constants.conForegroundAppActive);
            AddMediaPlayerEventHandlers();
        }

        #region Foreground App Lifecycle Handlers
        /// <summary>
        /// Sends message to background informing app has resumed
        /// Subscribe to MediaPlayer events
        /// </summary>
        private void ForegroundApp_Resuming(object sender, object e)
        {
            Global.Current.LocalSettings.SaveData(Constants.conAppState, Constants.conForegroundAppActive);
            // Verify if the task was running before
            if (IsMyBackgroundTaskRunning)
            {
                //if yes, reconnect to media play handlers
                AddMediaPlayerEventHandlers();
                //send message to background task that app is resumed, so it can start sending notifications
                ValueSet messageDictionary = new ValueSet();
                messageDictionary.Add(Constants.conAppResumed, DateTime.Now.ToString());
                BackgroundMediaPlayer.SendMessageToBackground(messageDictionary);
            }
        }

        /// <summary>
        /// Send message to Background process that app is to be suspended
        /// Stop clock and slider when suspending
        /// Unsubscribe handlers for MediaPlayer events
        /// </summary>
        void ForegroundApp_Suspending(object sender, Windows.ApplicationModel.SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            ValueSet messageDictionary = new ValueSet();
            messageDictionary.Add(Constants.conAppSuspended, DateTime.Now.ToString());
            BackgroundMediaPlayer.SendMessageToBackground(messageDictionary);
            RemoveMediaPlayerEventHandlers();
            Global.Current.LocalSettings.SaveData(Constants.conAppState, Constants.conForegroundAppSuspended);
            deferral.Complete();
        }
        #endregion

        #region Media Playback Helper methods
        /// <summary>
        /// Unsubscribes to MediaPlayer events. Should run only on suspend
        /// </summary>
        private void RemoveMediaPlayerEventHandlers()
        {
            BackgroundMediaPlayer.Current.CurrentStateChanged -= this.MediaPlayer_CurrentStateChanged;
            BackgroundMediaPlayer.MessageReceivedFromBackground -= this.BackgroundMediaPlayer_MessageReceivedFromBackground;
        }

        /// <summary>
        /// Subscribes to MediaPlayer events
        /// </summary>
        private void AddMediaPlayerEventHandlers()
        {
            BackgroundMediaPlayer.Current.CurrentStateChanged += this.MediaPlayer_CurrentStateChanged;
            BackgroundMediaPlayer.MessageReceivedFromBackground += this.BackgroundMediaPlayer_MessageReceivedFromBackground;
        }

        /// <summary>
        /// Initialize Background Media Player Handlers and starts playback
        /// </summary>
        private void StartBackgroundAudioTask()
        {
            if (!this.isFirstRunTask)
            {
                var backgroundtaskinitializationresult = this.page.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    bool result = SererInitialized.WaitOne(2000);
                    //Send message to initiate playback
                    if (result == true)
                    {
                        var message = new ValueSet();
                        message.Add(Constants.conStartPlayback, "0");
                        BackgroundMediaPlayer.SendMessageToBackground(message);
                    }
                    else
                    {
                        Debug.WriteLine("Background Audio Task didn't start in expected time");
                    }
                }
                );
                backgroundtaskinitializationresult.Completed = new AsyncActionCompletedHandler(BackgroundTaskInitializationCompleted);
            }
            else
            {
                var message = new ValueSet();
                message.Add(Constants.conStartPlayback, "0");
                BackgroundMediaPlayer.SendMessageToBackground(message);
            }
        }

        /// <summary>
        /// play select file
        /// </summary>
        /// <param name="file"></param>
        public void PlayFile(StorageFile file)
        {
            Debug.WriteLine("Play button pressed from App");
            var value = new ValueSet();
            var fileToken = Windows.Storage.AccessCache.StorageApplicationPermissions.FutureAccessList.Add(file);
            value.Add(Constants.conCurrentFile, fileToken);
            BackgroundMediaPlayer.SendMessageToBackground(value);
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
        #endregion

        #region Background MediaPlayer Event handlers
        /// <summary>
        /// MediaPlayer state changed event handlers. 
        /// Note that we can subscribe to events even if Media Player is playing media in background
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void MediaPlayer_CurrentStateChanged(MediaPlayer sender, object args)
        {
            if (this.CurrentStateChanged != null)
                this.CurrentStateChanged(sender, args);
        }

        /// <summary>
        /// This event fired when a message is recieved from Background Process
        /// </summary>
        private void BackgroundMediaPlayer_MessageReceivedFromBackground(object sender, MediaPlayerDataReceivedEventArgs e)
        {
            foreach (string key in e.Data.Keys)
            {
                if (key == Constants.conBackgroundTaskStarted)
                {
                    Debug.WriteLine("Background Task started");
                    SererInitialized.Set();
                }
            }
            if (this.MessageReceivedFromBackground != null)
                this.MessageReceivedFromBackground(sender, e);
        }

        #endregion

        #region Button Click Event Handlers
        /// <summary>
        /// Sends message to the background task to skip to the previous track.
        /// </summary>
        public void Prevent()
        {
            var value = new ValueSet();
            value.Add(Constants.conSkipPrevious, "");
            BackgroundMediaPlayer.SendMessageToBackground(value);
        }

        /// <summary>
        /// If the task is already running, it will just play/pause MediaPlayer Instance
        /// Otherwise, initializes MediaPlayer Handlers and starts playback
        /// track or to pause if we're already playing.
        /// </summary>
        public void Play()
        {
            Debug.WriteLine("Play button pressed from App");
            if (MediaPlayerState.Playing == BackgroundMediaPlayer.Current.CurrentState)
            {
                BackgroundMediaPlayer.Current.Pause();
            }
            else if (MediaPlayerState.Paused == BackgroundMediaPlayer.Current.CurrentState)
            {
                BackgroundMediaPlayer.Current.Play();
            }
            else if (MediaPlayerState.Closed == BackgroundMediaPlayer.Current.CurrentState)
            {
                StartBackgroundAudioTask();
            }
        }

        /// <summary>
        /// Tells the background audio agent to skip to the next track.
        /// </summary>
        public void Next()
        {
            var value = new ValueSet();
            value.Add(Constants.conSkipNext, "");
            BackgroundMediaPlayer.SendMessageToBackground(value);
        }

        #endregion Button Click Event Handlers
#else
        #region Field
        private MediaElement mediaPlay = null;
        private string fileName = string.Empty;
        private SystemMediaTransportControls systemControls;
        private static String[] tracks = { "ms-appx:///Assets/Media/Ring01.wma", 
                                          "ms-appx:///Assets/Media/Ring02.wma",
                                          "ms-appx:///Assets/Media/Ring03.wma"
                                };
        private int CurrentTrackId = -1;
        /// <summary>
        /// Get the current track name
        /// </summary>
        public string CurrentTrackName
        {
            get
            {
                if (CurrentTrackId == -1)
                {
                    return String.Empty;
                }
                if (CurrentTrackId < tracks.Length)
                {
                    string fullUrl = tracks[CurrentTrackId];
                    return fullUrl.Split('/')[fullUrl.Split('/').Length - 1];
                }
                else
                    throw new ArgumentOutOfRangeException("Track Id is higher than total number of tracks");
            }
        }

        public AudioTaskManager(MediaElement mediaElement)
        {
            this.mediaPlay = mediaElement;
            if (this.mediaPlay != null)
            {
                this.mediaPlay.AudioCategory = Windows.UI.Xaml.Media.AudioCategory.BackgroundCapableMedia;
                this.mediaPlay.CurrentStateChanged += mediaPlay_CurrentStateChanged;
                this.mediaPlay.MediaOpened += mediaPlay_MediaOpened;
                this.mediaPlay.MediaEnded += mediaPlay_MediaEnded;
                this.mediaPlay.MediaFailed += mediaPlay_MediaFailed;
            }
            systemControls = SystemMediaTransportControls.GetForCurrentView();
            systemControls.ButtonPressed += systemControls_ButtonPressed;
            systemControls.PropertyChanged += systemControls_PropertyChanged;
            systemControls.IsEnabled = true;
            systemControls.IsPauseEnabled = true;
            systemControls.IsPlayEnabled = true;
            systemControls.IsNextEnabled = true;
            systemControls.IsPreviousEnabled = true;
        }
        #endregion

        #region methods
        void systemControls_PropertyChanged(SystemMediaTransportControls sender, SystemMediaTransportControlsPropertyChangedEventArgs args)
        {
        }

        private async void systemControls_ButtonPressed(SystemMediaTransportControls sender, SystemMediaTransportControlsButtonPressedEventArgs args)
        {
            await this.page.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                switch (args.Button)
                {
                    case SystemMediaTransportControlsButton.Play:
                        StartPlayback();
                        break;
                    case SystemMediaTransportControlsButton.Pause:
                        try
                        {
                            this.mediaPlay.Pause();
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
            });
        }

        private void mediaPlay_MediaFailed(object sender, ExceptionRoutedEventArgs e)
        {
            Debug.WriteLine("Failed with error message " + e.ErrorMessage.ToString());
        }

        private void mediaPlay_MediaEnded(object sender, RoutedEventArgs e)
        {
            SkipToNext();
        }

        private void mediaPlay_MediaOpened(object sender, RoutedEventArgs e)
        {
            systemControls.PlaybackStatus = MediaPlaybackStatus.Playing;
            systemControls.DisplayUpdater.Type = MediaPlaybackType.Music;
            systemControls.DisplayUpdater.MusicProperties.Title = !string.IsNullOrEmpty(this.fileName) ? this.fileName : CurrentTrackName;
            systemControls.DisplayUpdater.Update();
            (sender as MediaElement).Play();
        }

        private void mediaPlay_CurrentStateChanged(object sender, RoutedEventArgs e)
        {
            switch (this.mediaPlay.CurrentState)
            {
                case MediaElementState.Playing:
                    this.systemControls.PlaybackStatus = MediaPlaybackStatus.Playing;
                    break;
                case MediaElementState.Paused:
                    this.systemControls.PlaybackStatus = MediaPlaybackStatus.Paused;
                    break;
                case MediaElementState.Stopped:
                    this.systemControls.PlaybackStatus = MediaPlaybackStatus.Stopped;
                    break;
                case MediaElementState.Closed:
                    this.systemControls.PlaybackStatus = MediaPlaybackStatus.Closed;
                    break;
                default:
                    break;
            }
        }

        private void StartPlayback()
        {
            try
            {
                if (this.CurrentTrackName == string.Empty)
                {
                    PlayAllTracks();
                }
                else
                {
                    this.mediaPlay.Play();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
        }

        /// <summary>
        /// Starts track at given position in the track list
        /// </summary>
        private void StartTrackAt(int id)
        {
            string source = tracks[id];
            CurrentTrackId = id;
            this.mediaPlay.AutoPlay = false;
            this.mediaPlay.Source = new Uri(source);
        }

        /// <summary>
        /// Starts a given track by finding its name
        /// </summary>
        public void StartTrackAt(string TrackName)
        {
            for (int i = 0; i < tracks.Length; i++)
            {
                if (tracks[i].Contains(TrackName))
                {
                    string source = tracks[i];
                    CurrentTrackId = i;
                    this.mediaPlay.AutoPlay = false;
                    this.mediaPlay.Source = new Uri(source);
                    break;
                }
            }
        }

        /// <summary>
        /// Starts a given track by finding its name and at desired position
        /// </summary>
        public void StartTrackAt(string TrackName, TimeSpan position)
        {
            for (int i = 0; i < tracks.Length; i++)
            {
                if (tracks[i].Contains(TrackName))
                {
                    CurrentTrackId = i;
                    break;
                }
            }
            this.mediaPlay.AutoPlay = false;
            this.mediaPlay.Source = new Uri(tracks[CurrentTrackId]);
            this.mediaPlay.Position = position;
        }

        /// <summary>
        /// Play all tracks in the list starting with 0 
        /// </summary>
        public void PlayAllTracks()
        {
            StartTrackAt(0);
        }

        /// <summary>
        /// Skip to next track
        /// </summary>
        public void SkipToNext()
        {
            StartTrackAt((CurrentTrackId + 1) % tracks.Length);
        }

        /// <summary>
        /// Skip to next track
        /// </summary>
        public void SkipToPrevious()
        {
            if (CurrentTrackId == 0)
            {
                StartTrackAt(CurrentTrackId);
            }
            else
            {
                StartTrackAt(CurrentTrackId - 1);
            }
        }

        public async void StartFile(StorageFile file)
        {
            this.fileName = file.Name;
            mediaPlay.AutoPlay = false;
            var stram = await file.OpenAsync(FileAccessMode.Read);
            mediaPlay.SetSource(stram, file.FileType);
        }

        #endregion
#endif


    }
}
