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
        public Type BasePopupType => typeof(BasePopup);

        private ToolkitPopupNavigator() { }

        public static ToolkitPopupNavigator Build()
        {
            return new ToolkitPopupNavigator();
        }

        public Task<T> ShowPopup<T>(IXamZeroPopup<T> popup)
        {
            return Application.Current.MainPage.Navigation.ShowPopupAsync(popup as Popup<T>);
        }

        public Task DismissPopup<T>(IXamZeroPopup<T> popup, T result)
        {
            (popup as Popup<T>).Dismiss(result);
            return Task.CompletedTask;
        }
    }
}
