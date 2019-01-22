using System;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace Xam.Zero
{
    public static class Ioc
    {
        public static IContainer Container { get; private set; }

        /// <summary>
        /// Register all content page
        /// </summary>
        public static void RegisterPages()
        {
            Register(type => type.IsClass && !type.IsAbstract && type.IsSubclassOf(typeof(ContentPage)));
        }

        /// <summary>
        /// Register all viewmodel that extend ZeroBaseModel
        /// </summary>
        public static void RegisterViewmodels()
        {
            Register(type => type.IsClass && !type.IsAbstract && type.IsSubclassOf(typeof(ZeroBaseModel)));
        }
        
        /// <summary>
        /// Register all types using filter function
        /// Default registration is Singleton
        /// If type has attribute Transient will be register as transient
        /// </summary>
        /// <param name="filter"></param>
        private static void Register(Func<Type, bool> filter)
        {
            var types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes());

            var models = types.Where(filter.Invoke).ToArray();

            models.ForEach(type =>
            {
                var transient = type.GetCustomAttribute<TransientAttribute>();
                var isTransient = transient != null;

                Container.Register(type, isTransient);
            });
        }

        internal static void UseContainer(IContainer container)
        {
            Container = container;
        }
    }
}