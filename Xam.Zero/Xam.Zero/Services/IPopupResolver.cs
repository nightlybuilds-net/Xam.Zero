using System;
using System.Collections.Generic;
using System.Text;
using Xam.Zero.Popups;
using Xam.Zero.ViewModels;

namespace Xam.Zero.Services
{
    public interface IPopupResolver
    {
        /// <summary>
        /// Resolve a popup
        /// </summary>
        /// <typeparam name="P"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        P ResolvePopup<P>(ZeroBaseModel previousModel = null, object data = null) where P : IXamZeroPopup;

        /// <summary>
        /// Resolve a popup that return a value of type T
        /// </summary>
        /// <typeparam name="P"></typeparam>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        P ResolvePopup<P, T>(ZeroBaseModel previousModel = null, object data = null) where P : IXamZeroPopup<T>;
    }
}
