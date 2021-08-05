using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Xamarin.Forms.Internals;

namespace Xam.Zero.Classes
{
    public enum ContextBehaviour
    {
        /// <summary>
        /// If a key is alread present ignore registration
        /// </summary>
        Ignore = 0,
        
        /// <summary>
        /// If a key is alread present remove and add the new instance
        /// </summary>
        Override = 1,
        
        /// <summary>
        /// If a key is alread present throw exception
        /// </summary>
        Throw = 2
    }
    
    public class ZeroCommandContext 
    {
        private readonly Dictionary<string, object> _innerObjects = new Dictionary<string, object>();


        /// <summary>
        /// Add instance using Type fullname as KEY
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="behaviour"></param>
        /// <typeparam name="T"></typeparam>
        public void Add<T>(T instance,ContextBehaviour behaviour = ContextBehaviour.Override)
        {
            var key = typeof(T).FullName;
            if (key == null) throw new Exception("Key cannot be null");

            if (this.EvaluateBehaviour<T>(key, behaviour)) return;
            this._innerObjects[key] = instance;
        }
        
        
        /// <summary>
        /// Add an instance with behaviour
        /// </summary>
        /// <param name="key"></param>
        /// <param name="instance"></param>
        /// <param name="behaviour"></param>
        /// <typeparam name="T"></typeparam>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public void Add<T>(string key, T instance, ContextBehaviour behaviour = ContextBehaviour.Override)
        {
            if (key == null) throw new Exception("Key cannot be null");
            if (this.EvaluateBehaviour<T>(key, behaviour)) return;

            this._innerObjects[key] = instance;
        }

        /// <summary>
        /// Evaluate behaviour
        /// If TRUE must stop execution
        /// </summary>
        /// <param name="key"></param>
        /// <param name="behaviour"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        private bool EvaluateBehaviour<T>(string key, ContextBehaviour behaviour)
        {
            switch (behaviour)
            {
                case ContextBehaviour.Ignore:
                    if (this._innerObjects.ContainsKey(key)) return true;
                    break;
                case ContextBehaviour.Override:
                    this._innerObjects.Remove(key);
                    break;
                case ContextBehaviour.Throw:
                    // default dictionary behaviour
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(behaviour), behaviour, null);
            }

            return false;
        }

        /// <summary>
        /// Retrieve instance by key
        /// </summary>
        /// <param name="key"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T Get<T>(string key)
        {
            return (T)this._innerObjects[key];
        }
        
        /// <summary>
        /// Retrieve instance using t.gettyope.fullname as key
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T Get<T>()
        {
            var key = typeof(T).FullName;
            return (T)this._innerObjects[key ?? throw new InvalidOperationException("Key cannot be null")];
        }

        /// <summary>
        /// Remove all object
        /// If instance are disposable call dispose
        /// </summary>
        public void Clear()
        {
            this._innerObjects.Values.OfType<IDisposable>().ForEach(f=>f.Dispose());
            this._innerObjects.Clear();
        }

        /// <summary>
        /// Remove instance by key
        /// If obj is idisposable call dispose
        /// </summary>
        /// <param name="key"></param>
        public void Remove(string key)
        {
            if(!this._innerObjects.ContainsKey(key)) return;
            if(this._innerObjects[key] is IDisposable disposable)
                disposable.Dispose();

            this._innerObjects.Remove(key);
        }
    }
}