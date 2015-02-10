using System;
using System.Collections.Generic;
using System.Text;
using Windows.Storage;

namespace Feature
{
    public class LocalSettings
    {
        private const string conCacheKey = "WindowsStore_cachedata";

        /// <summary>
        /// LoadData from LocalSetting
        /// </summary>
        public object LoadData(string key)
        {
            object value = null;
            ApplicationDataCompositeValue dict = null;
            ApplicationDataContainer appsetting = ApplicationData.Current.LocalSettings;
            if (appsetting.Values.ContainsKey(conCacheKey))
                dict = (ApplicationDataCompositeValue)appsetting.Values[conCacheKey];
            if (dict != null)
            {
                if (dict.ContainsKey(key))
                    value = dict[key].ToString();
            }
            return value;
        }

        /// <summary>
        /// SaveData to LocalSetting
        /// </summary>
        public void SaveData(string key,object value)
        {
            ApplicationDataContainer appsetting = ApplicationData.Current.LocalSettings;
            ApplicationDataCompositeValue dict = new ApplicationDataCompositeValue();
            if (appsetting.Values.ContainsKey(conCacheKey))
                dict = (ApplicationDataCompositeValue)appsetting.Values[key];
            if(dict == null)
                dict = new ApplicationDataCompositeValue();
            dict[key] = value;
            appsetting.Values[conCacheKey] = dict;
        }
    }

    class Constants
    {
        public const string conCurrentTrack = "trackname";
        public const string conBackgroundTaskStarted = "conBackgroundTaskStarted";
        public const string conBackgroundTaskRunning = "conBackgroundTaskRunning";
        public const string conBackgroundTaskCancelled = "conBackgroundTaskCancelled";
        public const string conAppSuspended = "appsuspend";
        public const string conAppResumed = "appresumed";
        public const string conStartPlayback = "startplayback";
        public const string conSkipNext = "skipnext";
        public const string conPosition = "position";
        public const string conAppState = "appstate";
        public const string conBackgroundTaskState = "backgroundtaskstate";
        public const string conSkipPrevious = "skipprevious";
        public const string conTrackchanged = "songchanged";
        public const string conForegroundAppActive = "Active";
        public const string conForegroundAppSuspended = "Suspended";
        public const string conCurrentFile = "currentselectfile";
    }
}
