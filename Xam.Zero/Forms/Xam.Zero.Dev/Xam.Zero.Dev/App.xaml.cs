using System;
using System.Runtime.CompilerServices;
using DryIoc;
using Xam.Zero.Dev.Features.Home;
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

            ZeroApp.On(this)
                .WithContainer(DryIocZeroContainer.Build(Container))
                .RegisterShell(() => new AppShell())
                .RegisterShell(() => new TabbedShell())
                .StartWith<AppShell>();
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