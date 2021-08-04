using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Windows.Input;

namespace Xam.Zero.Classes
{
    public class ZeroCommand : ICommand
    {
        private readonly INotifyPropertyChanged _viewmodel;
        private Func<bool> _canExecute;
        private IEnumerable<string> _trackedProperties;
        private Action _action;

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

        public ZeroCommand WithCanExcecute(Expression<Func<bool>> canExcecuteExpression)
        {
            this._canExecute = canExcecuteExpression.Compile();
            this._trackedProperties = this.GetTrackProperties(canExcecuteExpression.Body, this._viewmodel.GetType());
            return this;
        }
        
        public ZeroCommand WithExcecute(Action action)
        {
            this._action = action;
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

            return allProperties;
        }
        
        
        public bool CanExecute(object parameter)
        {
            return this._canExecute?.Invoke() ?? true;
        }

        public void Execute(object parameter)
        {
            this._action?.Invoke();
        }

        public event EventHandler CanExecuteChanged;
    }

}