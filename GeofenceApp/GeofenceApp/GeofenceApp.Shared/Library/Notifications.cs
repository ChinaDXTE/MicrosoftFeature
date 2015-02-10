using Feature.NotificationsEx;
using Feature.NotificationsEx.ToastContent;
using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI.Notifications;
using Windows.UI.StartScreen;
using Windows.UI.Xaml;

namespace Feature
{
    public class Notifications
    {
        public void CreateToastNotifier(string title, string describe,string tag = null,string group = null)
        {
            var toast = this.getToastNotification(title, describe);
#if  WINDOWS_PHONE_APP
            if (!string.IsNullOrEmpty(tag))
                toast.Tag = tag;
            if (!string.IsNullOrEmpty(group))
                toast.Group = group;
#endif
            Windows.UI.Notifications.ToastNotificationManager.CreateToastNotifier().Show(toast);
        }

        private ToastNotification getToastNotification(string title, string describe)
        {
            var toastContent = ToastContentFactory.CreateToastText02();
            toastContent.TextHeading.Text = title;
            toastContent.TextBodyWrap.Text = describe;
            toastContent.Audio.Content = ToastAudioContent.LoopingAlarm;
            toastContent.Audio.Loop = true;
            toastContent.Duration = ToastDuration.Long;
            var toast = toastContent.CreateNotification();
            toast.ExpirationTime = null;
            return toast;
        }

#if WINDOWS_PHONE_APP
        public void ClearToastNotifier()
        {
            ToastNotificationManager.History.Clear();
        }

        public void RemoveToastNotifier(string group)
        {
            ToastNotificationManager.History.RemoveGroup(group);
        }

        public void RemoveToastNotifier(string tag,string group)
        {
            ToastNotificationManager.History.Remove(tag,group);
        }
#endif

        #region MainTile
        
        public void UpdateBadgeWithNumber(int number)
        {
            BadgeNumericNotificationContent badgeContent = new BadgeNumericNotificationContent((uint)number);
            BadgeUpdateManager.CreateBadgeUpdaterForApplication().Update(badgeContent.CreateNotification());
        }

        public void CreateTileSquare150x150Block(string title, string sub)
        {
            var tileContent = TileContentFactory.CreateTileSquare150x150Block();
            tileContent.TextBlock.Text = title;
            tileContent.TextSubBlock.Text = sub;
            var tileNotification = tileContent.CreateNotification();
            Windows.UI.Notifications.TileUpdateManager.CreateTileUpdaterForApplication().Update(tileNotification);
        }

        public void CreateTileSquare150x150PeekImageAndText02(string backTitle,string backContent)
        {
            var tileContent = TileContentFactory.CreateTileSquare150x150PeekImageAndText02();
            tileContent.TextHeading.Text = backTitle;
            tileContent.TextBodyWrap.Text = backContent;
            tileContent.Image.Src = "ms-appx:///Images/"+imageNum+"/150_150.jpg";
            var tileNotification = tileContent.CreateNotification();
            Windows.UI.Notifications.TileUpdateManager.CreateTileUpdaterForApplication().Update(tileNotification);
        }

        public void CreateTileWide310x150PeekImageCollection01(string backTitle,string backContent)
        {
            var tileContent = TileContentFactory.CreateTileWide310x150PeekImageCollection01();
            tileContent.ImageMain.Src = "";
            tileContent.TextHeading.Text = backTitle;
            tileContent.TextBodyWrap.Text = backContent;
            tileContent.ImageSmallColumn1Row1.Src = "";
            tileContent.ImageSmallColumn1Row2.Src = "";
            tileContent.ImageSmallColumn2Row1.Src = "";
            tileContent.ImageSmallColumn2Row2.Src = "";
            tileContent.RequireSquare150x150Content = false;
            var tileNotification = tileContent.CreateNotification();
            Windows.UI.Notifications.TileUpdateManager.CreateTileUpdaterForApplication().Update(tileNotification);
        }

        public void CreateTileWide310x150PeekImage01(string backTitle,string backContent)
        {
            var tileContent = TileContentFactory.CreateTileWide310x150PeekImage01();
            tileContent.Image.Src = "ms-appx:///Images/"+imageNum+"/310_150.jpg";
            tileContent.TextHeading.Text = backTitle;
            tileContent.TextBodyWrap.Text = backContent;
            tileContent.RequireSquare150x150Content = true;
            tileContent.Square150x150Content = get150x150Content(backTitle, backContent);
            var tileNotification = tileContent.CreateNotification();
            Windows.UI.Notifications.TileUpdateManager.CreateTileUpdaterForApplication().Update(tileNotification);
        }

