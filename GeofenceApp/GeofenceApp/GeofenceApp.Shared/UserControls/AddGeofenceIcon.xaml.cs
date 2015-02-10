using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// “用户控件”项模板在 http://go.microsoft.com/fwlink/?LinkId=234236 上提供

namespace GeofenceApp
{
    public delegate void AddGeofenceDelegate(Geopoint center);

    public sealed partial class AddGeofenceIcon : UserControl
    {
        public AddGeofenceIcon()
        {
            this.InitializeComponent();
            this.DataContext = Global.Current.Globalization;
        }

        private AddGeofenceDelegate addGeofence;
        public Geopoint GeofenceCenter { get; set; }

        public AddGeofenceIcon(AddGeofenceDelegate addGeofenceCallback)
        {
            InitializeComponent();
            addGeofence = addGeofenceCallback;
        }

        private void grid_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (addGeofence != null)
            {
                addGeofence(GeofenceCenter);
                e.Handled = true;
            }
        }
    }
}
