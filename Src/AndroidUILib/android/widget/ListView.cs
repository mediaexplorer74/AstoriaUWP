using AndroidInteropLib.android.view;
using Windows.UI.Xaml.Controls;

namespace AndroidInteropLib.android.widget
{
    public class ListView : ViewGroup
    {
        private Windows.UI.Xaml.Controls.ListView listView;

        public ListView() : base(null, null)
        {
            listView = new Windows.UI.Xaml.Controls.ListView();
            //this.WinUI = (ContentControl)listView;
        }

        public void addView(object child)
        {
            if (child is View view)
            {
                listView.Items.Add(view.WinUI);
            }
        }

        public override void addView(View view, LayoutParams param)
        {
            listView.Items.Add(view.WinUI);
        }

        public override void addView(View view)
        {
            listView.Items.Add(view.WinUI);
        }

        public override void CreateWinUI(params object[] obj)
        {
            throw new System.NotImplementedException();
        }

        //public override void removeView(object child)
        //{
        //    if (child is View view)
        //    {
        //        listView.Items.Remove(view.WinUI);
        //    }
        //}

        public override void removeView(View view)
        {
            listView.Items.Remove(view.WinUI);
        }
    }
}