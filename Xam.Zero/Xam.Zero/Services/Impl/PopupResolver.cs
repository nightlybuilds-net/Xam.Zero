﻿using System;
using System.Collections.Generic;
using System.Linq;
using Xam.Zero.Ioc;
using Xam.Zero.Popups;
using Xam.Zero.Services;
using Xam.Zero.ViewModels;
using Xamarin.Forms;

namespace Xam.Zero.Services.Impl
{
    internal class PopupResolver : IPopupResolver
    {
        private static Type[] _assemblyTypes;

        public P ResolvePopup<P>(ZeroBaseModel previousModel = null, object data = null) where P : IXamZeroPopup
        {
            var popup = ZeroIoc.Container.Resolve<P>();
            var navigablePopup = popup as NavigableElement;
            var context = (ZeroPopupBaseModel)(navigablePopup.BindingContext ?? ResolveViewModelByConvention(popup));
            context.CurrentPopup = popup;
            context.PreviousModel = previousModel;
            Utility.Utility.InvokeReflectionPrepareModel(context, data);
            return popup;
        }

        public P ResolvePopup<P, T>(ZeroBaseModel previousModel = null, object data = null) where P : IXamZeroPopup<T>
        {
            var popup = ZeroIoc.Container.Resolve<P>();
            var navigablePopup = popup as NavigableElement;
            var context = (ZeroPopupBaseModel<T>)(navigablePopup.BindingContext ?? ResolveViewModelByConvention(popup));
            context.CurrentPopup = popup;
            context.PreviousModel = previousModel;
            Utility.Utility.InvokeReflectionPrepareModel(context, data);
            return popup;
        }

        private ZeroPopupBaseModel ResolveViewModelByConvention(IXamZeroPopup popup)
        {
            if (_assemblyTypes == null)
            {
                var pageAssemply = popup.GetType().Assembly;
                _assemblyTypes = pageAssemply.GetTypes().Where(w => w.IsClass).Where(w => !w.IsAbstract)
                    .Where(w => w.IsSubclassOf(typeof(IZeroPopupBaseModel))).ToArray();
            }

            var viewModelName = $"{popup.GetType().Name}ViewModel";
            var vmType = _assemblyTypes.Single(sd => sd.Name == viewModelName);
            var context = (ZeroPopupBaseModel)ZeroIoc.Container.Resolve(vmType);

            var navigablePopup = popup as NavigableElement;
            navigablePopup.BindingContext = context;
            return context;
        }
    }
}
