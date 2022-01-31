using System.Threading.Tasks;
using Xam.Zero.Ioc;
using Xam.Zero.Popups;

namespace Xam.Zero.ViewModels
{
    public interface IZeroPopupBaseModel
    {
    }

    public abstract class ZeroPopupBaseModel : NotifyBaseModel, IZeroPopupBaseModel
    {
        public IXamZeroPopup CurrentPopup { get; set; }
        public ZeroBaseModel PreviousModel { get; set; }

        public Task DismissPopup(object data = null)
        {
            var popupNavigator = ZeroIoc.PopupNavigator;
            if (this.PreviousModel != null) Utility.Utility.InvokeReflectionReversePrepareModel(this.PreviousModel, data);
            return popupNavigator.DismissPopup(this.CurrentPopup);
        }

        protected virtual void PrepareModel(object data)
        {
        }
    }

    public abstract class ZeroPopupBaseModel<T> : NotifyBaseModel, IZeroPopupBaseModel
    {
        private T _value;
        public T Value
        {
            get => _value;
            set
            {
                _value = value;
                RaisePropertyChanged(nameof(Value));
            }
        }

        public IXamZeroPopup<T> CurrentPopup { get; set; }
        public ZeroBaseModel PreviousModel { get; set; }

        public Task DismissPopup(object data = null)
        {
            var popupNavigator = ZeroIoc.PopupNavigator;
            if (this.PreviousModel != null) Utility.Utility.InvokeReflectionReversePrepareModel(this.PreviousModel, data);
            return popupNavigator.DismissPopup(this.CurrentPopup, this.Value);
        }

        protected virtual void PrepareModel(object data)
        {
        }
    }
}
