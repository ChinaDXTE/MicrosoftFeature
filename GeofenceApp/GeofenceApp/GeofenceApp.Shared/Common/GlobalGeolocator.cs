using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.UI.Popups;

namespace GeofenceApp
{
    public delegate void GeolocatorPositionChanged(Geolocator locator, PositionChangedEventArgs args);
    public delegate void GeolocatorStatusChanged (Geolocator locator, StatusChangedEventArgs args);
    public class GlobalGeolocator
    {
        private static GlobalGeolocator instance = null;
        private Geolocator locator = null;
        private TypedEventHandler<Geolocator, PositionChangedEventArgs> positionChanged;
        private TypedEventHandler<Geolocator, StatusChangedEventArgs> statusChanged;

        public static GlobalGeolocator Instance
        {
            get
            {
                if(instance==null)
                {
                    instance = new GlobalGeolocator();
                }
                return instance;
            }
        }

        private GlobalGeolocator()
        {
            locator = new Geolocator();
#if WINDOWS_PHONE_APP
                    locator.MovementThreshold = 1;
#endif
        }

        public void addPositionChangedEvent(GeolocatorPositionChanged callback)
        {
            if (callback != null)
            {
                if (positionChanged!=null)
                {
                    locator.PositionChanged -= positionChanged; 
                }
                positionChanged = new Windows.Foundation.TypedEventHandler<Geolocator, PositionChangedEventArgs>(callback);
                locator.PositionChanged += positionChanged;
            }
        }

        public void removePositionChangedEvent()
        {
            locator.PositionChanged -= positionChanged;
            positionChanged = null;
        }

        public void addStatusChangedEvent(GeolocatorStatusChanged callback)
        {
            if (callback != null)
            {
                if (statusChanged != null)
                {
                    locator.StatusChanged -= statusChanged;
                }
                statusChanged = new Windows.Foundation.TypedEventHandler<Geolocator, StatusChangedEventArgs>(callback);
                locator.StatusChanged += statusChanged;
            }
        }

        public void removeStatusChangedEvent()
        {
            locator.StatusChanged -= statusChanged;
            statusChanged = null;
        }

        public async Task<Geoposition> doLocate()
        {
            Geoposition result = null;
            try
            {
                result = await locator.GetGeopositionAsync();
            }
            catch (Exception e)
            {
#if WINDOWS_APP
                if (e.HResult == -2147024891)
#endif
#if WINDOWS_PHONE_APP
                    if(e.HResult==-2147467260)
#endif
                {
                    showMessage();
                }
            }
            return result;
        }

        private bool hasShow = false;
        private void showMessage()
        {
            if(!hasShow)
            {
                hasShow = true;
                MessageDialogWithLanguage.showDialog("定位功能没有开启,这将导致应用的定位和地理围栏功能不可用，请通过系统设置开启定位功能！", "The device's location function is not open, this will cause the location and geofence function of this app is not available. Please open through the system setting!");
            }
        }
    }
}
