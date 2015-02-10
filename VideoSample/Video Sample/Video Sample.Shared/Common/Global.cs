using System;
using System.Collections.Generic;
using System.Text;
using Video_Sample.Common;
using Windows.Networking.BackgroundTransfer;

namespace Feature
{
    public class Global
    {
        private volatile static Global _curr = null;
        private static readonly object _lockObj = new object();

        private Global()
        {
            this.LoadData();
        }

        public static Global Current
        {
            get
            {
                if (_curr == null)
                {
                    lock (_lockObj)
                    {
                        if (_curr == null)
                            _curr = new Global();
                    }
                }
                return _curr;
            }
        }

        public Globalization Globalization { get; private set; }

        public Notifications Notifications { get; private set; }

        public LocalSettings LocalSettings { get; private set; }

        //internal List<DownloadOperation> ListDownloads { get; set; }
        private void LoadData()
        {
            this.Globalization = new Globalization();
            this.LocalSettings = new LocalSettings();
            //this.ListDownloads = new List<DownloadOperation>();
        }

    }
}
