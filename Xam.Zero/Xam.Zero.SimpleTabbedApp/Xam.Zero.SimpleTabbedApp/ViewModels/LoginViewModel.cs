using System.Threading.Tasks;
using System.Windows.Input;
using Xam.Zero.Services;
using Xam.Zero.SimpleTabbedApp.Pages;
using Xam.Zero.SimpleTabbedApp.Shells;
using Xam.Zero.ViewModels;
using Xamarin.Forms;

namespace Xam.Zero.SimpleTabbedApp.ViewModels
{
    public class LoginViewModel: ZeroBaseModel
    {
        private readonly IShellService _shellService;
        public ICommand GoToRegistrationCommand { get; set; }
        public ICommand LoginCommand { get; set; }

        public LoginViewModel(IShellService shellService)
        {
            _shellService = shellService;
            
            this.GoToRegistrationCommand = new Command(async () =>
            {
                await base.GoModalTo<RegistrationPage>(null);
            });
            this.LoginCommand = new Command( () =>
            {
                _shellService.SwitchContainer<TabbedShell>();
            });
        }

        protected override async Task Init(object data)
        {
            await base.Init(data);
        }
    }
}