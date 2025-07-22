using AndroidInteropLib.android.content;
using AndroidInteropLib.android.view;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DalvikUWPCSharp.Reassembly
{
    public class AstoriaWindow : Window
    {
        private EmuPage emuPage;


        public AstoriaWindow(Context c, EmuPage e) : base(c)
        {
            emuPage = e;

            SetDefaultColors();
            
            //setStatusBarColor((int)(c.getR().color.get("colorPrimaryDark") ?? -1));
            //setNavigationBarColor((int)(c.getR().color.get("navigationBarColor") ?? -1));
        }

        private void SetDefaultColors()
        {
            //hack for now. will use getResources in future.
            int statusBarRef = (int)(mContext.getR().color.get("colorPrimaryDark") ?? -1);
            if(statusBarRef != -1)
            {
                List<string> res = ((AstoriaContext)mContext).runningApp.metadata.resStrings["@" + statusBarRef.ToString("X")];
                setStatusBarColor(int.Parse(res[0]));
            }

            int windowBackRef = (int)(mContext.getR().color.get("windowBackground") ?? -1);
            if (windowBackRef != -1)
            {
                List<string> res = ((AstoriaContext)mContext).runningApp.metadata.resStrings["@" + statusBarRef.ToString("X")];
                int color = (int.Parse(res[0]));

                Windows.UI.Color winColor = AndroidInteropLib.ticomware.interop.Util.IntToColor(color);
                emuPage.SetWinBackColor(winColor);
            }

        }

        public override View getDecorView()
        {
            System.Diagnostics.Debug.WriteLine("[AstoriaWindow] getDecorView not implemented");
            return null;
        }

        public override int getNavigationBarColor()
        {
            System.Diagnostics.Debug.WriteLine("[AstoriaWindow] getNavigationBarColor not implemented");
            return -1;
        }

        public override int getStatusBarColor()
        {
            System.Diagnostics.Debug.WriteLine("[AstoriaWindow] getStatusBarColor not implemented");
            return -1;
        }

        public override bool isFloating()
        {
            System.Diagnostics.Debug.WriteLine("[AstoriaWindow] isFloating not implemented");
            return false;
        }

        public override void setContentView(View view)
        {
            emuPage.SetContentView(view);
        }

        // set ContentView (layout resourse ID)
        public override void setContentView(int layoutResID)
        {
            LayoutInflater li = (LayoutInflater)getContext().getSystemService(Context.LAYOUT_INFLATER_SERVICE);
            setContentView(li.inflate(layoutResID, null));
        }

        public override void setContentView(View view, ViewGroup.LayoutParams _params)
        {
            System.Diagnostics.Debug.WriteLine("[AstoriaWindow] setContentView(view, params) not implemented");
            SetContentView(view);
        }

        private void SetContentView(View view)
        {
            emuPage.SetContentView(view);
        }

        public override void setNavigationBarColor(int color)
        {
            if (color != -1)
            {
                Windows.UI.Color winColor = AndroidInteropLib.ticomware.interop.Util.IntToColor(color);
                emuPage.SetNavBarColor(winColor);
            }
        }

        public override void setStatusBarColor(int color)
        {
            if (color != -1)
            {
                Windows.UI.Color winColor = AndroidInteropLib.ticomware.interop.Util.IntToColor(color);
                emuPage.SetTitleBarColor(winColor);
            }
        }
    }
}
