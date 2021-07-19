using System;
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

        /// <summary>
        /// Resolve a page using an explicit Type
        /// </summary>
        /// <param name="pageType"></param>
        /// <param name="previousModel"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        Page ResolvePage(Type pageType, ZeroBaseModel previousModel, object data);
    }
}