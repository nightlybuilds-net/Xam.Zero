using Xam.Zero.ViewModels;
using Xamarin.Forms;

namespace Xam.Zero.Services
{
    public interface IPageResolver
    {
        /// <summary>
        /// Resolve a page
        /// </summary>
        /// <param name="previousModel"></param>
        /// <param name="data"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        T ResolvePage<T>(ZeroBaseModel previousModel = null, object data = null) where T : Page;
    }
}