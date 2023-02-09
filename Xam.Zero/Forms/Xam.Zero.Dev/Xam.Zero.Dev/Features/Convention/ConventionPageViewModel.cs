using System.Windows.Input;
using Xam.Zero.Commands.ZCommand;
using Xam.Zero.ViewModels;

namespace Xam.Zero.Dev.Features.Convention
{
    public class ConventionPageViewModel : ZeroBaseModel
    {
        public ICommand TestCommand { get; private set; }
        
        protected override async void PrepareModel(object data)
        {
            await base.DisplayAlert("PrepareModel", "Called by navigation service", "ok");
        }


        public ConventionPageViewModel()
        {
            this.TestCommand = ZeroCommand.On(this)
                .WithExecute((o, context) => base.DisplayAlert("Button clicked", "Binding is working", "ok"))
                .Build();
        }
        
        
    }
}