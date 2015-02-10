using System;
using System.Collections.Generic;
using System.Text;
using Feature.NotificationsEx;

namespace Video_Sample.Common
{
    public class Notifications
    {
        public static void CreateTile(string title, string sub)
        {

            var tileContent = TileContentFactory.CreateTileSquare150x150Block();
            tileContent.TextBlock.Text = title;
            tileContent.TextSubBlock.Text = sub;
            var tileNotification = tileContent.CreateNotification();
            Windows.UI.Notifications.TileUpdateManager.CreateTileUpdaterForApplication().Update(tileNotification);
        }
    }
}
