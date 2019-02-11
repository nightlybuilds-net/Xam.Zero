using System.ComponentModel;
using System.Reflection;
using Container = DryIoc.Container;
using NUnit.Framework;
using Xam.Zero.DryIoc;
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

//        [Test]
//        public void Test_PushModel()
//        {
//
//            var currentPage = new ContentPage();
//            var currentViewModel = new FirstViewModel();
//            currentPage.BindingContext = currentViewModel;
//            
//            var nextPage = new ContentPage();
//            var nextViewModel = new SecondViewModel();
//            nextPage.BindingContext = nextViewModel;
//            
//            
//            var shellContent = new ShellContent {Content = currentPage};
//            var shellSection = new ShellSection {CurrentItem = shellContent};
//            var shellItem = new ShellItem {Items = {shellSection}};
//            var shell = new Shell {Items = {shellItem}};
//            
//            
//            var container = new Container();
//            
//            var app = new Application();
//            ZeroApp
//                .On(app)
//                .WithContainer(DryIocZeroContainer.Build(container))
//                .RegisterShell(() => shell);
//
//            currentViewModel.PushModal<FirstPage>();
//
//
//        }

        internal class FirstPage : ContentPage
        {
            
        }
    }
}