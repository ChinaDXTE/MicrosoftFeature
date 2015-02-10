using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Windows.Devices.Geolocation;
using Windows.Devices.Geolocation.Geofencing;
using Windows.Foundation;
using Windows.UI.Popups;

namespace GeofenceApp
{
    public delegate void GeofenceStateChangedCallback(Geofence geofence,GeofenceState newState);
    public class GeofenceManager
    {
        private static GeofenceManager instance;

        public static GeofenceManager Instance
        {
            get
            {
                if(instance==null)
                {
                    instance=new GeofenceManager();
                }
                return instance;
            }
        }

        //存放geofence状态改变时的回调函数，key与GeofenceMonitor.Current.Geofences中geofence的index一致
        private Dictionary<int,GeofenceStateChangedCallback> callbacks = new Dictionary<int,GeofenceStateChangedCallback>();

        public IList<Geofence> GetGeofences()
        {
            return GeofenceMonitor.Current.Geofences;
        }

        //创建Geofence
        public bool CreateGeofence(string id, double lat, double lon, double radius, GeofenceStateChangedCallback callback)
        {
            if (GeofenceMonitor.Current.Geofences.SingleOrDefault(g => g.Id == id) != null) return false;

            var position = new BasicGeoposition();
            position.Latitude = lat;
            position.Longitude = lon;
            var geocircle = new Geocircle(position, radius);

            MonitoredGeofenceStates mask = MonitoredGeofenceStates.Entered | MonitoredGeofenceStates.Exited;

            // Create Geofence with the supplied id, geocircle and mask, not for single use
            // and with a dwell time of 1 seconds
            var geofence = new Geofence(id, geocircle, mask, false, new TimeSpan(0, 0, 1));
            GeofenceMonitor.Current.Geofences.Add(geofence);
            //将回调函数存入geofencesStateChangedCallback
            if (callbacks.ContainsKey(GetGeofenceIndex(geofence)))
            {
                callbacks.Remove(GetGeofenceIndex(geofence));
            }
            callbacks.Add(GetGeofenceIndex(geofence), callback);
            //注册Geofence状态改变回调事件
            GeofenceMonitor.Current.GeofenceStateChanged -= Current_GeofenceStateChanged;
            GeofenceMonitor.Current.GeofenceStateChanged += Current_GeofenceStateChanged;

            return true;
        }

        //移除指定id的Geofence
        public void RemoveGeofence(string id)
        {
            var geofence = GeofenceMonitor.Current.Geofences.SingleOrDefault(g => g.Id == id);

            if (geofence != null)
                GeofenceMonitor.Current.Geofences.Remove(geofence);
        }

        //GeofenceMonitor.Current.GeofenceStateChanged回调
        private void Current_GeofenceStateChanged(GeofenceMonitor sender, object args)
        {
            var geoReports = sender.ReadReports();
            foreach (var report in geoReports)
            {
                Geofence geo = report.Geofence;
                GeofenceState newState = report.NewState;
                GeofenceStateChangedCallback callback;
                try
                {
                    callbacks.TryGetValue(GetGeofenceIndex(geo), out callback);
                    if(callback!=null)
                    {
                        callback(geo, newState);
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        //获取geofence的index
        private int GetGeofenceIndex(Geofence g)
        {
            if (!GeofenceMonitor.Current.Geofences.Contains(g)) return -1;
            return GeofenceMonitor.Current.Geofences.IndexOf(g);
        }
    }
}
