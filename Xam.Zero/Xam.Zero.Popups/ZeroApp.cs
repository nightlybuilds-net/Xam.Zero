using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Xam.Zero.Classes;
using Xam.Zero.Ioc;
using Xam.Zero.Popups.Ioc;
using Xam.Zero.Popups.Services;
using Xam.Zero.Services;

namespace Xam.Zero.Popups
{
    public static class ZeroAppExtensions
    {
        static bool _popupAreTransient = false;

        public static ZeroApp WithPopupsContainer(this ZeroApp zeroApp, IContainer container)
        {
            ZeroIoc.Container = container;
            return zeroApp;
        }

        public static ZeroApp WithTransientPopups(this ZeroApp zeroApp)
        {
            _popupAreTransient = true;
            return zeroApp;
        }

        public static void StartWithWithPopups(this ZeroApp zeroApp, Func<IContainer, Type> shellSelector)
        {
            zeroApp.StartWith(shellSelector);
            InnerBootStrap();
        }

        private static void InnerBootStrap()
        {
            ZeroIoc.RegisterPopups(_popupAreTransient);
            ZeroIoc.RegisterPopupViewModels(true);
            ZeroIoc.Container.Register<IPopupResolver, PopupResolver>(true);
        }

        internal static void RegisterMany(Func<Type, bool> filter, bool isDefaultTransient)
        {
            var types = AppDomain.CurrentDomain.GetAssemblies()
                .Where(w => !w.FullName.StartsWith("JetBrains")) // kludge fix error in rider
                .SelectMany(s => s.GetTypes())
                .ToArray();

            var models = types.Where(filter.Invoke).ToArray();

            foreach(var type in models)
            {
                var transient = type.GetCustomAttribute<TransientAttribute>();
                var isTransient = isDefaultTransient || transient != null;

                ZeroIoc.Container.Register(type, isTransient);
            }
        }
    }
}
