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
            _tinyIoCContainer = tinyIoCContainer;
        }
        
        public void Register<T>(bool transient) where T : class
        {
            if (transient)
            {
                _tinyIoCContainer.Register<T>().AsMultiInstance();
                return;
            }

            _tinyIoCContainer.Register<T>().AsSingleton();
        }

        public void Register<T, TImpl>(bool transient) where TImpl : T
        {
            if (transient)
            {
                _tinyIoCContainer.Register(typeof(T), typeof(TImpl)).AsMultiInstance();
                return;
            }

            _tinyIoCContainer.Register(typeof(T), typeof(TImpl)).AsSingleton();
        }

        public void Register(Type type, bool transient)
        {
            if (transient)
            {
                _tinyIoCContainer.Register(type).AsMultiInstance();
                return;
            }

            _tinyIoCContainer.Register(type).AsSingleton();
        }

        public void RegisterInstance<T>(T instance)
        {
            _tinyIoCContainer.Register(instance.GetType(), instance);
        }

        public T Resolve<T>()
        {
            var type = (T)_tinyIoCContainer.Resolve(typeof(T));
            return type;
        }

        public object Resolve(Type type)
        {
            return _tinyIoCContainer.Resolve(type);
        }
        
        public static TinyIocZeroContainer Build()
        {
            return new TinyIocZeroContainer(TinyIoCContainer.Current);
        }
    }
}