using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Xam.Zero.Ioc;
using Xam.Zero.ViewModels;
using Xamarin.Forms;

namespace Xam.Zero.Services.Impl
{
    public class PageResolver : IPageResolver
    {
        private static Type[] _assemblyTypes;

        public T ResolvePage<T>(ZeroBaseModel previousModel = null, object data = null) where T : Page
        {
            var page = ZeroIoc.Container.Resolve<T>();
            var context = (ZeroBaseModel)page.BindingContext ?? this.ResolveViewModelByConvention(page);

            context.CurrentPage = page;
            context.PreviousModel = previousModel;
            Utility.Utility.InvokeReflectionPrepareModel(context, data);

            return page;
        }

        public Page ResolvePage(Type pageType, ZeroBaseModel previousModel, object data)
        {
            var page = (Page)ZeroIoc.Container.Resolve(pageType);
            var context = (ZeroBaseModel)page.BindingContext ?? this.ResolveViewModelByConvention(page);

            context.CurrentPage = page;
            context.PreviousModel = previousModel;
            Utility.Utility.InvokeReflectionPrepareModel(context, data);

            return page;
        }

        private ZeroBaseModel ResolveViewModelByConvention(Page page)
        {
            if (_assemblyTypes == null)
            {
                var pageAssemply = page.GetType().Assembly;
                _assemblyTypes = pageAssemply.GetTypes().Where(w => w.IsClass).Where(w => !w.IsAbstract)
                    .Where(w => w.IsSubclassOf(typeof(ZeroBaseModel))).ToArray();
            }

            var viewModelName = $"{page.GetType().Name}ViewModel";
            var vmType = _assemblyTypes.Single(sd => sd.Name == viewModelName);
            var context =  (ZeroBaseModel)ZeroIoc.Container.Resolve(vmType);
            page.BindingContext = context;
            return context;
        }
    }
}