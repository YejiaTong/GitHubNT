using System.Threading;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Util;
using Android.Views;
using Android.Webkit;

namespace AndroidTM
{
    [Activity(Label = "TY Tech IM", Theme = "@style/TMTheme.Regular")]
    public class MainActivity : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "ActionBarLayout" layout resource
            SetContentView(Resource.Layout.Main);

            WebView localWebView = FindViewById<WebView>(Resource.Id.localWebView);
            localWebView.Settings.JavaScriptEnabled = true;
            localWebView.Settings.SetAppCacheEnabled(true);
            localWebView.Settings.DomStorageEnabled = true;
            localWebView.Settings.SetGeolocationEnabled(true);
            localWebView.SetWebViewClient(new WebViewClient());
            localWebView.LoadUrl("https://im.ttechcode.com");
        }
    }
}

