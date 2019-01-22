using System;
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
            return Ioc.Container.Resolve(this.ViewModel);
        }
    }
}