using System.Windows.Input;
using Xam.Zero.ViewModels;
using Xamarin.Forms;

namespace Xam.Zero.SimpleTabbedApp.ViewModels
{
    public class RegistrationViewModel: ZeroBaseModel
    {
        public ICommand BackToLoginCommand { get; private set; }
        public ICommand ClickMeCommand { get; private set; }

        public RegistrationViewModel()
        {
            this.BackToLoginCommand = new Command(async () => await base.PopModal());
            this.ClickMeCommand = new Command(async () =>
            {
                await base.DisplayAlert("Info", "You clicked!", "OK");
            });
        }
    }
}