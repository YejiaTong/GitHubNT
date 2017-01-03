using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace XamarinCPTTechIM
{
    public class FrontPage : NonNavigationBarPage
    {
        WebView mBrowser;
        Frame mFrame;
        StackLayout mLayout;

        public FrontPage() : base()
        {
            mBrowser = new WebView();
            mBrowser.Source = @"https://im.ttechcode.com";

            mFrame = new Frame();
            mFrame.Padding = new Thickness(10, 10, 10, 10);
            mFrame.OutlineColor = Color.Accent;
            mFrame.HorizontalOptions = LayoutOptions.FillAndExpand;
            mFrame.VerticalOptions = LayoutOptions.FillAndExpand;
            mFrame.Content = mBrowser;

            mLayout = new StackLayout();
            mLayout.Children.Add(mFrame);

            this.Content = mLayout;
            this.BackgroundImage = @"drawable/background.png";
        }
    }
}
