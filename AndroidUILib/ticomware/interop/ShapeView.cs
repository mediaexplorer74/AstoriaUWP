using AndroidInteropLib.android.content;
using AndroidInteropLib.android.util;
using AndroidInteropLib.android.view;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

namespace AndroidInteropLib.ticomware.interop
{
    public class ShapeView : View
    {
        private Rectangle rectangle = new Rectangle();
        private Grid container = new Grid();
        private SolidColorBrush backgroundBrush = new SolidColorBrush(Windows.UI.Colors.Black);
        private SolidColorBrush shapeBrush = new SolidColorBrush(Windows.UI.Colors.White);
        private Ellipse ellipse = new Ellipse();
        private Line line = new Line();
        private Ellipse point = new Ellipse(); // Use a small ellipse for point rendering

        public ShapeView(Context c, AttributeSet a) : base(c, a)
        {
        }

        public override void CreateWinUI(params object[] obj)
        {
            System.Diagnostics.Debug.WriteLine("[ShapeView] CreateWinUI called");
            // Set up the container with a black background
            //container.Background = backgroundBrush; // COMMENTED OUT: Remove hardcoded black background
            // Configure the rectangle
            //rectangle.Fill = shapeBrush; // COMMENTED OUT: Remove hardcoded white rectangle
            // Default size for the rectangle (can be adjusted through properties)
            //rectangle.Width = 100;
            //rectangle.Height = 100;
            // Center the rectangle in the container
            //rectangle.HorizontalAlignment = HorizontalAlignment.Center;
            //rectangle.VerticalAlignment = VerticalAlignment.Center;
            // Add the rectangle to the container
            //container.Children.Add(rectangle);
            // Set the container as the content of the WinUI
            WinUI.Content = container;
            System.Diagnostics.Debug.WriteLine($"[ShapeView] Rectangle size: {rectangle.Width}x{rectangle.Height}, Background: {backgroundBrush.Color}, Shape: {shapeBrush.Color}");
        }

        // Method to set the size of the rectangle
        public void SetRectangleSize(double width, double height)
        {
            rectangle.Width = width;
            rectangle.Height = height;
            System.Diagnostics.Debug.WriteLine($"[ShapeView] SetRectangleSize: {width}x{height}");
        }

        // Method to set the color of the rectangle
        public void SetRectangleColor(Windows.UI.Color color)
        {
            shapeBrush = new SolidColorBrush(color);
            rectangle.Fill = shapeBrush;
            System.Diagnostics.Debug.WriteLine($"[ShapeView] SetRectangleColor: {color}");
        }

        // Method to set the background color
        public void SetBackgroundColor(Windows.UI.Color color)
        {
            backgroundBrush = new SolidColorBrush(color);
            container.Background = backgroundBrush;
            System.Diagnostics.Debug.WriteLine($"[ShapeView] SetBackgroundColor: {color}");
        }

        public void DrawRectangle(double width, double height, Windows.UI.Color color)
        {
            rectangle.Width = width;
            rectangle.Height = height;
            rectangle.Fill = new SolidColorBrush(color);
            rectangle.HorizontalAlignment = HorizontalAlignment.Center;
            rectangle.VerticalAlignment = VerticalAlignment.Center;
            if (!container.Children.Contains(rectangle))
                container.Children.Add(rectangle);
            System.Diagnostics.Debug.WriteLine($"[ShapeView] DrawRectangle: {width}x{height}, Color: {color}");
        }

        public void DrawEllipse(double width, double height, Windows.UI.Color color)
        {
            ellipse.Width = width;
            ellipse.Height = height;
            ellipse.Fill = new SolidColorBrush(color);
            ellipse.HorizontalAlignment = HorizontalAlignment.Center;
            ellipse.VerticalAlignment = VerticalAlignment.Center;
            if (!container.Children.Contains(ellipse))
                container.Children.Add(ellipse);
            System.Diagnostics.Debug.WriteLine($"[ShapeView] DrawEllipse: {width}x{height}, Color: {color}");
        }

        public void DrawLine(double x1, double y1, double x2, double y2, double thickness, Windows.UI.Color color)
        {
            line.X1 = x1;
            line.Y1 = y1;
            line.X2 = x2;
            line.Y2 = y2;
            line.StrokeThickness = thickness;
            line.Stroke = new SolidColorBrush(color);
            if (!container.Children.Contains(line))
                container.Children.Add(line);
            System.Diagnostics.Debug.WriteLine($"[ShapeView] DrawLine: ({x1},{y1})-({x2},{y2}), Thickness: {thickness}, Color: {color}");
        }

        public void DrawPoint(double x, double y, double diameter, Windows.UI.Color color)
        {
            point.Width = diameter;
            point.Height = diameter;
            point.Fill = new SolidColorBrush(color);
            point.HorizontalAlignment = HorizontalAlignment.Left;
            point.VerticalAlignment = VerticalAlignment.Top;
            point.Margin = new Thickness(x - diameter/2, y - diameter/2, 0, 0);
            if (!container.Children.Contains(point))
                container.Children.Add(point);
            System.Diagnostics.Debug.WriteLine($"[ShapeView] DrawPoint: ({x},{y}), Diameter: {diameter}, Color: {color}");
        }
    }
}