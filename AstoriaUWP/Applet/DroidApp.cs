using AndroidInteropLib.android.content;
using AndroidXml;
using DalvikUWPCSharp.Classes;
using DalvikUWPCSharp.Disassembly.APKReader; 
using DalvikUWPCSharp.Reassembly;
using dex.net;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression; //using ICSharpCode.SharpZipLib.Zip;
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

        //
        public static event EventHandler appletLoaded = delegate { };

        // !RnD !
        public static event EventHandler DiagEventAppeared = delegate { };


    public EnhancedDexWriter dexWriter { get; private set; }
        public Dex dex;
        public AstoriaContext context;
        public DalvikCPU cpu;

        private static StorageFolder localFolder = ApplicationData.Current.LocalFolder;
        private StorageFolder appsRoot { get; set; }

        private EmuPage emPage;
        
        public StorageFolder localAppRoot { get; private set; }
        public StorageFolder resFolder { get; private set; }

        // CreateAsync(StorageFile sf)
        public static async Task<DroidApp> CreateAsync(StorageFile sf)
        {
            DroidApp result = new DroidApp();

            await result.CopyAPKToLocalStorage(sf);
            
            return result;
        }


        // CreateAsync(StorageFolder sf)
        public static async Task<DroidApp> CreateAsync(StorageFolder sf)
        {
            DroidApp result = new DroidApp();
            
            result.localAppRoot = sf;

            // Get APK Info from Local App Folder ...
            await result.GetAPKInfoFromLocalAppFolder();
            
            return result;
        }

        // CreateAsync(StorageFolder sf, EmuPage ep)
        public static async Task<DroidApp> CreateAsync(StorageFolder sf, EmuPage ep)
        {
            DroidApp result = new DroidApp();
            result.localAppRoot = sf;
            await result.GetAPKInfoFromLocalAppFolder();

            result.emPage = ep;

            return result;
        }

        private DroidApp() { }


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

            // Old case: extraction APK contents to localAppRoot
            /*try
            {
                //System.IO.Compression.ZipFile.ExtractToDirectory(copiedFile.Path, localAppRoot.Path);
                ZipFile.ExtractToDirectory(copiedFile.Path, localAppRoot.Path, Encoding.UTF8);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("[ERROR] APK extraction failed: " + ex.ToString());
            }*/

            // Experimental case: Step-by-Step extraction APK contents to localAppRoot
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
                                Debug.WriteLine("[ERROR] APK zip element ("+ extractPath + ") extraction failed: " + ex1.Message);
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
                    var meta = new {
                        label = appinfo.label,
                        packageName = appinfo.packageName,
                        versionName = appinfo.versionName,
                        icon = appinfo.iconFileName != null && appinfo.iconFileName.Count > 0 ? appinfo.iconFileName[0] : null
                    };
                    string json = Newtonsoft.Json.JsonConvert.SerializeObject(meta);
                    StorageFile metaFile = await localAppRoot.CreateFileAsync("appinfo.json", CreationCollisionOption.ReplaceExisting);
                    await FileIO.WriteTextAsync(metaFile, json);
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
            catch(System.IO.FileNotFoundException fnfEx)
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


        // GetAPKInfoFromLocalAppFolder
        public async Task GetAPKInfoFromLocalAppFolder()
        {

            // * manifest File parsing *
            
            StorageFile manifestFile;
            
            try
            {
                manifestFile = await localAppRoot.GetFileAsync("AndroidManifest.old");
                //manifestFile = await localAppRoot.GetFileAsync("AndroidManifest.xml");
            }
            catch (Exception ex1)
            {
                var dialog = new MessageDialog($"AndroidManifest.old manifest " +
                    $"file error \n\n{ex1.Message} \n\n " +
                    $"Application was not correctly installed. " +
                    $"Delete app (purge all app storage) and re-install app again.");
                //var dialog = new MessageDialog($"AndroidManifest.xml manifest file error \n\n{ex1.Message}");
                
                await dialog.ShowAsync();

                return;
            }

            // StorageFile resFile = await localAppRoot.GetFileAsync("resources.arsc");
            StorageFile resFile;
            try
            {
                resFile = await localAppRoot.GetFileAsync("resources.arsc");
            }
            catch (Exception ex2)
            {
                var dialog = new MessageDialog($"resources.arsc resource file error  \n\n{ex2.Message}");
                await dialog.ShowAsync();

                return;
            }


            // 
            try
            {
                resFolder = await localAppRoot.GetFolderAsync("res");
            }
            catch (Exception ex3)
            {
                var dialog = new MessageDialog($"GetFolderAsync(res) res folder error \n\n{ex3.Message}");
                await dialog.ShowAsync();

                return;
            }

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

            // foreach(Class m in methods)
            foreach (Class m in methods)
            {
                //DEBUG
                //Debug.WriteLine("[DroidApp info] " + m.ToString());

                if ( m.Name.Equals("com.example.ticom.myapp.MainActivity") 
                    || m.Name.Equals("com.example.ticom.myapp.MainActivity$1")
                    || m.Name.StartsWith("com.example.ticom.myapp.R") )
                {
                    TextWriter tw = new StringWriter();
                    dexWriter.WriteOutClass(m, ClassDisplayOptions.Fields, tw);
                    Debug.WriteLine(tw.ToString());

                    foreach(Field f in m.GetFields())
                    {
                        Debug.WriteLine(f.ToString());
                    }

                    var meths = m.GetMethods();

                    foreach (Method x in meths)
                    {
                        //DEBUG
                        //Debug.WriteLine(x.ToString());
                        Debug.WriteLine(dexWriter.WriteOutMethod(m, x, new Indentation()));

                        if (x.Name.Equals("onCreate") || x.Name.Equals("<init>"))
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

                    }//foreach(Method x...

                }//if...

            }//foreach(Class m...

            //context = new AstoriaContext();

        }// GetAPKInfoFromLocalFolder




        public BitmapImage GetAppIcon()
        {
            //TODO: change picked icon based on device DPI

            BitmapImage BI = null;
            if (metadata != null)
            { 
                try
                {
                    BI = new BitmapImage(new Uri(localAppRoot.Path 
                        + Disassembly.Util.ConvertPath(
                            metadata.iconFileName[metadata.iconFileName.Count - 1])));
                }
                catch
                {
                    BI = null;
                }
            }
            return BI;
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

        // Erase apps Root + apkFile data
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

        // Install
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
                            await sf.CopyAsync(localAppRoot, "AndroidManifest.old", 
                                NameCollisionOption.ReplaceExisting);
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
                                decoded.Replace("@layout/content_main", 
                                    res.layout.get("content_main").ToString());
                            }
                        }


                        //Debug.WriteLine(decoded);
                        //string decoded = Disassembly.Manifest.ManifestDecompressor.DecompressAXML(sfBytes);
                        //Inflated XML *should* always be larger than the binary one, so we shouldn't have to clear the file first (could be a future bug though.)
                        await Windows.Storage.FileIO.WriteTextAsync(sf, decoded);
                        Debug.WriteLine($"Converted file: {sf.Name}");
                    }
                    catch (Exception ex)
                    {
                        //Non-binary xml file found.
                        Debug.WriteLine("[Exception] Converting xaml: Non-binary xml file found. " +
                            "Details:"  + ex.Message + ", "+ sf.Name);
                    }
                    
                }
            }

            //Remove copied APK to save on disk space
            await apkFile.DeleteAsync();
            
        }//Install

        // Run
        public void Run(Frame frame)
        {
            frame.Navigate(typeof(EmuPage), this);
        }

        // Invoking func..
        public static void InvokeLoadEvent()
        {
            appletLoaded.Invoke("", EventArgs.Empty);
        }

        // ! RnD !
        public static void InvokeDiagEvent()
        {
            DiagEventAppeared.Invoke("", EventArgs.Empty);
        }

    }
}
