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

            //DIAG 
            //AddDebugMessage($"File Name: {Disassembly.Util.CurrentFile.Name}");

            AddDebugMessage("Loading...");

            //appletLoaded();

            //Applet.Applet.appletLoaded += Applet_appletLoaded;

            //Applet_appletLoaded(null, EventArgs.Empty);

            Applet.DroidApp.appletLoaded += appletLoaded;

        }//InstallApkPage

        // SetDisplayName
        public void SetDisplayName(string s)
        {
            displayTitle.Text = s;
        }//SetDisplayName

        // appletLoaded
        //async
        public void appletLoaded(object sender, EventArgs e)
        {

            installProgbar.Visibility = Visibility.Collapsed;

            installBarChrome.Visibility = Visibility.Visible;

            //displayTitle.Text = Disassembly.Util.CurrentApp.metadata.label;
            //AddDebugMessage("Icon Path: " + Disassembly.Util.CurrentApp.metadata.iconFileName[0]);

            return;
            try
            {
                app_image.Source = Disassembly.Util.CurrentApp.appIcon;
            }
            catch (Exception ex1)
            {
                var dialog = new MessageDialog($"Disassembly.Util.CurrentApp object is null!  \n\n{ex1.Message}");
                //await 
                    dialog.ShowAsync();

                return;
            }

            AddDebugMessage("page loaded.");


            // Permissions cycle
            foreach (string s in Disassembly.Util.CurrentApp.metadata.Permissions)
            {
                Debug.WriteLine(s);
                AddDebugMessage(s);
            }

        }//appletLoaded

        // AddDebugMessage
        private void AddDebugMessage(string text)
        {
            Description_Textblock.Text += $"\n{text}";
        }//AddDebugMessage



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

            install_Button.Content = "Open";

            cancel_Button.Content = "Done";

            //remove old button click event handlers and use new ones
            install_Button.Click -= install_Click;

            cancel_Button.Click -= cancel_Button_Click;

            install_Button.Click += Done_Click;

            cancel_Button.Click += Open_Click;

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
