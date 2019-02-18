using System;
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

        [TestCase("default string", "updated string")]
        public void Test_PrepareModel(string defaultString, string updatedString)
        {
            var baseModel = new MockedZeroBaseModel();
            baseModel.StringProperty = defaultString;
            
            Assert.AreEqual(baseModel.StringProperty, defaultString);

            var method = baseModel.GetType().GetMethod("PrepareModel", BindingFlags.NonPublic | BindingFlags.Instance);
            method.Invoke(baseModel, new object[]{updatedString});
            
            Assert.AreEqual(baseModel.StringProperty, updatedString);

        }
        
        [TestCase("default string", "updated string")]
        public void Test_ReversePrepareModel(string defaultString, string updatedString)
        {
            var baseModel = new MockedZeroBaseModel();
            baseModel.StringProperty = defaultString;
            
            Assert.AreEqual(baseModel.StringProperty, defaultString);

            var method = baseModel.GetType().GetMethod("ReversePrepareModel", BindingFlags.NonPublic | BindingFlags.Instance);
            method.Invoke(baseModel, new object[]{updatedString});

            Assert.AreEqual(baseModel.StringProperty, updatedString);
        }

        [Test]
        public void Test_Current_Page()
        {
            var baseModel = new MockedZeroBaseModel();
            var currentPage = new ContentPage();

            baseModel.CurrentPage = currentPage;
            
            Assert.IsNotNull(baseModel.CurrentPage);
            Assert.AreEqual(currentPage, baseModel.CurrentPage);
        }
        
        [Test]
        public void Test_Previous_Model()
        {
            var baseModel = new MockedZeroBaseModel();
            var previousModel = new FirstViewModel();

            baseModel.PreviousModel = previousModel;
            
            Assert.IsNotNull(baseModel.PreviousModel);
            Assert.AreEqual(previousModel, baseModel.PreviousModel);
        }

        [Test]
        public void Test_ViewModels_ShellService_And_MessagingCenter_Are_Registered_On_Startup()
        {

            var container = new Container();
            var constructor = typeof(ZeroApp).GetConstructor (BindingFlags.NonPublic|BindingFlags.Instance, null, new Type[0], null);
            var zeroApp = (ZeroApp)constructor.Invoke(null);
            zeroApp
                .WithContainer(DryIocZeroContainer.Build(container))
                .RegisterShell(() => new FirstShell());
            
            var method = typeof(ZeroApp).GetMethod("InnerBootStrap", BindingFlags.NonPublic | BindingFlags.Instance);
            method.Invoke(zeroApp, new object[0]);

            var firstViewModel = container.Resolve<FirstViewModel>();
            var secondViewModel = container.Resolve<SecondViewModel>();

            var shellService = container.Resolve<IShellService>();
            var messagingCenter = container.Resolve<IMessagingCenter>();
            
            Assert.NotNull(firstViewModel);
            Assert.NotNull(secondViewModel);
            Assert.NotNull(shellService);
            Assert.NotNull(messagingCenter);

        }

        [Test]
        public async Task Push_Model()
        {
            var container = new Container();
            var constructor = typeof(ZeroApp).GetConstructor (BindingFlags.NonPublic|BindingFlags.Instance, null, new Type[0], null);
            var zeroApp = (ZeroApp)constructor.Invoke(null);
            zeroApp
                .WithContainer(DryIocZeroContainer.Build(container))
                .RegisterShell(() => new FirstShell());
            
            var method = typeof(ZeroApp).GetMethod("InnerBootStrap", BindingFlags.NonPublic | BindingFlags.Instance);
            method.Invoke(zeroApp, new object[0]);

            var firstViewModel = container.Resolve<FirstViewModel>();
//            await firstViewModel.Push<SecondPage>();

        }

    }
}