using System;
using System.Collections.Generic;
using System.Text;
using Windows.ApplicationModel.Resources;

namespace NewsSample.Common
{
    public class TileUitl
    {
        TileUitl() 
        {
        }
        //更新中磁贴和宽磁贴
        public static void UpdateTile(string content)
        {
            var resourceLoader = ResourceLoader.GetForCurrentView("Resources");
           
            //更新宽磁贴
            Global.Current.Notifications.CreateTileWide310x150PeekImage01(resourceLoader.GetString("ReadListTitle"), content);
            //更新中磁贴
            //Global.Current.Notifications.CreateTileSquare150x150PeekImageAndText02(resourceLoader.GetString("ReadListTitle"), content);
        }
        //清除磁贴内容
        public static void ClearTile()
        {
            Global.Current.Notifications.ClearMainTile();
        }
    }
}
