﻿// EmuPage
//
//
//

using AndroidInteropLib;
using AndroidInteropLib.android.view;
using AndroidXml;
using DalvikUWPCSharp.Applet;
using DalvikUWPCSharp.Classes;
using DalvikUWPCSharp.Reassembly;
using DalvikUWPCSharp.Reassembly.UI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using System.Xml.Linq;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Foundation.Metadata;
using Windows.Storage;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace DalvikUWPCSharp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class EmuPage : Page
    {
        public DroidApp RunningApp;

        // !
        private Renderer UIRenderer;

        private DalvikCPU cpu;



        public EmuPage()
        {
            this.InitializeComponent();
            this.Loaded += EmuPage_Loaded;

            //RnD
            Windows.UI.Xaml.Window.Current.SizeChanged += Current_SizeChanged;
            
        }

        // RnD start
        private void Current_SizeChanged(object sender, Windows.UI.Core.WindowSizeChangedEventArgs e)
        {
            //UserControl appView = (UserControl)RenderTargetBox.Child;
            //appView.Width = (this.ActualWidth) * (40/37);
            //appView.Height = (this.ActualHeight - 48) * (40 / 37);
        }
        // RnD end

        // 
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            try
            {
                //base.OnNavigatedTo(e);
                if (e.Parameter.GetType().Equals(typeof(DroidApp)))
                {
                    RunningApp = (DroidApp)e.Parameter;
                    appImage.Source = RunningApp.appIcon;

                    // RnD start
                    UIRenderer = new Renderer((DroidApp)e.Parameter);
                    cpu = new DalvikCPU(((DroidApp)e.Parameter).dex, ((DroidApp)e.Parameter).metadata.packageName, this);
                    cpu.Start();
                    //await 
                    Render();
                    // RnD end
                }
                else if (e.Parameter.GetType().Equals(typeof(StorageFolder)))
                {
                    setPreloadStatusText("Setting up app environment");
                    RunningApp = await DroidApp.CreateAsync((StorageFolder)e.Parameter);
                }
            }
            catch
            {
                // Plan B - go home 
                Frame.Navigate(typeof(MainPage));
            }

        }//OnNavigatedTo end




        public void setPreloadStatusText(string text)
        {
            statusTextblock.Text = text;
        }

        public void preloadDone()
        {
            PreSplashGrid.Visibility = Visibility.Collapsed;
        }

        private async void Render()
        {
            var layout = await UIRenderer.CurrentApp.resFolder.GetFolderAsync("layout");
            
            StorageFile sf = null;
            try
            {
                sf = await layout.GetFileAsync("activity_main.xml");
            }
            catch(Exception ex)
            {
                Debug.WriteLine("EmuPage - Render - layout.GetFileAsync - Exception: " + ex.Message);

                ContentDialog msgDialog = new ContentDialog()
                {
                    Title = "EmuPage - Render - layout.GetFileAsync - Exception",
                    Content = "Activity_main.xml critical problems: " + ex.Message,
                    PrimaryButtonText = "OK"
                };

                ContentDialogResult result = await msgDialog.ShowAsync();
            }

            UIElement child = null;

            try
            {
                child = await UIRenderer.RenderXmlFile(sf);
            }
            catch (Exception ex2)
            {
                Debug.WriteLine("EmuPage - Render - UIRenderer.RenderXmlFile Exception: " + ex2.Message);
            }

            //UserControl uc = (UserControl)child;
            UIElement uc = child;


            var widthBinding = new Binding();
            widthBinding.Converter = new EPDPConverter();
            widthBinding.ElementName = "RenderTargetBox";
            widthBinding.ConverterParameter = RenderTargetBox.Width;

            var hBinding = new Binding();
            hBinding.Converter = new EPDPConverter();
            hBinding.ElementName = "RenderTargetBox";
            hBinding.ConverterParameter = RenderTargetBox.Height;

            //uc.SetBinding(FrameworkElement.WidthProperty, widthBinding);
            //uc.SetBinding(FrameworkElement.HeightProperty, hBinding);

            try
            {
                RenderTargetBox.Child = (await UIRenderer.RenderXmlFile(sf));
            }
            catch (Exception ex3)
            {
                Debug.WriteLine("EmuPage - Render - RenderTargetBox.Child Exception: " + ex3.Message);
            }

            try
            {
                RenderTargetGrid.Children.Add(await UIRenderer.RenderXmlFile(sf));
            }
            catch (Exception ex4)
            {
                Debug.WriteLine("EmuPage - Render - RenderTargetGrid.Children.Add Exception: " + ex4.Message);
            }


            SetTitleBarColor(attr.colorPrimaryDark);

            cpu.Start();
            
            Windows.UI.ViewManagement.ApplicationView.GetForCurrentView();//.

            //RnD
            //await RenderPage();

        }//

        public void SetContentView(View v)
        {
            //TEST
            RenderTargetGrid.Children.Clear();
            RenderTargetGrid.Children.Add(v);
        }

        public void SetTitleBarColor(Color color)
        {
            var appView = Windows.UI.ViewManagement.ApplicationView.GetForCurrentView();
            var titleBar = appView.TitleBar;
            titleBar.BackgroundColor = color;
            titleBar.ButtonBackgroundColor = color;
            titleBar.ButtonInactiveBackgroundColor = color;
            titleBar.InactiveBackgroundColor = color;

            Color hover = ColorUtil.HoverColor(color);
            Color pressed = ColorUtil.PressedColor(color);
            titleBar.ButtonHoverBackgroundColor = hover;
            titleBar.ButtonPressedBackgroundColor = pressed;

            //appView.Title = UIRenderer.CurrentApp.metadata.label;
            //titleBar.ButtonHoverBackgroundColor = Color.FromArgb(10, 255, 255, 255);

            if (ApiInformation.IsTypePresent("Windows.UI.ViewManagement.StatusBar"))
            {
                Windows.UI.ViewManagement.StatusBar.GetForCurrentView().BackgroundColor = color;
                Windows.UI.ViewManagement.StatusBar.GetForCurrentView().BackgroundOpacity = 1;
                Windows.UI.ViewManagement.StatusBar.GetForCurrentView().ForegroundColor = Colors.White;
            }

        }

        public void SetNavBarColor(Color color)
        {
            NavBarBackgroundGrid.Background = new SolidColorBrush(color);
        }

        public void SetWinBackColor(Color color)
        {
            //TEST
            RenderTargetGrid.Background = new SolidColorBrush(color);
        }

        private async Task RenderPage()
        {
            

            //Take content_main.xml and render it (for now)
            //var resFolder doc = RunningApp.localAppRoot.GetFolderAsync()

            // RnD start
            var layout = await UIRenderer.CurrentApp.resFolder.GetFolderAsync("layout");

            StorageFile sf = await layout.GetFileAsync("content_main.xml");

            using (MemoryStream stream = new MemoryStream(await Disassembly.Util.ReadFile(sf)))
            {
                AndroidXmlReader reader = new AndroidXmlReader(stream);

                reader.MoveToContent();
                XDocument document = XDocument.Load(reader);

                string p1nspace = "{http://schemas.android.com/apk/res/android}";

                foreach (XElement xe in document.Element("RelativeLayout").Elements())
                {
                    if(xe.Name.ToString().Equals("TextView"))
                    {
                        TextBlock tv = new TextBlock();

                        //default position for android app

                        tv.HorizontalAlignment = HorizontalAlignment.Center;
                        tv.VerticalAlignment = VerticalAlignment.Center;
                        
                        string content = "";
                        foreach(XAttribute xa in xe.Attributes())
                        {
                            content += $"Attribute: {xa.Name}\nValue: {xa.Value}\nIsNamespaceDeclaration: {xa.IsNamespaceDeclaration}\n\n";
                        } // * /
                        
                        //
                        foreach(XAttribute attr in xe.Attributes())
                        {
                            //attr.
                        }
                        //
                        //var ns = document.Root.Name.Namespace;
                        
                        //This is a hack
                        string[] content1 = xe.ToString().Split('"');
                        
                        // *
                        foreach(string s in content1)
                        {
                            if(!s.Contains("p1") && !s.Contains("-2"))
                            {
                                tv.Text = s;
                                break;
                            }
                        }
                        string content2 = xe.Attribute(p1nspace+"text").Value;
                        //-2 represents "wrap_content", essentially "autosize"
                        int width = int.Parse(xe.Attribute(p1nspace + "layout_width").Value);
                        int height = int.Parse(xe.Attribute(p1nspace + "layout_height").Value);
                        //string content2 = "null";
                        tv.Text = content2;

                        //RnD
                        RenderTargetGrid.Children.Add(tv);
                    }
                }
                
                //decoded = document.ToString();
            }
            // RnD end
        }//RenderPage

        //Currently only support content_main.xml since the dissassembler cant yet parse activity_main.xml
        private void EmuPage_Loaded(object sender, RoutedEventArgs e)
        {
            var appView = Windows.UI.ViewManagement.ApplicationView.GetForCurrentView();

            try
            {
                appView.Title = RunningApp.metadata.label;
            }
            catch (Exception ex1)
            {
                //Plan A
                //var dialog = new MessageDialog($"RunningApp.metadata is null (broken object)  \n\n{ex1.Message}");
                //dialog.ShowAsync();

                //Plan B
                Frame.Navigate(typeof(MainPage));

                return;
            }


            cpu = new DalvikCPU(RunningApp.dex, RunningApp.metadata.packageName, this);

            cpu.Start();

            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;

            SystemNavigationManager.GetForCurrentView().BackRequested += EmuPage_BackRequested;

        }// EmuPage_Loaded

        private void EmuPage_BackRequested(object sender, BackRequestedEventArgs e)
        {
            GoBack(sender, null);
        }

        private void GoHome(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage));
        }

        private void GoBack(object sender, RoutedEventArgs e)
        {
            try
            {
                cpu.GoBack();
            }
            catch
            {
                // Plan B - go home
                Frame.Navigate(typeof(MainPage));
            }
        }
    }
}