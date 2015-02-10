using Feature;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Windows.ApplicationModel.Resources;

namespace Video_Sample.Common
{
    public class Globalization : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void NotifyPropertyChanged(string propertyName)
        {
            var eventHandler = this.PropertyChanged;
            if (eventHandler != null && !string.IsNullOrEmpty(propertyName))
            {
                eventHandler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion

        public ResourceLoader CurrentResourceLoader
        {
            get
            {
                return ResourceLoader.GetForCurrentView();
            }
        }

        private string region = Windows.Globalization.ApplicationLanguages.Languages[0];
        public string Video_CurrentRegion
        {
            get { return region; }
            set
            {
                this.region = value;
                this.resetRegion(this.region);
            }
        }
        public string Video_English
        {
            get
            {
                return CurrentResourceLoader.GetString("Video_English");
            }
        }

        public string Video_Chinese
        {
            get
            {
                return CurrentResourceLoader.GetString("Video_Chinese");
            }
        }

        public string Video_CurrentLanguage
        {
            get
            {
                if (this.region.Contains("CN"))
                    return this.Video_Chinese;
                else
                    return this.Video_English;
            }
        }

        public string Video_Audio
        {
            get
            {
                return CurrentResourceLoader.GetString("Video_Audio");
            }
        }

        public string Video_Video
        {
            get
            {
                return CurrentResourceLoader.GetString("Video_Video");
            }
        }

        public string Video_Setting
        {
            get
            {
                return CurrentResourceLoader.GetString("Video_Setting");
            }
        }

        public string Video_MediaSample
        {
            get
            {
                return CurrentResourceLoader.GetString("Video_MediaSample");
            }
        }

        public string Video_MyMusic
        {
            get
            {
                return CurrentResourceLoader.GetString("Video_MyMusic");
            }
        }
        public string Video_MusicHall
        {
            get
            {
                return CurrentResourceLoader.GetString("Video_MusicHall");
            }
        }

        public string Video_LocalMusic
        {
            get
            {
                return CurrentResourceLoader.GetString("Video_LocalMusic");
            }
        }

        public string Video_DownloadMusic
        {
            get
            {
                return CurrentResourceLoader.GetString("Video_DownloadMusic");
            }
        }

        public string Video_LikeMusic
        {
            get
            {
                return CurrentResourceLoader.GetString("Video_LikeMusic");
            }
        }

        public string Video_WebSong
        {
            get
            {
                return CurrentResourceLoader.GetString("Video_WebSong");
            }
        }

        public string Video_PopularClassic
        {
            get
            {
                return CurrentResourceLoader.GetString("Video_PopularClassic");
            }
        }

        public string Video_IAmSinger
        {
            get
            {
                return CurrentResourceLoader.GetString("Video_IAmSinger");
            }
        }

        public string Video_YouMayLike
        {
            get
            {
                return CurrentResourceLoader.GetString("Video_YouMayLike");
            }
        }

        public string AudioTypeIndex;
        public string Video_AudioType
        {
            get
            {
                return CurrentResourceLoader.GetString("Video_AudioType");
            }
            //get
            //{
            //    if (this.AudioTypeIndex.Contains("1"))
            //        return this.FirstSong;
            //    else if (this.AudioTypeIndex.Contains("2"))
            //        return this.PopularClassic;
            //    else if (this.AudioTypeIndex.Contains("3"))
            //        return this.IAmSinger;
            //    else
            //        return this.YouMayLike;
            //}
        }

        public string Video_Download
        {
            get
            {
                return CurrentResourceLoader.GetString("Video_Download");
            }
        }

        public string Video_MediaFile
        {
            get
            {
                return CurrentResourceLoader.GetString("Video_MediaFile");
            }
        }

        public string Video_Start
        {
            get
            {
                return CurrentResourceLoader.GetString("Video_Start");
            }
        }

        public string Video_Pause
        {
            get
            {
                return CurrentResourceLoader.GetString("Video_Pause");
            }
        }

        public string Video_Downloading
        {
            get
            {
                return CurrentResourceLoader.GetString("Video_Downloading");
            }
        }

        public string Video_DownloadError
        {
            get
            {
                return CurrentResourceLoader.GetString("Video_DownloadError");
            }
        }

        public string Video_DownloadFinish
        {
            get
            {
                return CurrentResourceLoader.GetString("Video_DownloadFinish");
            }
        }

        public string Video_DownloadMessage
        {
            get
            {
                return CurrentResourceLoader.GetString("Video_DownloadMessage");
            }
        }

        public string Video_VideoList
        {
            get
            {
                return CurrentResourceLoader.GetString("Video_VideoList");
            }
        }
        public string Video_LocalVideo
        {
            get
            {
                return CurrentResourceLoader.GetString("Video_LocalVideo");
            }
        }

        public string Video_PlayCount
        {
            get
            {
                return CurrentResourceLoader.GetString("Video_PlayCount");
            }
        }

        public string Video_VideoName
        {
            get
            {
                return CurrentResourceLoader.GetString("Video_VideoName");
            }
        }

        public string Video_SelectLoaclVideoText
        {
            get
            {
                return CurrentResourceLoader.GetString("Video_SelectLoaclVideoText");
            }
        }

        public string Video_SelectFile
        {
            get
            {
                return CurrentResourceLoader.GetString("Video_SelectFile");
            }
        }

        public string Video_VideoInfo
        {
            get
            {
                return CurrentResourceLoader.GetString("Video_VideoInfo");
            }
        }

        public string Video_VideoDetails
        {
            get
            {
                return CurrentResourceLoader.GetString("Video_VideoDetails");
            }
        }
        public string Video_Discuss
        {
            get
            {
                return CurrentResourceLoader.GetString("Video_Discuss");
            }
        }

        public string Video_Direct
        {
            get
            {
                return CurrentResourceLoader.GetString("Video_Direct");
            }
        }

        public string Video_CITVV
        {
            get
            {
                return CurrentResourceLoader.GetString("Video_CITVV");
            }
        }

        private string _downloadPathValue = "ML";
        public string _downloadPath
        {
            set
            {
                this._downloadPathValue = value;
                this.NotifyPropertyChanged("Video_DownloadPath");
            }
            get
            {
                string ret = "ML";
                var paht = Global.Current.LocalSettings.LoadData("Video_DownloadPath");
                if (paht != null)
                {
                    ret = paht.ToString();
                }
                return ret;
            }
        }
        public string Video_DownloadPath
        {
            get
            {
                if (_downloadPath == "SD")
                {
                    return Video_SDCard;
                }
                else
                {
                    return Video_MusicLibrary;
                }
            }
        }
        public string Video_SDCard
        {
            get
            {
                return CurrentResourceLoader.GetString("Video_SDCard");
            }
        }

        public string Video_MusicLibrary
        {
            get
            {
                return CurrentResourceLoader.GetString("Video_MusicLibrary");
            }
        }

        public string Video_Singer
        {
            get
            {
                return CurrentResourceLoader.GetString("Video_Singer");
            }
        }

        public string Video_MaleArtist
        {
            get
            {
                return CurrentResourceLoader.GetString("Video_MaleArtist");
            }
        }

        public string Video_LanguageSetting
        {
            get
            {
                return CurrentResourceLoader.GetString("Video_LanguageSetting");
            }
        }

        public string Video_Movie
        {
            get
            {
                return CurrentResourceLoader.GetString("Video_Movie");
            }
        }

        public string Video_Play
        {
            get
            {
                return CurrentResourceLoader.GetString("Video_Play");
            }
        }

        public string Video_Starring
        {
            get
            {
                return CurrentResourceLoader.GetString("Video_Starring");
            }
        }

        private string _stateDownland;
        public string StateDownland
        {
            set
            {
                _stateDownland = value;
                this.NotifyPropertyChanged("Video_StateDownland");
            }
            get
            {
                return _stateDownland;
            }
        }


        public string Video_EnterDownloadPage
        {
            get
            {
                return CurrentResourceLoader.GetString("Video_EnterDownloadPage");
            }
        }

        public string Video_IssuedCompany
        {
            get
            {
                return CurrentResourceLoader.GetString("Video_IssuedCompany");
            }
        }

        public string Video_IssuedTime
        {
            get
            {
                return CurrentResourceLoader.GetString("Video_IssuedTime");
            }
        }

        public string Video_Recommend
        {
            get
            {
                return CurrentResourceLoader.GetString("Video_Recommend");
            }
        }

        public string Video_RankingList
        {
            get
            {
                return CurrentResourceLoader.GetString("Video_RankingList");
            }
        }

        public string Video_Previous
        {
            get
            {
                return CurrentResourceLoader.GetString("Video_Previous");
            }
        }
        public string Video_Next
        {
            get
            {
                return CurrentResourceLoader.GetString("Video_Next");
            }
        }
        public string Video_PlayPage
        {
            get
            {
                return CurrentResourceLoader.GetString("Video_PlayPage");
            }
        }
        public string Video_Back
        {
            get
            {
                return CurrentResourceLoader.GetString("Video_Back");
            }
        }

        private bool _iPlaying = false;
        public bool IPlaying
        {
            set
            {
                _iPlaying = value;
                this.NotifyPropertyChanged("Video_PlayState");
            }
            get
            {
                return _iPlaying;
            }
        }
        public string Video_PlayState
        {
            get
            {
                if (IPlaying)
                {
                    return this.Video_Pause;
                }
                else
                {
                    return this.Video_Play;
                }
            }
        }

        public string Video_VideoPlay
        {
            get
            {
                return CurrentResourceLoader.GetString("Video_VideoPlay");
            }
        }
        
        private void resetRegion(string region)
        {
            this.region = region;
            string language = string.Empty;
            if (region.Contains("CN"))
            {
                language = "zh-CN";
            }
            else
                language = "en-US";
            Windows.Globalization.ApplicationLanguages.PrimaryLanguageOverride = language;
            Windows.ApplicationModel.Resources.Core.ResourceContext.GetForCurrentView().Reset();
#if WINDOWS_PHONE_APP
            this.NotifyPropertyChanged("Video_English");
            this.NotifyPropertyChanged("Video_Chinese");
            this.NotifyPropertyChanged("Video_CurrentLanguage");
            this.NotifyPropertyChanged("Video_Audio");
            this.NotifyPropertyChanged("Video_Video");
            this.NotifyPropertyChanged("Video_Setting");
            this.NotifyPropertyChanged("Video_MediaSample");
            this.NotifyPropertyChanged("Video_MyMusic");
            this.NotifyPropertyChanged("Video_MusicHall");
            this.NotifyPropertyChanged("Video_LocalMusic");
            this.NotifyPropertyChanged("Video_DownloadMusic");
            this.NotifyPropertyChanged("Video_LikeMusic");
            this.NotifyPropertyChanged("Video_WebSong");
            this.NotifyPropertyChanged("Video_PopularClassic");
            this.NotifyPropertyChanged("Video_IAmSinger");
            this.NotifyPropertyChanged("Video_YouMayLike");
            this.NotifyPropertyChanged("Video_AudioType");
            this.NotifyPropertyChanged("Video_Download");
            this.NotifyPropertyChanged("Video_MediaFile");
            this.NotifyPropertyChanged("Video_Start");
            this.NotifyPropertyChanged("Video_Pause");
            this.NotifyPropertyChanged("Video_Downloading");
            this.NotifyPropertyChanged("Video_DownloadError");
            this.NotifyPropertyChanged("Video_DownloadFinish");
            this.NotifyPropertyChanged("Video_DownloadMessage");
            this.NotifyPropertyChanged("Video_VideoList");
            this.NotifyPropertyChanged("Video_LocalVideo");
            this.NotifyPropertyChanged("Video_VideoInfo");
            this.NotifyPropertyChanged("Video_PlayCount");
            this.NotifyPropertyChanged("Video_VideoName");
            this.NotifyPropertyChanged("Video_SelectLoaclVideoText");
            this.NotifyPropertyChanged("Video_VideoDetails");
            this.NotifyPropertyChanged("Video_Discuss");
            this.NotifyPropertyChanged("Video_Direct");
            this.NotifyPropertyChanged("Video_CITVV");
            this.NotifyPropertyChanged("Video_DownloadPath");
            this.NotifyPropertyChanged("Video_SDCard");
            this.NotifyPropertyChanged("Video_MusicLibrary");
            this.NotifyPropertyChanged("Video_Singer");
            this.NotifyPropertyChanged("Video_MaleArtist");
            this.NotifyPropertyChanged("Video_LanguageSetting");
            this.NotifyPropertyChanged("Video_Movie");
            this.NotifyPropertyChanged("Video_Play");
            this.NotifyPropertyChanged("Video_EnterDownloadPage");
            this.NotifyPropertyChanged("Video_IssuedCompany");
            this.NotifyPropertyChanged("Video_IssuedTime");
            this.NotifyPropertyChanged("Video_Recommend");
            this.NotifyPropertyChanged("Video_RankingList");
            this.NotifyPropertyChanged("Video_Previous");
            this.NotifyPropertyChanged("Video_Next");
            this.NotifyPropertyChanged("Video_PlayPage");
            this.NotifyPropertyChanged("Video_Back");
            this.NotifyPropertyChanged("Video_PlayState");
            this.NotifyPropertyChanged("Video_VideoPlay");
#endif
        }

        public void Updata()
        {
            this.NotifyPropertyChanged("Video_English");
            this.NotifyPropertyChanged("Video_Chinese");
            this.NotifyPropertyChanged("Video_CurrentLanguage");
            this.NotifyPropertyChanged("Video_Audio");
            this.NotifyPropertyChanged("Video_Video");
            this.NotifyPropertyChanged("Video_Setting");
            this.NotifyPropertyChanged("Video_MediaSample");
            this.NotifyPropertyChanged("Video_MyMusic");
            this.NotifyPropertyChanged("Video_MusicHall");
            this.NotifyPropertyChanged("Video_LocalMusic");
            this.NotifyPropertyChanged("Video_DownloadMusic");
            this.NotifyPropertyChanged("Video_LikeMusic");
            this.NotifyPropertyChanged("Video_WebSong");
            this.NotifyPropertyChanged("Video_PopularClassic");
            this.NotifyPropertyChanged("Video_IAmSinger");
            this.NotifyPropertyChanged("Video_YouMayLike");
            this.NotifyPropertyChanged("Video_AudioType");
            this.NotifyPropertyChanged("Video_Download");
            this.NotifyPropertyChanged("Video_MediaFile");
            this.NotifyPropertyChanged("Video_Start");
            this.NotifyPropertyChanged("Video_Pause");
            this.NotifyPropertyChanged("Video_Downloading");
            this.NotifyPropertyChanged("Video_DownloadError");
            this.NotifyPropertyChanged("Video_DownloadFinish");
            this.NotifyPropertyChanged("Video_DownloadMessage");
            this.NotifyPropertyChanged("Video_VideoList");
            this.NotifyPropertyChanged("Video_LocalVideo");
            this.NotifyPropertyChanged("Video_VideoInfo");
            this.NotifyPropertyChanged("Video_PlayCount");
            this.NotifyPropertyChanged("Video_VideoName");
            this.NotifyPropertyChanged("Video_SelectLoaclVideoText");
            this.NotifyPropertyChanged("Video_VideoDetails");
            this.NotifyPropertyChanged("Video_Discuss");
            this.NotifyPropertyChanged("Video_Direct");
            this.NotifyPropertyChanged("Video_CITVV");
            this.NotifyPropertyChanged("Video_DownloadPath");
            this.NotifyPropertyChanged("Video_SDCard");
            this.NotifyPropertyChanged("Video_MusicLibrary");
            this.NotifyPropertyChanged("Video_Singer");
            this.NotifyPropertyChanged("Video_MaleArtist");
            this.NotifyPropertyChanged("Video_LanguageSetting");
            this.NotifyPropertyChanged("Video_Movie");
            this.NotifyPropertyChanged("Video_Play");
            this.NotifyPropertyChanged("Video_EnterDownloadPage");
            this.NotifyPropertyChanged("Video_IssuedCompany");
            this.NotifyPropertyChanged("Video_IssuedTime");
            this.NotifyPropertyChanged("Video_Recommend");
            this.NotifyPropertyChanged("Video_RankingList");
            this.NotifyPropertyChanged("Video_Previous");
            this.NotifyPropertyChanged("Video_Next");
            this.NotifyPropertyChanged("Video_PlayPage");
            this.NotifyPropertyChanged("Video_Back");
            this.NotifyPropertyChanged("Video_PlayState");
            this.NotifyPropertyChanged("Video_VideoPlay");
            
        }
    }
}
