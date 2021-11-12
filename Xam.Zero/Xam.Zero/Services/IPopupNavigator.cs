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
        Type BasePopupType { get; }

        Task<T> ShowPopup<T>(IXamZeroPopup<T> popup);
        Task DismissPopup<T>(IXamZeroPopup<T> popup, T result);
    }
}
