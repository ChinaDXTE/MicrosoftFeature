using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Devices.Geolocation;
using Windows.Devices.Geolocation.Geofencing;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
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
    public sealed partial class AddGeofencePage : Page
    {
        public AddGeofencePage()
        {
            this.InitializeComponent();
            this.DataContext = Global.Current.Globalization;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            Geopoint point = e.Parameter as Geopoint;
            txb_latitude.Text = point.Position.Latitude.ToString();
            txb_longitude.Text = point.Position.Longitude.ToString();
        }

        //创建geofence按钮响应事件
        private void btn_create_Click(object sender, RoutedEventArgs e)
        {
            string name = txb_identifier.Text;
            double lat;
            double longit;
            double radius;
            if (name == "")
            {
                MessageDialogWithLanguage.showDialog("请输入名称", "Please input the name!");
                return;
            }
            if(!(Double.TryParse(txb_latitude.Text,out lat) && Double.TryParse(txb_longitude.Text,out longit) && Double.TryParse(txb_radius.Text,out radius)))
            {
                MessageDialogWithLanguage.showDialog("数据格式不正确", "The data format is not correct");
                return;
            }
            if(lat<0 || lat>90)
            {
                MessageDialogWithLanguage.showDialog("纬度超出范围，应在0-90之间！", "Latitude out of scope, should be between 0-90！");
                return;
            }
            if(longit>180 || longit<-180)
            {
                MessageDialogWithLanguage.showDialog("经度超出范围，应在-180-180之间！", "Longitude out of scope, should be between -180-180！");
                return;
            }
            if (GeofenceManager.Instance.CreateGeofence(name, lat, longit, radius, async (geofence, newState) =>
            {
                await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () => stateChangedCallback(geofence, newState));
            }))
            {
                MessageDialogWithLanguage.showDialog("创建成功", "Create success!");
                GoBack();
            }
            else
            {
                MessageDialogWithLanguage.showDialog("该名称已存在，请尝试其他名称！", "This name has been used. Please try with anthor name!");
            } 
        }

        //取消按钮响应事件
        private void btn_cancel_Click(object sender, RoutedEventArgs e)
        {
            GoBack();
        }

        //返回上一页
        private void GoBack()
        {
            if (MainPage.Current.ContentFrame.CanGoBack)
            {
                MainPage.Current.ContentFrame.GoBack();
            }
        }

        //geofence状态改变时的回调函数
        private void stateChangedCallback(Geofence g, GeofenceState s)
        {
            Debug.WriteLine(string.Format("您已{0}“{1}”区域", s == GeofenceState.Entered ? "进入" : "离开", g.Id));
            
            string message = "";
            if (Global.Current.Globalization.geo_CurrentLanguage == Global.Current.Globalization.geo_Chinese)
            {
                message = string.Format("您已{0}“{1}”区域", s == GeofenceState.Entered ? "进入" : "离开", g.Id);
            }
            else
            {
                message = string.Format("You have {0} the geofence region of \"{1}\"", s == GeofenceState.Entered ? "entered" : "left", g.Id);
            }
            //弹Toast消息
            Global.Current.Notifications.CreateToastNotifier("", message);
            //更新锁屏信息
            Global.Current.Notifications.UpdateBadgeWithNumber(1);
            //更新磁贴信息
            Global.Current.Notifications.CreateTileWide310x150PeekImage01("Geofence", message);
        }
    }
}
