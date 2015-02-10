using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Windows.Data.Json;
using Windows.Storage;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using NewsSample.Common;

// 此文件定义的数据模型可充当在添加、移除或修改成员时
// 。  所选属性名称与标准项模板中的数据绑定一致。
//
// 应用程序可以使用此模型作为起始点并以它为基础构建，或完全放弃它并
// 替换为适合其需求的其他内容。如果使用此模式，则可提高应用程序
// 响应速度，途径是首次启动应用程序时启动 App.xaml 隐藏代码中的数据加载任务
//。

namespace NewsSample.Data
{
    /// <summary>
    /// 泛型项数据模型。
    /// </summary>
    public class SampleDataItem : INotifyPropertyChanged
    {
        public SampleDataItem(String uniqueId, String title, String subtitle, String imagePath, String description, String content, bool read)
        {
            this.UniqueId = uniqueId;
            this.Title = title;
            this.Subtitle = subtitle;
            this.Description = description;
            this.ImagePath = imagePath;
            this.Content = content;
            this.Read = read;
        }
        private string _uniqueId;
        public string UniqueId 
        { 
            get 
            { 
                return _uniqueId; 
            }
            private set
            {
                if (value == _uniqueId) return;
                _uniqueId = value;
                NotifyPropertyChange("UniqueId");
            }
        }
        private string _title;
        public string Title 
        {
            get
            {
                return _title;
            }
            private set
            {
                if (value == _title) return;
                _title = value;
                NotifyPropertyChange("Title");
            }
        }
        private string _subtitle;
        public string Subtitle 
        {
            get 
            {
                return _subtitle;
            }
            private set
            {
                if (value == _subtitle) return;
                _subtitle = value;
                NotifyPropertyChange("Subtitle");
            }
        }
        private string _discription;
        public string Description
        {
            get
            {
                return _discription;
            }
            private set
            {
                if (value == _discription) return;
                _discription = value;
                NotifyPropertyChange("Description");
            }
        }
        private string _imagePath;
        public string ImagePath 
        {
            get 
            {
                return _imagePath;
            }
            private set 
            {
                if (value == _imagePath) return;
                _imagePath = value;
                NotifyPropertyChange("ImagePath");
            }
        }
        private string _content;
        public string Content 
        {
            get 
            {
                return _content;
            }
            private set 
            {
                if (value == _content) return;
                _content = value;
                NotifyPropertyChange("Content");
            }
        }
        private bool _read;
        public bool Read
        {
            get
            {
                return _read;
            }
            set
            {
                if (value == _read) return;
                _read = value;
                NotifyPropertyChange("Read");
            }
        }

