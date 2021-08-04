using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Xam.Zero.Classes
{
    public class ZeroCommand : ICommand
    {
        private readonly INotifyPropertyChanged _viewmodel;
        private readonly Func<bool> _canExecute;
        private readonly IEnumerable<string> _trackedProperties;
        private readonly Func<bool> _beforeExecute;
        private readonly Func<Task<bool>> _beforeExecuteAsync;
        private readonly Action _action;
        private readonly Func<Task> _asyncAction;
        private readonly bool _swallowException;
        private readonly Action<Exception> _onError;
        private readonly Func<Exception, Task> _onErrorAsync;

        internal ZeroCommand(INotifyPropertyChanged viewmodel, Action action, Func<Task> asyncAction,
            Func<bool> canExecute, Action<Exception> onError, Func<Exception, Task> onErrorAsync, bool swallowException,
            IEnumerable<string> trackedProperties, Func<bool> beforeExecute, Func<Task<bool>> beforeExecuteAsync)
        {
            this._viewmodel = viewmodel;
            this._action = action;
            this._asyncAction = asyncAction;
            this._canExecute = canExecute;
            this._onError = onError;
            this._onErrorAsync = onErrorAsync;
            this._swallowException = swallowException;
            this._trackedProperties = trackedProperties;
            this._beforeExecute = beforeExecute;
            this._beforeExecuteAsync = beforeExecuteAsync;
            viewmodel.PropertyChanged +=
                new WeakEventHandler<PropertyChangedEventArgs>(this.InnerEvaluateCanExcecute).Handler;
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
            try
            {
                if (await this.EvaluateBeforeRun()) return;

                if (this._asyncAction != null)
                    await this._asyncAction.Invoke();

                this._action?.Invoke();
            }
            catch (Exception e)
            {
                if (this._onErrorAsync != null)
                    await this._onErrorAsync?.Invoke(e);

                this._onError?.Invoke(e);

                if (!this._swallowException)
                    throw;
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
                beforeRun = await this._beforeExecuteAsync.Invoke();

            if (this._beforeExecute != null)
                beforeRun = this._beforeExecute.Invoke();

            return !beforeRun;
        }

        public event EventHandler CanExecuteChanged;
    }

    public class ZeroCommandBuilder
    {
        private readonly INotifyPropertyChanged _viewmodel;
        private IEnumerable<string> _trackedProperties;
        private bool _swallowException;
        private Action<Exception> _onError;
        private Func<Exception, Task> _onErrorAsync;
        private Func<bool> _canExecute;
        private Action _action;
        private Func<Task> _actionAsync;
        private Func<bool> _beforeExecute;
        private Func<Task<bool>> _beforeExecuteAsync;

        private ZeroCommandBuilder(INotifyPropertyChanged viewmodel)
        {
            this._viewmodel = viewmodel;
        }

        /// <summary>
        /// Start Builder
        /// </summary>
        /// <param name="viewmodel"></param>
        /// <returns></returns>
        public static ZeroCommandBuilder On(INotifyPropertyChanged viewmodel)
        {
            return new ZeroCommandBuilder(viewmodel);
        }

        /// <summary>
        /// Add can execute expression
        /// This expression is used for evalauation of canexecute and
        /// for tracking raise canexecute dependencies 
        /// </summary>
        /// <param name="canExcecuteExpression"></param>
        /// <returns></returns>
        public ZeroCommandBuilder WithCanExecute(Expression<Func<bool>> canExcecuteExpression)
        {
            this._canExecute = canExcecuteExpression.Compile();
            this._trackedProperties = this.GetTrackProperties(canExcecuteExpression.Body, this._viewmodel.GetType());
            return this;
        }


        /// <summary>
        /// Retrieve track properties that exist on tracked object
        /// </summary>
        /// <param name="canExcecuteExpression"></param>
        /// <param name="trackedType"></param>
        /// <returns></returns>
        private IEnumerable<string> GetTrackProperties(Expression canExcecuteExpression, Type trackedType)
        {
            var allProperties = new List<string>();

            switch (canExcecuteExpression)
            {
                case MemberExpression memberExpression:
                {
                    if (memberExpression.Member.DeclaringType == trackedType)
                        allProperties.Add(memberExpression.Member.Name);
                    break;
                }
                case BinaryExpression binaryExpression:
                    allProperties.AddRange(this.GetTrackProperties(binaryExpression.Left, trackedType));
                    allProperties.AddRange(this.GetTrackProperties(binaryExpression.Right, trackedType));
                    break;
                case MethodCallExpression methodCallExpression:
                    foreach (var expression in methodCallExpression.Arguments)
                    {
                        allProperties.AddRange(this.GetTrackProperties(expression, trackedType));
                    }

                    break;
                case UnaryExpression unaryExpression:
                    allProperties.AddRange(this.GetTrackProperties(unaryExpression.Operand, trackedType));
                    break;
            }

            return allProperties.Distinct();
        }


        /// <summary>
        /// Do not throw exception on execute
        /// </summary>
        /// <returns></returns>
        public ZeroCommandBuilder WithSwallowException()
        {
            this._swallowException = true;
            return this;
        }

        /// <summary>
        /// Catch execution error
        /// </summary>
        /// <param name="onError"></param>
        /// <returns></returns>
        public ZeroCommandBuilder WithErrorHandler(Action<Exception> onError)
        {
            if (this._onErrorAsync != null || this._onError != null)
                throw new Exception("On error action already added!");
            
            this._onError = onError;
            return this;
        }

        /// <summary>
        /// Catch execution error
        /// </summary>
        /// <param name="onErrorTask"></param>
        /// <returns></returns>
        public ZeroCommandBuilder WithErrorHandler(Func<Exception, Task> onErrorTask)
        {
            if (this._onErrorAsync != null || this._onError != null)
                throw new Exception("On error action already added!");
            
            this._onErrorAsync = onErrorTask;
            return this;
        }

        /// <summary>
        /// Add Execute for Action
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public ZeroCommandBuilder WithExecute(Action action)
        {
            if (this._action != null || this._actionAsync != null)
                throw new Exception("Execute action already added!");
            
            this._action = action;
            return this;
        }

        /// <summary>
        /// Add Execute for Task
        /// </summary>
        /// <param name="taskAction"></param>
        /// <returns></returns>
        public ZeroCommandBuilder WithExecute(Func<Task> taskAction)
        {
            if (this._action != null || this._actionAsync != null)
                throw new Exception("Execute action already added!");
            
            this._actionAsync = taskAction;
            return this;
        }

        /// <summary>
        /// Add action before execute
        /// </summary>
        /// <param name="beforeExecute"></param>
        /// <returns>If return false stop the execution</returns>
        public ZeroCommandBuilder WithBeforeExecute(Func<bool> beforeExecute)
        {
            if (this._beforeExecute != null || this._beforeExecuteAsync != null)
                throw new Exception("Before Execute action already added!");
            
            this._beforeExecute = beforeExecute;
            return this;
        }
        
        /// <summary>
        /// Add action before execute
        /// </summary>
        /// <param name="beforeExecuteAsync"></param>
        /// <returns></returns>
        public ZeroCommandBuilder WithBeforeExecute(Func<Task<bool>> beforeExecuteAsync)
        {
            this._beforeExecuteAsync = beforeExecuteAsync;
            return this;
        }

        /// <summary>
        /// Create a new ZeroCommand instance
        /// </summary>
        /// <returns></returns>
        public ZeroCommand Build()
        {
            return new ZeroCommand(this._viewmodel, this._action, this._actionAsync, this._canExecute, this._onError,
                this._onErrorAsync, this._swallowException, this._trackedProperties, this._beforeExecute,this._beforeExecuteAsync);
        }
    }
}