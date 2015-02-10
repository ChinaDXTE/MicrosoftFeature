using Bing.Maps;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上有介绍

namespace GeofenceApp
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MapPage : Page
    {
        CurrentLocationIcon pushpin;
        AddGeofenceIcon addGeofenceIcon;

        public static Geoposition CurrentPosition;

        public MapPage()
        {
            this.InitializeComponent();
            this.NavigationCacheMode = NavigationCacheMode.Required;

            GlobalGeolocator.Instance.addPositionChangedEvent(async (sender, args) =>
            {
                await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, new DispatchedHandler(() =>
                {
                    CurrentPosition = args.Position;
                    setLocation(args.Position);
                    updateTile(args.Position);
                }));
            });

            pushpin = new CurrentLocationIcon();
            myMap.Children.Add(pushpin);

            addGeofenceIcon = new AddGeofenceIcon((center) =>
            {
                HideAddGeofenceIcon();
                //导航到添加地理围栏页面
                MainPage.Current.ContentFrame.Navigate(typeof(AddGeofencePage),center);
            });
            myMap.Children.Add(addGeofenceIcon);
            HideAddGeofenceIcon();
        }

        private void myMap_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Map map = sender as Map;
            if (map == null) return;
            Point p = e.GetPosition(null);
            Location location=new Location();
            map.TryPixelToLocation(p, out location);
            ShowAddGeofenceIcon(location);
        }

        private void myMap_RightTapped(object sender, RightTappedRoutedEventArgs e)
        {
            HideAddGeofenceIcon();
        }

        private void ShowAddGeofenceIcon(Location loc)
        {
            addGeofenceIcon.GeofenceCenter = LocationToGeopoint(loc);
            MapLayer.SetPosition(addGeofenceIcon, loc);
            addGeofenceIcon.Visibility = Visibility.Visible;
        }
        private void HideAddGeofenceIcon()
        {
            addGeofenceIcon.Visibility = Visibility.Collapsed;
        }

        private Geopoint LocationToGeopoint(Location loc)
        {
            Geopoint result = new Geopoint(new BasicGeoposition() { Latitude = loc.Latitude, Longitude = loc.Longitude });
            return result;
        }

        private Location GeopointToLocation(Geopoint p)
        {
            Location result = new Location(p.Position.Latitude, p.Position.Longitude);
            return result;
        }

        private void setLocation(Geoposition position)
        {
            Location location = new Location(position.Coordinate.Point.Position.Latitude, position.Coordinate.Point.Position.Longitude);
            MapLayer.SetPosition(pushpin, location);
            myMap.SetView(location, 15.0f);
        }

        private void updateTile(Geoposition position)
        {
            Global.Current.Notifications.setImageNumber();
            string content = string.Format("\nLatitude:{0},  Longitude:{1}\nTime:{2}", position.Coordinate.Point.Position.Latitude, position.Coordinate.Point.Position.Longitude, position.Coordinate.Timestamp.DateTime);
            Global.Current.Notifications.CreateTileWide310x150PeekImage01("Location", content);
        }
    }
}
