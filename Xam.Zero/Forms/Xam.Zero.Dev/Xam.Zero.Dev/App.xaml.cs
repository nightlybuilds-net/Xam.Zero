using System;
using DryIoc;
using Xam.Zero.DryIoc;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]

namespace Xam.Zero.Dev
{
    public partial class App : Application
    {
        public static readonly Container Container = new Container();
        
        public App()
        {
            this.InitializeComponent();

            ZeroApp.InitApp(new DryIocZeroContainer(Container));
            this.MainPage = new AppShell();
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