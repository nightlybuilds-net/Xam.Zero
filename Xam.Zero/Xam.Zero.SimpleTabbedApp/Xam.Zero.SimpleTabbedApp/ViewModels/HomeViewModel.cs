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
            _dummyService = dummyService;
            this.GetDummyDataCommand = new Command(() =>
            {
                var dummyData = _dummyService.GetDummyString();
                base.DisplayAlert("Info", dummyData, "OK");
            });
        }
        
        
        
    }
}