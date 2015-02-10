using System;
using System.Net;
using System.Windows;
using System.Windows.Input;
using System.IO;
using System.ComponentModel;
using Windows.Storage;
using System.Threading.Tasks;
using System.Threading;
using Windows.Storage.Streams;
using System.Diagnostics;
using System.Linq;
using System.Collections.Generic;

namespace Feature
{
    public static class FileEx
    {
        private const string PathSeperater = "/";

        public async static void CreatDirectory(this string filename,StorageTarget ? target = null)
        {
            try
            {
                StorageFolder storageFolder = await _getFolderByType(target);
                filename = filename.Replace("/", "\\");
                string foldername = string.Empty;
                if (filename.LastIndexOf("\\") > 0)
                    foldername = filename.Substring(0, filename.LastIndexOf("\\"));
                if (!string.IsNullOrEmpty(foldername))
                {
                    string[] arrdirectory = foldername.Split('\\');
                    string currdirectory = "";
                    for (int i = 0; i < arrdirectory.Length; i++)
                    {
                        currdirectory = currdirectory + (i == 0 ? "" : "\\") + arrdirectory[i];
                        await storageFolder.CreateFolderAsync(currdirectory, CreationCollisionOption.OpenIfExists);
                    }
                }
            }
            catch (Exception ex) { Debug.WriteLine(ex.Message); }
        }

        public async static Task<string> Save(this byte[] source, string filename, StorageTarget? target = null)
        {
            try
            {
                StorageFolder storageFolder = await _getFolderByType(target);
                if (!string.IsNullOrEmpty(filename))
                {
                    filename = filename.Replace("/", "\\");
                    string foldername = string.Empty;
                    if (filename.LastIndexOf("\\") > 0)
                        foldername = filename.Substring(0, filename.LastIndexOf("\\"));
                    if (!string.IsNullOrEmpty(foldername))
                    {
                        string[] arrdirectory = foldername.Split('\\');
                        string currdirectory = "";
                        for (int i = 0; i < arrdirectory.Length; i++)
                        {
                            currdirectory = currdirectory + (i == 0 ? "" : "\\") + arrdirectory[i];
                            await storageFolder.CreateFolderAsync(currdirectory, CreationCollisionOption.OpenIfExists);
                        }
                    }
                    StorageFile storageFile = await storageFolder.CreateFileAsync(filename, CreationCollisionOption.ReplaceExisting);
                    await FileIO.WriteBytesAsync(storageFile, source);
                }
                return filename.Replace("\\", "/");
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return string.Empty;
            }
        }

        public async static Task<string> Save(this byte[] source, string foldername, string filename, StorageTarget? target = null)
        {
            try
            {
                StorageFolder storageFolder = await _getFolderByType(target);
                if (!string.IsNullOrEmpty(foldername))
                {
                    foldername = foldername.Replace("/", "\\");
                    string[] arrdirectory = foldername.Split('\\');
                    string currdirectory = "";
                    for (int i = 0; i < arrdirectory.Length; i++)
                    {
                        currdirectory = currdirectory + (i == 0 ? "" : "\\") + arrdirectory[i];
                        await storageFolder.CreateFolderAsync(currdirectory, CreationCollisionOption.OpenIfExists);
                    }
                    filename = foldername + "\\" + filename;
                    StorageFile storageFile = await storageFolder.CreateFileAsync(filename, CreationCollisionOption.ReplaceExisting);
                    if (storageFile != null)
                    {
                        await FileIO.WriteBytesAsync(storageFile, source);
                    }
                }
                return filename.Replace("\\", "/");
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return string.Empty;
            }
        }

        public async static Task<string> Save(this string source, string filename, StorageTarget? target = null)
        {
            try
            {
                StorageFolder storageFolder = await _getFolderByType(target);
                if (!string.IsNullOrEmpty(filename))
                {
                    filename = filename.Replace("/", "\\");
                    string foldername = string.Empty;
                    if (filename.LastIndexOf("\\") > 0)
                        foldername = filename.Substring(0, filename.LastIndexOf("\\"));
                    if (!string.IsNullOrEmpty(foldername))
                    {
                        string[] arrdirectory = foldername.Split('\\');
                        string currdirectory = "";
                        for (int i = 0; i < arrdirectory.Length; i++)
                        {
                            currdirectory = currdirectory + (i == 0 ? "" : "\\") + arrdirectory[i];
                            await storageFolder.CreateFolderAsync(currdirectory, CreationCollisionOption.OpenIfExists);
                        }
                    }
                    StorageFile storageFile = await storageFolder.CreateFileAsync(filename, CreationCollisionOption.ReplaceExisting);
                    await FileIO.WriteTextAsync(storageFile, source);
                }
                return filename.Replace("\\", "/");
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return string.Empty;
            }
        }

        public async static Task<string> Save(this string source, string foldername, string filename, StorageTarget? target = null)
        {
            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(source);
            return await Save(bytes, foldername, filename, target);
        }

