using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Windows.Data.Json;

namespace Feature.DataModel
{
    public class SampleItem : INotifyPropertyChanged
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

        #region field data
        
        /// <summary>
        /// 列表图片
        /// </summary>
        private string image = string.Empty;
        public string Image
        {
            get { return this.image; }
            set 
            { 
                this.image = value;
                this.NotifyPropertyChanged("Image");
            }
        }
        /// <summary>
        /// 标题
        /// </summary>
        private string title = string.Empty;
        public string Title
        {
            get { return this.title; }
            set
            {
                this.title = value;
                this.NotifyPropertyChanged("Title");
            }
        }
        /// <summary>
        /// 地址
        /// </summary>
        private string address = string.Empty;
        public string Address
        {
            get { return this.address; }
            set
            {
                this.address = value;
                this.NotifyPropertyChanged("Address");
            }
        }
        /// <summary>
        /// 价格
        /// </summary>
        private string price = string.Empty;
        public string Price
        {
            get { return this.price; }
            set
            {
                this.price = value;
                this.NotifyPropertyChanged("Price");
            }
        }
        /// <summary>
        /// 原价
        /// </summary>
        private string originalprice = string.Empty;
        public string OriginalPrice
        {
            get { return this.originalprice; }
            set
            {
                this.originalprice = value;
                this.NotifyPropertyChanged("OriginalPrice");
            }
        }
        /// <summary>
        /// 已售
        /// </summary>
        private string sold = string.Empty;
        public string Sold
        {
            get { return this.sold; }
            set
            {
                this.sold = value;
                this.NotifyPropertyChanged("Sold");
            }
        }
        /// <summary>
        /// 距离
        /// </summary>
        private string range = string.Empty;
        public string Range
        {
            get { return this.range; }
            set
            {
                this.range = value;
                this.NotifyPropertyChanged("Range");
            }
        }

        /// <summary>
        /// 电话号
        /// </summary>
        private string phoneNum = string.Empty;
        public string PhoneNum
        {
            get { return this.phoneNum; }
            set 
            {
                this.phoneNum = value;
                this.NotifyPropertyChanged("PhoneNum");
            }
        }

        /// <summary>
        /// 价格
        /// </summary>
        private double priceNum = 0;
        public double PriceNum
        {
            get { return priceNum; }
            set 
            { 
                this.priceNum = value;
                this.Price = this.priceNum + "元";
            }
        }

        /// <summary>
        /// 导演
        /// </summary>
        private string director = string.Empty;
        public string Director
        {
            get { return this.director; }
            set 
            {
                this.director = value;
                this.NotifyPropertyChanged("Director");
            }
        }

        /// <summary>
        /// 类型
        /// </summary>
        private string type = string.Empty;
        public string Type
        {
            get { return this.type; }
            set
            {
                this.type = value;
                this.NotifyPropertyChanged("Type");
            }
        }

        /// <summary>
        /// 地区
        /// </summary>
        private string location = string.Empty;
        public string Location
        {
            get { return this.location; }
            set
            {
                this.location = value;
                this.NotifyPropertyChanged("Location");
            }
        }

        /// <summary>
        /// 时长
        /// </summary>
        private string time = string.Empty;
        public string Time
        {
            get { return this.time; }
            set
            {
                this.time = value;
                this.NotifyPropertyChanged("Time");
            }
        }

        /// <summary>
        /// 上映时间
        /// </summary>
        private string publicdate = string.Empty;
        public string Publicdate
        {
            get { return this.publicdate; }
            set
            {
                this.publicdate = value;
                this.NotifyPropertyChanged("Publicdate");
            }
        }

        /// <summary>
        /// 主要演员
        /// </summary>
        private string mainactor = string.Empty;
        public string Mainactor
        {
            get { return this.mainactor; }
            set
            {
                this.mainactor = value;
                this.NotifyPropertyChanged("Mainactor");
            }
        }

        /// <summary>
        /// 剧情描述
        /// </summary>
        private string story = string.Empty;
        public string Story
        {
            get { return this.story; }
            set
            {
                this.story = value;
                this.NotifyPropertyChanged("Story");
            }
        }

        /// <summary>
        /// 开始时间
        /// </summary>
        private string beginTime = string.Empty;
        public string BeginTime
        {
            get { return this.beginTime; }
            set 
            {
                this.beginTime = value;
                this.NotifyPropertyChanged("BeginTime");
            }
        }

        /// <summary>
        /// 结束时间
        /// </summary>
        private string endTime = string.Empty;
        public string EndTime
        {
            get { return this.endTime; }
            set
            {
                this.endTime = value;
                this.NotifyPropertyChanged("EndTime");
            }
        }

        /// <summary>
        /// 放映厅
        /// </summary>
        private string hall = string.Empty;
        public string Hall
        {
            get { return this.hall; }
            set
            {
                this.hall = value;
                this.NotifyPropertyChanged("Hall");
            }
        }
        /// <summary>
        /// 操作项
        /// </summary>
        public string Option { get; set; }

        public string TitleGroup { get; set; }
        #endregion

        public SampleItem(string image,string title,string address,string price,string originalprice,string sold,string range)
        {
            this.Image = image;
            this.Title = title;
            this.Address = address;
            this.Price = price + "元";
            if (string.IsNullOrEmpty(price))
                price = "0";
            this.PriceNum = double.Parse(price);
            this.OriginalPrice = originalprice + "元";
            this.Sold = sold;
            this.Range = range;
            this.PhoneNum = "010-88888888";
        }

