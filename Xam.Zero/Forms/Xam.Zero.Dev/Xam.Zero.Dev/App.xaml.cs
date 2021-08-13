using System;
using System.Runtime.CompilerServices;
using DryIoc;
using TinyIoC;
using Xam.Zero.Dev.Features.Home;
using Xam.Zero.DryIoc;
using Xam.Zero.TinyIoc;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]

namespace Xam.Zero.Dev
{
    public partial class App : Application
    {
        public static readonly TinyIoCContainer Container = new TinyIoCContainer();


        public App()
        {
            this.InitializeComponent();

            ZeroApp.On(this)
                .WithContainer(TinyIocZeroContainer.Build(Container))
                .RegisterShell(() => new AppShell())
                .RegisterShell(() => new TabbedShell())
                // .StartWithPage<HomePage>(true);
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