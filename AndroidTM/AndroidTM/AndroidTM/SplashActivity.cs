using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Util;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace AndroidTM
{
    [Activity(Theme = "@style/TMTheme.Splash", MainLauncher = true, NoHistory = true)]
    public class SplashActivity : Activity
    {
        static readonly string TAG = "X:" + typeof(SplashActivity).Name;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        protected override void OnResume()
        {
            base.OnResume();

            Task startupWork = new Task(() => {
                Task.Delay(3000);  // Simulate a bit of startup work.
            });

            startupWork.ContinueWith(t => {
                StartActivity(new Intent(Application.Context, typeof(MainActivity)));
            }, TaskScheduler.FromCurrentSynchronizationContext());

            startupWork.Start();
        }
    }
}