using System;
using System.Collections.Generic;
using System.Text;
using Windows.Networking.PushNotifications;
using Windows.UI.Notifications;
using GeofenceApp;

namespace Feature
{
    public class Push
    {
        public const string conChannelUriKey = "channelUri";

        public async void Bind()
        {
            PushNotificationChannel pushChannel = await PushNotificationChannelManager.CreatePushNotificationChannelForApplicationAsync();
            if (pushChannel != null && !string.IsNullOrEmpty(pushChannel.Uri))
            {
                pushChannel.PushNotificationReceived -= pushChannel_PushNotificationReceived;
                pushChannel.PushNotificationReceived += pushChannel_PushNotificationReceived;
                string uri = Global.Current.LocalSettings.LoadData(conChannelUriKey) == null ? string.Empty : Global.Current.LocalSettings.LoadData(conChannelUriKey).ToString();
                if (string.IsNullOrEmpty(uri) || (!string.IsNullOrEmpty(pushChannel.Uri) && uri != pushChannel.Uri))
                {
                    Global.Current.LocalSettings.SaveData(conChannelUriKey,pushChannel.Uri);
                    //Updata web service channelUri 
                }
            }
        }

        public async void UnBind()
        {
            PushNotificationChannel pushChannel = await PushNotificationChannelManager.CreatePushNotificationChannelForApplicationAsync();
            if (pushChannel != null)
            {
                pushChannel.PushNotificationReceived -= pushChannel_PushNotificationReceived;
                pushChannel.Close();
                Global.Current.LocalSettings.SaveData(conChannelUriKey, string.Empty);
            }
        }

        private void pushChannel_PushNotificationReceived(PushNotificationChannel sender, PushNotificationReceivedEventArgs args)
        {
            switch (args.NotificationType)
            {
                case PushNotificationType.Badge: // badge Notify
                    BadgeUpdateManager.CreateBadgeUpdaterForApplication().Update(args.BadgeNotification);
                    break;
                case PushNotificationType.Raw: // raw Notify
                    string msg = args.RawNotification.Content;
                    break;
                case PushNotificationType.Tile: // tile Notify
                    TileUpdateManager.CreateTileUpdaterForApplication().Update(args.TileNotification);
                    break;
                case PushNotificationType.Toast: // toast Notify
                    ToastNotificationManager.CreateToastNotifier().Show(args.ToastNotification);
                    break;
                default:
                    break;
            }
            args.Cancel = true;
        }

    }
}
