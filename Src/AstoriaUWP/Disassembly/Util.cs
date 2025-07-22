using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;
using Windows.Storage;
using Windows.Storage.Streams;
using DalvikUWPCSharp.Applet;
using Windows.UI.Popups;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;
//using DalvikUWPCSharp.Disassembly.APKParser;
//using DalvikUWPCSharp.Disassembly.APKParser.bean;

namespace DalvikUWPCSharp.Disassembly
{
    public static class Util
    {
        //public static FileActivatedEventArgs CurrentFileActivatedArgs { get; set; }
        public static StorageFile CurrentFile { get; private set; }

        public static Applet.DroidApp CurrentApp { get; private set; }

        public static InstallApkPage apkpage { get; set; }

        private static StorageFolder localFolder = ApplicationData.Current.LocalFolder;


        // LoadAPK 
        public static async Task LoadAPK(FileActivatedEventArgs e)
        {
            StorageFile sf = (StorageFile)e.Files[0];

            //IStorageItem sss = e.Files[0];

            //Debug.WriteLine("When do I get called?");
            //var appsRoot = await localFolder.CreateFolderAsync("Apps", CreationCollisionOption.OpenIfExists);
            //StorageFile copiedFile = await sf.CopyAsync(appsRoot, sf.Name, NameCollisionOption.GenerateUniqueName);

            //ApkParser parser = await ApkParser.CreateAsync(copiedFile);
            //ApkMeta meta = await parser.getApkMeta();
            //apkpage.SetDisplayName(meta.getName());

            apkpage.SetDisplayName(sf.DisplayName);

            CurrentApp = await DroidApp.CreateAsync(sf);

            apkpage.appletLoaded(CurrentApp, EventArgs.Empty);

            //Debug.WriteLine($"cf is null: {CurrentFile == null}");


        }//LoadAPK


        // LoadAPK2 - temp. func, debug only for RnD
        public static async Task<bool> LoadAPK2(string s)
        {
            bool Flag = true;

            // Target : Music folder
            StorageFolder storageFolder = KnownFolders.PicturesLibrary; // Images Folder

            StorageFile sf = null;

            try
            {
                sf = await storageFolder.GetFileAsync(s);
            }
            catch
            {
                Flag = false;
                return Flag;
            }
        
                //sf = await StorageFile.GetFileFromPathAsync(e.Arguments);// = (StorageFile)e.Arguments;//.Files[0];

                //IStorageItem sss = e.Files[0];

                //Debug.WriteLine("When do I get called?");
                //var appsRoot = await localFolder.CreateFolderAsync("Apps", CreationCollisionOption.OpenIfExists);
                //StorageFile copiedFile = await sf.CopyAsync(appsRoot, sf.Name, NameCollisionOption.GenerateUniqueName);

                //ApkParser parser = await ApkParser.CreateAsync(copiedFile);
                //ApkMeta meta = await parser.getApkMeta();
                //apkpage.SetDisplayName(meta.getName());

                apkpage.SetDisplayName(sf.DisplayName);

                CurrentApp = await DroidApp.CreateAsync(sf);

                apkpage.appletLoaded(CurrentApp, EventArgs.Empty);

                //Debug.WriteLine($"cf is null: {CurrentFile == null}");            
          

            return Flag;

        }//LoadAPK2


        // ReadFile
        public static async Task<byte[]> ReadFile(StorageFile sf)
        {
            byte[] fileBytes = null;

            try
            {
                using (IRandomAccessStreamWithContentType stream = await sf.OpenReadAsync())
                {
                    fileBytes = new byte[stream.Size];
                    
                    using (DataReader reader = new DataReader(stream))
                    {
                        await reader.LoadAsync((uint)stream.Size);

                        reader.ReadBytes(fileBytes);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Util - ReadFile - OpenReadAsync Exception: " + ex.Message);
            }

            return fileBytes;

        }//end

        public static List<string> GeneratePermissions()
        {
            return CurrentApp.metadata.Permissions;
        }

        // PurgeAppsFolder()  
        public static async Task PurgeAppsFolder()
        {
            var localFolder = ApplicationData.Current.LocalFolder;

            // Create Folder : "Apps"
            var appsRoot = await localFolder.CreateFolderAsync("Apps", CreationCollisionOption.OpenIfExists);

            try
            {
                await appsRoot.DeleteAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("[ex] Util - PurgeAppsFolder - Delete files error: " + ex.Message);
            }

            var dialog = new MessageDialog("Apps folder " + appsRoot  +  " purged.");

            await dialog.ShowAsync();
                        

        }//PurgeAppsFolder end

        public static string ConvertPath(string path)
        {
            return "\\" + path.Replace('/', '\\');
        }

        
    }
}
