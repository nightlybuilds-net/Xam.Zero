using System;
using System.Linq.Expressions;
using System.Windows.Input;
using Xam.Zero.Classes;
using Xam.Zero.ViewModels;

namespace Xam.Zero.Dev.Features.CommandPage
{
    public class CommandViewModel : ZeroBaseModel
    {
        private string _name;
        private string _surname;
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
        
        public CommandViewModel()
        {
            this.TestCommand = ZeroCommand.On(this)
                // .WithCanExcecute(() => !string.IsNullOrEmpty(this.Name)  && !string.IsNullOrEmpty(this.Surname))
                .WithCanExcecute(this.InnerExpression())
                .WithExcecute(() =>
                    base.DisplayAlert("Test", "Name and surname are correct!", "ok"));
        }

        private Expression<Func<bool>> InnerExpression() =>
            () => !string.IsNullOrEmpty(this.Name) && !string.IsNullOrEmpty(this.Surname);

        
    }
}