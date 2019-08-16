using System.Threading.Tasks;
using System.Windows.Input;
using PropertyChanged;
using Xam.Zero.ViewModels;
using Xamarin.Forms;

namespace Xam.Zero.SimpleTabbedApp.ViewModels
{
    [AddINotifyPropertyChangedInterface]
    public class RegistrationViewModel: ZeroBaseModel
    {
        [DoNotNotify]
        public ICommand BackToLoginCommand { get; private set; }
        
        [DoNotNotify]
        public ICommand ClickMeCommand { get; private set; }

        public RegistrationViewModel()
        {
            this.BackToLoginCommand = new Command(async () => await this.Pop("I'm received back data!"));
            this.ClickMeCommand = new Command(async () =>
            {
                await this.DisplayAlert("Info", "You clicked!", "OK");
            });
        }

        // with Fody
        public string Param { get; set; }
        
//        // without Fody
//        private string _param;
//
//        public string Param
//        {
//            get => _param;
//            set
//            {
//                _param = value;
//                base.RaisePropertyChanged(() => this.Param);
//            }
//        }

        protected override Task PrepareModel(object data)
        {
            var param = data.ToString();
            this.Param = param;
            return base.PrepareModel(data);
        }
    }
}