        public SampleItem(string image, string title, string director, string type, string location, string time, string publicdate,string mainactor,string story)
        {
            this.Image = image;
            this.Title = title;
            this.Director ="导演：" +  director;
            this.Type = "类型：" + type;
            this.Location = "地区：" + location;
            this.Time = "时长：" + time;
            this.Publicdate ="上映时间：" + publicdate;
            this.Mainactor = mainactor;
            this.Story = story;
        }

        public SampleItem(string begintime, string endtime, string type, string hall, string price, string originalprece)
        {
            this.BeginTime = begintime;
            this.EndTime = endtime;
            this.Type = type;
            this.Hall = hall;
            this.Price = price + "元";
            this.OriginalPrice = originalprece + "元";
            if (string.IsNullOrEmpty(price))
                price = "0";
            this.PriceNum = double.Parse(price);
            this.Option = "选座购票";
        }

        public SampleItem()
        { }

        public SampleItem(string content)
        {
            JsonObject jObject = JsonObject.Parse(content);
            if (jObject != null)
            {
                this.Address = jObject["Address"] != null ? jObject["Address"].GetString() : "";
                this.BeginTime = jObject["BeginTime"] != null ? jObject["BeginTime"].GetString() : "";
                this.Director = jObject["Director"] != null ? jObject["Director"].GetString() : "";
                this.EndTime = jObject["EndTime"] != null ? jObject["EndTime"].GetString() : "";
                this.Hall = jObject["Hall"] != null ? jObject["Hall"].GetString() : "";
                this.Image = jObject["Image"] != null ? jObject["Image"].GetString() : "";
                this.Location = jObject["Location"] != null ? jObject["Location"].GetString() : "";
                this.Mainactor = jObject["Mainactor"] != null ? jObject["Mainactor"].GetString() : "";
                this.OriginalPrice = jObject["OriginalPrice"] != null ? jObject["OriginalPrice"].GetString() : "";
                this.PhoneNum = jObject["PhoneNum"] != null ? jObject["PhoneNum"].GetString() : "";
                string priceNum = jObject["PriceNum"] != null ? jObject["PriceNum"].GetString() : "0.0";
                this.PriceNum = Double.Parse(priceNum);
                this.Price = jObject["Price"] != null ? jObject["Price"].GetString() : "";
                this.Publicdate = jObject["Publicdate"] != null ? jObject["Publicdate"].GetString() : "";
                this.Range = jObject["Range"] != null ? jObject["Range"].GetString() : "";
                this.Sold = jObject["Sold"] != null ? jObject["Sold"].GetString() : "";
                this.Story = jObject["Story"] != null ? jObject["Story"].GetString() : "";
                this.Time = jObject["Time"] != null ? jObject["Time"].GetString() : "";
                this.Title = jObject["Title"] != null ? jObject["Title"].GetString() : "";
                this.Type = jObject["Type"] != null ? jObject["Type"].GetString() : "";
            }
        }

        public string Organizate()
        {
            string result = "{";
            result += "\"Address\":\"" + this.Address + "\",";
            result += "\"BeginTime\":\"" + this.BeginTime + "\",";
            result += "\"Director\":\"" + this.Director + "\",";
            result += "\"EndTime\":\"" + this.EndTime + "\",";
            result += "\"Hall\":\"" + this.Hall + "\",";
            result += "\"Image\":\"" + this.Image + "\",";
            result += "\"Location\":\"" + this.Location + "\",";
            result += "\"Mainactor\":\"" + this.Mainactor + "\",";
            result += "\"OriginalPrice\":\"" + this.OriginalPrice + "\",";
            result += "\"PhoneNum\":\"" + this.PhoneNum + "\",";
            result += "\"Price\":\"" + this.Price + "\",";
            result += "\"PriceNum\":\"" + this.PriceNum + "\",";
            result += "\"Publicdate\":\"" + this.Publicdate + "\",";
            result += "\"Range\":\"" + this.Range + "\",";
            result += "\"Sold\":\"" + this.Sold + "\",";
            result += "\"Story\":\"" + this.Story + "\",";
            result += "\"Time\":\"" + this.Time + "\",";
            result += "\"Title\":\"" + this.Title + "\",";
            result += "\"Type\":\"" + this.Type + "\"";
            result += "}";
            return result;
        }

        public void Update(SampleItem sampleItem)
        {
            this.Address = sampleItem.Address;
            this.BeginTime = sampleItem.BeginTime;
            this.Director = sampleItem.Director;
            this.EndTime = sampleItem.EndTime;
            this.Hall = sampleItem.Hall;
            this.Image = sampleItem.Image;
            this.Location = sampleItem.Location;
            this.Mainactor = sampleItem.Mainactor;
            this.OriginalPrice = sampleItem.OriginalPrice;
            this.PhoneNum = sampleItem.PhoneNum;
            this.Price = sampleItem.Price;
            this.PriceNum = sampleItem.PriceNum;
            this.Publicdate = sampleItem.Publicdate;
            this.Range = sampleItem.Range;
            this.Sold = sampleItem.Sold;
            this.Story = sampleItem.Story;
            this.Time = sampleItem.Time;
            this.Title = sampleItem.Title;
            this.Type = sampleItem.Type;
        }

    }
}
