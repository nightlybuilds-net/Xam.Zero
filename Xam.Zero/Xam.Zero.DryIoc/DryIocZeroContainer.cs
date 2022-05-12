using System;
using DryIoc;
using IContainer = Xam.Zero.Ioc.IContainer;

namespace Xam.Zero.DryIoc
{
    public class DryIocZeroContainer : IContainer
    {
        private readonly Container _dryIocContainer;

        private DryIocZeroContainer(Container dryIocContainer)
        {
            this._dryIocContainer = dryIocContainer;
        }
        
        public void Register<T>(bool transient) where T : class
        {
            this._dryIocContainer.Register<T>(transient ? Reuse.Transient : Reuse.Singleton);
        }

        public void Register<T, TImpl>(bool transient) where TImpl : T
        {
            this._dryIocContainer.Register<T, TImpl>(transient ? Reuse.Transient : Reuse.Singleton);
        }

        public void Register(Type type, bool transient)
        {
            this._dryIocContainer.Register(type,transient ? Reuse.Transient : Reuse.Singleton);

        }

        public void RegisterInstance<T>(T instance)
        {
            this._dryIocContainer.Use<T>(instance);
        }

        public T Resolve<T>()
        {
            return this._dryIocContainer.Resolve<T>();
        }

        public object Resolve(Type type)
        {
            return this._dryIocContainer.Resolve(type);
        }

        public static DryIocZeroContainer Build(Container dryIocContainer)
        {
            return new DryIocZeroContainer(dryIocContainer);
        }
        
    }
}