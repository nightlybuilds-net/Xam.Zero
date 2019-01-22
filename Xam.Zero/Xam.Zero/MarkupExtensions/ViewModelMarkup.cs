using System;
using Xam.Zero.Ioc;
using Xamarin.Forms.Xaml;

namespace Xam.Zero.MarkupExtensions
{
    /// <summary>
    /// Resolve viewmodel from XAML
    /// </summary>
    public class ViewModelMarkup : IMarkupExtension
    {
        public Type ViewModel { get; set; }
        
        public virtual object ProvideValue (IServiceProvider serviceProvider)
        {
            return ZeroIoc.Container.Resolve(this.ViewModel);
        }
    }
}