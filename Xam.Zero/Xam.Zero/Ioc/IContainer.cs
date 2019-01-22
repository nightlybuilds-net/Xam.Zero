using System;

namespace Xam.Zero.Ioc
{
    public interface IContainer
    {
        void Register<T>(bool transient);
        void Register<T, TImpl>(bool transient) where TImpl : T;
        void Register(Type type, bool transient);

        T Resolve<T>();

        object Resolve(Type type);
    }
}