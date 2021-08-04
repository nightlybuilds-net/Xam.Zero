using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Windows.Input;
using Xam.Zero.Classes;
using Xam.Zero.ViewModels;

namespace Xam.Zero.Dev.Features.CommandPage
{
    public class CommandViewModel : ZeroBaseModel
    {
        private string _name;
        private string _surname;
        private bool _checked;
        private bool _isBusy;
        public ICommand TestCommand { get; set; }

        public string Surname
        {
            get => this._surname;
            set
            {
                this._surname = value;
                this.RaisePropertyChanged();
            }
        }

        public string Name
        {
            get => this._name;
            set
            {
                this._name = value;
                this.RaisePropertyChanged();
            }
        }

        public bool Checked
        {
            get => this._checked;
            set
            {
                this._checked = value;
                this.RaisePropertyChanged();
            }
        }
        
        public bool IsBusy
        {
            get => this._isBusy;
            set
            {
                this._isBusy = value;
                this.RaisePropertyChanged();
            }
        }

        public CommandViewModel()
        {
            this.TestCommand = ZeroCommand.On(this)
                // .WithCanExcecute(() => !string.IsNullOrEmpty(this.Name)  && !string.IsNullOrEmpty(this.Surname))
                .WithCanExcecute(this.InnerExpression())
                .WithExcecute(async () => await this.InnerAction());
        }

        private async Task InnerAction()
        {
            this.IsBusy = true;
            await Task.Delay(1000);
            await base.DisplayAlert("Test", "Name and surname are correct!", "ok");
            await Task.Delay(1000);
            this.IsBusy = false;
        }

        private Expression<Func<bool>> InnerExpression() =>
            () => this.ValidateLength(this.Name) && 
                  !string.IsNullOrEmpty(this.Name) && !string.IsNullOrEmpty(this.Surname)
                  && this.Checked && !this.IsBusy;

        private bool ValidateLength(string name) =>  name?.Length > 5;

    }
}