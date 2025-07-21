using AndroidInteropLib.android.view;
using Windows.UI.Xaml.Controls;

namespace AndroidInteropLib.android.widget
{
    public class ScrollView : ViewGroup
    {
        private ScrollViewer scrollViewer;
        private StackPanel contentPanel;

        public ScrollView() : base(null, null)
        {
            scrollViewer = new ScrollViewer();
            contentPanel = new StackPanel();
            scrollViewer.Content = contentPanel;
            this.WinUI = scrollViewer;
        }

        public void addView(object child)
        {
            if (child is View view)
            {
                contentPanel.Children.Add(view.WinUI);
            }
        }

        public override void addView(View view, LayoutParams param)
        {
            contentPanel.Children.Add(view.WinUI);
        }

        public override void addView(View view)
        {
            contentPanel.Children.Add(view.WinUI);
        }

        public override void CreateWinUI(params object[] obj)
        {
            throw new System.NotImplementedException();
        }

        //public override void removeView(object child)
        //{
        //    if (child is View view)
        //    {
        //        contentPanel.Children.Remove(view.WinUI);
        //    }
        //}

        public override void removeView(View view)
        {
            contentPanel.Children.Remove(view.WinUI);
        }
    }
}