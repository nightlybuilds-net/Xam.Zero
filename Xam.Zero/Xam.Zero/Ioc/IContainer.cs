using System;

namespace Xam.Zero.Ioc
{
    public interface IContainer
    {
        void Register<T>(bool transient) where T : class;
        void Register<T, TImpl>(bool transient) where TImpl : T;
        void Register(Type type, bool transient);
        
        void RegisterInstance<T>(T instance);

        T Resolve<T>();

        object Resolve(Type type);
    }
}