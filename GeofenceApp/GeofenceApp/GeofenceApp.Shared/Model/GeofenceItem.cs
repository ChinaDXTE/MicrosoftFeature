using System;
using System.Collections.Generic;
using System.Text;
using Windows.Devices.Geolocation;
using Windows.Devices.Geolocation.Geofencing;

namespace GeofenceApp
{
    public class GeofenceItemCollection : System.Collections.ObjectModel.ObservableCollection<GeofenceItem>
    {
    }

    public class GeofenceItem : IEquatable<GeofenceItem>
    {
        private Geofence geofence;

        public GeofenceItem(Geofence geofence)
        {
            this.geofence = geofence;
        }

        public bool Equals(GeofenceItem other)
        {
            bool isEqual = false;
            if (Id == other.Id)
            {
                isEqual = true;
            }

            return isEqual;
        }

        public Windows.Devices.Geolocation.Geofencing.Geofence Geofence
        {
            get
            {
                return geofence;
            }
        }

        public string Id
        {
            get
            {
                return geofence.Id;
            }
        }

        public double Latitude
        {
            get
            {
                Geocircle circle = geofence.Geoshape as Geocircle;
                return circle.Center.Latitude;
            }
        }

        public double Longitude
        {
            get
            {
                Geocircle circle = geofence.Geoshape as Geocircle;
                return circle.Center.Longitude;
            }
        }

        public double Radius
        {
            get
            {
                Geocircle circle = geofence.Geoshape as Geocircle;
                return circle.Radius;
            }
        }

        public bool SingleUse
        {
            get
            {
                return geofence.SingleUse;
            }
        }

        public MonitoredGeofenceStates MonitoredStates
        {
            get
            {
                return geofence.MonitoredStates;
            }
        }

        public TimeSpan DwellTime
        {
            get
            {
                return geofence.DwellTime;
            }
        }

        public DateTimeOffset StartTime
        {
            get
            {
                return geofence.StartTime;
            }
        }

        public TimeSpan Duration
        {
            get
            {
                return geofence.Duration;
            }
        }
    }
}
