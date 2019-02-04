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
        
        public void Register<T>(bool transient)
        {
        }

        public void Register<T, TImpl>(bool transient) where TImpl : T
        {
        }

        public void Register(Type type, bool transient)
        {
            _tinyIoCContainer.
        }

        public void RegisterInstance<T>(T instance)
        {
            throw new NotImplementedException();
        }

        public T Resolve<T>()
        {
            throw new NotImplementedException();
        }

        public object Resolve(Type type)
        {
            throw new NotImplementedException();
        }
        
        public static TinyIocZeroContainer Build(TinyIoCContainer tinyIocContainer)
        {
            return new TinyIocZeroContainer(tinyIocContainer);
        }
    }
}