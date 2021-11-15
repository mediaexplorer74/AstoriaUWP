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

            var appsRoot = await ApplicationData.Current.LocalFolder.CreateFolderAsync("Apps", CreationCollisionOption.OpenIfExists);

            // DEBUG
            //var dialog = new MessageDialog("ppsRoot is:  " + appsRoot.GetFoldersAsync() + " .");
            //await dialog.ShowAsync();

            // appsRoot.GetFoldersAsync() - get subfolders of (in) appsRoot folder
            foreach (StorageFolder sf in await appsRoot.GetFoldersAsync())
            {

                // DEBUG
                //var dialog = new MessageDialog("App  " + sf.Path + " added.");
                //await dialog.ShowAsync();

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

        // Purge App folder
        private async void button_Click(object sender, RoutedEventArgs e)
        {
            await Disassembly.Util.PurgeAppsFolder();
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

                DroidApp da = await DroidApp.CreateAsync(await appsRoot.GetFolderAsync(lbi));

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

                if (str_idx == "Install Apk")
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
                            Title = "Caution",
                            Content = "Install button is for test purposes only (it's works only for testdpc7.apk in Pictures folder). To install your own apk file, run(click) it via Explorer.",

                            PrimaryButtonText = "OK"
                        };

                        ContentDialogResult result = await msgDialog.ShowAsync();

                        // Go to Setting Page
                        Frame.Navigate(typeof(MainPage));
                    }
                }

            }
        }
    }
}
