using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace SampleBackgroundAudioTask
{
    /// <summary>
    /// Collection of string constants used in the entire solution. This file is shared for all projects
    /// </summary>
    public static class ApplicationSettingsHelper
    {
        /// <summary>
        /// 函数读取设置值，读取后清除
        /// </summary>
        public static object ReadResetSettingsValue(string key)
        {
            Debug.WriteLine(key);
            if (!ApplicationData.Current.LocalSettings.Values.ContainsKey(key))
            {
                Debug.WriteLine("null returned");
                return null;
            }
            else
            {
                var value = ApplicationData.Current.LocalSettings.Values[key];
                //ApplicationData.Current.LocalSettings.Values.Remove(key);
                Debug.WriteLine("value found " + value.ToString());
                return value;
            }
        }

        /// <summary>
        /// 把保存键值对的设置内。如果它不存在创建，
        /// </summary>
        public static void SaveSettingsValue(string key, object value)
        {
            Debug.WriteLine(key + ":" + value.ToString());
            if (!ApplicationData.Current.LocalSettings.Values.ContainsKey(key))
            {
                ApplicationData.Current.LocalSettings.Values.Add(key, value);
            }
            else
            {
                ApplicationData.Current.LocalSettings.Values[key] = value;
            }
        }

    }
}
