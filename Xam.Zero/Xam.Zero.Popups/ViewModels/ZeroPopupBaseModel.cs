using Xam.Zero.ViewModels;
using Xamarin.CommunityToolkit.UI.Views;

namespace Xam.Zero.Popups.ViewModels
{
    public interface IZeroPopupBaseModel
    {

    }

    public class ZeroPopupBaseModel<T> : NotifyBaseModel, IZeroPopupBaseModel
    {
        private T _value;
        public T Value
        {
            get => this._value;
            set
            {
                this._value = value;
                this.RaisePropertyChanged(nameof(Value));
            }
        }
        public Popup<T> CurrentPopup { get; set; }

        public void LaunchPrepareModel(object data)
        {
            
        }
        
        public void Dismiss()
        {
            this.CurrentPopup.Dismiss(this.Value);
        }
    }
}
