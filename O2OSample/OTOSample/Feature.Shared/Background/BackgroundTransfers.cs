using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Networking.BackgroundTransfer;
using Windows.Storage;
using Windows.Web;

namespace Feature
{
    public abstract class BackgroundTransfers : IDisposable
    {
        public const string conDefaultFileName = "DownFile.txt";
        internal CancellationTokenSource _cts;
        public EventHandler<string> ErrorException;
        public delegate void GetPercentHandler(long current, long total);
        public GetPercentHandler GetPercent;
        public EventHandler<string> FinishHandler;

        public BackgroundTransfers()
        {
            this._cts = new CancellationTokenSource();
        }

        public void Dispose()
        {
            if (this._cts != null)
            {
                this._cts.Dispose();
                this._cts = null;
            }
            GC.SuppressFinalize(this);
        }

        internal void Cancel()
        {
            this._cts.Cancel();
            this._cts.Dispose();
            this._cts = new CancellationTokenSource();
        }

        public bool IsExceptionHandled(string title, Exception ex)
        {
            WebErrorStatus error = BackgroundTransferError.GetStatus(ex.HResult);
            if (error == WebErrorStatus.Unknown)
            {
                return false;
            }
            return true;
        }
    }
}
