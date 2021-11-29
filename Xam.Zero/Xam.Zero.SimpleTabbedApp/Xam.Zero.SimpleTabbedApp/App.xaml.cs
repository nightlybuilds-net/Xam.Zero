using DryIoc;
using Ninject;
using Xam.Zero.Ninject;
using Xam.Zero.SimpleTabbedApp.Services;
using Xam.Zero.SimpleTabbedApp.Services.Impl;
using Xam.Zero.SimpleTabbedApp.Shells;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]

namespace Xam.Zero.SimpleTabbedApp
{
    public partial class App : Application
    {
        public static readonly StandardKernel Kernel = new StandardKernel();
        private static readonly Container MyContainer = new Container(rules =>
        {
            rules = rules.WithDefaultIfAlreadyRegistered(IfAlreadyRegistered.Keep);
            return rules.WithoutFastExpressionCompiler();
        });


        public App()
        {
            this.InitializeComponent();
            
            ZeroApp
                .On(this)
                .WithContainer(DryIoc.DryIocZeroContainer.Build(MyContainer))
                .WithPopupNavigator(ToolkitPopup.ToolkitPopupNavigator.Build())
                .RegisterShell(() => new SimpleShell())
                .RegisterShell(() => new TabbedShell())
                .StartWith<SimpleShell>();

            Kernel.Bind<IDummyService>().To<DummyService>().InSingletonScope();

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