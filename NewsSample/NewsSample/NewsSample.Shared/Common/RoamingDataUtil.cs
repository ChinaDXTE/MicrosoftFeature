using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Storage;
using Windows.UI.Core;
using Windows.UI.Xaml;

namespace NewsSample.Common
{
    public class RoamingDataUtil
    {
        private const string conCacheKey = "Feature_Shared_Roamingdata";
        public TypedEventHandler<ApplicationData, object> dataChangedHandler;

        public RoamingDataUtil()
        {
            ApplicationData.Current.DataChanged += async (d, o) =>
                {
                    //Notice: Should execute in ui thread
                    if (this.dataChangedHandler != null)
                        this.dataChangedHandler(d, o);
                };
        }

        /// <summary>
        /// LoadData from LocalSetting
        /// </summary>
        public string LoadData(string key)
        {
            string value = string.Empty;
            ApplicationDataCompositeValue dict = null;
            ApplicationDataContainer appsetting = ApplicationData.Current.RoamingSettings;
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
        public void SaveData(string key, string value)
        {
            ApplicationDataContainer appsetting = ApplicationData.Current.RoamingSettings;
            ApplicationDataCompositeValue dict = new ApplicationDataCompositeValue();
            if (appsetting.Values.ContainsKey(conCacheKey))
                dict = (ApplicationDataCompositeValue)appsetting.Values[key];
            dict[key] = value;
            appsetting.Values[key] = dict;
            ApplicationData.Current.SignalDataChanged();
        }

        public async Task<string> Save(string source, string filename)
        {
            try
            {
                StorageFolder storageFolder = ApplicationData.Current.RoamingFolder;
                if (!string.IsNullOrEmpty(filename))
                {
                    filename = filename.Replace("/", "\\");
                    string foldername = string.Empty;
                    if (filename.LastIndexOf("\\") > 0)
                        foldername = filename.Substring(0, filename.LastIndexOf("\\"));
                    if (!string.IsNullOrEmpty(foldername))
                    {
                        string[] arrdirectory = foldername.Split('\\');
                        string currdirectory = "";
                        for (int i = 0; i < arrdirectory.Length; i++)
                        {
                            currdirectory = currdirectory + (i == 0 ? "" : "\\") + arrdirectory[i];
                            await storageFolder.CreateFolderAsync(currdirectory, CreationCollisionOption.OpenIfExists);
                        }
                    }
                    StorageFile storageFile = await storageFolder.CreateFileAsync(filename, CreationCollisionOption.ReplaceExisting);
                    await FileIO.WriteTextAsync(storageFile, source);
                    ApplicationData.Current.SignalDataChanged();
                }
                return filename.Replace("\\", "/");
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return string.Empty;
            }
        }

        public async Task<string> GetContent(string filename)
        {
            try
            {
                filename = filename.Replace("/", "\\");
                string content = string.Empty;
                StorageFolder storageFolder = ApplicationData.Current.RoamingFolder;
                StorageFile storageFile = await storageFolder.GetFileAsync(filename);
                content = await FileIO.ReadTextAsync(storageFile);
                return content;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return string.Empty;
            }
        }
    }
}
