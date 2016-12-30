using System.Threading;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Util;
using Android.Views;

namespace AndroidTM
{
    [Activity(Label = "TY Tech IM")]
    public class MainActivity : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            ActionBar.NavigationMode = ActionBarNavigationMode.Tabs;

            ActionBar.Tab tab = ActionBar.NewTab();
            tab.SetText(Resources.GetString(Resource.String.MainTabHome));
            tab.SetIcon(Resource.Drawable.home);
            tab.TabSelected += (sender, args) => {
                // Do something when tab is selected
            };
            ActionBar.AddTab(tab);

            tab = ActionBar.NewTab();
            tab.SetText(Resources.GetString(Resource.String.MainTabAddExpense));
            tab.SetIcon(Resource.Drawable.plus_black_symbol);
            tab.TabSelected += (sender, args) =>
            {
                // Do something when tab is selected
            };
            ActionBar.AddTab(tab);

            tab = ActionBar.NewTab();
            tab.SetText(Resources.GetString(Resource.String.MainTabAbout));
            tab.SetIcon(Resource.Drawable.question_sign);
            tab.TabSelected += (sender, args) =>
            {
                // Do something when tab is selected
            };
            ActionBar.AddTab(tab);

            tab = ActionBar.NewTab();
            tab.SetText(Resources.GetString(Resource.String.MainTabLeaveMsg));
            tab.SetIcon(Resource.Drawable.font_selection_editor);
            tab.TabSelected += (sender, args) =>
            {
                // Do something when tab is selected
            };
            ActionBar.AddTab(tab);

            // Set our view from the "ActionBarLayout" layout resource
            SetContentView(Resource.Layout.Main);

            /*Comment Out
            FragmentTransaction transaction = FragmentManager.BeginTransaction();
            SlidingTabsFragment fragment = new SlidingTabsFragment();
            transaction.Replace(Resource.Id.fragmentContainer, fragment);
            transaction.Commit();
            */
        }

        /*Comment Out
        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Id.main_tab_home, menu);
            return base.OnCreateOptionsMenu(menu);
        }*/
        }
    }

