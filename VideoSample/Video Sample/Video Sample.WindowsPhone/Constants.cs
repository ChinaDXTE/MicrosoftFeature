using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleBackgroundAudioTask
{
    /// <summary>
    /// Collection of string constants used in the entire solution. This file is shared for all projects
    /// </summary>
    class Constants
    {
        public const string CurrentTrack = "trackname";
        public const string BackgroundTaskStarted = "BackgroundTaskStarted";
        public const string BackgroundTaskRunning = "BackgroundTaskRunning";
        public const string BackgroundTaskCancelled = "BackgroundTaskCancelled";
        public const string AppSuspended = "appsuspend";
        public const string AppResumed = "appresumed";
        public const string StartPlayback = "startplayback";
        public const string SkipNext = "skipnext";
        public const string Position = "position";
        public const string AppState = "appstate";
        public const string BackgroundTaskState = "backgroundtaskstate";
        public const string SkipPrevious = "skipprevious";
        public const string Trackchanged = "songchanged";
        public const string Trackcomplete = "songcomplete";
        public const string Trackat = "Trackat";
        public const string ForegroundAppActive = "Active";
        public const string ForegroundAppSuspended = "Suspended";
        public const string conCurrentFile = "currentselectfile";
        public const string conPlaySingleFile = "playsinglefile";
        public const string conPlayByIndex = "playbyindex";
    }
}
