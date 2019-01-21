using System;
using DryIoc;
using Xamarin.Forms;

namespace Xam.Zero.MarkupExtensions
{
    public class ShellPagedViewModelMarkup : ViewModelMarkup
    {
        public Page Page { get; set; }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            var baseModel = (ZeroBaseModel)Ioc.Container.Resolve(this.ViewModel);
            baseModel.CurrentPage = this.Page;
            return baseModel;
        }
    }
}