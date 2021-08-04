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
        private Func<bool> _canExecute;
        private IEnumerable<string> _trackedProperties;
        private Action _action;
        private Func<Task> _asyncAction;
        private bool _swallowException;
        private Action<Exception> _onError;
        private Func<Exception,Task> _onErrorAsync;

        private ZeroCommand(INotifyPropertyChanged viewmodel)
        {
            this._viewmodel = viewmodel;
            viewmodel.PropertyChanged +=new WeakEventHandler<PropertyChangedEventArgs>(this.InnerEvaluateCanExcecute).Handler;
        }

        private void InnerEvaluateCanExcecute(object sender, PropertyChangedEventArgs e)
        {
            if (this._trackedProperties == null || !this._trackedProperties.Any()) return;
            if(this._trackedProperties.Contains(e.PropertyName))
                this.CanExecuteChanged?.Invoke(this,EventArgs.Empty);
        }

        public static ZeroCommand On(INotifyPropertyChanged viewmodel)
        {
            return new ZeroCommand(viewmodel);
        }

        public ZeroCommand WithCanExecute(Expression<Func<bool>> canExcecuteExpression)
        {
            this._canExecute = canExcecuteExpression.Compile();
            this._trackedProperties = this.GetTrackProperties(canExcecuteExpression.Body, this._viewmodel.GetType());
            return this;
        }

        /// <summary>
        /// Do not throw exception on execute
        /// </summary>
        /// <returns></returns>
        public ZeroCommand WithSwallowException()
        {
            this._swallowException = true;
            return this;
        }

        /// <summary>
        /// Catch execution error
        /// </summary>
        /// <param name="onError"></param>
        /// <returns></returns>
        public ZeroCommand WithErrorHandler(Action<Exception> onError)
        {
            this._onError = onError;
            return this;
        }
        
        /// <summary>
        /// Catch execution error
        /// </summary>
        /// <param name="onErrorTask"></param>
        /// <returns></returns>
        public ZeroCommand WithErrorHandler(Func<Exception,Task> onErrorTask)
        {
            this._onErrorAsync = onErrorTask;
            return this;
        }
        
        /// <summary>
        /// Add Execute for Action
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public ZeroCommand WithExecute(Action action)
        {
            this._action = action;
            return this;
        }
        
        /// <summary>
        /// Add Execute for Task
        /// </summary>
        /// <param name="taskAction"></param>
        /// <returns></returns>
        public ZeroCommand WithExecute(Func<Task> taskAction)
        {
            this._asyncAction = taskAction;
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
                    if(memberExpression.Member.DeclaringType == trackedType)
                        allProperties.Add(memberExpression.Member.Name);
                    break;
                }
                case BinaryExpression binaryExpression:
                    allProperties.AddRange(this.GetTrackProperties(binaryExpression.Left,trackedType));
                    allProperties.AddRange(this.GetTrackProperties(binaryExpression.Right,trackedType));
                    break;
                case MethodCallExpression methodCallExpression:
                    foreach (var expression in methodCallExpression.Arguments)
                    {
                        allProperties.AddRange(this.GetTrackProperties(expression,trackedType));
                    }

                    break;
                case UnaryExpression unaryExpression:
                    allProperties.AddRange(this.GetTrackProperties(unaryExpression.Operand,trackedType));
                    break;
            }

            return allProperties.Distinct();
        }

        /// <summary>
        /// Force evaluation of Can Execute
        /// </summary>
        public void RaiseEvaluateCanExecute()
        {
            this.CanExecuteChanged?.Invoke(this,EventArgs.Empty);
        }
        
        public bool CanExecute(object parameter)
        {
            return this._canExecute?.Invoke() ?? true;
        }

        public async void Execute(object parameter)
        {
            try
            {
                if(this._asyncAction != null)
                    await this._asyncAction?.Invoke();
                
                this._action?.Invoke();
            }
            catch (Exception e)
            {
                if(this._onErrorAsync != null)
                    await this._onErrorAsync?.Invoke(e);
                
                this._onError?.Invoke(e);
                
                if(!this._swallowException)
                    throw;
            }
        }

        public event EventHandler CanExecuteChanged;
    }


}