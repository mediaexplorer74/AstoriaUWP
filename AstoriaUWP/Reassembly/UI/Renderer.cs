// Reassembly - UI - Renderer

using AndroidInteropLib.android.content;
using AndroidInteropLib.android.support.design.widget;
using AndroidXml;
using DalvikUWPCSharp.Applet;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Windows.Storage;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;


namespace DalvikUWPCSharp.Reassembly.UI
{
    public class Renderer
    {
        public DroidApp CurrentApp;
        //public Frame CurrentFrame; //For statusbar color emulation

        private string p1nspace = "{http://schemas.android.com/apk/res/android}";

        public Renderer(DroidApp da)
        {
            CurrentApp = da;
        }

        public async Task<UIElement> RenderXmlFile(StorageFile sf)
        {
            string decoded;

            /*
            //using (MemoryStream stream = new MemoryStream(await Disassembly.Util.ReadFile(sf)))

            byte[] byteArray = await DalvikUWPCSharp.Disassembly.Util.ReadFile(sf);
            Stream stream1 = new MemoryStream(byteArray, true);

            //ZipArchive zzz = new ZipArchive(stream1);

            //AndroidXmlReader testreader1 = new AndroidXmlReader(stream1);

            //this.zf = new ZipArchive(stream);


            var testdocument1 = XDocument.Load(stream1);
            */

            MemoryStream teststream = new MemoryStream(await Disassembly.Util.ReadFile(sf));

            //AndroidXmlReader testreader = new AndroidXmlReader(teststream);

            //testreader.MoveToContent();

            XDocument testdocument = null;

            try
            {
                testdocument = XDocument.Load(teststream);//(testreader);
                decoded = testdocument.ToString();
            }
            catch 
            {
                Debug.WriteLine("Houston, we have some problems!");
            }

            try
            {
                foreach (XElement xe in testdocument.Elements())
                {
                    //Should only be 1 element
                    return await RenderObject(xe);
                }
            }
            catch
            {

                Debug.WriteLine("! CRITICAL ERROR: Invalid XML File " + sf.DisplayName + " !");

            }

            /*
            //(stream)
            using (MemoryStream stream = new MemoryStream(await Disassembly.Util.ReadFile(sf)))
            {
                AndroidXmlReader reader = new AndroidXmlReader(stream);

                //RnD
                //reader.MoveToContent();

                XDocument document = null;
                try
                {
                    if (reader.ReadState == System.Xml.ReadState.Interactive)
                    {
                        document = XDocument.Load(reader);
                    }
                    else
                    {
                        //document = XDocument.Parse(reader); //  case where there is no content returned
                    }

                    decoded = document.ToString();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Renderer - RenderXmlFile Exception: " + ex.Message);
                }

                //string p1nspace = "{http://schemas.android.com/apk/res/android}";

                try
                {
                    foreach (XElement xe in document.Elements())
                    {
                        //Should only be 1 element
                        return await RenderObject(xe);
                    }
                }
                catch
                {

                    Debug.WriteLine("! CRITICAL ERROR: Invalid XML File " + sf.DisplayName + " !");
                    
                }
                //decoded = document.ToString();

                
            }//using (MemoryStream...
            */

            //RnD (TODO)
            //return decoded as UIElement;

            return null;

        }//RenderXMLFile


