using System;
using System.Collections.Generic;
using System.Text;
using Windows.ApplicationModel.Resources;
using Windows.Foundation.Collections;
using System.ComponentModel;
using Windows.UI.Xaml;

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

        #region strings Resources

        public string camera_PhotoEditing
        {
            get
            {
                return CurrentResourceLoader.GetString("camera_PhotoEditing");
            }
        }

        public string camera_Camera
        {
            get
            {
                return CurrentResourceLoader.GetString("camera_Camera");
            }
        }

        public string camera_Setting
        {
            get
            {
                return CurrentResourceLoader.GetString("camera_Setting");
            }
        }

        public string camera_GeneralSetting
        {
            get
            {
                return CurrentResourceLoader.GetString("camera_GeneralSetting");
            }
        }

        public string camera_English
        {
            get
            {
                return CurrentResourceLoader.GetString("camera_English");
            }
        }

        public string camera_Chinese
        {
            get
            {
                return CurrentResourceLoader.GetString("camera_Chinese");
            }
        }

        public string camera_SD
        {
            get
            {
                return CurrentResourceLoader.GetString("camera_SD");
            }
        }

        public string camera_PictureLibrary
        {
            get
            {
                return CurrentResourceLoader.GetString("camera_PictureLibrary");
            }
        }

        public string camera_ClearMessage
        {
            get
            {
                return CurrentResourceLoader.GetString("camera_ClearMessage");
            }
        }

        public string camera_CurrentLanguage
        {
            get 
            {
                if (this.camera_region.Contains("CN"))
                    return this.camera_Chinese;
                else
                    return this.camera_English;
            }
        }

        private string camera_currentLocationValue = "roll";
        public string camera_CurrentLocationValue
        {
            set
            {
                this.camera_currentLocationValue = value;
                this.NotifyPropertyChanged("camera_CurrentLocation");
            }
            get
            {
                return this.camera_currentLocationValue;
            }
        }

        public string camera_CurrentLocation
        {
            get 
            {
                if (this.camera_CurrentLocationValue == "roll")
                    return this.camera_PictureLibrary;
                else
                    return this.camera_SD;
            }
        }

        public string camera_Tip
        {
            get
            {
                return CurrentResourceLoader.GetString("camera_Tip");
            }
        }

        public string camera_SaveSuccess
        {
            get
            {
                return CurrentResourceLoader.GetString("camera_SaveSuccess");
            }
        }

        public string camera_NoSD
        {
            get
            {
                return CurrentResourceLoader.GetString("camera_NoSD");
            }
        }

        public string camera_SaveError
        {
            get
            {
                return CurrentResourceLoader.GetString("camera_SaveError");
            }
        }

        public string camera_TileTitle
        {
            get
            {
                return CurrentResourceLoader.GetString("camera_TileTitle");
            }
        }

        public string camera_TileMessage
        {
            get
            {
                return CurrentResourceLoader.GetString("camera_TileMessage");
            }
        }

        public string camera_LanguageSetting
        {
            get
            {
                return CurrentResourceLoader.GetString("camera_LanguageSetting");
            }
        }

        public string camera_SavePath
        {
            get
            {
                return CurrentResourceLoader.GetString("camera_SavePath");
            }
        }

        public string camera_CameraSetting
        {
            get
            {
                return CurrentResourceLoader.GetString("camera_CameraSetting");
            }
        }

        public string camera_AppName
        {
            get
            {
                return CurrentResourceLoader.GetString("camera_AppName");
            }
        }

        public string camera_ScreenCapture
        {
            get
            {
                return CurrentResourceLoader.GetString("camera_ScreenCapture");
            }
        }

        #endregion

        private string camera_region = Windows.Globalization.ApplicationLanguages.Languages[0];//Windows.System.UserProfile.GlobalizationPreferences.HomeGeographicRegion;
        public string camera_CurrentRegion
        {
            get { return camera_region; }
            set 
            {
                this.camera_region = value;
                this.resetRegion(this.camera_region);
            }
        }

        private void resetRegion(string region)
        {
            this.camera_region = region;
            string language = string.Empty;
            if (region.Contains("CN"))
            {
                language = "zh-CN";
            }
            else
                language = "en-US";
            Windows.Globalization.ApplicationLanguages.PrimaryLanguageOverride = language;
            Windows.ApplicationModel.Resources.Core.ResourceContext.GetForCurrentView().Reset();
#if WINDOWS_PHONE_APP
            this.NotifyPropertyChanged("camera_PhotoEditing");
            this.NotifyPropertyChanged("camera_Camera");
            this.NotifyPropertyChanged("camera_Setting");
            this.NotifyPropertyChanged("camera_GeneralSetting");
            this.NotifyPropertyChanged("camera_English");
            this.NotifyPropertyChanged("camera_Chinese");
            this.NotifyPropertyChanged("camera_SD");
            this.NotifyPropertyChanged("camera_PictureLibrary");
            this.NotifyPropertyChanged("camera_ClearMessage");
            this.NotifyPropertyChanged("camera_CurrentLanguage");
            this.NotifyPropertyChanged("camera_CurrentLocation");
            this.NotifyPropertyChanged("camera_Tip");
            this.NotifyPropertyChanged("camera_SaveSuccess");
            this.NotifyPropertyChanged("camera_SaveError");
            this.NotifyPropertyChanged("camera_TileTitle");
            this.NotifyPropertyChanged("camera_TileMessage");
            this.NotifyPropertyChanged("camera_LanguageSetting");
            this.NotifyPropertyChanged("camera_SavePath");
            this.NotifyPropertyChanged("camera_ScreenCapture");
            this.NotifyPropertyChanged("camera_AppName");
#endif
        }

        public void Updata()
        {
            this.NotifyPropertyChanged("camera_PhotoEditing");
            this.NotifyPropertyChanged("camera_Camera");
            this.NotifyPropertyChanged("camera_Setting");
            this.NotifyPropertyChanged("camera_GeneralSetting");
            this.NotifyPropertyChanged("camera_English");
            this.NotifyPropertyChanged("camera_Chinese");
            this.NotifyPropertyChanged("camera_SD");
            this.NotifyPropertyChanged("camera_PictureLibrary");
            this.NotifyPropertyChanged("camera_ClearMessage");
            this.NotifyPropertyChanged("camera_CurrentLanguage");
            this.NotifyPropertyChanged("camera_CurrentLocation");
            this.NotifyPropertyChanged("camera_Tip");
            this.NotifyPropertyChanged("camera_SaveSuccess");
            this.NotifyPropertyChanged("camera_SaveError");
            this.NotifyPropertyChanged("camera_TileTitle");
            this.NotifyPropertyChanged("camera_TileMessage");
            this.NotifyPropertyChanged("camera_LanguageSetting");
            this.NotifyPropertyChanged("camera_SavePath");
            this.NotifyPropertyChanged("camera_ScreenCapture");
            this.NotifyPropertyChanged("camera_AppName");
        }
    }
}
