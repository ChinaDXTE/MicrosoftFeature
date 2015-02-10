using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Maps;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkID=390556 上有介绍

namespace GeofenceApp
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MapPage : Page
    {
        CurrentLocationIcon locationIcon;
        AddGeofenceIcon addGeofenceIcon;

        public static Geoposition CurrentPosition;

        public MapPage()
        {
            this.InitializeComponent();
            this.NavigationCacheMode = NavigationCacheMode.Required;

            locationIcon = new CurrentLocationIcon();
            myMap.Children.Add(locationIcon);
            locationIcon.Visibility = Visibility.Collapsed;

            GlobalGeolocator.Instance.addPositionChangedEvent(async (sender, args) =>
            {
                await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, new DispatchedHandler(() =>
                {
                    CurrentPosition = args.Position;
                    setLocation(args.Position);
                    updateTile(args.Position);
                }));
            });

            addGeofenceIcon = new AddGeofenceIcon((center) =>
            {
                HideAddGeofenceIcon();
                //导航到添加地理围栏页面
                MainPage.Current.ContentFrame.Navigate(typeof(AddGeofencePage), center);
            });

            myMap.Children.Add(addGeofenceIcon);
            HideAddGeofenceIcon();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            HideAddGeofenceIcon();
        }

        private void myMap_MapTapped(MapControl sender, MapInputEventArgs args)
        {
            ShowAddGeofenceIcon(args.Location);
        }

        private void ShowAddGeofenceIcon(Geopoint loc)
        {
            addGeofenceIcon.GeofenceCenter = loc;
            MapControl.SetLocation(addGeofenceIcon, loc);
            addGeofenceIcon.Visibility = Visibility.Visible;
        }
        private void HideAddGeofenceIcon()
        {
            addGeofenceIcon.Visibility = Visibility.Collapsed;
        }

        private async void setLocation(Geoposition position)
        {
            Geopoint point = new Geopoint(new BasicGeoposition()
            {
                Latitude = position.Coordinate.Point.Position.Latitude,
                Longitude = position.Coordinate.Point.Position.Longitude
            });
            MapControl.SetLocation(locationIcon, point);
            locationIcon.Visibility = Visibility.Visible;
            await myMap.TrySetViewAsync(point, 15);
        }

        private void updateTile(Geoposition position)
        {
            Global.Current.Notifications.setImageNumber();
            string content = string.Format("\nLatitude:{0},  Longitude:{1}\nTime:{2}", position.Coordinate.Point.Position.Latitude, position.Coordinate.Point.Position.Longitude, position.Coordinate.Timestamp.DateTime);
            Global.Current.Notifications.CreateTileWide310x150PeekImage01("Location", content);
        }
    }
}
