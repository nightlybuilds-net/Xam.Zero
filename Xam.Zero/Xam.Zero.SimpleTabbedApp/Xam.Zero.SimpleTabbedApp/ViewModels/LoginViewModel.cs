using System.Threading.Tasks;
using System.Windows.Input;
using PropertyChanged;
using Xam.Zero.Services;
using Xam.Zero.SimpleTabbedApp.Pages;
using Xam.Zero.SimpleTabbedApp.Shells;
using Xam.Zero.ViewModels;
using Xamarin.Forms;

namespace Xam.Zero.SimpleTabbedApp.ViewModels
{
    [AddINotifyPropertyChangedInterface]
    public class LoginViewModel: ZeroBaseModel
    {
        
        
        private readonly IShellService _shellService;
        
        [DoNotNotify]
        public ICommand GoToRegistrationCommand { get; set; }
        
        [DoNotNotify]
        public ICommand LoginCommand { get; set; }

        public LoginViewModel(IShellService shellService)
        {
            this._shellService = shellService;
            
            this.GoToRegistrationCommand = new Command(async () =>
            {
                var param = "I'm data!";
                await this.Push<RegistrationPage>(param);
            });
            
            this.LoginCommand = new Command( () =>
            {
                this._shellService.SwitchContainer<TabbedShell>();
            });
        }
        
        //with Fody
        public string Param { get; set; }
        
//        //without Fody
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

        protected override Task ReversePrepareModel(object data)
        {
            if (data == null)
            {
                this.Param = "no data back";
                return Task.CompletedTask;
            }
           
            var param = data.ToString();
            this.Param = param;
            return base.PrepareModel(data);
        }
    }
}