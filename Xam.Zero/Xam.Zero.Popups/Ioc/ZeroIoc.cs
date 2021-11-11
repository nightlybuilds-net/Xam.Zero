using Xam.Zero.Ioc;
using Xam.Zero.Popups.ViewModels;
using Xam.Zero.ViewModels;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Forms;

namespace Xam.Zero.Popups.Ioc
{
    internal static class ZeroIoc
    {
        internal static IContainer Container { get; set; }

        internal static void RegisterPopups(bool popupsAreTransient)
        {
            ZeroAppExtensions.RegisterMany(type => type.IsClass && 
                !type.IsAbstract && 
                typeof(IXamZeroPopup).IsAssignableFrom(type) &&
                type.IsSubclassOf(typeof(BasePopup)) , popupsAreTransient);
        }

        public static void RegisterPopupViewModels(bool viewmodelsAreTransient)
        {
            ZeroAppExtensions.RegisterMany(type => type.IsClass && 
                !type.IsAbstract &&
                typeof(IZeroPopupBaseModel).IsAssignableFrom(type), viewmodelsAreTransient);
        }
    }
}