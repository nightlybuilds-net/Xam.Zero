using System;
using TinyIoC;
using IContainer = Xam.Zero.Ioc.IContainer;

namespace Xam.Zero.TinyIoc
{
    public class TinyIocZeroContainer: IContainer
    {
        private readonly TinyIoCContainer _tinyIoCContainer;

        public TinyIocZeroContainer(TinyIoCContainer tinyIoCContainer)
        {
            this._tinyIoCContainer = tinyIoCContainer;
        }
        
        public void Register<T>(bool transient) where T : class
        {
            if (transient)
            {
                this._tinyIoCContainer.Register<T>().AsMultiInstance();
                return;
            }

            this._tinyIoCContainer.Register<T>().AsSingleton();
        }

        public void Register<T, TImpl>(bool transient) where TImpl : T
        {
            if (transient)
            {
                this._tinyIoCContainer.Register(typeof(T), typeof(TImpl)).AsMultiInstance();
                return;
            }

            this._tinyIoCContainer.Register(typeof(T), typeof(TImpl)).AsSingleton();
        }

        public void Register(Type type, bool transient)
        {
            if (transient)
            {
                this._tinyIoCContainer.Register(type).AsMultiInstance();
                return;
            }

            this._tinyIoCContainer.Register(type).AsSingleton();
        }

        public void RegisterInstance<T>(T instance)
        {
            this._tinyIoCContainer.Register(instance.GetType(), instance);
        }

        public T Resolve<T>()
        {
            var type = (T) this._tinyIoCContainer.Resolve(typeof(T));
            return type;
        }

        public object Resolve(Type type)
        {
            return this._tinyIoCContainer.Resolve(type);
        }
        
        public static TinyIocZeroContainer Build(TinyIoCContainer container)
        {
            return new TinyIocZeroContainer(container);
        }
    }
}