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
        private readonly Action _action;
        private readonly Func<Task> _asyncAction;
        private readonly bool _swallowException;
        private readonly Action<Exception> _onError;
        private readonly Func<Exception, Task> _onErrorAsync;

        internal ZeroCommand(INotifyPropertyChanged viewmodel, Action action, Func<Task> asyncAction,
            Func<bool> canExecute, Action<Exception> onError, Func<Exception, Task> onErrorAsync, bool swallowException,
            IEnumerable<string> trackedProperties)
        {
            this._viewmodel = viewmodel;
            this._action = action;
            this._asyncAction = asyncAction;
            this._canExecute = canExecute;
            this._onError = onError;
            this._onErrorAsync = onErrorAsync;
            this._swallowException = swallowException;
            this._trackedProperties = trackedProperties;
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
                if (this._asyncAction != null)
                    await this._asyncAction?.Invoke();

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

        public event EventHandler CanExecuteChanged;
    }

    public class ZeroCommandBuilder
    {
        private readonly INotifyPropertyChanged _viewmodel;
        private Func<bool> _canExecute;
        private IEnumerable<string> _trackedProperties;
        private bool _swallowException;
        private Action<Exception> _onError;
        private Func<Exception, Task> _onErrorAsync;
        private Action _action;
        private Func<Task> _asyncAction;

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
            this._asyncAction = taskAction;
            return this;
        }

        /// <summary>
        /// Create a new ZeroCommand instance
        /// </summary>
        /// <returns></returns>
        public ZeroCommand Build()
        {
            return new ZeroCommand(this._viewmodel, this._action, this._asyncAction, this._canExecute, this._onError,
                this._onErrorAsync, this._swallowException, this._trackedProperties);
        }
    }
}