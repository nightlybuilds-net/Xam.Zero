using Xam.Zero.ViewModels;
using Xamarin.Forms;

namespace Xam.Zero.Ioc
{
    internal static class ZeroIoc
    {
        public static IContainer Container { get; private set; }

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
        
        internal static void UseContainer(IContainer container)
        {
            Container = container;
        }
    }
}