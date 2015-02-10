using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Feature;
using Windows.Devices.Geolocation;

// “用户控件”项模板在 http://go.microsoft.com/fwlink/?LinkId=234236 上提供

namespace GeofenceApp
{
    public sealed partial class SettingPanel : UserControl
    {
        public SettingPanel()
        {
            this.InitializeComponent();
            this.DataContext = Global.Current.Globalization;
        }

        private async void MenuFlyoutItem_Click(object sender, RoutedEventArgs e)
        {
            var language = (sender as MenuFlyoutItem).Tag.ToString();
            Global.Current.Globalization.CurrentRegion = language;
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Low, () => 
            {
                Global.Current.Globalization.updateProperty();
            });
        }

        private void ToggleSwitch_Toggled(object sender, RoutedEventArgs e)
        {
            if((sender as ToggleSwitch).IsOn)
            {
                GeofenceBackgroundTaskManager.Register();
            }
            else
            {
                GeofenceBackgroundTaskManager.Unregister();
            }
        }

        private async void share_Click(object sender, RoutedEventArgs e)
        {
            Geoposition position=MapPage.CurrentPosition;
            if(position==null)
            {
                position = await GlobalGeolocator.Instance.doLocate();
            }
            string shareText = Global.Current.Globalization.geo_Latitude + "：" + position.Coordinate.Point.Position.Latitude + " , " + Global.Current.Globalization.geo_Longitude + "：" + position.Coordinate.Point.Position.Longitude;
            shareText = string.Format("{0}:{1}", Global.Current.Globalization.geo_MyLocation, shareText);
            shareText.RegisterForShare(Global.Current.Globalization.geo_Share, "");
        }

#if WINDOWS_PHONE_APP
        private void clearToast_Click(object sender, RoutedEventArgs e)
        {
            Global.Current.Notifications.ClearToastNotifier();
        }
#endif
    }
}
