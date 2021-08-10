using System;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Windows.Input;
using Xam.Zero.Classes;
using Xam.Zero.ViewModels;
using Xam.Zero.ZCommand;


namespace Xam.Zero.Dev.Features.CommandPage
{
    public class CommandViewModel : ZeroBaseModel
    {
        private string _name;
        private string _surname;
        private bool _checked;
        private bool _isBusy;
        public ICommand TestSuccessCommand { get; set; }
        public ICommand TestSwallowErrorCommand { get; set; }
        public ICommand TestErrorCommand { get; set; }
        public ICommand BeforeRunEvaluationCommadn { get; set; }
        public ICommand ContextEvaluationCommand { get; set; }
        public ICommand OneByOneCommand { get; set; }
        public ICommand AutoInvalidateCommand { get; set; }
        public ICommand CommandWithParameter { get; set; }
        public ICommand CommandWithValidation { get; set; }

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
            this.TestSuccessCommand = ZeroCommand.On(this)
                .WithCanExecute(this.InnerExpression())
                .WithExecute((commandParam, context) => this.InnerShowMessageAction())
                .Build();
            
            this.TestSwallowErrorCommand = ZeroCommand.On(this)
                .WithCanExecute(this.InnerExpression())
                .WithExecute((commandParam,context) => this.InnerManageErrorWithSwallow())
                .WithErrorHandler(exception => base.DisplayAlert("Managed Exception",exception.Message,"ok"))
                .WithSwallowException()
                .Build();
            
            this.TestErrorCommand = ZeroCommand.On(this)
                .WithCanExecute(this.InnerExpression())
                .WithExecute((commandParam,context)  => this.InnerManageErrorWithoutSwallow())
                .WithErrorHandler(exception => base.DisplayAlert("Managed Exception",exception.Message,"ok"))
                .Build();
            
            this.BeforeRunEvaluationCommadn = ZeroCommand.On(this)
                .WithCanExecute(this.InnerExpression())
                .WithExecute((commandParam,context)  => this.InnerEvaluateCanRun())
                .WithBeforeExecute(context => base.DisplayAlert("Before Run Question","Can i run?","yes","no"))
                .WithAfterExecute(context => base.DisplayAlert("I'm running after a execution","I'll not run if evaluation fail!","ok"))
                .Build();
            
            this.ContextEvaluationCommand = ZeroCommand.On(this)
                .WithCanExecute(this.InnerExpression())
                .WithExecute((commandParam,context)  => this.InnerShowMessageAction())
                .WithBeforeExecute(context =>
                {
                    var stopWatch = new Stopwatch();
                    context.Add(stopWatch);
                    stopWatch.Start();
                    return true;
                })
                .WithAfterExecute(async context =>
                {
                    var stopWatch = context.Get<Stopwatch>();
                    stopWatch.Stop();
                    await this.DisplayAlert("Evaluation", $"Executed in {stopWatch.ElapsedMilliseconds} ms", "OK");
                    stopWatch.Reset();
                })
                .Build();

            this.OneByOneCommand = ZeroCommand.On(this)
                .WithBeforeExecute(context =>
                {
                    var executionCount = context.Get<int>(0);
                    executionCount += 1;
                    context.Add<int>(executionCount);
                    return true;
                })
                .WithExecute(async (commandParam,context)  =>
                {
                    await Task.Delay(500);
                    await this.DisplayAlert("Execution", $"Execution number {context.Get<int>()} times", "OK");
                })
                .Build();

            this.AutoInvalidateCommand = ZeroCommand.On(this)
                .WithAutoInvalidateWhenExecuting()
                .WithExecute(async (o, context) =>
                {
                    await Task.Delay(1000);
                    await base.DisplayAlert("Auto invalidate", "Now button should be disabled!", "ok");
                }).Build();

            this.CommandWithParameter = ZeroCommand<string>.On(this)
                .WithCanExecute(this.InnerExpression())
                .WithExecute(async (s, context) =>
                {
                    await base.DisplayAlert("Command parameter", $"Parameter is {s}", "ok");
                })
                .Build();

            this.CommandWithValidation = ZeroCommand.On(this)
                .WithValidator(async context =>
                {
                    var canExecute = this.InnerExpression().Compile().Invoke();
                    if(!canExecute)
                        await base.DisplayAlert("Cannot execute", "Check the validation rulez!", "ok");

                    return canExecute;
                })
                .WithExecute(async (o, context) =>
                {
                    await base.DisplayAlert("Yes!", "Validation has passed!", "ok");
                })
                .Build();
        }


        private async Task InnerShowMessageAction()
        {
            this.IsBusy = true;
            await Task.Delay(1000);
            await base.DisplayAlert("Test", "Name and surname are correct!", "ok");
            await Task.Delay(1000);
            this.IsBusy = false;
        }
        
        private void InnerManageErrorWithSwallow()
        {
            throw new Exception("MEssage from exception");
        }
        
        private void InnerManageErrorWithoutSwallow()
        {
            throw new Exception("App is going to crash! EVERY MAN FOR HIMSELF!");
        }
        
        private Task InnerEvaluateCanRun()
        {
            return base.DisplayAlert("Evaluation OK", "Thanks, I'M ALIVE!!!", "OK");
        }

        private Expression<Func<bool>> InnerExpression() =>
            () => this.ValidateLength(this.Name) && 
                  !string.IsNullOrEmpty(this.Name) && !string.IsNullOrEmpty(this.Surname)
                  && this.Checked && !this.IsBusy;

        private bool ValidateLength(string name) =>  name?.Length > 5;

    }
}