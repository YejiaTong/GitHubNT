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
        NavigationPage mNavigationPage;
        ContentPage mContentPage;
        Dictionary<string, string> mLinkUrlMappings;

        public MainDetailPage() : base()
        {
            mLinkUrlMappings = new Dictionary<string, string>();
            mLinkUrlMappings.Add("Home", @"https://im.ttechcode.com");
            mLinkUrlMappings.Add("Add Expenses", @"https://im.ttechcode.com/InvoiceManager/AddExpense");
            mLinkUrlMappings.Add("About", @"https://im.ttechcode.com/Account/About");
            mLinkUrlMappings.Add("Contact", @"https://im.ttechcode.com/Account/Contact");
            mLinkUrlMappings.Add("Feedback", @"https://im.ttechcode.com/Account/MessageBoard");

            mBrowser = new WebView();
            mBrowser.Source = mLinkUrlMappings["Home"];

            mContentPage = new ContentPage
            {
                Title = "Home",
                Content = mBrowser
            };

            mNavigationPage = new NavigationPage(mContentPage)
            {
                BarBackgroundColor = Color.FromRgb(248, 248, 248),
                BarTextColor = Color.FromRgb(119, 119, 119),
                //Icon = @"drawable/nav.png"
            };

            this.Master = new ContentPage
            {
                Title = "Menu",
                BackgroundColor = Color.FromRgb(238, 238, 238),
                //Icon = Device.OS == TargetPlatform.iOS ? @"drawable/nav.png" : @"drawable/nav.png",
                Content = new StackLayout
                {
                    Padding = new Thickness(5, 50),
                    Children = { Link("Home"), Link("Add Expenses"), Link("About"), Link("Contact"), Link("Feedback") }
                },
            };

            this.Detail = mNavigationPage;
        }

        private Button Link(string name)
        {
            var button = new Button
            {
                Text = name,
                BackgroundColor = Color.FromRgb(222, 153, 94),
                TextColor = Color.FromRgb(255, 255, 255)
            };

            button.Clicked += delegate {
                mContentPage.Title = name;
                mBrowser.Source = mLinkUrlMappings[name];
                /*this.Detail = new ContentPage
                {
                    Content = new Label
                    {
                        Text = name
                    }
                };*/

                this.IsPresented = false;
            };

            return button;
        }
    }
}
