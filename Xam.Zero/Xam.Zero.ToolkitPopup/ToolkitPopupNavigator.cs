using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xam.Zero.Popups;
using Xam.Zero.Services;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Forms;

namespace Xam.Zero.ToolkitPopup
{
    public class ToolkitPopupNavigator : IPopupNavigator
    {
        private ToolkitPopupNavigator() { }

        public static ToolkitPopupNavigator Build()
        {
            return new ToolkitPopupNavigator();
        }

        public Task ShowPopup(IXamZeroPopup popup)
        {
            var popupPage = (Popup)popup;
            Application.Current.MainPage.Navigation.ShowPopup(popupPage);
            return Task.CompletedTask;
        }

        public Task<T> ShowPopup<T>(IXamZeroPopup<T> popup)
        {
            var popupPage = (Popup<T>)popup;
            return Application.Current.MainPage.Navigation.ShowPopupAsync(popupPage);
        }

        public Task DismissPopup(IXamZeroPopup popup)
        {
            var popupPage = (Popup)popup;
            popupPage.Dismiss(null);
            return Task.CompletedTask;
        }

        public Task DismissPopup<T>(IXamZeroPopup<T> popup, T result)
        {
            var popupPage = (Popup<T>)popup;
            popupPage.Dismiss(result);
            return Task.CompletedTask;
        }
    }
}
