using System;
using System.Collections.Generic;
using System.Text;

namespace Video_Sample.Data
{
    class AudioData
    {
    }

    public enum PLAYTYPE { Default = -1, PlayLoacl = 0, PlaySingle = 1, PlayList = 2 }
    public class AudioPlayInfo
    {
        public PLAYTYPE PlayType { get; set; }
        public string Name { get; set; }
        public string Song { get; set; }
        public string Path { get; set; }
        public string fileToken { get; set; }
    }
}
