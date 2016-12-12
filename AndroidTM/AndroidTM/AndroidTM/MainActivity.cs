using System.Threading;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Util;

namespace AndroidTM
{
    [Activity(Label = "TY Tech IM", MainLauncher = true)] //Theme = "@style/TMTheme.Splash"
    public class MainActivity : Activity
    {
        static readonly string TAG = "X:";

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            ActionBar.NavigationMode = ActionBarNavigationMode.Tabs;

            ActionBar.Tab tab = ActionBar.NewTab();
            tab.SetText(Resources.GetString(Resource.String.MainTabHome));
            //tab.SetIcon(Resource.Drawable.tab1_icon);
            tab.TabSelected += (sender, args) => {
                // Do something when tab is selected
            };
            ActionBar.AddTab(tab);

            tab = ActionBar.NewTab();
            tab.SetText(Resources.GetString(Resource.String.MainTabAddExpense));
            //tab.SetIcon(Resource.Drawable.tab2_icon);
            tab.TabSelected += (sender, args) =>
            {
                // Do something when tab is selected
            };
            ActionBar.AddTab(tab);

            Log.Debug(TAG, "SplashActivity.OnCreate");

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);
        }
    }
}

