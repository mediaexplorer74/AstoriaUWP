using AndroidInteropLib.android.content;
using AndroidInteropLib.android.util;
using Windows.UI.Xaml.Controls;

namespace AndroidInteropLib.android.widget
{
    public class Button : TextView
    {
        private Windows.UI.Xaml.Controls.Button button;

        public Button()
             : base(null, null)
        {
            button = new Windows.UI.Xaml.Controls.Button();
            this.WinUI = button;
        }

        public Button(Context context, AttributeSet attrs)
        : base(context, attrs)
        {
            button = new Windows.UI.Xaml.Controls.Button();
            this.WinUI = button;
        }

        public override void setText(string text)
        {
            button.Content = text;
        }

        // Additional Android Button APIs can be mapped here
    }
}