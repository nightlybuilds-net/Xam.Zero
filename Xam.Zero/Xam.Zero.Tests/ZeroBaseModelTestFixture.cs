using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using DryIoc;
using Container = DryIoc.Container;
using NUnit.Framework;
using Xam.Zero.DryIoc;
using Xam.Zero.Services;
using Xam.Zero.Tests.MockedResources;
using Xam.Zero.Tests.MockedResources.Pages;
using Xam.Zero.Tests.MockedResources.Shells;
using Xam.Zero.Tests.MockedResources.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;


namespace Xam.Zero.Tests
{
    [TestFixture]
    public class ZeroBaseModelTestFixture
    {
        [Test]
        public void Test_ZeroBaseModel_Implements_INotifyPropertyChanged()
        {
            var baseModel = new MockedZeroBaseModel();
            Assert.IsInstanceOf<INotifyPropertyChanged>(baseModel);
        }

        [TestCase("test string")]
        public void Test_Properties_Are_Notifying(string testString)
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
        public void Test_ViewModels_ShellService_And_MessagingCenter_Are_Registered_On_Startup()
        {

            var container = new Container();
            var app = new Application();

            ZeroApp
                .On(app)
                .WithContainer(DryIocZeroContainer.Build(container))
                .RegisterShell(() => new FirstShell())
                .Start();

            var firstViewModel = container.Resolve<FirstViewModel>();
            var secondViewModel = container.Resolve<SecondViewModel>();

            var shellService = container.Resolve<IShellService>();
            var messagingCenter = container.Resolve<IMessagingCenter>();
            
            Assert.NotNull(firstViewModel);
            Assert.NotNull(secondViewModel);
            Assert.NotNull(shellService);
            Assert.NotNull(messagingCenter);

        }

        [TestCase("PushedStringValue")]
        public async Task Push_And_Prepare_Model(string stringValue)
        {
            var container = new Container();
            var app = new Application();

            ZeroApp
                .On(app)
                .WithContainer(DryIocZeroContainer.Build(container))
                .RegisterShell(() => new FirstShell())
                .Start();
            
            var firstViewModel = container.Resolve<FirstViewModel>();
            var secondViewModel = container.Resolve<SecondViewModel>();

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
            var container = new Container();
            var app = new Application();

            ZeroApp
                .On(app)
                .WithContainer(DryIocZeroContainer.Build(container))
                .RegisterShell(() => new FirstShell())
                .Start();
            
            var firstViewModel = container.Resolve<FirstViewModel>();
            var secondViewModel = container.Resolve<SecondViewModel>();
            
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
        public void ViewModel_Markup_Returns_ActualViewModel()
        {
            var firstPage = new ContentPage();
            firstPage.LoadFromXaml(
                "<ContentPage xmlns=\"http://xamarin.com/schemas/2014/forms\" xmlns:x=\"http://schemas.microsoft.com/winfx/2009/xaml\" xmlns:markupExtensions=\"clr-namespace:Xam.Zero.MarkupExtensions;assembly=Xam.Zero\" xmlns:viewModels=\"clr-namespace:Xam.Zero.Tests.MockedResources.ViewModels;assembly=Xam.Zero.Tests\" x:Class=\"Xam.Zero.Tests.MockedResources.Pages.FirstPage\" x:Name=\"First\" BindingContext=\"{markupExtensions:ShellPagedViewModelMarkup ViewModel={x:Type viewModels:FirstViewModel}, Page={x:Reference First}}\"></ContentPage>");
            Assert.AreEqual(firstPage.BindingContext.GetType(), typeof(FirstViewModel));
        }
        
        [SetUp]
        public void SetUp()
        {
            Xamarin.Forms.Mocks.MockForms.Init();
            
            //todo change when xamarin.forms.mocks will support xamarin.forms 4
            Device.SetFlags(new List<string>{"Shell_Experimental"});

        }

    }
}