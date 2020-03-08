using System.ComponentModel;
using System.Threading.Tasks;
using DryIoc;
using Container = DryIoc.Container;
using NUnit.Framework;
using Xam.Zero.DryIoc;
using Xam.Zero.MarkupExtensions;
using Xam.Zero.Services;
using Xam.Zero.Tests.MockedResources.Pages;
using Xam.Zero.Tests.MockedResources.Shells;
using Xam.Zero.Tests.MockedResources.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml.Internals;


namespace Xam.Zero.Tests
{
    [TestFixture]
    public class ZeroBaseModelTestFixtureDryIocShould
    {
        private Container _container;
        private Application _app;

        [Test]
        public void Implements_INotifyPropertyChanged()
        {
            var baseModel = new MockedZeroBaseModel();
            Assert.IsInstanceOf<INotifyPropertyChanged>(baseModel);
        }

        [TestCase("test string")]
        public void Notify_Properties(string testString)
        {
            var baseModel = new MockedZeroBaseModel();

            var notifiedPropertyName = string.Empty;
            var notifiedPropertyValue = string.Empty;
            
            baseModel.PropertyChanged += (sender, args) =>
            {
                notifiedPropertyName = args.PropertyName;
                notifiedPropertyValue = baseModel.StringProperty;
            };

            baseModel.StringProperty = testString;

            Assert.IsNotEmpty(notifiedPropertyName);
            Assert.IsNotEmpty(notifiedPropertyValue);
            Assert.AreEqual(notifiedPropertyName, "StringProperty");
            Assert.AreEqual(testString, notifiedPropertyValue);
            Assert.AreEqual(testString, baseModel.StringProperty);
        }

        [Test]
        public void Register_On_Startup_ViewModels_ShellService_And_MessagingCenter()
        {
            var firstViewModel = this._container.Resolve<FirstViewModel>();
            var secondViewModel = this._container.Resolve<SecondViewModel>();

            var shellService = this._container.Resolve<IShellService>();
            var messagingCenter = this._container.Resolve<IMessagingCenter>();
            
            Assert.NotNull(firstViewModel);
            Assert.NotNull(secondViewModel);
            Assert.NotNull(shellService);
            Assert.NotNull(messagingCenter);

        }

        [TestCase("PushedStringValue")]
        public async Task Push_And_Prepare_Model(string stringValue)
        {

            var firstViewModel = this._container.Resolve<FirstViewModel>();
            var secondViewModel = this._container.Resolve<SecondViewModel>();

            var secondViewModelPageBeforePush = secondViewModel.CurrentPage;
            var secondViewModelPreviousModelBeforePush = secondViewModel.PreviousModel;
            
            Assert.IsNull(secondViewModelPageBeforePush);
            Assert.IsNull(secondViewModelPreviousModelBeforePush);
            Assert.IsNull(secondViewModel.SecondStringProperty);
            
            await firstViewModel.Push<SecondPage>(stringValue);

            var secondViewModelPageAfterPush = secondViewModel.CurrentPage;
            var secondViewModelPreviousModelAfterPush = secondViewModel.PreviousModel;

            Assert.IsNotNull(secondViewModelPageAfterPush);
            Assert.IsNotNull(secondViewModelPreviousModelAfterPush);
            Assert.AreEqual(secondViewModelPageAfterPush.GetType(), typeof(SecondPage));
            Assert.AreEqual(secondViewModelPreviousModelAfterPush.GetType(), typeof(FirstViewModel));
            Assert.AreEqual(firstViewModel, secondViewModel.PreviousModel);
            Assert.IsNotNull(secondViewModel.SecondStringProperty);
            Assert.AreEqual(stringValue, secondViewModel.SecondStringProperty);
        }

        [TestCase("ReceivedStringValue")]
        public async Task Pop_And_ReversePrepare_Model(string stringValue)
        {
            var firstViewModel = this._container.Resolve<FirstViewModel>();
            var secondViewModel = this._container.Resolve<SecondViewModel>();
            
            Assert.AreEqual(firstViewModel.CurrentPage.GetType(), typeof(FirstPage));
            Assert.IsNull(secondViewModel.CurrentPage);

            Assert.AreEqual(firstViewModel.CurrentPage.Navigation.NavigationStack.Count, 1);

            await firstViewModel.Push<SecondPage>();

            Assert.AreEqual(firstViewModel.CurrentPage.Navigation.NavigationStack.Count, 2);

            Assert.AreEqual(firstViewModel.CurrentPage.GetType(), typeof(FirstPage));
            Assert.AreEqual(secondViewModel.CurrentPage.GetType(), typeof(SecondPage));

            await secondViewModel.Pop(stringValue);

            Assert.AreEqual(firstViewModel.CurrentPage.Navigation.NavigationStack.Count, 1);

            Assert.AreEqual(firstViewModel.CurrentPage.GetType(), typeof(FirstPage));
            Assert.AreEqual(secondViewModel.CurrentPage.GetType(), typeof(SecondPage));

            Assert.NotNull(firstViewModel.FirstStringProperty);
            Assert.AreEqual(firstViewModel.FirstStringProperty, stringValue);
            

        }

        [Test]
        public void GuaranteeThat_ViewModel_Markup_Returns_ActualViewModel()
        {
            var markup = new ViewModelMarkup()
            {
                ViewModel = typeof(FirstViewModel)
            };

            var provider = new XamlServiceProvider();

            var vmValue = (FirstViewModel) markup.ProvideValue(provider);
            Assert.AreEqual(typeof(FirstViewModel), vmValue.GetType());
            Assert.AreEqual(vmValue, this._container.Resolve<FirstViewModel>());
        }
        
        [Test]
        public void GuaranteeThat_ShellPagedViewModel_Markup_Returns_ActualViewModel()
        {
            var firstPage = this._container.Resolve<FirstPage>();
            
            var markup = new ShellPagedViewModelMarkup()
            {
                ViewModel = typeof(FirstViewModel),
                Page = firstPage
            };

            var provider = new XamlServiceProvider();

            var vmValue = (FirstViewModel) markup.ProvideValue(provider);
            Assert.AreEqual(typeof(FirstViewModel), vmValue.GetType());
            Assert.AreEqual(vmValue, this._container.Resolve<FirstViewModel>());
        }
        
        [SetUp]
        public void SetUp()
        {
            Xamarin.Forms.Mocks.MockForms.Init();
            
            this._container = new Container();
            this._app = new Application();

            ZeroApp
                .On(this._app)
                .WithContainer(DryIocZeroContainer.Build(this._container))
                .RegisterShell(() => new FirstShell())
                .Start();
        }

    }
}