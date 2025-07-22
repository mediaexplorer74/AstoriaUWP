using AndroidInteropLib.android.content;
using AndroidXml;
using DalvikUWPCSharp.Classes;
//using DalvikUWPCSharp.Disassembly.APKParser;
using DalvikUWPCSharp.Disassembly.APKReader;
//using DalvikUWPCSharp.Disassembly.AXMLPort;
using DalvikUWPCSharp.Reassembly;
using dex.net;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using Windows.Storage;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

namespace DalvikUWPCSharp.Applet
{
    public class DroidApp
    {
        public ApkInfo metadata { get; private set; }

        //public Manifest manifest { get; private set; }
        //public Resources resources { get; private set; }
        private string apkName { get; set; }
        public StorageFile apkFile { get; private set; }
        public BitmapImage appIcon { get; private set; }
        public static event EventHandler appletLoaded = delegate { };
        public EnhancedDexWriter dexWriter { get; private set; }
        public Dex dex;
        public AstoriaContext context;
        public DalvikCPU cpu;

        private static StorageFolder localFolder = ApplicationData.Current.LocalFolder;
        private StorageFolder appsRoot { get; set; }
        private EmuPage emPage;
        public StorageFolder localAppRoot { get; private set; }
        public StorageFolder resFolder { get; private set; }

        public static async Task<DroidApp> CreateAsync(StorageFile sf)
        {
            DroidApp result = new DroidApp();
            await result.CopyAPKToLocalStorage(sf);
            return result;
        }

        public static async Task<DroidApp> CreateAsync(StorageFolder sf)
        {
            DroidApp result = new DroidApp();
            result.localAppRoot = sf;
            await result.GetAPKInfoFromLocalAppFolder();
            return result;
        }

        public static async Task<DroidApp> CreateAsync(StorageFolder sf, EmuPage ep)
        {
            DroidApp result = new DroidApp();
            result.localAppRoot = sf;
            await result.GetAPKInfoFromLocalAppFolder();
            result.emPage = ep;

            return result;
        }