        // RenderObject
        public async Task<UIElement> RenderObject(XElement xe)
        {
            string xeName = xe.Name.ToString();
            
            bool nestedObjs = xe.HasElements;

            //RnD
            //AstoriaContext context = new AstoriaContext();
            //AstoriaContext context = new AstoriaContext(da, res);

            if (xeName.Equals("android.support.design.widget.AppBarLayout"))
            {
                //This manipulates the appbar. For now, let's just make it a grid. Usually contains toolbar.
                
                //double height = DPtoEP(56);
                
                Grid container = new Grid();
                container.VerticalAlignment = VerticalAlignment.Top;
                container.HorizontalAlignment = HorizontalAlignment.Stretch;

                container.Height = attr.actionBarSize; //DPtoEP(56);

                if(nestedObjs)
                {
                    foreach(XElement xe1 in xe.Elements())
                    {
                        container.Children.Add(await RenderObject(xe1));
                    }
                }
                return container;
            }

            else if (xeName.Equals("android.support.design.widget.CoordinatorLayout"))
            {
                CoordinatorLayout cl = new CoordinatorLayout();
                if(nestedObjs)
                {
                    foreach(XElement xe1 in xe.Elements())
                    {
                        cl.Add(await RenderObject(xe1));
                    }
                }

                return cl;
                
            }

            else if(xeName.Equals("android.support.design.widget.FloatingActionButton"))
            {
                //Ignore gravity for now, is typically bottom right
                //Button FAB = new Button();
                //FloatingActionButton FAB = new FloatingActionButton(context, new AstoriaAttrSet(xe));
                //FAB.HorizontalAlignment = HorizontalAlignment.Right;
                //FAB.VerticalAlignment = VerticalAlignment.Bottom;
                //FAB.Width = 52;
                //FAB.Height = 52;
                //xe.Attribute(XName.Get())
                //FAB.Content = "src: " + xe.Attribute(XName.Get(p1nspace + "src")).Value;
                //FAB.Content = "src: " + CurrentApp.metadata.resStrings[xe.Attribute(XName.Get(p1nspace + "src")).Value][0];
                //FAB.Margin = new Thickness(10);
                return null;
                //return FAB;
            }

            else if(xeName.Equals("android.support.v7.widget.Toolbar"))
            {
                //return null;
                //Read attributes on toolbar and return it
                AndroidToolbar at = new AndroidToolbar();
                at.SetTitle(CurrentApp.metadata.label);
                //at.Height = DPtoEP(56);

                return at;
            }

            else if(xeName.Equals("include"))
            {
                //Get layout attribute, render the .xml, and return that
                //return null;
                string relUri = xe.Attribute("layout").Value;
                //Take current app path, pass 
                string path = CurrentApp.resFolder.Path + relUri.Replace('@', '\\').Replace('/', '\\') + ".xml";
                StorageFile sf = await StorageFile.GetFileFromPathAsync(path);

                return await RenderXmlFile(sf);
            }

            else if (xeName.Equals("RelativeLayout"))
            {
                //Return Grid with objects inside
                Grid container = new Grid();
                if (nestedObjs)
                {
                    foreach (XElement xe1 in xe.Elements())
                    {
                        container.Children.Add(await RenderObject(xe1));
                    }
                }

                return container;
            }

            else if (xeName.Equals("TextView"))
            {
                TextBlock tv = new TextBlock();

                string content = xe.Attribute(p1nspace + "text").Value;
                tv.Text = content;
                //Default left padding is 16dp left, 8dp tall
                tv.Margin = new Thickness(14.8, 7.4, 14.8, 7.4);
                //-2 represents "wrap_content", essentially "autosize"
                int width = int.Parse(xe.Attribute(p1nspace + "layout_width").Value);
                int height = int.Parse(xe.Attribute(p1nspace + "layout_height").Value);

                if(width > -1)
                {
                    tv.Width = width;
                }

                if(height > -1)
                {
                    tv.Height = height;
                }

                //Default text color is gray 115
                tv.Foreground = new SolidColorBrush(Color.FromArgb(255, 115, 115, 115));
                return tv;
            }
            // *** experimental - begin ***
            else if (xeName.Equals("FrameLayout"))
            {
                /*
                //Return Grid with objects inside
                Grid container = new Grid();
                if (nestedObjs)
                {
                    foreach (XElement xe1 in xe.Elements())
                    {
                        container.Children.Add(await RenderObject(xe1));
                    }
                }

                return container;
                */

                //Ignore gravity for now, is typically bottom right
                Button FAB = new Button();
                
                //Context context = new Context();

                //FloatingActionButton FAB = new FloatingActionButton(context, new AstoriaAttrSet(xe));
                FAB.HorizontalAlignment = HorizontalAlignment.Right;
                FAB.VerticalAlignment = VerticalAlignment.Bottom;
                FAB.Width = 52;
                FAB.Height = 52;
                
                //xe.Attribute(XName.Get())
                //FAB.Content = "src: " + xe.Attribute(XName.Get(p1nspace + "src")).Value;
                //FAB.Content = "src: " + CurrentApp.metadata.resStrings[xe.Attribute(XName.Get(p1nspace + "src")).Value][0];
                
                FAB.Margin = new Thickness(10);
                //return null;
                return FAB;
            }
            // *** experimental - end ***
            else
            {
                throw new NotImplementedException($"UIElement {xe.Name.ToString()} is not currently implemented on this renderer.");
                //return null;
            }

        }//RenderObject end



        // DPtoEP(int i)
        public static double DPtoEP(int i)
        {
            //TODO: scale android dp sizes to windows dp sizes

            //Android dp size = dp = (width in pixels * 160) / screen density in dpi; 1dp = 1px on a 160dpi screen; px = dp * (dpi / 160)
            //Android DP is equivelent to the current screen size at 160dpi
            //Windows Effictive Pixels (ep) = 146.86 dpi (phone) ~150 (Tablet) ~110 (Desktop)
            //Let's make it 148 dpi for simplicity sake.

            //ep = (dp/160) * 148
            return (i / 160) * 148;

        }//DPtoEP end
    
    }//Renderer class end

}// namespace end