        public override string ToString()
        {
            return this.Title;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChange(string propertyName)
        {
            if (PropertyChanged != null)
            {
                //根据PropertyChanged事件的委托类，实现PropertyChanged事件：
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }

    /// <summary>
    /// 泛型组数据模型。
    /// </summary>
    public class SampleDataGroup : INotifyPropertyChanged
    {
        public SampleDataGroup(String uniqueId, String title, String subtitle, String imagePath, String description)
        {
            this.UniqueId = uniqueId;
            this.Title = title;
            this.Subtitle = subtitle;
            this.Description = description;
            this.ImagePath = imagePath;
            this.Items = new ObservableCollection<SampleDataItem>();
        }

        private string _uniqueId;
        public string UniqueId
        {
            get
            {
                return _uniqueId;
            }
            private set
            {
                if (value == _uniqueId) return;
                _uniqueId = value;
                NotifyPropertyChange("UniqueId");
            }
        }
        private string _title;
        public string Title
        {
            get
            {
                return _title;
            }
            private set
            {
                if (value == _title) return;
                _title = value;
                NotifyPropertyChange("Title");
            }
        }
        private string _subtitle;
        public string Subtitle
        {
            get
            {
                return _subtitle;
            }
            private set
            {
                if (value == _subtitle) return;
                _subtitle = value;
                NotifyPropertyChange("Subtitle");
            }
        }
        private string _discription;
        public string Description
        {
            get
            {
                return _discription;
            }
            private set
            {
                if (value == _discription) return;
                _discription = value;
                NotifyPropertyChange("Description");
            }
        }
        private string _imagePath;
        public string ImagePath
        {
            get
            {
                return _imagePath;
            }
            private set
            {
                if (value == _imagePath) return;
                _imagePath = value;
                NotifyPropertyChange("ImagePath");
            }
        }
        public ObservableCollection<SampleDataItem> Items { get; set; }

        public override string ToString()
        {
            return this.Title;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChange(string propertyName)
        {
            if (PropertyChanged != null)
            {
                //根据PropertyChanged事件的委托类，实现PropertyChanged事件：
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }

    /// <summary>
    /// 创建包含从静态 json 文件读取内容的组和项的集合。
    /// 
    /// SampleDataSource 通过从项目中包括的静态 json 文件读取的数据进行
    /// 初始化。 这样在设计时和运行时均可提供示例数据。
    /// </summary>
    public sealed class SampleDataSource
    {
        private static SampleDataSource _sampleDataSource = new SampleDataSource();

        private ObservableCollection<SampleDataGroup> _groups = new ObservableCollection<SampleDataGroup>();
        public ObservableCollection<SampleDataGroup> Groups
        {
            get { return this._groups; }
        }

        public static async Task<ObservableCollection<SampleDataGroup>> GetGroupsAsync()
        {
            await _sampleDataSource.GetSampleDataAsync();

            return _sampleDataSource.Groups;
        }

        public static async Task<SampleDataGroup> GetGroupAsync(string uniqueId)
        {
            await _sampleDataSource.GetSampleDataAsync();
            // 对于小型数据集可接受简单线性搜索
            var matches = _sampleDataSource.Groups.Where((group) => group.UniqueId.Equals(uniqueId));
            if (matches.Count() == 1) return matches.First();
            return null;
        }

        public static async Task<SampleDataItem> GetItemAsync(string uniqueId)
        {
            await _sampleDataSource.GetSampleDataAsync();
            // 对于小型数据集可接受简单线性搜索
            var matches = _sampleDataSource.Groups.SelectMany(group => group.Items).Where((item) => item.UniqueId.Equals(uniqueId));
            if (matches.Count() == 1) return matches.First();
            return null;
        }

        private async Task GetSampleDataAsync()
        {
            if (this._groups.Count != 0)
                return;

            Uri dataUri;
            if (Windows.Globalization.ApplicationLanguages.Languages[0] == "zh-Hans-CN")
            {
                dataUri = new Uri("ms-appx:///DataModel/SampleDataCN.json");
            }
            else
            {
                dataUri = new Uri("ms-appx:///DataModel/SampleData.json");
            }

            StorageFile file = await StorageFile.GetFileFromApplicationUriAsync(dataUri);
            string jsonText = await FileIO.ReadTextAsync(file);
            JsonObject jsonObject = JsonObject.Parse(jsonText);
            JsonArray jsonArray = jsonObject["Groups"].GetArray();

            foreach (JsonValue groupValue in jsonArray)
            {
                JsonObject groupObject = groupValue.GetObject();
                SampleDataGroup group = new SampleDataGroup(groupObject["UniqueId"].GetString(),
                                                            groupObject["Title"].GetString(),
                                                            groupObject["Subtitle"].GetString(),
                                                            groupObject["ImagePath"].GetString(),
                                                            groupObject["Description"].GetString());

                foreach (JsonValue itemValue in groupObject["Items"].GetArray())
                {
                    JsonObject itemObject = itemValue.GetObject();
                    group.Items.Add(new SampleDataItem(itemObject["UniqueId"].GetString(),
                                                       itemObject["Title"].GetString(),
                                                       itemObject["Subtitle"].GetString(),
                                                       itemObject["ImagePath"].GetString(),
                                                       itemObject["Description"].GetString(),
                                                       itemObject["Content"].GetString(),
                                                       ReadListUtil.IsRead(itemObject["UniqueId"].GetString())
                                                       ));
                }
                this.Groups.Add(group);
            }
        }
    }

    public class ZoomItem
    {
        public string Key { get; set; }

        public object ItemContent { get; set; }
    }

    public class HeroImageItem
    {
        public string HeroTitle { get; set; }
    }
    public class AboutItem
    {
        public string AboutString { get; set; }
        public string tag { get; set; }
    }
}