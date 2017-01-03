using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace XamarinCPTTechIM
{
    public class MainDetailPage : NonNavigationBarMasterDetailPage
    {
        WebView mBrowser;

        public MainDetailPage() : base()
        {
            mBrowser = new WebView();
            mBrowser.Source = @"https://im.ttechcode.com";

            this.Master = new ContentPage
            {
                Title = "Menu",
                BackgroundColor = Color.Silver,
                Icon = Device.OS == TargetPlatform.iOS ? "menu.png" : null,
                Content = new StackLayout
                {
                    Padding = new Thickness(5, 50),
                    Children = { Link("Login"), Link("Logout"), Link("Home") }
                },
            };
            this.Detail = new NavigationPage(new ContentPage
            {
                Title = "Home",
                Content = mBrowser
            });
        }

        private Button Link(string name)
        {
            var button = new Button
            {
                Text = name,
                BackgroundColor = Color.FromRgb(0.9, 0.9, 0.9)
            };
            button.Clicked += delegate {
                this.Detail = new NavigationPage(new ContentPage
                {
                    Title = name,
                    Content = new Label { Text = name }
                });
                this.IsPresented = false;
            };
            return button;
        }
    }
}
