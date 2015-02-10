using System;
using System.Collections.Generic;
using System.Text;
using Windows.ApplicationModel;
using Windows.Foundation;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.DataTransfer;
using Windows.ApplicationModel.DataTransfer.ShareTarget;

namespace Feature
{
    public static class ShareContractEx
    {
        /// <summary>
        /// Share Text
        /// </summary>
        /// <param name="text"></param>
        /// <param name="title"></param>
        /// <param name="description"></param>
        public static void RegisterForShare(this string text, string title,string description)
        {
            DataTransferManager dataTransferManager = DataTransferManager.GetForCurrentView();
            dataTransferManager.DataRequested += new TypedEventHandler<DataTransferManager,
               DataRequestedEventArgs>((s,e) =>
            {
                DataRequest request = e.Request;
                request.Data.Properties.Title = title;
                request.Data.Properties.Description = description;
                request.Data.SetText(text);
            });
            DataTransferManager.ShowShareUI();
        }

        /// <summary>
        /// Share weblink
        /// </summary>
        /// <param name="weblink"></param>
        /// <param name="title"></param>
        /// <param name="description"></param>
        public static void RegisterForShare(this Uri weblink, string title, string description)
        {
            DataTransferManager dataTransferManager = DataTransferManager.GetForCurrentView();
            dataTransferManager.DataRequested += new TypedEventHandler<DataTransferManager,
               DataRequestedEventArgs>((s, e) =>
               {
                   DataRequest request = e.Request;
                   request.Data.Properties.Title = title;
                   request.Data.Properties.Description = description;
                   request.Data.SetWebLink(weblink);
               });
            DataTransferManager.ShowShareUI();
        }

        /// <summary>
        /// Share Location html
        /// </summary>
        /// <param name="html">such as :"<p>Here is a local image: <img src=\"" + localImage + "\">.</p>";</param>
        /// <param name="htmlImages">such as: localImage = "ms-appx:///Assets/Logo.png";</param>
        /// <param name="title"></param>
        /// <param name="description"></param>
        public static void RegisterForShare(this string html, List<string> htmlImages, string title, string description)
        {
            DataTransferManager dataTransferManager = DataTransferManager.GetForCurrentView();
            dataTransferManager.DataRequested += new TypedEventHandler<DataTransferManager,
               DataRequestedEventArgs>((s, e) =>
               {
                   DataRequest request = e.Request;
                   request.Data.Properties.Title = title;
                   request.Data.Properties.Description = description;
                   string htmlFormat = HtmlFormatHelper.CreateHtmlFormat(html);
                   request.Data.SetHtmlFormat(htmlFormat);
                   foreach (string imageUri in htmlImages)
                   {
                       RandomAccessStreamReference streamRef =
                                RandomAccessStreamReference.CreateFromUri(new Uri(imageUri));
                       request.Data.ResourceMap[imageUri] = streamRef;
                   }

               });
            DataTransferManager.ShowShareUI();
        }

        /// <summary>
        /// Share Location File
        /// </summary>
        /// <param name="file"> Package.Current.InstalledLocation.GetFileAsync("Assets\\Logo.png") or location other file</param>
        /// <param name="title"></param>
        /// <param name="description"></param>
        public static void RegisterForShare(this StorageFile file, string title, string description)
        {
            DataTransferManager dataTransferManager = DataTransferManager.GetForCurrentView();
            dataTransferManager.DataRequested += new TypedEventHandler<DataTransferManager,
               DataRequestedEventArgs>((s, e) =>
               {
                   DataRequest request = e.Request;
                   request.Data.Properties.Title = title;
                   request.Data.Properties.Description = description;
                   DataRequestDeferral deferral = request.GetDeferral();
                   try
                   {
                       List<IStorageItem> storageItems = new List<IStorageItem>();
                       storageItems.Add(file);
                       request.Data.SetStorageItems(storageItems);
                   }
                   finally
                   {
                       deferral.Complete();
                   }
               });
            DataTransferManager.ShowShareUI();
        }

        public static void RegisterForShare(this IRandomAccessStream bitmap, string title, string description)
        {
            DataTransferManager dataTransferManager = DataTransferManager.GetForCurrentView();
            dataTransferManager.DataRequested += new TypedEventHandler<DataTransferManager,
               DataRequestedEventArgs>((s, e) =>
               {
                   DataRequest request = e.Request;
                   request.Data.Properties.Title = title;
                   request.Data.Properties.Description = description;
                   bitmap.Seek(0);
                   RandomAccessStreamReference imageStreamRef = RandomAccessStreamReference.CreateFromStream(bitmap);
                   request.Data.Properties.Thumbnail = imageStreamRef;
                   request.Data.SetBitmap(imageStreamRef);
               });
            DataTransferManager.ShowShareUI();
        }

    }
}
