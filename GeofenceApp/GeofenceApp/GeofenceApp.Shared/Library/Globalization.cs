using System;
using System.Collections.Generic;
using System.Text;
using Windows.ApplicationModel.Resources;
using Windows.Foundation.Collections;
using System.ComponentModel;
using Windows.UI.Xaml;
using System.Threading.Tasks;

namespace Feature
{
    public class Globalization:INotifyPropertyChanged
    {
        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void NotifyPropertyChanged(string propertyName)
        {
            var eventHandler = this.PropertyChanged;
            if (eventHandler != null && !string.IsNullOrEmpty(propertyName))
            {
                eventHandler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion

        public ResourceLoader CurrentResourceLoader
        {
            get
            {
                return ResourceLoader.GetForCurrentView();
            }
        }

        #region fiField   updata for Globalization


        public string geo_NoItemHint
        {
            get
            {
                return CurrentResourceLoader.GetString("geo_NoItemHint");
            }
        }
        public string geo_RadiusPlaceholderText
        {
            get
            {
                return CurrentResourceLoader.GetString("geo_RadiusPlaceholderText");
            }
        }

        public string geo_GeofenceList
        {
            get
            {
                return CurrentResourceLoader.GetString("geo_GeofenceList");
            }
        }
        public string geo_Remove
        {
            get
            {
                return CurrentResourceLoader.GetString("geo_Remove");
            }
        }

        public string geo_ClearToast
        {
            get
            {
                return CurrentResourceLoader.GetString("geo_ClearToast");
            }
        }
        public string geo_Share
        {
            get
            {
                return CurrentResourceLoader.GetString("geo_Share");
            }
        }
        public string geo_MyLocation
        {
            get
            {
                return CurrentResourceLoader.GetString("geo_MyLocation");
            }
        }
        public string geo_Language
        {
            get
            {
                return CurrentResourceLoader.GetString("geo_Language");
            }
        }
        public string geo_CurrentLanguage
        {
            get
            {
                if (this.region.Contains("CN"))
                    return this.geo_Chinese;
                else
                    return this.geo_English;
            }
        }
        public string geo_Chinese
        {
            get
            {
                return CurrentResourceLoader.GetString("geo_Chinese");
            }
        }
        public string geo_English
        {
            get
            {
                return CurrentResourceLoader.GetString("geo_English");
            }
        }
        public string geo_Setting
        {
            get
            {
                return CurrentResourceLoader.GetString("geo_Setting");
            }
        }
        public string geo_ShowGeofenceListButtonText
        {
            get
            {
                return CurrentResourceLoader.GetString("geo_ShowGeofenceListButtonText");
            }
        }

        public string geo_BackgroundGeofence
        {
            get
            {
                return CurrentResourceLoader.GetString("geo_BackgroundGeofence");
            }
        }

        public string geo_AddGeofencePageTitle
        {
            get
            {
                return CurrentResourceLoader.GetString("geo_AddGeofencePageTitle");
            }
        }

        public string geo_Create
        {
            get
            {
                return CurrentResourceLoader.GetString("geo_Create");
            }
        }

        public string geo_Cancel
        {
            get
            {
                return CurrentResourceLoader.GetString("geo_Cancel");
            }
        }

        public string geo_AddGeofenceIconText
        {
            get
            {
                return CurrentResourceLoader.GetString("geo_AddGeofenceIconText");
            }
        }

        public string geo_Latitude
        {
            get
            {
                return CurrentResourceLoader.GetString("geo_Latitude");
            }
        }

        public string geo_Longitude
        {
            get
            {
                return CurrentResourceLoader.GetString("geo_Longitude");
            }
        }

        public string geo_Radius
        {
            get
            {
                return CurrentResourceLoader.GetString("geo_Radius");
            }
        }

        public string geo_Name
        {
            get
            {
                return CurrentResourceLoader.GetString("geo_Name");
            }
        }
        #endregion

        private string region = Windows.Globalization.ApplicationLanguages.Languages[0];
        public string CurrentRegion
        {
            get { return region; }
            set 
            {
                this.region = value;
                this.resetRegion(this.region);
            }
        }

        private void resetRegion(string region)
        {
            this.region = region;
            string language = string.Empty;
            if (region.Contains("CN"))
            {
                language = "zh-cn";
            }
            else
                language = "en-us";
            Windows.Globalization.ApplicationLanguages.PrimaryLanguageOverride = language;
            Windows.ApplicationModel.Resources.Core.ResourceContext.GetForCurrentView().Reset();
            updateProperty();
        }

        public void updateProperty()
        {
            this.NotifyPropertyChanged("geo_NoItemHint");
            this.NotifyPropertyChanged("geo_RadiusPlaceholderText");
            this.NotifyPropertyChanged("geo_GeofenceList");
            this.NotifyPropertyChanged("geo_Remove");
            this.NotifyPropertyChanged("geo_ClearToast");
            this.NotifyPropertyChanged("geo_Share");
            this.NotifyPropertyChanged("geo_MyLocation");
            this.NotifyPropertyChanged("geo_CurrentLanguage");
            this.NotifyPropertyChanged("geo_Language");
            this.NotifyPropertyChanged("geo_Chinese");
            this.NotifyPropertyChanged("geo_English");
            this.NotifyPropertyChanged("geo_Setting");
            this.NotifyPropertyChanged("geo_ShowGeofenceListButtonText");
            this.NotifyPropertyChanged("geo_BackgroundGeofence");
            this.NotifyPropertyChanged("geo_Cancel");
            this.NotifyPropertyChanged("geo_Create");
            this.NotifyPropertyChanged("geo_AddGeofencePageTitle");
            this.NotifyPropertyChanged("geo_AddGeofenceIconText");
            this.NotifyPropertyChanged("geo_Latitude");
            this.NotifyPropertyChanged("geo_Longitude");
            this.NotifyPropertyChanged("geo_Radius");
            this.NotifyPropertyChanged("geo_Name");
        }
    }
}
