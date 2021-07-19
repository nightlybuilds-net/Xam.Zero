using System;
using Xam.Zero.Ioc;
using Xam.Zero.ViewModels;
using Xamarin.Forms;

namespace Xam.Zero.Services.Impl
{
    public class PageResolver : IPageResolver
    {
        public T ResolvePage<T>(ZeroBaseModel previousModel = null,object data = null) where T : Page
        {
            var page = ZeroIoc.Container.Resolve<T>();
            var context = (ZeroBaseModel) page.BindingContext;
            context.CurrentPage = page;
            context.PreviousModel = previousModel;
            Utility.Utility.InvokeReflectionPrepareModel(context, data);

            return page;
        }

        public Page ResolvePage(Type pageType, ZeroBaseModel previousModel, object data)
        {
            var page = (Page)ZeroIoc.Container.Resolve(pageType);
            var context = (ZeroBaseModel) page.BindingContext;
            context.CurrentPage = page;
            context.PreviousModel = previousModel;
            Utility.Utility.InvokeReflectionPrepareModel(context, data);

            return page;
        }
    }
}