        public DroidApp() { }

       
        // CopyAPKToLocalStorage
        public async Task CopyAPKToLocalStorage(StorageFile sf)
        {
            apkName = sf.Name.Replace(".apk", "");

            appsRoot = default;
            try
            {
                appsRoot = await localFolder.CreateFolderAsync("Apps",
                    CreationCollisionOption.OpenIfExists);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("[ex] DroidApp - CreateFolder Apps error: " + ex.Message);
            }

            localAppRoot = default;

            try
            {
                localAppRoot = await appsRoot.CreateFolderAsync(apkName,
                    CreationCollisionOption.GenerateUniqueName);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("[ex] DroidApp - CreateFolder apkName error: " + ex.Message);
            }
            apkName = localAppRoot.Name;

            StorageFile copiedFile = default;
            try
            {
                copiedFile = await sf.CopyAsync(appsRoot, apkName + ".apk");
            }
            catch (Exception ex)
            {
                Debug.WriteLine("[ex] DroidApp - Copy to .Apk error: " + ex.Message);
            }

            apkFile = copiedFile;

            // Ensure the extraction directory is empty to avoid IOException
            try
            {
                var files = Directory.GetFiles(localAppRoot.Path, "*", SearchOption.AllDirectories);
                foreach (var file in files) File.Delete(file);
                var dirs = Directory.GetDirectories(localAppRoot.Path, "*", SearchOption.AllDirectories);
                foreach (var dir in dirs) Directory.Delete(dir, true);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("[WARN] Could not fully clean extraction directory: " + ex.Message);
            }

            //Step-by-Step extraction APK contents to localAppRoot
            try
            {
                using (ZipArchive zip = ZipFile.OpenRead(copiedFile.Path))
                {
                    foreach (ZipArchiveEntry entry in zip.Entries)
                    {
                        if (!entry.FullName.EndsWith("/"))
                        {
                            var extractPath = Path.Combine(localAppRoot.Path, entry.FullName);
                            var directoryPath = Path.GetDirectoryName(extractPath);
                            if (!Directory.Exists(directoryPath))
                            {
                                Directory.CreateDirectory(directoryPath);
                            }

                            try
                            {
                                entry.ExtractToFile(extractPath, true);
                            }
                            catch (Exception ex1)
                            {
                                Debug.WriteLine("[ERROR] APK zip element (" + extractPath + ") extraction failed: " + ex1.Message);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("[ERROR] APK extraction failed: " + ex.ToString());
            }

            // Проверка и поиск resources.arsc
            bool arscExists = false;
            try { var resFileCheck = await localAppRoot.GetFileAsync("resources.arsc"); arscExists = true; }
            catch { arscExists = false; }

            if (!arscExists)
            {
                Debug.WriteLine("[WARN] resources.arsc not found after extraction, searching subfolders...");
                StorageFile foundArsc = null;
                async Task SearchFolders(StorageFolder folder)
                {
                    var files = await folder.GetFilesAsync();
                    foreach (var file in files)
                    {
                        if (file.Name == "resources.arsc") { foundArsc = file; return; }
                    }
                    var subfolders = await folder.GetFoldersAsync();
                    foreach (var sub in subfolders)
                    {
                        await SearchFolders(sub); if (foundArsc != null) return;
                    }
                }
                await SearchFolders(localAppRoot);
                if (foundArsc != null)
                {
                    Debug.WriteLine($"[INFO] resources.arsc found at {foundArsc.Path}, copying to root...");
                    await foundArsc.CopyAsync(localAppRoot, "resources.arsc", Windows.Storage.NameCollisionOption.ReplaceExisting);
                }
                else
                {
                    Debug.WriteLine("[ERROR] resources.arsc not found anywhere in extracted APK.");
                }
            }


            try
            {
                StorageFile manifestFile = await localAppRoot.GetFileAsync("AndroidManifest.xml");
                StorageFile resFile = await localAppRoot.GetFileAsync("resources.arsc");

                byte[] manifestBytes = await Disassembly.Util.ReadFile(manifestFile);
                byte[] resBytes = await Disassembly.Util.ReadFile(resFile);

                ApkReader apkReader = new ApkReader();
                ApkInfo appinfo = apkReader.extractInfo(manifestBytes, resBytes);
                metadata = appinfo;

                appIcon = GetAppIcon();

                // Сохраняем метаданные приложения в appinfo.json
                try
                {
                    var meta = new
                    {
                        label = appinfo.label,
                        packageName = appinfo.packageName,
                        versionName = appinfo.versionName,
                        icon = appinfo.iconFileName != null && appinfo.iconFileName.Count > 0 ? appinfo.iconFileName[0] : null
                    };
                    string json = Newtonsoft.Json.JsonConvert.SerializeObject(meta);
                    StorageFile metaFile = await localAppRoot.CreateFileAsync("appinfo.json", CreationCollisionOption.ReplaceExisting);

                    try
                    {
                        await FileIO.WriteTextAsync(metaFile, json);
                    }
                    catch { }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("[WARN] Failed to write appinfo.json: " + ex.Message);
                }

                // Копируем иконку приложения как app_image.png
                try
                {
                    if (appinfo.iconFileName != null && appinfo.iconFileName.Count > 0)
                    {
                        string iconPath = appinfo.iconFileName[0];
                        StorageFile iconFile = null;
                        try
                        {
                            iconFile = await localAppRoot.GetFileAsync(iconPath);
                        }
                        catch
                        {
                            // Попробуем найти в подпапках, если прямой путь не сработал
                            var files = await localAppRoot.GetFilesAsync();
                            foreach (var file in files)
                            {
                                if (file.Name.ToLower().Contains("icon"))
                                {
                                    iconFile = file;
                                    break;
                                }
                            }
                        }
                        if (iconFile != null)
                        {
                            await iconFile.CopyAsync(localAppRoot, "app_image.png", NameCollisionOption.ReplaceExisting);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("[WARN] Failed to copy app icon as app_image.png: " + ex.Message);
                }

                // ! RnD !
                //InvokeLoadEvent();
                //DroidApp.InvokeDiagEvent();

                resFolder = await localAppRoot.GetFolderAsync("res");

                //manifest = new Manifest(manifestFile);
                /*if (localAppRoot.GetFileAsync("apktool.yml") != null)
                {
                    manifest = new Manifest(manifestFile, true);
                }
                else
                {
                    manifest = new Manifest(manifestFile);
                }*/
                //manifest = new Manifest(manifestFile);

                //StorageFile resFile = await localAppRoot.GetFileAsync("resources.arsc");
                //resources = new Resources(resFile);


            }
            catch (System.IO.FileNotFoundException fnfEx)
            {
                var dialog = new MessageDialog($"Not a valid APK file. Please try a different APK file. \n\n{fnfEx.Message}");
                await dialog.ShowAsync();

                //Provided APK is not valid, purge from app data
                await localAppRoot.DeleteAsync();
                await copiedFile.DeleteAsync();

                //Go home


                return;
            }

        }//CopyAPKToLocalStorage


        // 
        public async Task GetAPKInfoFromLocalAppFolder()
        {
            StorageFile manifestFile = await localAppRoot.GetFileAsync("AndroidManifest.old");
            StorageFile resFile = await localAppRoot.GetFileAsync("resources.arsc");
            resFolder = await localAppRoot.GetFolderAsync("res");

            byte[] manifestBytes = await Disassembly.Util.ReadFile(manifestFile);
            byte[] resBytes = await Disassembly.Util.ReadFile(resFile);

            ApkReader apkReader = new ApkReader();
            ApkInfo appinfo = apkReader.extractInfo(manifestBytes, resBytes);
            metadata = appinfo;

            appIcon = GetAppIcon();

            //dexNet
            dexWriter = new EnhancedDexWriter();
            //dexWriter = new PlainDexWriter();
            StorageFile dexFile = await localAppRoot.GetFileAsync("classes.dex");
            byte[] dexBytes = await Disassembly.Util.ReadFile(dexFile);
            MemoryStream dexStream = new MemoryStream(dexBytes);
            dex = new Dex(dexStream);
            dexWriter.dex = dex;

            //dexWriter.dex.GetMethod().
            var methods = dexWriter.dex.GetClasses();

            foreach(Class m in methods)
            {
                // TEMP
                //Debug.WriteLine(m.ToString());
                if(m.Name.Equals("com.example.ticom.myapp.MainActivity") 
                    || m.Name.Equals("com.example.ticom.myapp.MainActivity$1") 
                    || m.Name.StartsWith("com.example.ticom.myapp.R"))
                {
                    TextWriter tw = new StringWriter();
                    dexWriter.WriteOutClass(m, ClassDisplayOptions.Fields, tw);
                    Debug.WriteLine(tw.ToString());

                    foreach(Field f in m.GetFields())
                    {
                        Debug.WriteLine(f.ToString());
                    }

                    var meths = m.GetMethods();
                    foreach(Method x in meths)
                    {
                        //Debug.WriteLine(x.ToString());
                        Debug.WriteLine(dexWriter.WriteOutMethod(m, x, new Indentation()));
                        if(x.Name.Equals("onCreate") || x.Name.Equals("<init>"))
                        {
                            var opcodes = x.GetInstructions();
                            foreach(OpCode o in opcodes)
                            {
                                string opCodeString = o.ToString();
                                Debug.WriteLine(opCodeString);
                                if(opCodeString.Contains("meth@"))
                                {
                                    int pos = opCodeString.IndexOf("meth@");
                                    string methId = opCodeString.Substring(pos + 5);
                                    uint id = uint.Parse(methId);
                                    Method mx = dexWriter.dex.GetMethod(id);
                                    Debug.WriteLine("meth name: " + mx.Name + "\n");
                                }
                                else if (opCodeString.Contains("type@"))
                                {
                                    int pos = opCodeString.IndexOf("type@");
                                    string methId = opCodeString.Substring(pos + 5);
                                    uint id = uint.Parse(methId);
                                    string mx = dexWriter.dex.GetTypeName(id);
                                    Debug.WriteLine("type name: " + mx + "\n");
                                }
                                else if (opCodeString.Contains("field@"))
                                {
                                    int pos = opCodeString.IndexOf("field@");
                                    string methId = opCodeString.Substring(pos + 6);
                                    uint id = uint.Parse(methId);
                                    Field mx = dexWriter.dex.GetField(id);
                                    Debug.WriteLine("field info: " + mx.ToString() + "\n");
                                }
                                //Debug.WriteLine("instruction: " + o.ToString() + "\n");
                            }
                        }
                    }
                }
            }

            //context = new AstoriaContext();
        }

        public BitmapImage GetAppIcon()
        {
            BitmapImage image = new BitmapImage();
            //TODO: change picked icon based on device DPI
            if (metadata.iconFileName.Count > 0)
               image =  new BitmapImage(new Uri(localAppRoot.Path + Disassembly.Util.ConvertPath(metadata.iconFileName[metadata.iconFileName.Count - 1])));

            return image;

        }

        public async Task LoadAsync()
        {
            //Not sure why none of this is working. 
            //HOW IT SHOULD WORK: If manifest is not found then it's not a valid APK, throw an error

            //StorageFolder appFolder = await ApplicationData.Current.LocalFolder.GetFolderAsync();
            StorageFile manifestFile = await localAppRoot.TryGetItemAsync("AndroidManifest.xml") as StorageFile;
            

            if(manifestFile == null)
            {
                var dialog = new MessageDialog($"Not a valid APK file. Please try a different APK file. \n\n ERROR: NO MANIFEST FOUND");
                await dialog.ShowAsync();

                return;
            }

            //manifest = new Manifest(manifestFile);
        }

        public async void Purge()
        {
            await appsRoot.DeleteAsync();
            await apkFile.DeleteAsync();
        }


        //hack for Win10 Mobile RTM
        private async Task<List<StorageFile>> GetFilesFromPaths(string[] filepaths)
        {
            List<StorageFile> files = new List<StorageFile>();

            foreach(string path in filepaths)
            {
                StorageFile sf1 = await StorageFile.GetFileFromPathAsync(path);
                files.Add(sf1);
            }

            return files;
        }

        public async Task Install()
        {
            StorageFile dexFile = await localAppRoot.GetFileAsync("classes.dex");
            byte[] dexBytes = await Disassembly.Util.ReadFile(dexFile);
            MemoryStream dexStream = new MemoryStream(dexBytes);
            dex = new Dex(dexStream);

            AstoriaR res = new AstoriaR(this);

            //StaticApkParser parser = new StaticApkParser(localAppRoot);
            //Find every binary .xml and convert it to human-readable xml so it can be parsed easily
            //crashes on Win10Mobile
            //foreach (StorageFile sf in await localAppRoot.GetFilesAsync(Windows.Storage.Search.CommonFileQuery.OrderByName))

            string[] filePaths = Directory.GetFiles(localAppRoot.Path, "*.xml", SearchOption.AllDirectories);
            List<StorageFile> files = await GetFilesFromPaths(filePaths);

            foreach (StorageFile sf in files)
            {
                
                if(sf.FileType.Equals(".xml"))
                {
                    //byte[] sfBytes = await Disassembly.Util.ReadFile(sf);
                    try //TODO: IF manifest, create mainfest.OLD
                    {
                        if(sf.Name.Equals("AndroidManifest.xml"))
                        {
                            //Create binary copy of old manifest with .old extension
                            await sf.CopyAsync(localAppRoot, "AndroidManifest.old", NameCollisionOption.ReplaceExisting);
                            //Continue decoding it.
                        }
                        //AXMLPrinter printer = new AXMLPrinter();
                        //printer.main(sf);
                        //APKManifest man = new APKManifest();
                        //string decoded = man.ReadManifestFileIntoXml(sfBytes);
                        //string decoded = await parser.transBinaryXml(sf);
                        string decoded;
                        //using (Stream stream = File.OpenRead(sf.Path))
                        using (MemoryStream stream = new MemoryStream(await Disassembly.Util.ReadFile(sf)))
                        {
                            AndroidXmlReader reader = new AndroidXmlReader(stream);
                            reader.MoveToContent();

                            XDocument document = XDocument.Load(reader);
                            decoded = document.ToString();

                            //hack for android xml
                            if(decoded.Contains("@layout/content_main"))
                            {
                                decoded.Replace("@layout/content_main", res.layout.get("content_main").ToString());
                            }
                        }


                        //Debug.WriteLine(decoded);
                        //string decoded = Disassembly.Manifest.ManifestDecompressor.DecompressAXML(sfBytes);
                        //Inflated XML *should* always be larger than the binary one, so we shouldn't have to clear the file first (could be a future bug though.)
                        await Windows.Storage.FileIO.WriteTextAsync(sf, decoded);
                        Debug.WriteLine($"Converted file: {sf.Name}");
                    }
                    catch
                    {
                        //Non-binary xml file found.
                        Debug.WriteLine("Exception: Non-binary xml file found.");
                    }
                    
                }
            }

            //Remove copied APK to save on disk space
            await apkFile.DeleteAsync();

            
        }

        public void Run(Frame frame)
        {
            frame.Navigate(typeof(EmuPage), this);
        }

        public static void InvokeLoadEvent()
        {
            appletLoaded.Invoke("", EventArgs.Empty);
        } 
        
    }
}
