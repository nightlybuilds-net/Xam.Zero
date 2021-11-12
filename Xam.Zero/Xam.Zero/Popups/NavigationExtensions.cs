using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xam.Zero.Ioc;
using Xam.Zero.Services;
using Xamarin.Forms;

namespace Xam.Zero.Popups
{
    internal static class NavigationExtensions
    {
        public static Task<T> ShowPopupAsync<T>(this INavigation navigation, IXamZeroPopup<T> popup)
        {
            var popupNavigator = ZeroIoc.PopupNavigator;
            return popupNavigator.ShowPopup<T>(popup);
        }
    }
}
