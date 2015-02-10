using BackgroundTask;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Windows.ApplicationModel.Background;
using Windows.UI.Popups;

namespace GeofenceApp
{
    public class GeofenceBackgroundTaskManager
    {
        static string TaskName = "GeofenceTask";

        public async static void Register()
        {
            if (!IsTaskRegistered())
            {
                try
                { 
                    BackgroundAccessStatus backgroundAccessStatus = await BackgroundExecutionManager.RequestAccessAsync();
                    doRegist();
                    switch (backgroundAccessStatus)
                    {
                        case BackgroundAccessStatus.Unspecified:
                        case BackgroundAccessStatus.Denied:
                            MessageDialogWithLanguage.showDialog("必须将该应用添加到锁屏，后台任务才能正常运行！", "You must add this app to lock screen so that the background task can run normally!");
                            break;
                        default:
                            break;
                    }
                }
                catch (Exception e)
                {
                    if(e.HResult==-2147024846)
                    {
                        MessageDialogWithLanguage.showDialog("模拟器不支持后台任务，请使用本地计算机或远程计算机！", "The simulator doesn't support background task. Please try again using local computer or remote computer!");
                    }
                }
                
            }
        }

        public static void Unregister()
        {
            var entry = BackgroundTaskRegistration.AllTasks.FirstOrDefault(kvp => kvp.Value.Name == TaskName);

            if (entry.Value != null)
                entry.Value.Unregister(true);
        }

        public static bool IsTaskRegistered()
        {
            var taskRegistered = false;
            var entry = BackgroundTaskRegistration.AllTasks.FirstOrDefault(kvp => kvp.Value.Name == TaskName);

            if (entry.Value != null)
                taskRegistered = true;

            return taskRegistered;
        }

        private static void doRegist()
        {
            var builder = new BackgroundTaskBuilder();
            builder.Name = TaskName;
            builder.TaskEntryPoint = typeof(GeofenceTask).FullName;
            builder.SetTrigger(new LocationTrigger(LocationTriggerType.Geofence));
            builder.Register();
        }
    }
}
