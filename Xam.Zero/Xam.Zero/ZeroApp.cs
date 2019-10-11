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
        /// <summary>
        /// Builded Instance of ZeroApp
        /// </summary>
        internal static ZeroApp Builded { get; private set; }

        internal Application App { get; private set; }

        internal readonly Dictionary<Type, Lazy<Shell>> Shells = new Dictionary<Type, Lazy<Shell>>();

        private IContainer _container;

        private ZeroApp(Application application)
        {
            this.App = application;
        }

        private ZeroApp()
        {
            
        }


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


        public ZeroApp RegisterShell<T>(Func<T> shell) where T : Shell
        {
            this.Shells.Add(typeof(T), new Lazy<Shell>(shell));
            return this;
        }


        /// <summary>
        /// Start app when only one shell is registered
        /// Initialize ZeroApp
        /// Register Pages
        /// Register ViewModels
        /// </summary>
        public void Start()
        {
            if (this.Shells.Count > 1)
                throw new Exception("Mutiple shells registered, use StartWith<T>!");

            this.InnerBootStrap();
            Builded.App.MainPage = this.Shells.Single().Value.Value;
        }


        /// <summary>
        /// Initialize ZeroApp
        /// Register Pages
        /// Register ViewModels
        /// </summary>
        public void StartWith<T>() where T : Shell
        {
            this.InnerBootStrap();
            Builded.App.MainPage = this.Shells.Single(s => s.Key == typeof(T)).Value.Value;
        }

        /// <summary>
        /// Start with a shell using shellselector func
        /// </summary>
        /// <param name="shellSelector"></param>
        public void StartWith(Func<IContainer, Type> shellSelector)
        {
            this.InnerBootStrap();
            Builded.App.MainPage = this.Shells.Single(s => s.Key == shellSelector.Invoke(this._container)).Value.Value;
        }
        

        /// <summary>
        /// Bootstrap application
        /// Register services, pages, models
        /// </summary>
        private void InnerBootStrap()
        {
            ZeroIoc.UseContainer(this._container);
            ZeroIoc.RegisterPages();
            ZeroIoc.RegisterViewModels();

            this._container.Register<IShellService, ShellService>(true);
            this._container.RegisterInstance<IMessagingCenter>(MessagingCenter
                .Instance); // register messaging center for injection

            Builded = this; // set builded
        }

        /// <summary>
        /// Register all types using filter function
        /// Default registration is Singleton
        /// If type has attribute Transient will be register as transient
        /// </summary>
        /// <param name="filter"></param>
        internal static void RegisterMany(Func<Type, bool> filter)
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