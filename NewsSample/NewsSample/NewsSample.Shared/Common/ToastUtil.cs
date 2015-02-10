using System;
using System.Collections.Generic;
using System.Text;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml;

namespace NewsSample.Common
{
    public class ToastUtil
    {
        static DispatcherTimer dt;

        ToastUtil()
        {
        }

        static void dt_Tick(object sender, object e)
        {
#if WINDOWS_PHONE_APP
            Global.Current.Notifications.ClearToastNotifier();
#endif
            dt.Stop();
        }
        //发出toast通知
        public static void ShowToast(string title, string describe)
        {
            if (dt == null)
            {
                dt = new DispatcherTimer() { Interval = TimeSpan.FromSeconds(4) };
                dt.Tick += dt_Tick;
            }
            else
            {
                dt.Stop();
            }

            var resourceLoader = ResourceLoader.GetForCurrentView("Resources");
#if WINDOWS_PHONE_APP
            Global.Current.Notifications.ClearToastNotifier();
#endif
            Global.Current.Notifications.CreateToastNotifier(title, describe, "tag", "group");
            //Clear Toast notification by a dispatcher timer, interval is 4 seconds
            dt.Start();
        }
    }
}
