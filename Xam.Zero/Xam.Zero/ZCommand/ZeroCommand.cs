using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Xam.Zero.Annotations;
using Xam.Zero.Classes;

namespace Xam.Zero.ZCommand
{
    public class ZeroCommand : ZeroCommand<object>
    {
        internal ZeroCommand(INotifyPropertyChanged viewmodel, Action<object, ZeroCommandContext> action,
            Func<object, ZeroCommandContext, Task> asyncAction, Func<bool> canExecute, Action<Exception> onError,
            Func<Exception, Task> onErrorAsync, bool swallowException, IEnumerable<string> trackedProperties,
            Func<ZeroCommandContext, bool> beforeExecute, Func<ZeroCommandContext, Task<bool>> beforeExecuteAsync,
            Action<ZeroCommandContext> afterExecute, Func<ZeroCommandContext, Task> afterExecuteAsync,
            int concurrentExecution, bool autoCanExecute, Func<ZeroCommandContext, bool> validate,
            Func<ZeroCommandContext, Task<bool>> validateAsync, List<INotifyCollectionChanged> notifyCollectionChangeds)
            : base(viewmodel, action, asyncAction, canExecute, onError,
                onErrorAsync, swallowException, trackedProperties, beforeExecute, beforeExecuteAsync, afterExecute,
                afterExecuteAsync, concurrentExecution, autoCanExecute, validate, validateAsync,
                notifyCollectionChangeds)
        {
        }
    }

    public class ZeroCommand<T> : ICommand, INotifyPropertyChanged
    {
        private readonly Func<bool> _canExecute;
        private readonly IEnumerable<string> _trackedProperties;
        private readonly Func<ZeroCommandContext, bool> _beforeExecute;
        private readonly Func<ZeroCommandContext, Task<bool>> _beforeExecuteAsync;
        private readonly Action<ZeroCommandContext> _afterExecute;
        private readonly Func<ZeroCommandContext, Task> _afterExecuteAsync;
        private readonly bool _autoCanExecute;
        private readonly Func<ZeroCommandContext, bool> _validate;
        private readonly Func<ZeroCommandContext, Task<bool>> _validateAsync;
        private readonly Action<T, ZeroCommandContext> _action;
        private readonly Func<T, ZeroCommandContext, Task> _asyncAction;
        private readonly bool _swallowException;
        private readonly Action<Exception> _onError;
        private readonly Func<Exception, Task> _onErrorAsync;

        private readonly ZeroCommandContext _context = new ZeroCommandContext();
        private readonly SemaphoreSlim _concurrentSemaphore;

        private bool _isExecuting;
        /// <summary>
        /// This is internal managed by ZeroCommand
        /// Is set to true before onbeforerun
        /// Is set to false on after execute
        /// This raise notifypropertychanged  
        /// </summary>
        public bool IsExecuting
        {
            get => this._isExecuting;
            private set
            {
                this._isExecuting = value;
                this.OnPropertyChanged();
            }
        }

        internal ZeroCommand(INotifyPropertyChanged viewmodel, Action<T, ZeroCommandContext> action,
            Func<T, ZeroCommandContext, Task> asyncAction,
            Func<bool> canExecute, Action<Exception> onError, Func<Exception, Task> onErrorAsync, bool swallowException,
            IEnumerable<string> trackedProperties, Func<ZeroCommandContext, bool> beforeExecute,
            Func<ZeroCommandContext, Task<bool>> beforeExecuteAsync,
            Action<ZeroCommandContext> afterExecute, Func<ZeroCommandContext, Task> afterExecuteAsync,
            int concurrentExecution, bool autoCanExecute, Func<ZeroCommandContext, bool> validate,
            Func<ZeroCommandContext, Task<bool>> validateAsync, List<INotifyCollectionChanged> notifyCollectionChangeds)
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
            this._autoCanExecute = autoCanExecute;
            this._validate = validate;
            this._validateAsync = validateAsync;
            this._concurrentSemaphore = new SemaphoreSlim(concurrentExecution);
            viewmodel.PropertyChanged +=
                new WeakEventHandler<PropertyChangedEventArgs>(this.InnerEvaluateCanExcecute).Handler;
            notifyCollectionChangeds.ForEach(collection =>
            {
                collection.CollectionChanged +=
                    new WeakEventHandler<NotifyCollectionChangedEventArgs>((sender, args) =>
                        this.CanExecuteChanged?.Invoke(this, EventArgs.Empty)).Handler;
            });
        }

        /// <summary>
        /// Start creation of a ZeroCommand
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        public static ZeroCommandBuilder<T> On(INotifyPropertyChanged viewModel)
        {
            return new ZeroCommandBuilder<T>(viewModel);
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
            if (this._autoCanExecute && this.IsExecuting) return false;

            return this._canExecute?.Invoke() ?? true;
        }

        public async void Execute(object parameter)
        {
            this.IsExecuting = true;
            if (this._autoCanExecute) this.RaiseEvaluateCanExecute();

            await this._concurrentSemaphore.WaitAsync();

            var beforeRunEvaluation = true;
            try
            {
                beforeRunEvaluation = await this.EvaluateBeforeRun();
                if (!beforeRunEvaluation)
                    return;

                if (this._asyncAction != null)
                    await this._asyncAction.Invoke((T)parameter, this._context);

                this._action?.Invoke((T)parameter, this._context);
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

                this._concurrentSemaphore.Release();
                this.IsExecuting = false;
                if (this._autoCanExecute) this.RaiseEvaluateCanExecute();
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

            if (this._validate != null)
                beforeRun = this._validate.Invoke(this._context);

            if (this._validateAsync != null)
                beforeRun = await this._validateAsync.Invoke(this._context);

            return beforeRun;
        }

        public event EventHandler CanExecuteChanged;
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}