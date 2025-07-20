using DalvicUWPCSharp.Model;
using DalvikUWPCSharp.Applet;
using DalvikUWPCSharp.Classes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Foundation.Metadata;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace DalvikUWPCSharp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();

            this.Loaded += MainPage_Loaded;
        }

        private async void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            SetTitleBarColor();

            // Load and render the new ShapeView primitives demo layout
            /*try
            {
                var layoutFile = await StorageFile.GetFileFromApplicationUriAsync(
                      new Uri("ms-appx:///Assets/SampleLayouts/shapeview_primitives_demo.xml"));
                var renderer = new DalvikUWPCSharp.Reassembly.UI.Renderer(null);
                var layoutElement = await renderer.RenderXmlFile(layoutFile);
                if (layoutElement != null)
                {
                    bgGrid.Children.Add(layoutElement);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Failed to load shapeview_primitives_demo.xml: {ex.Message}");
            }*/

            var appsRoot = await ApplicationData.Current.LocalFolder.CreateFolderAsync("Apps", CreationCollisionOption.OpenIfExists);
            foreach (StorageFolder sf in await appsRoot.GetFoldersAsync())
            {
                AppListBox.Items.Add(sf.Name);
            }


            //DalvikCPU MainCPU = new DalvikCPU();
            //MainCPU.Code = Util.GetSampleCode();
            //MainCPU.RunVM();

            // ************Experimental ***********
            //DalvikCPU MainCPU = new DalvikCPU();
            //MainCPU.Code = Util.GetSampleCode();
            //MainCPU.RunVM();
            // ************************************


        }

        public void SetTitleBarColor()
        {
            var appView = Windows.UI.ViewManagement.ApplicationView.GetForCurrentView();
            var titleBar = appView.TitleBar;
            titleBar.BackgroundColor = null;
            titleBar.ButtonBackgroundColor = null;
            titleBar.InactiveBackgroundColor = null;
            titleBar.ButtonInactiveBackgroundColor = null;
            titleBar.ButtonPressedBackgroundColor = null;
            titleBar.ButtonHoverBackgroundColor = null;
            

            appView.Title = string.Empty;
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;
            //appView.Title = UIRenderer.CurrentApp.metadata.label;
            //titleBar.ButtonHoverBackgroundColor = Color.FromArgb(10, 255, 255, 255);

            if (ApiInformation.IsTypePresent("Windows.UI.ViewManagement.StatusBar"))
            {
                Windows.UI.ViewManagement.StatusBar.GetForCurrentView().BackgroundColor = null;
                Windows.UI.ViewManagement.StatusBar.GetForCurrentView().BackgroundOpacity = 0;
                Windows.UI.ViewManagement.StatusBar.GetForCurrentView().ForegroundColor = null;
            }

        }

        
        // AppListBox_SelectionChanged
        private async void AppListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Check Operation
            if (e.AddedItems[0] is string)
            {
                // Case A : Run App

                bgGrid.Children.Add(new DroidAppLoadingPopup());

                var appsRoot = await ApplicationData.Current.LocalFolder.CreateFolderAsync("Apps", CreationCollisionOption.OpenIfExists);

                string lbi = (string)e.AddedItems[0];

                DroidApp da = null;

                try
                {
                    da = await DroidApp.CreateAsync(await appsRoot.GetFolderAsync(lbi));
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("DroidApp.CreateAsync - Exception: " + ex.Message);

                    ContentDialog msgDialog = new ContentDialog()
                    {
                        Title = "DroidApp.CreateAsync - Exception",
                        Content = ex.Message,
                        PrimaryButtonText = "OK"
                    };

                    ContentDialogResult result = await msgDialog.ShowAsync();

                    // Go to Setting Page
                    Frame.Navigate(typeof(MainPage));

                    return;
                }

                da.Run(Frame);
            }
            else
            {
                // Case B: Navigation

                Windows.UI.Xaml.Controls.ListBoxItem T1 = e.AddedItems[0] as Windows.UI.Xaml.Controls.ListBoxItem; 

                // get string index 
                string str_idx = T1.Content.ToString();

                if (str_idx == "Settings")
                {
                    // Go to Setting Page
                    Frame.Navigate(typeof(SettingsPage));
                }

                if (str_idx == "Load and install testdpc7.apk")
                {
                    // Go to Setting Page
                    Frame.Navigate(typeof(InstallApkPage));

                    // Ensure the current window is active
                    Window.Current.Activate();

                    bool Result = await Disassembly.Util.LoadAPK2("testdpc7.apk");
                    

                    if (Result == false)
                    {
                        Debug.WriteLine("Some problems: testdpc7.apk test file not found at Pictures folder");

                        ContentDialog msgDialog = new ContentDialog()
                        {
                            Title = "Caution: testdpc7.apk test file not found at Pictures folder",
                            Content = "Install button is for test purposes only.",
                            PrimaryButtonText = "OK"
                        };

                        ContentDialogResult result = await msgDialog.ShowAsync();

                        // Go to Setting Page
                        Frame.Navigate(typeof(MainPage));
                    }
                }

            }//else

        }//AppListBox_SelectionChanged end

        // chooseFileButton Click handler
        private async void chooseFileButton_Click(object sender, RoutedEventArgs e)
        {
            FileOpenPicker openPicker = new FileOpenPicker();
            openPicker.ViewMode = PickerViewMode.Thumbnail;
            openPicker.SuggestedStartLocation = PickerLocationId.Desktop;
            openPicker.CommitButtonText = "Choose APK file";
            openPicker.FileTypeFilter.Add(".apk");
            var file = await openPicker.PickSingleFileAsync();

            // check the file choosed or not
            if (file != null)
            {
                string filePath = file.Name;//.Path;
                Debug.WriteLine("Choosed apk file: " + filePath);

                // ----------------------------------------------------------------

                // Go to Setting Page
                Frame.Navigate(typeof(InstallApkPage));

                // Ensure the current window is active
                Window.Current.Activate();

                bool Result = await Disassembly.Util.LoadAPK2(filePath);


                if (Result == false)
                {
                    Debug.WriteLine("Some problems: "+ filePath +" file not found at Pictures folder!");

                    ContentDialog msgDialog = new ContentDialog()
                    {
                        Title = "Caution: " + filePath + " file not found at Pictures folder.",
                        Content = 
                        "Please place apk file on special location (Picture folder) before choosing it.",
                        PrimaryButtonText = "OK"
                    };

                    ContentDialogResult result = await msgDialog.ShowAsync();

                    // Go to Setting Page
                    Frame.Navigate(typeof(MainPage));
                }

                // ----------------------------------------------------------------
            }

        }//openButton_Click end

        // Purge Button handler: Purge App folder operation
        private async void purgeButton_Click(object sender, RoutedEventArgs e)
        {
            await Disassembly.Util.PurgeAppsFolder();

            // Go to Main Page
            Frame.Navigate(typeof(MainPage));

        }//purgeButton_Click end

        
    }
}