        public async static Task<string> GetContent(this string filename, StorageTarget? target = null)
        {
            try
            {
                filename = filename.Replace("/", "\\");
                string content = string.Empty;
                string filePath = await Exist(filename);
                if (!string.IsNullOrEmpty(filePath))
                {
                    StorageFolder storageFolder = await _getFolderByType(target);
                    StorageFile storageFile = await storageFolder.GetFileAsync(filename);
                    content = await FileIO.ReadTextAsync(storageFile);
                }
                return content;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return string.Empty;
            }
        }

        public async static Task<string> GetContent(this string filename, string foldername, StorageTarget? target = null)
        {
            try
            {
                foldername = foldername.Replace("/", "\\");
                string content = string.Empty;
                string filePath = await Exist(foldername, filename);
                if (!string.IsNullOrEmpty(filePath))
                {
                    if (!string.IsNullOrEmpty(foldername))
                    {
                        filename = foldername + "\\" + filename;
                    }
                    content = await GetContent(filename, target);
                }
                return content;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return string.Empty;
            }
        }

        public async static Task<string> Exist(this string filename, StorageTarget? target = null)
        {
            try
            {
                filename = filename.Replace("/", "\\");
                StorageFolder storageFolder = await _getFolderByType(target);
                StorageFile storageFile = await storageFolder.GetFileAsync(filename);
                if (storageFile != null)
                {
                    return filename.Replace("\\", "/");
                }
                return string.Empty;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return string.Empty;
            }
        }

        public async static Task<string> Exist(this string foldername, string filename, StorageTarget? target = null)
        {
            try
            {
                foldername = foldername.Replace("/", "\\");
                StorageFolder storageFolder = await _getFolderByType(target);
                if (!string.IsNullOrEmpty(foldername))
                {
                    filename = foldername + "\\" + filename;
                }
                StorageFile storageFile = await storageFolder.GetFileAsync(filename);
                if (storageFile != null)
                {
                    return filename.Replace("\\", "/");
                }
                return string.Empty;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return string.Empty;
            }
        }

        public async static Task<bool> DeleteFile(this String filename, StorageTarget? target = null)
        {
            try
            {
                bool result = true;
                filename = filename.Replace("/", "\\");
                StorageFolder storageFolder = await _getFolderByType(target);
                StorageFile storageFile = await storageFolder.GetFileAsync(filename);
                await storageFile.DeleteAsync();
                return result;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return false;
            }
        }

        public async static Task<bool> DeleteDirectory(this string foldername, StorageTarget? target = null)
        {
            try
            {
                bool result = true;
                foldername = foldername.Replace("/", "\\");
                StorageFolder storageFolder = await _getFolderByType(target);
                StorageFolder currentFolder = await storageFolder.GetFolderAsync(foldername);
                await currentFolder.DeleteAsync(StorageDeleteOption.PermanentDelete);
                return result;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return false;
            }
        }

        public async static Task<byte[]> GetByteContent(this string path, StorageTarget? target = null)
        {
            byte[] buff = null;
            try
            {
                string filename = path.Replace("/", "\\");
                StorageFolder storageFolder = await _getFolderByType(target);
                StorageFile storageFile = await storageFolder.GetFileAsync(filename);
                if (storageFile != null)
                {
                    var ibuff = await FileIO.ReadBufferAsync(storageFile);
                    if (ibuff != null)
                    {
                        using (var dataReader = DataReader.FromBuffer(ibuff))
                        {
                            buff = new byte[ibuff.Capacity];
                            dataReader.ReadBytes(buff);
                        }
                    }
                }
            }
            catch (Exception ex) { Debug.WriteLine(ex.Message); }
            return buff;
        }

        public static async Task<bool> IsAvailableRemoveDevice(this string removename)
        {
            bool rev = false;
            StorageFolder externalDevices = KnownFolders.RemovableDevices;
            IReadOnlyList<StorageFolder> folderList = await externalDevices.GetFoldersAsync();
            for (int i = 0; i < folderList.Count; i++)
            {
                if (folderList[i].Attributes == FileAttributes.Directory)
                {
                    rev = true;
                    break;
                }
            }
            return rev;
        }

        private static async Task<StorageFolder> _getFolderByType(StorageTarget ? target = null)
        {
            StorageFolder folder = ApplicationData.Current.LocalFolder;
            switch (target)
            {
                case StorageTarget.music:
                    folder = KnownFolders.MusicLibrary;
                    break;
                case StorageTarget.pic:
                    folder = KnownFolders.PicturesLibrary;
                    break;
                case StorageTarget.video:
                    folder = KnownFolders.VideosLibrary;
                    break;
                case StorageTarget.SD:
                    StorageFolder externalDevices = KnownFolders.RemovableDevices;
                    IReadOnlyList<StorageFolder> folderList = await externalDevices.GetFoldersAsync();
                    for (int i = 0; i < folderList.Count; i++)
                    {
                        if (folderList[i].Attributes == FileAttributes.Directory)
                        {
                            folder = folderList[i];
                            break;
                        }
                    }
                    break;
                case StorageTarget.temp:
                    folder = ApplicationData.Current.TemporaryFolder;
                    break;
            }
            return folder;
        }

    }

    public enum StorageTarget
    {
        music,
        pic,
        video,
        SD,
        temp
    }
}
