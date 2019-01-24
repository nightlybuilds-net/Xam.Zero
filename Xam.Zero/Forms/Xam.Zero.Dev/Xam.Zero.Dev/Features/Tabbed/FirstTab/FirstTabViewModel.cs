using System.Windows.Input;
using Xam.Zero.Services;
using Xam.Zero.ViewModels;
using Xamarin.Forms;

namespace Xam.Zero.Dev.Features.Tabbed.FirstTab
{
    public class FirstTabViewModel : ZeroBaseModel
    {
        private readonly IShellService _shellService;
        public string Prova { get; set; }

        public ICommand BackCommand { get; set; }

        public FirstTabViewModel(IShellService shellService)
        {
            this._shellService = shellService;
            this.Prova = "Tab 1";
            
            this.BackCommand = new Command(() =>
            {
                this._shellService.SwitchContainer<AppShell>();
            });
        }
    }
}