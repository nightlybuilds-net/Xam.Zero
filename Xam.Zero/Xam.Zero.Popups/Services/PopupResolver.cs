using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xam.Zero.Popups.Ioc;
using Xam.Zero.Popups.ViewModels;
using Xam.Zero.Services;
using Xamarin.CommunityToolkit.UI.Views;

namespace Xam.Zero.Popups.Services
{
    internal class PopupResolver : IPopupResolver
    {
        private static Type[] _assemblyTypes;

        public P ResolvePopup<P,T>(object data) where P : IXamZeroPopup<T>
        {
            var xamPopup = (P)ZeroIoc.Container.Resolve<P>();

            if (xamPopup.GetType().IsSubclassOf(typeof(Popup<T>)) == false)
                throw new Exception("You must resolve a Popup");

            var popup = xamPopup as Popup<T>;
            var context = (ZeroPopupBaseModel<T>)(popup.BindingContext ?? this.ResolveViewModelByConvention(popup));

            context.CurrentPopup = popup;
            context.LaunchPrepareModel(data);

            return xamPopup;
        }

        private ZeroPopupBaseModel<T> ResolveViewModelByConvention<T>(Popup<T> page)
        {
            if (_assemblyTypes == null)
            {
                var pageAssemply = page.GetType().Assembly;
                _assemblyTypes = pageAssemply.GetTypes().Where(w => w.IsClass).Where(w => !w.IsAbstract)
                    .Where(w => w.IsSubclassOf(typeof(ZeroPopupBaseModel<T>))).ToArray();
            }

            var viewModelName = $"{page.GetType().Name}ViewModel";
            var vmType = _assemblyTypes.Single(sd => sd.Name == viewModelName);
            var context = (ZeroPopupBaseModel<T>)ZeroIoc.Container.Resolve(vmType);
            page.BindingContext = context;
            return context;
        }
    }
}
