using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using Xam.Zero.Utility;

namespace Xam.Zero.ViewModels
{
    public abstract class NotifyBaseModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(
            Expression<Func<object>> expression)
        {
            var propertyName = PropertyName.For(expression);
            this.RaisePropertyChanged(propertyName);
        }
        protected virtual void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}