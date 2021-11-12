using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xam.Zero.Popups;
using Xamarin.Forms;

namespace Xam.Zero.Services
{
    public interface IPopupNavigator
    {
        Type BasePopupType { get; set; }

        Task<T> ShowPopup<T>(INavigation navigation, IXamZeroPopup<T> popup);
        Task<T> DismissPopup<T>(INavigation navigation, IXamZeroPopup<T> popup);
    }
}
