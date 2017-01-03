using System;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;
using System.Threading.Tasks;

namespace XamarinCPTTechIM
{
    public class App : Application
    {
        public App()
        {
            // The root page of your application

            WelcomePage();

            Device.StartTimer(TimeSpan.FromMinutes(0.05), () => { MainApp(); return false; });
        }

        protected void WelcomePage()
        {
            var content = new WelcomePage();

            MainPage = content;
        }

        protected void MainApp()
        {
            var content = new MainDetailPage();

            MainPage = content;
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
