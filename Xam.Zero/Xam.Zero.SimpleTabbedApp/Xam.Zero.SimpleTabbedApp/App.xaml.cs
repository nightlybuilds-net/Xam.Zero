using DryIoc;
using Xam.Zero.DryIoc;
using Xam.Zero.SimpleTabbedApp.Shells;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]

namespace Xam.Zero.SimpleTabbedApp
{
    public partial class App : Application
    {
        public static readonly Container Container = new Container();

        public App()
        {
            InitializeComponent();
            
            ZeroApp
                .On(this)
                .WithContainer(DryIocZeroContainer.Build(Container))
                .RegisterShell(() => new SimpleShell())
                .RegisterShell(() => new TabbedShell())
                .StartWith<SimpleShell>();
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