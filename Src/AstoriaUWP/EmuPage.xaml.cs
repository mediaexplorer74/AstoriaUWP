// EmuPage

using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Xml.Linq;
using Windows.Foundation.Metadata;
using Windows.Storage;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

using AndroidInteropLib;
using AndroidInteropLib.android.view;
using AndroidXml;

using DalvikUWPCSharp.Applet;
using DalvikUWPCSharp.Classes;
using DalvikUWPCSharp.Reassembly;
using DalvikUWPCSharp.Reassembly.UI;


// DalvikUWPCSharp
namespace DalvikUWPCSharp
{
    // EmuPage class
    public sealed partial class EmuPage : Page
    {
        public DroidApp RunningApp; //

        private Renderer UIRenderer; // 

        private DalvikCPU cpu; //


        // EmuPage
        public EmuPage()
        {
            this.InitializeComponent();
            this.Loaded += EmuPage_Loaded;

            //RnD
            Windows.UI.Xaml.Window.Current.SizeChanged += Current_SizeChanged;
            
        }//EmuPage end


        // Current_SizeChanged
        private void Current_SizeChanged(object sender, Windows.UI.Core.WindowSizeChangedEventArgs e)
        {
            //UserControl appView = (UserControl)RenderTargetBox.Child;
            //appView.Width = (this.ActualWidth) * (40/37);
            //appView.Height = (this.ActualHeight - 48) * (40 / 37);

        }//Current_SizeChanged end 


        // OnNavigatedTo
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
                    cpu = default;
                    if ( ((DroidApp)e.Parameter).metadata != null )
                    {
                        try
                        {
                            cpu = new DalvikCPU(((DroidApp)e.Parameter).dex,
                                ((DroidApp)e.Parameter).metadata.packageName,
                                this);
                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine("[ex] Dalvik CPU create error: " + ex.Message);
                        }
                    }
                    
                    if (cpu != null)
                    {
                        cpu.Start();
                        //await 
                        Render();
                    }
                    else
                    {
                        // Plan B - go home 
                        Frame.Navigate(typeof(MainPage));
                    }
                    
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



        // setPreloadStatusText
        public void setPreloadStatusText(string text)
        {
            statusTextblock.Text = text;

        }//setPreloadStatusText end


        // preloadDone
        public void preloadDone()
        {
            PreSplashGrid.Visibility = Visibility.Collapsed;
        }//preloadDone end


        // Render
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
                Debug.WriteLine("[EmuPage] [Render] activity_main.xml Layout Exception: " + ex.Message);

                ContentDialog msgDialog = new ContentDialog()
                {
                    Title = "Activity_main.xml not found! " +
                      "It must be created manually (in App Storage).",
                    Content = "Activity_main.xml critical problems: " + ex.Message,
                    PrimaryButtonText = "OK"
                };

                ContentDialogResult result = await msgDialog.ShowAsync();

                // return because of sf is null 
                return;
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

        }//Render end


        // SetContentView
        public void SetContentView(View v)
        {
            //TEST
            RenderTargetGrid.Children.Clear();
            RenderTargetGrid.Children.Add(v);

        }//SetContentView end


        // SetTitleBarColor 
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

        }//SetTitleBarColor end

        // SetNavBarColor
        public void SetNavBarColor(Color color)
        {
            NavBarBackgroundGrid.Background = new SolidColorBrush(color);

        }//SetNavBarColor end


        // SetWinBackColor
        public void SetWinBackColor(Color color)
        {
            //TEST
            RenderTargetGrid.Background = new SolidColorBrush(color);

        }//SetWinBackColor end


        // * RenderPage *
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
                        } 
                        
                        //
                        foreach(XAttribute attr in xe.Attributes())
                        {
                            //attr.
                        }
                        //
                        //var ns = document.Root.Name.Namespace;
                        
                        //This is a hack
                        string[] content1 = xe.ToString().Split('"');
                        
                        // ...
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

                        RenderTargetGrid.Children.Add(tv);
                    }
                }
                
                //decoded = document.ToString();
            }
           
        }//RenderPage end


        // * EmuPage_Loaded *
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
                Debug.WriteLine("[ex] EmuPage_Loaded problems: " + ex1.Message);
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

        }// EmuPage_Loaded end


        // EmuPage_BackRequested
        private void EmuPage_BackRequested(object sender, BackRequestedEventArgs e)
        {
            GoBack(sender, null);

        }//EmuPage_BackRequested end


        // GoHome
        private void GoHome(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage));

        }//GoHome end


        // GoBack
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
        }//GoBack end

    }//EmuPage class end

}//namespace end
