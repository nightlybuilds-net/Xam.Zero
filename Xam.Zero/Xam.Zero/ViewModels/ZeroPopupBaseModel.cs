using Xam.Zero.Popups;

namespace Xam.Zero.ViewModels
{
    public interface IZeroPopupBaseModel
    {

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

        public void Dismiss()
        {
            CurrentPopup.DismissPopup(Value);
        }
    }
}
