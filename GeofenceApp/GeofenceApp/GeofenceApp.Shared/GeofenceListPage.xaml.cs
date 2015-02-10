using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Devices.Geolocation.Geofencing;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上提供

namespace GeofenceApp
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class GeofenceListPage : Page
    {
        //RegisteredGeofenceListBox数据源
        private GeofenceItemCollection geoItemCollection;
        public GeofenceListPage()
        {
            this.InitializeComponent();
            this.DataContext = Global.Current.Globalization;
            geoItemCollection = new GeofenceItemCollection();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            //设置RegisteredGeofenceListBox数据源
            FillRegisteredGeofenceListBoxWithhExistingGeofences();
            RegisteredGeofenceListBox.Height = Window.Current.Bounds.Height - 100;
        }

        private void OnRemoveGeofenceItem(object sender, RoutedEventArgs e)
        {
            string name = (sender as FrameworkElement).Tag.ToString();
            //移除该geofence
            GeofenceManager.Instance.RemoveGeofence(name);
            //重新填充GeofenceListBox
            FillRegisteredGeofenceListBoxWithhExistingGeofences();
        }

        //设置geofence listbox的数据源
        private void FillRegisteredGeofenceListBoxWithhExistingGeofences()
        {
            //获取当前所有Geofence
            IList<Geofence> geofences = GeofenceManager.Instance.GetGeofences();
            //清空并重新填充geoItemCollection
            geoItemCollection.Clear();
            foreach (var g in geofences)
            {
                geoItemCollection.Insert(0, new GeofenceItem(g));
            }
            //重新绑定
            RegisteredGeofenceListBox.DataContext = geoItemCollection;

            if(geoItemCollection.Count>0)
            {
                noItemHintTextBlock.Visibility = Visibility.Collapsed;
            }
            else
            {
                noItemHintTextBlock.Visibility = Visibility.Visible;
            }
        }
    }
}
