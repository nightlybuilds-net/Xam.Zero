using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Xam.Zero.Classes
{
    public class ZeroCommand : ICommand
    {
        private readonly Func<bool> _canExecute;
        private readonly IEnumerable<string> _trackedProperties;
        private readonly Func<ZeroCommandContext, bool> _beforeExecute;
        private readonly Func<ZeroCommandContext, Task<bool>> _beforeExecuteAsync;
        private readonly Action<ZeroCommandContext> _afterExecute;
        private readonly Func<ZeroCommandContext, Task> _afterExecuteAsync;
        private readonly Action<ZeroCommandContext> _action;
        private readonly Func<ZeroCommandContext, Task> _asyncAction;
        private readonly bool _swallowException;
        private readonly Action<Exception> _onError;
        private readonly Func<Exception, Task> _onErrorAsync;

        private readonly ZeroCommandContext _context = new ZeroCommandContext();

        internal ZeroCommand(INotifyPropertyChanged viewmodel, Action<ZeroCommandContext> action, Func<ZeroCommandContext, Task> asyncAction,
            Func<bool> canExecute, Action<Exception> onError, Func<Exception, Task> onErrorAsync, bool swallowException,
            IEnumerable<string> trackedProperties, Func<ZeroCommandContext, bool> beforeExecute, Func<ZeroCommandContext, Task<bool>> beforeExecuteAsync,
            Action<ZeroCommandContext> afterExecute, Func<ZeroCommandContext, Task> afterExecuteAsync)
        {
            this._action = action;
            this._asyncAction = asyncAction;
            this._canExecute = canExecute;
            this._onError = onError;
            this._onErrorAsync = onErrorAsync;
            this._swallowException = swallowException;
            this._trackedProperties = trackedProperties;
            this._beforeExecute = beforeExecute;
            this._beforeExecuteAsync = beforeExecuteAsync;
            this._afterExecute = afterExecute;
            this._afterExecuteAsync = afterExecuteAsync;
            viewmodel.PropertyChanged +=
                new WeakEventHandler<PropertyChangedEventArgs>(this.InnerEvaluateCanExcecute).Handler;
        }

        /// <summary>
        /// Start creation of a ZeroCommand
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        public static ZeroCommandBuilder On(INotifyPropertyChanged viewModel)
        {
            return new ZeroCommandBuilder(viewModel);
        }

        private void InnerEvaluateCanExcecute(object sender, PropertyChangedEventArgs e)
        {
            if (this._trackedProperties == null || !this._trackedProperties.Any()) return;
            if (this._trackedProperties.Contains(e.PropertyName))
                this.CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Force evaluation of Can Execute
        /// </summary>
        public void RaiseEvaluateCanExecute()
        {
            this.CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

        public bool CanExecute(object parameter)
        {
            return this._canExecute?.Invoke() ?? true;
        }

        public async void Execute(object parameter)
        {
            var beforeRunEvaluation = true;
            try
            {
                beforeRunEvaluation = await this.EvaluateBeforeRun();
                if(!beforeRunEvaluation)
                    return;

                if (this._asyncAction != null)
                    await this._asyncAction.Invoke(this._context);

                this._action?.Invoke(this._context);
            }
            catch (Exception e)
            {
                if (this._onErrorAsync != null)
                    await this._onErrorAsync?.Invoke(e);

                this._onError?.Invoke(e);

                if (!this._swallowException)
                    throw;
            }
            finally
            {
                if (beforeRunEvaluation)
                {
                    if (this._afterExecuteAsync != null)
                        await this._afterExecuteAsync?.Invoke(this._context);

                    this._afterExecute?.Invoke(this._context);
                }
            }
        }

        /// <summary>
        /// Evaluate if before run condition are met
        /// </summary>
        /// <returns></returns>
        private async Task<bool> EvaluateBeforeRun()
        {
            var beforeRun = true;
            if (this._beforeExecuteAsync != null)
                beforeRun = await this._beforeExecuteAsync.Invoke(this._context);

            if (this._beforeExecute != null)
                beforeRun = this._beforeExecute.Invoke(this._context);

            return beforeRun;
        }

        public event EventHandler CanExecuteChanged;
    }
}