using System;
using System.ComponentModel;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.CommunityToolkit.UI.Views;
using System.Linq;
using Xam.Zero.ViewModels;
using Xam.Zero.Services;
using Xam.Zero.Popups.Ioc;
using System.Threading.Tasks;

namespace Xam.Zero.Popups.ViewModels
{
    public abstract class ZeroBaseModelWithPopups : ZeroBaseModel
    {
        protected ZeroBaseModelWithPopups()
        {
            
        }

        public void ShowPopup<P, T>(object data = null) where P : Popup<T>, IXamZeroPopup<T>
        {
            var popup = ResolvePopupWithContext<P,T>(data);
            this.CurrentPage.Navigation.ShowPopup(popup);
        }

        public Task<T> ShowPopupAsync<P, T>(object data = null) where P : Popup<T>, IXamZeroPopup<T>
        {
            var popup = ResolvePopupWithContext<P, T>(data);
            return this.CurrentPage.Navigation.ShowPopupAsync(popup);
        }

        private Popup<T> ResolvePopupWithContext<P,T>(object data) where P : Popup<T>, IXamZeroPopup<T>
        {
            var resolver = ZeroIoc.Container.Resolve<IPopupResolver>();
            var popup = resolver.ResolvePopup<P,T>(data);
            return popup;
        }
    }
}
