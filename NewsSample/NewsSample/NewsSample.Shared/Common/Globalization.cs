using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Windows.ApplicationModel.Resources;

namespace NewsSample.Common
{
    public class Globalization : INotifyPropertyChanged
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

        public string ManageToast
        {
            get
            {
                return CurrentResourceLoader.GetString("ManageToast");
            }
        }

        public string TileManage
        {
            get
            {
                return CurrentResourceLoader.GetString("TileManage");
            }
        }

        public string ShareContract
        {
            get
            {
                return CurrentResourceLoader.GetString("ShareContract");
            }
        }

        public string FileAccess
        {
            get
            {
                return CurrentResourceLoader.GetString("FileAccess");
            }
        }

        public string FilePicker
        {
            get
            {
                return CurrentResourceLoader.GetString("FilePicker");
            }
        }

        public string Roaming
        {
            get
            {
                return CurrentResourceLoader.GetString("Roaming");
            }
        }

        public string BackgroundTansfer
        {
            get
            {
                return CurrentResourceLoader.GetString("BackgroundTansfer");
            }
        }

        public string Prevent
        {
            get
            {
                return CurrentResourceLoader.GetString("Prevent");
            }
        }

        public string Play
        {
            get
            {
                return CurrentResourceLoader.GetString("Play");
            }
        }

        public string Next
        {
            get
            {
                return CurrentResourceLoader.GetString("Next");
            }
        }

        public string GlobalizationText
        {
            get
            {
                return CurrentResourceLoader.GetString("GlobalizationText");
            }
        }

        #endregion

        private string region = Windows.System.UserProfile.GlobalizationPreferences.HomeGeographicRegion;
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
            this.NotifyPropertyChanged("ManageToast");
            this.NotifyPropertyChanged("TileManage");
            this.NotifyPropertyChanged("ShareContract");
            this.NotifyPropertyChanged("FileAccess");
            this.NotifyPropertyChanged("FilePicker");
            this.NotifyPropertyChanged("Roaming");
            this.NotifyPropertyChanged("BackgroundTansfer");
            this.NotifyPropertyChanged("Prevent");
            this.NotifyPropertyChanged("Play");
            this.NotifyPropertyChanged("Next");
            this.NotifyPropertyChanged("GlobalizationText");
        }


    }
}
