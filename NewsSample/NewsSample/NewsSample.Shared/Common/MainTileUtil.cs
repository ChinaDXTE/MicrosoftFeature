using System;
using System.Collections.Generic;
using System.Text;

namespace NewsSample.Common
{
    public class MainTileUtil
    {
        public static void UpdateMainTile(string title, string desc)
        {
            Global.Current.Notifications.CreateTileSquare150x150PeekImageAndText02(title, desc);
            Global.Current.Notifications.CreateTileWide310x150PeekImage01(title, desc);
        }
    }
}
