using System.Threading.Tasks;
using Xam.Zero.Ioc;
using Xam.Zero.Popups;

namespace Xam.Zero.ViewModels
{
    public interface IZeroPopupBaseModel
    {

    }

    public class ZeroPopupBaseModel : NotifyBaseModel, IZeroPopupBaseModel
    {
        public IXamZeroPopup CurrentPopup { get; set; }

        public Task DismissPopup()
        {
            var popupNavigator = ZeroIoc.PopupNavigator;
            return popupNavigator.DismissPopup(this.CurrentPopup);
        }
    }

    public class ZeroPopupBaseModel<T> : NotifyBaseModel, IZeroPopupBaseModel
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

        public Task DismissPopup()
        {
            var popupNavigator = ZeroIoc.PopupNavigator;
            return popupNavigator.DismissPopup(this.CurrentPopup, this.Value);
        }
    }
}
