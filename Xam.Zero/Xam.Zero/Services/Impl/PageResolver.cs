using System;
using System.Diagnostics;
using Xam.Zero.Ioc;
using Xam.Zero.ViewModels;
using Xamarin.Forms;

namespace Xam.Zero.Services.Impl
{
    public class PageResolver : IPageResolver
    {
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
            var typeFullName = page.GetType().FullName;
            var viewModelFullName = $"{typeFullName}ViewModel";
            var pageQualifiedName = page.GetType().AssemblyQualifiedName;
            var viewModelQualifiedName = pageQualifiedName?.Replace(typeFullName ?? throw new InvalidOperationException("Page.GetType is null"),viewModelFullName);
            var viewModelType = Type.GetType(viewModelQualifiedName ?? throw new InvalidOperationException("Viewmodel type is null"));
            var context =  (ZeroBaseModel)ZeroIoc.Container.Resolve(viewModelType);
            page.BindingContext = context;
            return context;
        }
    }
}