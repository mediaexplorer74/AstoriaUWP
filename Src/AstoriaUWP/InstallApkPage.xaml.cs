// InstallApkPage

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.System;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace DalvikUWPCSharp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class InstallApkPage : Page
    {

        // InstallApkPage
        public InstallApkPage()
        {
            this.InitializeComponent();

            Disassembly.Util.apkpage = this;

            // Hide Install button
            install_Button.Visibility = Visibility.Collapsed;

            //DIAG 
            //AddDebugMessage($"File Name: {Disassembly.Util.CurrentFile.Name}");

            // DIAG -- TODO
            AddDebugMessage("Loading...");

            //appletLoaded();

            //Applet.Applet.appletLoaded += Applet_appletLoaded;

            //Applet_appletLoaded(null, EventArgs.Empty);

            // !
            Applet.DroidApp.appletLoaded += appletLoaded;

            Applet.DroidApp.DiagEventAppeared += DiagEventAppeared;

        }//InstallApkPage


        // AddDebugMessage
        private void AddDebugMessage(string text)
        {
            Window.Current.Activate();

            Description_Textblock.Text += $" {text} ";//$"\n{text}";
        }//AddDebugMessage



        // SetDisplayName
        public void SetDisplayName(string s)
        {
            displayTitle.Text = s;
        }//SetDisplayName

        // SetAppMetadata
        public void SetAppMetadata(DalvikUWPCSharp.Disassembly.APKReader.ApkInfo info)
        {
            appLabelText.Text = $"App Name: {info.label}";
            packageNameText.Text = $"Package: {info.packageName}";
            versionText.Text = $"Version: {info.versionName} (Code: {info.versionCode})";
            if (info.Permissions != null && info.Permissions.Count > 0)
                permissionsText.Text = $"Permissions: {string.Join(", ", info.Permissions)}";
            else
                permissionsText.Text = "Permissions: None";
        }

        // async
        //   appletLoaded        
        public void appletLoaded(object sender, EventArgs e)
        {
            installProgbar.Visibility = Visibility.Collapsed;
            installBarChrome.Visibility = Visibility.Visible;
            AddDebugMessage("Page loaded.");
            // Show Install button
            install_Button.Visibility = Visibility.Visible;
            // Set app metadata UI
            if (Disassembly.Util.CurrentApp != null && Disassembly.Util.CurrentApp.metadata != null)
                SetAppMetadata(Disassembly.Util.CurrentApp.metadata);

            // Показать иконку приложения, если она есть
            try
            {
                var appRoot = Disassembly.Util.CurrentApp.localAppRoot;
                var imageFileTask = appRoot.GetFileAsync("app_image.png");
                imageFileTask.AsTask().ContinueWith(t =>
                {
                    if (t.Status == TaskStatus.RanToCompletion)
                    {
                        var file = t.Result;
                        var uri = new Uri(file.Path);
                        Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                        {
                            app_image.Source = new BitmapImage(uri);
                        });
                    }
                });
            }
            catch { }
        }//appletLoaded

        // ******************************************************************            
        public void DiagEventAppeared(object sender, EventArgs e)
        {
            // POP 
            AddDebugMessage(App.GlobalStr);  // REDO IT          
        }//appletLoaded

        // ******************************************************************



        // cancel_Button_Click
        private async void cancel_Button_Click(object sender, RoutedEventArgs e)
        {
            //Disassembly.Util.CurrentApp.Purge();

            try
            {
                //TEST
                //Disassembly.Util.CurrentApp.Purge();
            }
            catch (Exception ex1)
            {
                var dialog = new MessageDialog($"Disassembly.Util.CurrentApp.Purge() error!  \n\n{ex1.Message}");
                await dialog.ShowAsync();

                //return;
            }

            // Navigate to MainPage
            Frame.Navigate(typeof(MainPage));

        }//cancel_Button_Click


        // forcerlbutton_Click (appletLoaded ?)
        private void forcerlbutton_Click(object sender, RoutedEventArgs e)
        {
            appletLoaded("", EventArgs.Empty);

        }//forcerlbutton_Click


        // install_Click
        private async void install_Click(object sender, RoutedEventArgs e)
        {
            //Put up loading screen
            Description_Textblock.Text = "Installing...";

            installBarChrome.Visibility = Visibility.Collapsed;
            installProgbar.Visibility = Visibility.Visible;


            //Install app
            //await Disassembly.Util.CurrentApp.Install();

            try
            {
                await Disassembly.Util.CurrentApp.Install();
            }
            catch (Exception ex1)
            {
                var dialog = new MessageDialog($"Disassembly.Util.CurrentApp.Install() error!  \n\n{ex1.Message}");
                await dialog.ShowAsync();

                //return;
            }


            //Stop install elements and show "Done"/"Open" screen
            installProgbar.Visibility = Visibility.Collapsed;

            installBarChrome.Visibility = Visibility.Visible;

            Description_Textblock.Text = "App installed";

            install_Button.Content = "Apps list";
            //cancel_Button.Content = "Done"; // TEMP

            //remove old button click event handlers and use new ones
            install_Button.Click -= install_Click;

            //cancel_Button.Click -= cancel_Button_Click; // TEMP

            install_Button.Click += Done_Click;

            //cancel_Button.Click += Open_Click; // TEMP

            install_Button.Visibility = Visibility.Visible;

        }//install_Click

        // Done_Click
        private void Done_Click(object sender, RoutedEventArgs e)
        {
            //Go to home screen
            Frame.Navigate(typeof(MainPage));
        }

        // Open_Click
        private void Open_Click(object sender, RoutedEventArgs e)
        {
            //Start app (run on Emulator Page)
            Frame.Navigate(typeof(EmuPage));
        }
    }
}
