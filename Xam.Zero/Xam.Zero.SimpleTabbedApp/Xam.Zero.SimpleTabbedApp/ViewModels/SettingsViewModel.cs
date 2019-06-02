using System.Windows.Input;
using Xam.Zero.Services;
using Xam.Zero.SimpleTabbedApp.Shells;
using Xam.Zero.ViewModels;
using Xamarin.Forms;

namespace Xam.Zero.SimpleTabbedApp.ViewModels
{
    public class SettingsViewModel: ZeroBaseModel
    {
        private readonly IShellService _shellService;

        public ICommand LogoutCommand { get; set; }
        
        public SettingsViewModel(IShellService shellService)
        {
            this._shellService = shellService;
            
            this.LogoutCommand = new Command(() =>
            {
                this._shellService.SwitchContainer<SimpleShell>();
            });
        }
    }
}