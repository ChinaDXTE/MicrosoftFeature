using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Networking.BackgroundTransfer;
using Windows.Storage;

namespace Feature.Background
{
    public class BackgroundDownload :BackgroundTransfers
    {
        internal List<DownloadOperation> _listDownloads;

        public BackgroundDownload()
        {
            this._listDownloads = new List<DownloadOperation>();
        }

        /// <summary>
        /// StartDownload
        /// </summary>
        /// <param name="url">Download url</param>
        /// <param name="priority"></param>
        /// <param name="saveStorage">default LocalFolder</param>
        public async void StartDownload(string url, BackgroundTransferPriority priority = BackgroundTransferPriority.Default, StorageFile saveStorage = null)
        {
            Uri uri = null;
            if (!Uri.TryCreate(url, UriKind.Absolute, out uri))
            {
                if (this.ErrorException != null)
                    this.ErrorException(this, "Please Invalid DownloadURl");
                return;
            }
            if (priority == null)
                priority = BackgroundTransferPriority.Default;
            if (saveStorage == null)
            {
                var storageFolder = ApplicationData.Current.LocalFolder;
                saveStorage = await storageFolder.CreateFileAsync(conDefaultFileName, CreationCollisionOption.GenerateUniqueName);
            }
            BackgroundDownloader downloader = new BackgroundDownloader();
            DownloadOperation download = downloader.CreateDownload(uri, saveStorage);
            download.Priority = priority;
            await this.HandleDownloadAsync(download, true);
        }

        public void PauseAll()
        {
            foreach (DownloadOperation download in this._listDownloads)
            {
                if (download.Progress.Status == BackgroundTransferStatus.Running)
                {
                    download.Pause();
                }
            }
        }

        public void ResumeAll()
        {
            foreach (DownloadOperation download in this._listDownloads)
            {
                if (download.Progress.Status == BackgroundTransferStatus.PausedByApplication)
                {
                    download.Resume();
                }
            }
        }

        public void CancelAll()
        {
            base.Cancel();
            this._listDownloads = new List<DownloadOperation>();
        }

        /// <summary>
        /// Download Handle
        /// </summary>
        /// <param name="download"></param>
        /// <param name="start">ture or false; ture:start,false:attach</param>
        /// <returns></returns>
        private async Task HandleDownloadAsync(DownloadOperation download, bool start)
        {
            try
            {
                this._listDownloads.Add(download);
                Progress<DownloadOperation> progressCallback = new Progress<DownloadOperation>(DownloadProgress);
                if (start)
                {
                    await download.StartAsync().AsTask(this._cts.Token, progressCallback);
                }
                else
                {
                    await download.AttachAsync().AsTask(this._cts.Token, progressCallback);
                }
            }
            catch (TaskCanceledException)
            {
                Debug.WriteLine("task canceled");
            }
            catch (Exception ex)
            {
                if (!IsExceptionHandled("Execution error", ex))
                {
                    if (this.ErrorException != null)
                        this.ErrorException(this, ex.Message);
                }
            }
            finally
            {
                this._listDownloads.Remove(download);
            }
        }

        private void DownloadProgress(DownloadOperation download)
        {
            double percent = 100;
            if (download.Progress.TotalBytesToReceive > 0)
            {
                percent = download.Progress.BytesReceived * 100 / download.Progress.TotalBytesToReceive;
                if (this.GetPercent != null)
                    this.GetPercent((long)percent, (long)download.Progress.TotalBytesToReceive);
            }
            if (download.Progress.BytesReceived == download.Progress.TotalBytesToReceive)
            {
                if (this.FinishHandler != null)
                    this.FinishHandler(this, "Finish");
            }
        }
    }
}
