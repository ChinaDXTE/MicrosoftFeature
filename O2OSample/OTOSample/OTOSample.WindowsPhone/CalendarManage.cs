using Feature;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Appointments;

namespace OTOSample
{
    public class CalendarManage
    {
        private AppointmentStore appointmentStore = null;
        private AppointmentCalendar appCalendar = null;

        public CalendarManage()
        {
            this.init();
        }

        private async void init()
        {
            this.appointmentStore = await AppointmentManager.RequestStoreAsync
                    (AppointmentStoreAccessType.AppCalendarsReadWrite);
            IReadOnlyList<AppointmentCalendar> appCalendars =
                await this.appointmentStore.FindAppointmentCalendarsAsync(FindAppointmentCalendarsOptions.IncludeHidden);
            // Apps can create multiple calendars. This example creates only one.
            if (appCalendars.Count == 0)
            {
                this.appCalendar = await appointmentStore.CreateAppointmentCalendarAsync("Example App Calendar");
            }
            else
            {
                this.appCalendar = appCalendars[0];
            }
            this.appCalendar.OtherAppReadAccess = AppointmentCalendarOtherAppReadAccess.Full;
            this.appCalendar.OtherAppWriteAccess = AppointmentCalendarOtherAppWriteAccess.SystemOnly;
            // This app will show the details for the appointment. Use System to let the system show the details.
            this.appCalendar.SummaryCardView = AppointmentSummaryCardView.App;
        }

        private async void save()
        {
            try
            {
                await this.appCalendar.SaveAsync();
                this.appointmentStore.ChangeTracker.Enable();
                this.appointmentStore.ChangeTracker.Reset();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        public async Task CreateNewAppointment(string title,DateTimeOffset startTime,TimeSpan duration,string location)
        {
            try
            {
                Appointment newAppointment = new Appointment();
                newAppointment.Subject = title;
                newAppointment.StartTime = startTime;
                newAppointment.Duration = duration;
                newAppointment.Location = location;
                //save appointment to calendar
                await this.appCalendar.SaveAppointmentAsync(newAppointment);
                this.save();
                Global.Current.Notifications.CreateToastNotifier("提示","添加日程成功");
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

    }
}
