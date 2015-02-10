using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;
using Windows.Devices.Geolocation.Geofencing;
using Windows.UI.Notifications;

namespace BackgroundTask
{
    public sealed class GeofenceTask : IBackgroundTask
    {
        public void Run(IBackgroundTaskInstance taskInstance)
        {
            var reports = GeofenceMonitor.Current.ReadReports();

            foreach (var report in reports)
            {
                //创建toast通知
                var toastXmlContent = ToastNotificationManager.GetTemplateContent(ToastTemplateType.ToastText02);
                var txtNodes = toastXmlContent.GetElementsByTagName("text");
                txtNodes[0].AppendChild(toastXmlContent.CreateTextNode(string.Format("您已{0}“{1}”区域", report.NewState == GeofenceState.Entered ? "进入" : "离开", report.Geofence.Id)));
                txtNodes[1].AppendChild(toastXmlContent.CreateTextNode(report.Geofence.Id));
                var toast = new ToastNotification(toastXmlContent);
                var toastNotifier = ToastNotificationManager.CreateToastNotifier();
                toastNotifier.Show(toast);
            }
        }        
    }
}
