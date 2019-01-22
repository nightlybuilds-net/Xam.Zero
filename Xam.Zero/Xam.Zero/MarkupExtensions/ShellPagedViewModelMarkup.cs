using System;
using System.Reflection;
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

        /// <summary>
        /// Call init on VM
        /// </summary>
        public bool CallInit { get; set; }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            var baseModel = (ZeroBaseModel)ZeroIoc.Container.Resolve(this.ViewModel);
            baseModel.CurrentPage = this.Page;

            if (this.CallInit)
            {
                var dynMethod = typeof(ZeroBaseModel).GetMethod("Init", 
                    BindingFlags.NonPublic | BindingFlags.Instance);
                if (dynMethod != null) dynMethod.Invoke(baseModel, new object[] {null});
            }
          

            return baseModel;
        }
    }
}