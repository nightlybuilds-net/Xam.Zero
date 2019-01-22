using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using Xamarin.Forms.Internals;

namespace Xam.Zero
{
    public class ZeroApp
    {
        /// <summary>
        /// Initialize ZeroApp
        /// Register Pages
        /// Register ViewModels
        /// </summary>
        /// <param name="container"></param>
        public static void InitApp(IContainer container)
        {
            ZeroIoc.UseContainer(container);
            ZeroIoc.RegisterPages();
            ZeroIoc.RegisterViewmodels();
        }
        
        /// <summary>
        /// Register all types using filter function
        /// Default registration is Singleton
        /// If type has attribute Transient will be register as transient
        /// </summary>
        /// <param name="filter"></param>
        public static void RegisterMany(Func<Type, bool> filter)
        {
            var types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes());

            var models = types.Where(filter.Invoke).ToArray();

            models.ForEach(type =>
            {
                var transient = type.GetCustomAttribute<TransientAttribute>();
                var isTransient = transient != null;

                ZeroIoc.Container.Register(type, isTransient);
            });
        }
    }
}