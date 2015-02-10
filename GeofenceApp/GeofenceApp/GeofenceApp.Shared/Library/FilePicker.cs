using System;
using System.Collections.Generic;
using System.Text;
using Windows.ApplicationModel.Activation;
using Windows.Storage.Pickers;
using Windows.Storage;
using Windows.Storage.Provider;
using Windows.Storage.AccessCache;
using Windows.UI.Xaml;

namespace Feature
{
    public partial class FilePicker
    {
        public event EventHandler FilePickerFinish;
        private IReadOnlyList<StorageFile> _files = null;
        public IReadOnlyList<StorageFile> Files
        {
            get { return this._files; }
            private set { this._files = value; }
        }

        private StorageFile _file = null;
        public StorageFile File
        {
            get { return this._file; }
            private set { this._file = value; }
        }

        private StorageFolder _folder = null;
        public StorageFolder Folder
        {
            get { return this._folder; }
            private set { this._folder = value; }
        }

        private string _content = string.Empty;
        public string Content
        {
            get { return this._content; }
            private set { this._content = value; }
        }

        public async void OpenFilePicker(FileType fileType)
        {
            FileOpenPicker openPicker = new FileOpenPicker();
            switch (fileType)
            { 
                case FileType.all:
                    openPicker.ViewMode = PickerViewMode.List;
                    openPicker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
                    openPicker.FileTypeFilter.Add("*");
                    break;
                case FileType.pic:
                    openPicker.ViewMode = PickerViewMode.Thumbnail;
                    openPicker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
                    openPicker.FileTypeFilter.Add(".jpg");
                    openPicker.FileTypeFilter.Add(".jpeg");
                    openPicker.FileTypeFilter.Add(".png");
                    break;
                case FileType.audio:
                    openPicker.ViewMode = PickerViewMode.Thumbnail;
                    openPicker.SuggestedStartLocation = PickerLocationId.MusicLibrary;
                    openPicker.FileTypeFilter.Add(".mp3");
                    openPicker.FileTypeFilter.Add(".wma");
                    openPicker.FileTypeFilter.Add(".wav");
                    break;
                case FileType.video:
                    openPicker.ViewMode = PickerViewMode.Thumbnail;
                    openPicker.SuggestedStartLocation = PickerLocationId.VideosLibrary;
                    openPicker.FileTypeFilter.Add(".mp4");
                    openPicker.FileTypeFilter.Add(".wmv");
                    openPicker.FileTypeFilter.Add(".avi");
                    break;
            }
#if !WINDOWS_PHONE_APP
            this.File = await openPicker.PickSingleFileAsync();
            if (this.FilePickerFinish != null)
            {
                this.FilePickerFinish(this.File, null);
            }
#else
            openPicker.PickSingleFileAndContinue();
#endif
        }

        public async void OpenFolderPicker()
        {
            FolderPicker folderPicker = new FolderPicker();
            folderPicker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
            folderPicker.FileTypeFilter.Add("*");
#if !WINDOWS_PHONE_APP
            this.Folder = await folderPicker.PickSingleFolderAsync();
            if (this.Folder != null)
            {
                StorageApplicationPermissions.FutureAccessList.AddOrReplace("PickedFolderToken", this.Folder);
                if (this.FilePickerFinish != null)
                    this.FilePickerFinish(this.Folder, null);
            }
#else
            folderPicker.PickFolderAndContinue();
#endif
        }

        public async void SavePickerText(string fileName,string content)
        {
            this.Content = content;
            FileSavePicker savePicker = new FileSavePicker();
            savePicker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
            savePicker.FileTypeChoices.Add("Feature_SavePicker", new List<string>() { ".txt"});
            savePicker.SuggestedFileName = fileName;

#if !WINDOWS_PHONE_APP
            StorageFile file = await savePicker.PickSaveFileAsync();
            if (file != null)
            {
                CachedFileManager.DeferUpdates(file);
                await FileIO.WriteTextAsync(file, content);
                FileUpdateStatus status = await CachedFileManager.CompleteUpdatesAsync(file);
                if (status == FileUpdateStatus.Complete)
                {
                    if (this.FilePickerFinish != null)
                        this.FilePickerFinish("Complate", null);
                }
            }
#else
            savePicker.PickSaveFileAndContinue();
#endif

        }

#if WINDOWS_PHONE_APP
        public async void ContinueFileOpenPicker(IContinuationActivatedEventArgs args)
        {
            switch (args.Kind)
            {
                case ActivationKind.PickFileContinuation:
                     this.Files = (args as FileOpenPickerContinuationEventArgs).Files;
                        if (this.Files != null && this.Files.Count > 0)
                        {
                            this.File = this.Files[0];
                            if (FilePickerFinish != null)
                                this.FilePickerFinish(this.File, null);
                        }
                    break;
                case ActivationKind.PickFolderContinuation:
                    this.Folder = (args as FolderPickerContinuationEventArgs).Folder;
                    if (this.Folder != null)
                    {
                        StorageApplicationPermissions.FutureAccessList.AddOrReplace("FeaturePickFolder", this.Folder);
                        if (FilePickerFinish != null)
                            this.FilePickerFinish(this.Folder, null);
                    }
                    break;
                case ActivationKind.PickSaveFileContinuation:
                    StorageFile file = (args as FileSavePickerContinuationEventArgs).File;
                    if (file != null)
                    {
                        CachedFileManager.DeferUpdates(file);
                        await FileIO.WriteTextAsync(file,this.Content);
                        FileUpdateStatus status = await CachedFileManager.CompleteUpdatesAsync(file);
                        if (status == FileUpdateStatus.Complete)
                        {
                            if (FilePickerFinish != null)
                                this.FilePickerFinish("Complete", null);
                        }
                    }
                    break;
            }
        }
#endif
    }

    public enum FileType
    { 
        all,
        pic,
        video,
        audio
    }

}
