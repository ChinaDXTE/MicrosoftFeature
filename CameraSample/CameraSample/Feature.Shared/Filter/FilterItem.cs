using Nokia.Graphics.Imaging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Windows.UI;
using Windows.UI.Xaml.Media;

namespace Feature.Filter
{
    public class FilterItem : INotifyPropertyChanged
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

        /// <summary>
        /// filtername
        /// </summary>
        private string filtername = string.Empty;
        public string FilterName
        {
            get { return this.filtername; }
            set 
            { 
                this.filtername = value;
                this.NotifyPropertyChanged("FilterName");
            }
        }
        /// <summary>
        /// English Name
        /// </summary>
        public string EnglishName { get; set; }
        /// <summary>
        /// China Name
        /// </summary>
        public string ChinaName { get; set; }

        /// <summary>
        /// filter
        /// </summary>
        private IFilter filter = null;
        public IFilter Filter
        {
            get { return this.filter; }
            set { this.filter = value; }
        }
        /// <summary>
        /// is Select
        /// </summary>
        private bool isSelect = false;
        public bool IsSelect
        {
            get { return this.isSelect; }
            set 
            {
                this.isSelect = value;
                if(this.isSelect)
                    this.Backgroundcolor = new SolidColorBrush(Color.FromArgb(255, 173, 165, 180));
                else
                    this.Backgroundcolor = new SolidColorBrush(Colors.Transparent);
            }
        }

        /// <summary>
        /// Backgroundcolor
        /// </summary>
        private SolidColorBrush backgroundcolor = new SolidColorBrush(Colors.Transparent);
        public SolidColorBrush Backgroundcolor
        {
            get { return this.backgroundcolor; }
            set 
            { 
                this.backgroundcolor = value;
                this.NotifyPropertyChanged("Backgroundcolor");
            }
        }

        public FilterItem(string englishName,string chinaName,IFilter filter,bool isSelect = false)
        {
            this.EnglishName = englishName;
            this.ChinaName = chinaName;
            if (Global.Current.Globalization.camera_CurrentRegion.Contains("CN"))
                this.FilterName = chinaName;
            else
                this.FilterName = englishName;
            this.IsSelect = isSelect;
            this.Filter = filter;
        }
    }
}
