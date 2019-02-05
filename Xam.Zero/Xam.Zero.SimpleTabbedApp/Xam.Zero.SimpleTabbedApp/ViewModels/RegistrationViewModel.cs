using System.Threading.Tasks;
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
            this.BackToLoginCommand = new Command(async () => await base.PopModal("I'm received back data!"));
            this.ClickMeCommand = new Command(async () =>
            {
                await base.DisplayAlert("Info", "You clicked!", "OK");
            });
        }

        private string _param;

        public string Param
        {
            get => _param;
            set
            {
                _param = value;
                base.RaisePropertyChanged(() => this.Param);
            }
        }

        protected override Task PrepareModel(object data)
        {
            var param = data.ToString();
            this.Param = param;
            return base.PrepareModel(data);
        }
    }
}