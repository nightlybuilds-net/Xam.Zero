using Xam.Zero.Popups;
using Xam.Zero.Services;
using Xam.Zero.ViewModels;
using Xamarin.Forms;

namespace Xam.Zero.Ioc
{
    internal static class ZeroIoc
    {
        public static IContainer Container { get; private set; }
        public static IPopupNavigator PopupNavigator { get; private set; }

        /// <summary>
        /// Register all content page
        /// </summary>
        /// <param name="pagesAreTransient"></param>
        public static void RegisterPages(bool pagesAreTransient)
        {
            ZeroApp.RegisterMany(type => type.IsClass && !type.IsAbstract && type.IsSubclassOf(typeof(ContentPage)), pagesAreTransient);
        }

        /// <summary>
        /// Register all viewmodel that extend ZeroBaseModel
        /// </summary>
        /// <param name="viewmodelsAreTransient"></param>
        public static void RegisterViewModels(bool viewmodelsAreTransient)
        {
            ZeroApp.RegisterMany(type => type.IsClass && !type.IsAbstract && type.IsSubclassOf(typeof(ZeroBaseModel)), viewmodelsAreTransient);
        }

        /// <summary>
        /// Register all popups that extends IXamZeroPopup
        /// and are of a valid popup implementation type
        /// </summary>
        /// <param name="popupsAreTransient"></param>
        public static void RegisterPopups(bool popupsAreTransient)
        {
            if (PopupNavigator == null)
                return;

            ZeroApp.RegisterMany(type => type.IsClass &&
                !type.IsAbstract &&
                typeof(IXamZeroPopup).IsAssignableFrom(type) &&
                type.IsSubclassOf(PopupNavigator.BasePopupType), popupsAreTransient);
        }

        /// <summary>
        /// Register all viewmodels that extends IZeroPopupBaseModel
        /// </summary>
        /// <param name="viewmodelsAreTransient"></param>
        public static void RegisterPopupViewModels(bool viewmodelsAreTransient)
        {
            if (PopupNavigator == null)
                return;

            ZeroApp.RegisterMany(type => type.IsClass &&
                !type.IsAbstract &&
                typeof(IZeroPopupBaseModel).IsAssignableFrom(type), viewmodelsAreTransient);
        }

        internal static void UseContainer(IContainer container)
        {
            Container = container;
        }

        internal static void UsePopupNavigator(IPopupNavigator popupNavigator)
        {
            PopupNavigator = popupNavigator;
        }
    }
}