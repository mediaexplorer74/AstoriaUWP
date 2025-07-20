// Reassembly - UI - Renderer

using AndroidInteropLib.android.content;
using AndroidInteropLib.android.support.design.widget;
using AndroidInteropLib.ticomware.interop;
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
                Grid container = new Grid();
                container.VerticalAlignment = VerticalAlignment.Top;
                container.HorizontalAlignment = HorizontalAlignment.Stretch;
                if (xe.Attribute(p1nspace + "layout_width") != null)
                    container.Width = double.TryParse(xe.Attribute(p1nspace + "layout_width").Value, out var w) ? w : container.Width;
                if (xe.Attribute(p1nspace + "layout_height") != null)
                    container.Height = double.TryParse(xe.Attribute(p1nspace + "layout_height").Value, out var h) ? h : container.Height;
                if (xe.Attribute(p1nspace + "background") != null)
                    container.Background = new SolidColorBrush(ColorUtil.FromString(xe.Attribute(p1nspace + "background").Value));
                if (nestedObjs)
                {
                    foreach (XElement xe1 in xe.Elements())
                    {
                        container.Children.Add(await RenderObject(xe1));
                    }
                }
                return container;
            }
            else if (xeName.Equals("android.support.design.widget.CoordinatorLayout"))
            {
                CoordinatorLayout cl = new CoordinatorLayout();
                if (xe.Attribute(p1nspace + "layout_width") != null)
                    cl.Width = double.TryParse(xe.Attribute(p1nspace + "layout_width").Value, out var w) ? w : cl.Width;
                if (xe.Attribute(p1nspace + "layout_height") != null)
                    cl.Height = double.TryParse(xe.Attribute(p1nspace + "layout_height").Value, out var h) ? h : cl.Height;
                if (xe.Attribute(p1nspace + "background") != null)
                    cl.Background = new SolidColorBrush(ColorUtil.FromString(xe.Attribute(p1nspace + "background").Value));
                if (nestedObjs)
                {
                    foreach (XElement xe1 in xe.Elements())
                    {
                        cl.Add(await RenderObject(xe1));
                    }
                }
                return cl;
            }
            else if (xeName.Equals("android.support.design.widget.FloatingActionButton"))
            {
                Button fab = new Button();
                fab.Width = 56; fab.Height = 56;
                fab.HorizontalAlignment = HorizontalAlignment.Right;
                fab.VerticalAlignment = VerticalAlignment.Bottom;
                fab.Margin = new Thickness(16);
                fab.Content = "+";
                fab.Background = new SolidColorBrush(Windows.UI.Colors.DeepSkyBlue);
                fab.Foreground = new SolidColorBrush(Windows.UI.Colors.White);
                fab.CornerRadius = new Windows.UI.Xaml.CornerRadius(28);
                return fab;
            }
            else if (xeName.Equals("android.support.v7.widget.Toolbar"))
            {
                AndroidToolbar at = new AndroidToolbar();
                if (xe.Attribute(p1nspace + "title") != null)
                    at.SetTitle(xe.Attribute(p1nspace + "title").Value);
                else
                    at.SetTitle(CurrentApp.metadata.label);
                if (xe.Attribute(p1nspace + "background") != null)
                    at.Background = new SolidColorBrush(ColorUtil.FromString(xe.Attribute(p1nspace + "background").Value));
                return at;
            }
            else if (xeName.Equals("include"))
            {
                try {
                    string relUri = xe.Attribute("layout").Value;
                    string path = CurrentApp.resFolder.Path + relUri.Replace('@', '\\').Replace('/', '\\') + ".xml";
                    StorageFile sf = await StorageFile.GetFileFromPathAsync(path);
                    return await RenderXmlFile(sf);
                } catch {
                    Debug.WriteLine("[Renderer] Failed to include layout: " + xe);
                    return null;
                }
            }
            else if (xeName.Equals("RelativeLayout"))
            {
                Grid container = new Grid();
                if (xe.Attribute(p1nspace + "layout_width") != null)
                    container.Width = double.TryParse(xe.Attribute(p1nspace + "layout_width").Value, out var w) ? w : container.Width;
                if (xe.Attribute(p1nspace + "layout_height") != null)
                    container.Height = double.TryParse(xe.Attribute(p1nspace + "layout_height").Value, out var h) ? h : container.Height;
                if (xe.Attribute(p1nspace + "background") != null)
                    container.Background = new SolidColorBrush(ColorUtil.FromString(xe.Attribute(p1nspace + "background").Value));
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
                if (xe.Attribute(p1nspace + "text") != null)
                    tv.Text = xe.Attribute(p1nspace + "text").Value;
                tv.Margin = new Thickness(14.8, 7.4, 14.8, 7.4);
                if (xe.Attribute(p1nspace + "layout_width") != null && double.TryParse(xe.Attribute(p1nspace + "layout_width").Value, out var w))
                    tv.Width = w;
                if (xe.Attribute(p1nspace + "layout_height") != null && double.TryParse(xe.Attribute(p1nspace + "layout_height").Value, out var h))
                    tv.Height = h;
                if (xe.Attribute(p1nspace + "textColor") != null)
                    tv.Foreground = new SolidColorBrush(ColorUtil.FromString(xe.Attribute(p1nspace + "textColor").Value));
                if (xe.Attribute(p1nspace + "textSize") != null && double.TryParse(xe.Attribute(p1nspace + "textSize").Value, out var sz))
                    tv.FontSize = sz;
                if (xe.Attribute(p1nspace + "gravity") != null)
                {
                    string gravity = xe.Attribute(p1nspace + "gravity").Value.ToLower();
                    if (gravity.Contains("center")) tv.TextAlignment = TextAlignment.Center;
                    else if (gravity.Contains("right")) tv.TextAlignment = TextAlignment.Right;
                    else tv.TextAlignment = TextAlignment.Left;
                }
                return tv;
            }
            else if (xeName.Equals("FrameLayout"))
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
            // *** experimental - begin ***
            else if (xeName.Equals("ShapeView") 
                || xeName.Equals(/*"android.ticomware.interop.ShapeView"*/"com.example.shapeviewdemo.ShapeView"))
            {
                Debug.WriteLine($"[Renderer] Creating ShapeView for element: {xe}");
                ShapeView shapeView = new ShapeView(new AstoriaContext(), new AstoriaAttrSet(xe));
                try
                {
                    string primitive = xe.Attribute("primitive")?.Value?.ToLower() ?? "rectangle";
                    Windows.UI.Color color = Windows.UI.Colors.White;
                    if (xe.Attribute("color") != null)
                    {
                        color = ColorUtil.FromString(xe.Attribute("color").Value);
                    }
                    switch (primitive)
                    {
                        case "rectangle":
                        {
                            double width = xe.Attribute("width") != null ? double.Parse(xe.Attribute("width").Value) : 100;
                            double height = xe.Attribute("height") != null ? double.Parse(xe.Attribute("height").Value) : 100;
                            shapeView.DrawRectangle(width, height, color);
                            break;
                        }
                        case "ellipse":
                        {
                            double width = xe.Attribute("width") != null ? double.Parse(xe.Attribute("width").Value) : 100;
                            double height = xe.Attribute("height") != null ? double.Parse(xe.Attribute("height").Value) : 100;
                            shapeView.DrawEllipse(width, height, color);
                            break;
                        }
                        case "line":
                        {
                            double x1 = xe.Attribute("x1") != null ? double.Parse(xe.Attribute("x1").Value) : 0;
                            double y1 = xe.Attribute("y1") != null ? double.Parse(xe.Attribute("y1").Value) : 0;
                            double x2 = xe.Attribute("x2") != null ? double.Parse(xe.Attribute("x2").Value) : 100;
                            double y2 = xe.Attribute("y2") != null ? double.Parse(xe.Attribute("y2").Value) : 100;
                            double thickness = xe.Attribute("thickness") != null ? double.Parse(xe.Attribute("thickness").Value) : 2;
                            shapeView.DrawLine(x1, y1, x2, y2, thickness, color);
                            break;
                        }
                        case "point":
                        {
                            double x = xe.Attribute("x") != null ? double.Parse(xe.Attribute("x").Value) : 50;
                            double y = xe.Attribute("y") != null ? double.Parse(xe.Attribute("y").Value) : 50;
                            double diameter = xe.Attribute("diameter") != null ? double.Parse(xe.Attribute("diameter").Value) : 8;
                            shapeView.DrawPoint(x, y, diameter, color);
                            break;
                        }
                        default:
                            Debug.WriteLine($"[Renderer] Unknown primitive type: {primitive}, defaulting to rectangle.");
                            shapeView.DrawRectangle(100, 100, color);
                            break;
                    }
                    if (xe.Attribute("background") != null)
                    {
                        Windows.UI.Color bgColor = ColorUtil.FromString(xe.Attribute("background").Value);
                        shapeView.SetBackgroundColor(bgColor);
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"[Renderer] Error rendering ShapeView primitive: {ex.Message}");
                }
                return shapeView;
            }
            // *** experimental - begin ***
            else
            {
                Debug.WriteLine($"[Renderer] UIElement {xe.Name.ToString()} is not currently implemented on this renderer.");
                // Return a placeholder UI element to avoid crash
                Border placeholder = new Border
                {
                    Background = new SolidColorBrush(Colors.Red),
                    Child = new TextBlock { Text = $"Not implemented: {xe.Name}", Foreground = new SolidColorBrush(Colors.White) },
                    Margin = new Thickness(4),
                    CornerRadius = new CornerRadius(4)
                };
                return placeholder;
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
