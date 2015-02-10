using System;
using System.Collections.Generic;
using System.Text;
using Windows.ApplicationModel.Resources;
using Windows.Foundation.Collections;
using System.ComponentModel;
using Windows.UI.Xaml;
using Feature.DataModel;

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

        #region Field   updata for Globalization

        public string o2o_AppTitle
        {
            get
            {
                return CurrentResourceLoader.GetString("o2o_AppTitle");
            }
        }

        public string o2o_Category
        {
            get
            {
                return CurrentResourceLoader.GetString("o2o_Category");
            }
        }

        public string o2o_Near
        {
            get
            {
                return CurrentResourceLoader.GetString("o2o_Near");
            }
        }

        public string o2o_Setting
        {
            get
            {
                return CurrentResourceLoader.GetString("o2o_Setting");
            }
        }

        public string o2o_Movie
        {
            get
            {
                return CurrentResourceLoader.GetString("o2o_Movie");
            }
        }

        public string o2o_Food
        {
            get
            {
                return CurrentResourceLoader.GetString("o2o_Food");
            }
        }

        public string o2o_Hotel
        {
            get
            {
                return CurrentResourceLoader.GetString("o2o_Hotel");
            }
        }

        public string o2o_Entertainment
        {
            get
            {
                return CurrentResourceLoader.GetString("o2o_Entertainment");
            }
        }

        public string o2o_Beauty
        {
            get
            {
                return CurrentResourceLoader.GetString("o2o_Beauty");
            }
        }

        public string o2o_Tour
        {
            get
            {
                return CurrentResourceLoader.GetString("o2o_Tour");
            }
        }

        public string o2o_Shop
        {
            get
            {
                return CurrentResourceLoader.GetString("o2o_Shop");
            }
        }

        public string o2o_Life
        {
            get
            {
                return CurrentResourceLoader.GetString("o2o_Life");
            }
        }

        public string o2o_Gift
        {
            get
            {
                return CurrentResourceLoader.GetString("o2o_Gift");
            }
        }

        public string o2o_Lottery
        {
            get
            {
                return CurrentResourceLoader.GetString("o2o_Lottery");
            }
        }

        public string o2o_ShardContract
        {
            get
            {
                return CurrentResourceLoader.GetString("o2o_ShardContract");
            }
        }

        public string o2o_ShowWallet
        {
            get
            {
                return CurrentResourceLoader.GetString("o2o_ShowWallet");
            }
        }

        public string o2o_ClearMessage
        {
            get
            {
                return CurrentResourceLoader.GetString("o2o_ClearMessage");
            }
        }

        public string o2o_English
        {
            get
            {
                return CurrentResourceLoader.GetString("o2o_English");
            }
        }

        public string o2o_Chinese
        {
            get
            {
                return CurrentResourceLoader.GetString("o2o_Chinese");
            }
        }

        public string o2o_O2OSetting
        {
            get
            {
                return CurrentResourceLoader.GetString("o2o_O2OSetting");
            }
        }

        public string o2o_OrderTrack
        {
            get
            {
                return CurrentResourceLoader.GetString("o2o_OrderTrack"); 
            }
        }

        public string o2o_CurrentLanguage
        {
            get
            {
                if (this.o2o_region.Contains("CN"))
                    return this.o2o_Chinese;
                else
                    return this.o2o_English;
            }
        }

        public List<SampleItem> ListBusiness { get; private set; }

        #endregion

        private string o2o_region = Windows.Globalization.ApplicationLanguages.Languages[0];
        public string o2o_CurrentRegion
        {
            get { return o2o_region; }
            set 
            {
                this.o2o_region = value;
                this.resetRegion(this.o2o_region);
            }
        }

        private void resetRegion(string region)
        {
            this.o2o_region = region;
            string language = string.Empty;
            if (region.Contains("CN"))
            {
                language = "zh-cn";
            }
            else
                language = "en-us";
            Windows.Globalization.ApplicationLanguages.PrimaryLanguageOverride = language;
            Windows.ApplicationModel.Resources.Core.ResourceContext.GetForCurrentView().Reset();
            this.NotifyPropertyChanged("o2o_CurrentLanguage");
            this.NotifyPropertyChanged("o2o_AppTitle");
            this.NotifyPropertyChanged("o2o_Category");
            this.NotifyPropertyChanged("o2o_Near");
            this.NotifyPropertyChanged("o2o_Setting");
            this.NotifyPropertyChanged("o2o_Movie");
            this.NotifyPropertyChanged("o2o_Food");
            this.NotifyPropertyChanged("o2o_Hotel");
            this.NotifyPropertyChanged("o2o_Entertainment");
            this.NotifyPropertyChanged("o2o_Beauty");
            this.NotifyPropertyChanged("o2o_Tour");
            this.NotifyPropertyChanged("o2o_Shop");
            this.NotifyPropertyChanged("o2o_Life");
            this.NotifyPropertyChanged("o2o_Gift");
            this.NotifyPropertyChanged("o2o_Lottery");
            this.NotifyPropertyChanged("o2o_ShardContract");
            this.NotifyPropertyChanged("o2o_ShowWallet");
            this.NotifyPropertyChanged("o2o_ClearMessage");
            this.NotifyPropertyChanged("o2o_English");
            this.NotifyPropertyChanged("o2o_Chinese");
            this.NotifyPropertyChanged("o2o_O2OSetting");
            this.NotifyPropertyChanged("o2o_OrderTrack");
        }

        public void Updata()
        {
            this.NotifyPropertyChanged("o2o_CurrentLanguage");
            this.NotifyPropertyChanged("o2o_AppTitle");
            this.NotifyPropertyChanged("o2o_Category");
            this.NotifyPropertyChanged("o2o_Near");
            this.NotifyPropertyChanged("o2o_Setting");
            this.NotifyPropertyChanged("o2o_Movie");
            this.NotifyPropertyChanged("o2o_Food");
            this.NotifyPropertyChanged("o2o_Hotel");
            this.NotifyPropertyChanged("o2o_Entertainment");
            this.NotifyPropertyChanged("o2o_Beauty");
            this.NotifyPropertyChanged("o2o_Tour");
            this.NotifyPropertyChanged("o2o_Shop");
            this.NotifyPropertyChanged("o2o_Life");
            this.NotifyPropertyChanged("o2o_Gift");
            this.NotifyPropertyChanged("o2o_Lottery");
            this.NotifyPropertyChanged("o2o_ShardContract");
            this.NotifyPropertyChanged("o2o_ShowWallet");
            this.NotifyPropertyChanged("o2o_ClearMessage");
            this.NotifyPropertyChanged("o2o_English");
            this.NotifyPropertyChanged("o2o_Chinese");
            this.NotifyPropertyChanged("o2o_O2OSetting");
            this.NotifyPropertyChanged("o2o_OrderTrack");
        }

        public void LoadSampleData(Channel channel)
        {
            SampleItems items = new SampleItems(channel);
            this.ListBusiness = items.List;
            this.NotifyPropertyChanged("ListBusiness");
        }


    }
}
