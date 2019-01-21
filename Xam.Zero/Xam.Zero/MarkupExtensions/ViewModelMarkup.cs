using System;
using DryIoc;
using Xamarin.Forms.Xaml;

namespace Xam.Zero.MarkupExtensions
{
    public class ViewModelMarkup : IMarkupExtension
    {
        public Type ViewModel { get; set; }
        
        public virtual object ProvideValue (IServiceProvider serviceProvider)
        {
            return Ioc.Container.Resolve(this.ViewModel);
        }
    }
}