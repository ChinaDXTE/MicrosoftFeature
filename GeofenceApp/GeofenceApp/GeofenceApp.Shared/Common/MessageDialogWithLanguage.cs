using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI.Popups;

namespace GeofenceApp
{
    public class MessageDialogWithLanguage
    {
        public static async void  showDialog(string chinese, string enlish)
        {
            string message = "";
            if (Global.Current.Globalization.geo_CurrentLanguage == Global.Current.Globalization.geo_Chinese)
            {
                message = chinese;
            }
            else
            {
                message = enlish;
            }
            MessageDialog dialog = new MessageDialog(message);
            await dialog.ShowAsync();
        }
    }
}
