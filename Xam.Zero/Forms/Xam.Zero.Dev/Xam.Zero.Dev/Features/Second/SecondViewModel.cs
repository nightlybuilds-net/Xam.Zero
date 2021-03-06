using System.Windows.Input;
using Xam.Zero.Services;
using Xam.Zero.ViewModels;
using Xamarin.Forms;

namespace Xam.Zero.Dev.Features.Second
{
    public class SecondViewModel : ZeroBaseModel
    {
        public ICommand NavigateCommand { get; set; }
        
        public SecondViewModel()
        {
            this.NavigateCommand = new Command(async ()=>
            {
                await this.Pop();
            });
        }
        
        
    }
}