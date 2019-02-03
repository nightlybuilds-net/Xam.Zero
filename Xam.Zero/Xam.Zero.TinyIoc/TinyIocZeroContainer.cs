using System;
using IContainer = Xam.Zero.Ioc.IContainer;

namespace Xam.Zero.TinyIoc
{
    public class TinyIocZeroContainer: IContainer
    {
        public TinyIocZeroContainer()
        {
        }
        
        public void Register<T>(bool transient)
        {
            throw new NotImplementedException();
        }

        public void Register<T, TImpl>(bool transient) where TImpl : T
        {
            throw new NotImplementedException();
        }

        public void Register(Type type, bool transient)
        {
            throw new NotImplementedException();
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
    }
}