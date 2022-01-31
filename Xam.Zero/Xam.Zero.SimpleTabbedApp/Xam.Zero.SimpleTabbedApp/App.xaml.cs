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

       public static bool UseRgPluginPopups = true;

        public App()
        {
            this.InitializeComponent();
            
            var zeroApp = ZeroApp
                .On(this)
                .WithContainer(DryIoc.DryIocZeroContainer.Build(MyContainer))
                .WithTransientPages()
                .WithTransientViewModels()
                .RegisterShell(() => new SimpleShell())
                .RegisterShell(() => new TabbedShell());

            if (App.UseRgPluginPopups)
                zeroApp.WithPopupNavigator(RGPopup.RGPopupNavigator.Build());
            else
                zeroApp.WithPopupNavigator(ToolkitPopup.ToolkitPopupNavigator.Build());

            zeroApp.StartWith<SimpleShell>();

            Kernel.Bind<IDummyService>().To<DummyService>().InSingletonScope();
            MyContainer.Register<IDummyService, DummyService>();
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