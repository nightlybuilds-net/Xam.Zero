using System;
using System.Collections.Generic;
using System.Text;
using Xam.Zero.Popups;

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
        P ResolvePopup<P>(object data = null) where P : IXamZeroPopup;

        /// <summary>
        /// Resolve a popup that return a value of type T
        /// </summary>
        /// <typeparam name="P"></typeparam>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        P ResolvePopup<P, T>(object data = null) where P : IXamZeroPopup<T>;
    }
}
