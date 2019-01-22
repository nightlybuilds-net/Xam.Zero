using System;
using Xam.Zero.Ioc;
using Xam.Zero.ViewModels;
using Xamarin.Forms;

namespace Xam.Zero.MarkupExtensions
{
    /// <summary>
    /// Resolve ViewModel from XAML
    /// pass Page too 
    /// </summary>
    public class ShellPagedViewModelMarkup : ViewModelMarkup
    {
        public Page Page { get; set; }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            var baseModel = (ZeroBaseModel)ZeroIoc.Container.Resolve(this.ViewModel);
            baseModel.CurrentPage = this.Page;
            return baseModel;
        }
    }
}