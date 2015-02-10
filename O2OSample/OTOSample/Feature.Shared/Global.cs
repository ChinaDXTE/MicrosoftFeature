using Feature.DataModel;
using System;
using System.Collections.Generic;
using System.Text;

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

        public LocalSettings LocalSettings { get; private set; }

        public Push Push { get; private set; }

        public Globalization Globalization { get; private set; }

        public FilePicker FilePicker { get; private set; }

        public Roaming Roaming { get; private set; }

        public Notifications Notifications { get; private set; }

        public SampleItems SampleItems { get; private set; }

        public Serialization Serialization { get; private set; }


        private void LoadData()
        {
            this.LocalSettings = new LocalSettings();
            this.Push = new Push();
            this.Globalization = new Globalization();
            this.FilePicker = new FilePicker();
            this.Roaming = new Roaming();
            this.Notifications = new Notifications();
            this.SampleItems = new SampleItems();
            this.Serialization = new Serialization();
        }

    }
}
