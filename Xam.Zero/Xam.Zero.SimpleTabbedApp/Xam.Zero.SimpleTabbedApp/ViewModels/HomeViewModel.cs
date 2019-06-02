using System.Windows.Input;
using Xam.Zero.SimpleTabbedApp.Services;
using Xam.Zero.ViewModels;
using Xamarin.Forms;

namespace Xam.Zero.SimpleTabbedApp.ViewModels
{
    public class HomeViewModel: ZeroBaseModel
    {
        private readonly IDummyService _dummyService;

        public ICommand GetDummyDataCommand { get; private set; }
        
        public HomeViewModel(IDummyService dummyService)
        {
            this._dummyService = dummyService;
            this.GetDummyDataCommand = new Command(() =>
            {
                var dummyData = this._dummyService.GetDummyString();
                this.DisplayAlert("Info", dummyData, "OK");
            });
        }
        
        
        
    }
}