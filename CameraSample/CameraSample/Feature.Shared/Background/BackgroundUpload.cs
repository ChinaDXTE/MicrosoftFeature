using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Networking.BackgroundTransfer;
using Windows.Storage;
using Windows.Storage.FileProperties;

namespace Feature.Background
{
    public class BackgroundUpload :BackgroundTransfers
    {
        private const int maxUploadFileSize = 100 * 1024 * 1024; // 100 MB

        /// <summary>
        /// StartUploadFiles
        /// </summary>
        /// <param name="url"></param>
        /// <param name="files"></param>
        public async void StartUploadFiles(string url, IReadOnlyList<StorageFile> files)
        {
            Uri uri = null;
            if (!Uri.TryCreate(url, UriKind.Absolute, out uri))
            {
                if (this.ErrorException != null)
                    this.ErrorException(this, "Please Invalid UploadURl");
                return;
            }
            if (files==null || files.Count == 0)
            {
                if (this.ErrorException != null)
                    this.ErrorException(this, "Please Invalid files");
                return;
            }
            ulong totalFileSize = 0;
            for (int i = 0; i < files.Count; i++)
            {
                BasicProperties properties = await files[i].GetBasicPropertiesAsync();
                totalFileSize += properties.Size;
                if (totalFileSize > maxUploadFileSize)
                {
                    if (this.ErrorException != null)
                        this.ErrorException(this, "Please Invalid maxUploadFileSize");
                    return;
                }
            }
            List<BackgroundTransferContentPart> parts = new List<BackgroundTransferContentPart>();
            for (int i = 0; i < files.Count; i++)
            {
                BackgroundTransferContentPart part = new BackgroundTransferContentPart("File" + i, files[i].Name);
                part.SetFile(files[i]);
                parts.Add(part);
            }
            BackgroundUploader uploader = new BackgroundUploader();
            UploadOperation upload = await uploader.CreateUploadAsync(uri, parts);
            await HandleUploadAsync(upload, true);
        }

        public void CancelAll()
        {
            base.Cancel();
        }

        /// <summary>
        /// Uploas Handle
        /// </summary>
        /// <param name="upload"></param>
        /// <param name="start"></param>
        /// <returns></returns>
        private async Task HandleUploadAsync(UploadOperation upload, bool start=true)
        {
            try
            {
                Progress<UploadOperation> progressCallback = new Progress<UploadOperation>(UploadProgress);
                if (start)
                {
                    await upload.StartAsync().AsTask(this._cts.Token, progressCallback);
                }
                else
                {
                    await upload.AttachAsync().AsTask(this._cts.Token, progressCallback);
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
        }

        private void UploadProgress(UploadOperation upload)
        {
            BackgroundUploadProgress progress = upload.Progress;
            double percentSent = 100;
            if (progress.TotalBytesToSend > 0)
            {
                percentSent = progress.BytesSent * 100 / progress.TotalBytesToSend;
                if (this.GetPercent != null)
                    this.GetPercent((long)percentSent, (long)progress.TotalBytesToSend);
            }
            if (progress.BytesSent == progress.TotalBytesToSend)
            {
                if (this.FinishHandler != null)
                    this.FinishHandler(this, "Finish");
            }
        }

    }
}
