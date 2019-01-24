using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using Xam.Zero.Classes;
using Xam.Zero.Ioc;
using Xam.Zero.Services;
using Xam.Zero.Services.Impl;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using IContainer = Xam.Zero.Ioc.IContainer;

namespace Xam.Zero
{
    public class ZeroApp
    {
        internal static ZeroApp Builded { get; private set; }
        internal Application App { get; private set; }

        private ZeroApp(Application application)
        {
            this.App = application;
        }

        private IContainer _container;

        public static ZeroApp On(Application app)
        {
            return new ZeroApp(app);
        }

        /// <summary>
        /// </summary>
        /// <param name="container"></param>
        public ZeroApp WithContainer(IContainer container)
        {
            this._container = container;
            return this;
        }

        internal readonly Dictionary<Type,Lazy<Shell>> Shells = new Dictionary<Type, Lazy<Shell>>();

        
        public ZeroApp RegisterShell<T>(Func<T> shell) where T : Shell
        {
            this.Shells.Add(typeof(T), new Lazy<Shell>(shell));
            return this;
        }


        
        /// <summary>
        /// Initialize ZeroApp
        /// Register Pages
        /// Register ViewModels
        /// </summary>
        public void Start<T>() where T : Shell
        {
            ZeroIoc.UseContainer(this._container);
            ZeroIoc.RegisterPages();
            ZeroIoc.RegisterViewModels();

            // todo init shellnavigation
            this._container.Register<IShellService,ShellService>(true);
            this._container.RegisterInstance<IMessagingCenter>(MessagingCenter.Instance); // register messaging center for injection

            Builded = this; // set builded

            Builded.App.MainPage = this.Shells.Single(s => s.Key == typeof(T)).Value.Value;

//            (Builded.App.MainPage as Shell)
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