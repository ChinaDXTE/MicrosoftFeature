
//条件编译，由于测试的时候无法使用roaming存储，所以使用local setting作为存储使用，到正式发布的时候再直接切换到roaming
//使用一个条件编译，可以避免发布的时候切换出现问题。

#define LOCALSETTINGS
//#define ROAMING

using NewsSample.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Resources;

namespace NewsSample.Common
{
    public class ReadListUtil
    {
        public static void SaveReadId(string id)
        {
#if LOCALSETTINGS
            string readKey = Global.Current.LocalSettings.LoadData("ReadKey") as string;
#endif

#if ROAMING
            string readKey = Global.Current.Roaming.LoadData("ReadKey") as string;
#endif
            if (string.IsNullOrEmpty(readKey))
            {
                readKey = id;
            }
            else if (readKey.IndexOf(id) < 0) 
            {
                readKey = readKey + "," + id;
            }

#if LOCALSETTINGS
            Global.Current.LocalSettings.SaveData("ReadKey", readKey);
#endif
#if ROAMING
            Global.Current.Roaming.SaveData("ReadKey", readKey);
#endif
        }

        public static string LoadReadId()
        {
#if ROAMING
            string readKey = Global.Current.Roaming.LoadData("ReadKey") as string;
#endif
#if LOCALSETTINGS
            string readKey = Global.Current.LocalSettings.LoadData("ReadKey") as string;
#endif
            return string.IsNullOrEmpty(readKey) ? "" : readKey;
        }
        public static bool IsRead(string id)
        {
#if ROAMING
            string readKey = Global.Current.Roaming.LoadData("ReadKey") as string;
#endif
#if LOCALSETTINGS
            string readKey = Global.Current.LocalSettings.LoadData("ReadKey") as string;
#endif
            return string.IsNullOrEmpty(readKey) ? false : readKey.IndexOf(id) >= 0;
        }

        public static void AddFavId(string id)
        {
#if LOCALSETTINGS
            string readKey = Global.Current.LocalSettings.LoadData("FavKey") as string;
#endif
#if ROAMING
            string readKey = Global.Current.Roaming.LoadData("FavKey") as string;
#endif
            if(string.IsNullOrEmpty(readKey))
            {
                readKey = id;
            }
            else if (readKey.IndexOf(id) < 0)
            {
                readKey = readKey + "," + id;
            }

#if LOCALSETTINGS
            Global.Current.LocalSettings.SaveData("FavKey", readKey);
#endif
#if ROAMING
            Global.Current.Roaming.SaveData("ReadKey", readKey);
#endif
        }
        public static void RemoveFavId(string id)
        {
#if LOCALSETTINGS
            string readKey = Global.Current.LocalSettings.LoadData("FavKey") as string;
#endif
#if ROAMING
            string readKey = Global.Current.Roaming.LoadData("FavKey") as string;
#endif
            if (string.IsNullOrEmpty(readKey) || (readKey.IndexOf(id) < 0))
            {
                return;
            }
            else
            {
                if (readKey.IndexOf(id + ",") >= 0)
                {
                    readKey = readKey.Remove(readKey.IndexOf(id), id.Length + 1);
                }
                else
                {
                    readKey = readKey.Remove(readKey.IndexOf(id), id.Length);
                }
#if LOCALSETTINGS
                Global.Current.LocalSettings.SaveData("FavKey", readKey);
#endif
#if ROAMING
                Global.Current.Roaming.SaveData("FavKey", readKey);
#endif
            }
        }
        private static string LoadFavId()
        {
#if ROAMING
            string readKey = Global.Current.Roaming.LoadData("FavKey") as string;
#endif
#if LOCALSETTINGS
            string readKey = Global.Current.LocalSettings.LoadData("FavKey") as string;
#endif
            return string.IsNullOrEmpty(readKey) ? "" : readKey;
        }
        public static bool IsFav(string id)
        {
#if ROAMING
            string readKey = Global.Current.Roaming.LoadData("FavKey") as string;
#endif
#if LOCALSETTINGS
            string readKey = Global.Current.LocalSettings.LoadData("FavKey") as string;
#endif
            return string.IsNullOrEmpty(readKey) ? false : readKey.IndexOf(id) >= 0;
        }
        public static async Task<SampleDataGroup> GetReadList()
        {
            ObservableCollection<SampleDataItem> list = new ObservableCollection<SampleDataItem>();
            string readIdStr = LoadReadId();
            var sampleDataGroups = await SampleDataSource.GetGroupsAsync();
            foreach (var group in sampleDataGroups)
            {
                foreach (var item in group.Items)
                {
                    if (readIdStr.IndexOf(item.UniqueId) >= 0)
                    {
                        list.Add(item);
                    }
                }
            }
            var resourceLoader = ResourceLoader.GetForCurrentView("Resources");
            SampleDataGroup groupData = new SampleDataGroup("ReadListId", resourceLoader.GetString("ReadListTitle"), resourceLoader.GetString("ReadListSubtitle"), "ms-appx:///Assets/DarkGray.png", "ReadListDesc");
            groupData.Items = list;
            return groupData;
        }
        public static async Task<SampleDataGroup> GetFavList()
        {
            ObservableCollection<SampleDataItem> list = new ObservableCollection<SampleDataItem>();
            string favIdStr = LoadFavId();
            var sampleDataGroups = await SampleDataSource.GetGroupsAsync();
            foreach (var group in sampleDataGroups)
            {
                foreach (var item in group.Items)
                {
                    if (favIdStr.IndexOf(item.UniqueId) >= 0)
                    {
                        list.Add(item);
                    }
                }
            }
            var resourceLoader = ResourceLoader.GetForCurrentView("Resources");
            SampleDataGroup groupData = new SampleDataGroup("FavListId", resourceLoader.GetString("FavListTitle"), resourceLoader.GetString("FavListSubtitle"), "ms-appx:///Assets/DarkGray.png", "FavListDesc");
            groupData.Items = list;
            return groupData;
        }
    }
}
