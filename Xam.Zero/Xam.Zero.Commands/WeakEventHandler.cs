using System;
using System.Reflection;

namespace Xam.Zero.Commands
{
    sealed class WeakEventHandler<TEventArgs>  
    {
        private readonly WeakReference _targetReference;
        private readonly MethodInfo _method;

        public WeakEventHandler(EventHandler<TEventArgs> callback)
        {
            this._method = callback.Method;
            this._targetReference = new WeakReference(callback.Target, true);
        }

        public void Handler(object sender, TEventArgs e)
        {
            var target = this._targetReference.Target;
            if (target == null) return;
            var callback = (Action<object, TEventArgs>)Delegate.CreateDelegate(typeof(Action<object, TEventArgs>), target, this._method, true);
            callback?.Invoke(sender, e);
        }
    }
}