        public void CreateTileWide310x150PeekImage05(string backTitle, string backContent)
        {
            var tileContent = TileContentFactory.CreateTileWide310x150PeekImage05();
            tileContent.TextHeading.Text = backTitle;
            tileContent.TextBodyWrap.Text = backContent;
            tileContent.ImageMain.Src = "";
            tileContent.ImageSecondary.Src = "";
            tileContent.RequireSquare150x150Content = false;
            var tileNotification = tileContent.CreateNotification();
            Windows.UI.Notifications.TileUpdateManager.CreateTileUpdaterForApplication().Update(tileNotification);
        }

        public void CreateTileWide310x150Text09(string title,string content)
        {
            var tileContent = TileContentFactory.CreateTileWide310x150Text09();
            tileContent.TextHeading.Text = title;
            tileContent.TextBodyWrap.Text = content;
            tileContent.RequireSquare150x150Content = false;
            var tileNotification = tileContent.CreateNotification();
            Windows.UI.Notifications.TileUpdateManager.CreateTileUpdaterForApplication().Update(tileNotification);
        }

        public void ClearMainTile()
        {
            TileUpdater updater = TileUpdateManager.CreateTileUpdaterForApplication();
            updater.Clear();
        }
        #endregion

        #region Secondary Tile
        /// <summary>
        /// create secondaryTile
        /// </summary>
        /// <param name="tileID"></param>
        /// <param name="content"></param>
        /// <param name="argument"></param>
        public async void OpenSecondaryTile(string tileID, string title, string argument)
        {
            SecondaryTile secondaryTile = new SecondaryTile(tileID);
            if (SecondaryTile.Exists(tileID))
                await secondaryTile.RequestDeleteAsync();
            secondaryTile = new SecondaryTile(tileID, title, argument, new Uri("ms-appx:///Assets/Tile150x150.png"), TileSize.Default);
            secondaryTile.VisualElements.ShowNameOnSquare150x150Logo = true;
            bool rev = await secondaryTile.RequestCreateAsync();
        }

        public void UpdateSecondaryTile(string tileID, string backtitle, string backcontent)
        {
            if (string.IsNullOrEmpty(tileID) || string.IsNullOrEmpty(backcontent))
                return;
            if (!SecondaryTile.Exists(tileID))
                return;
            var tileContent = TileContentFactory.CreateTileSquare150x150PeekImageAndText02();
            tileContent.TextHeading.Text = backtitle;
            tileContent.TextBodyWrap.Text = backcontent;
            tileContent.Image.Src = "ms-appx:///Assets/Tile150x150.png";
            var tileNotification = tileContent.CreateNotification();
            TileUpdater updater = TileUpdateManager.CreateTileUpdaterForSecondaryTile(tileID);
            updater.EnableNotificationQueue(false);
            updater.Update(tileNotification);
        }

        public void ClearSecondaryTile(string tileID)
        {
            if (!SecondaryTile.Exists(tileID))
                return;
            TileUpdater updater = TileUpdateManager.CreateTileUpdaterForSecondaryTile(tileID);
            updater.Clear();
        }

        public async void DeleteSecondaryTile(string tileID)
        {
            if (!SecondaryTile.Exists(tileID))
                return;
            SecondaryTile secondaryTile = new SecondaryTile(tileID);
            await secondaryTile.RequestDeleteAsync();
        }

        #endregion

        private int imageNum=0;
        public int setImageNumber()
        {
            return imageNum = (imageNum + 1) % 5;
        }

        private TileSquare150x150PeekImageAndText02 get150x150Content(string title, string content)
        {
            TileSquare150x150PeekImageAndText02 tileContent = TileContentFactory.CreateTileSquare150x150PeekImageAndText02() as TileSquare150x150PeekImageAndText02;
            tileContent.TextHeading.Text = title;
            tileContent.TextBodyWrap.Text = content;
            tileContent.Image.Src = "ms-appx:///Images/" + imageNum + "/150_150.jpg";
            return tileContent;
        }
    }
}
