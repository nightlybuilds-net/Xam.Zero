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

        internal readonly Dictionary<Type, Func<Shell>> Shells = new Dictionary<Type, Func<Shell>>();

        private IContainer _container;
        private bool _viewmodelsAreTransient;
        private bool _pagesAreTransient;

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
            this.Shells.Add(typeof(T), shell);
            return this;
        }

        /// <summary>
        /// Setup default register behaviour as transient
        /// </summary>
        /// <returns></returns>
        public ZeroApp WithTransientViewModels()
        {
            this._viewmodelsAreTransient = true;
            return this;
        }

        /// <summary>
        /// Setup default register behaviour as transient
        /// </summary>
        /// <returns></returns>
        public ZeroApp WithTransientPages()
        {
            this._pagesAreTransient = true;
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
            Builded.App.MainPage = this.Shells.Single().Value();
        }


        /// <summary>
        /// Initialize ZeroApp
        /// Register Pages
        /// Register ViewModels
        /// </summary>
        public void StartWith<T>() where T : Shell
        {
            this.InnerBootStrap();
            Builded.App.MainPage = this.Shells.Single(s => s.Key == typeof(T)).Value();
        }

        /// <summary>
        /// Start with a shell using shellselector func
        /// </summary>
        /// <param name="shellSelector"></param>
        public void StartWith(Func<IContainer, Type> shellSelector)
        {
            this.InnerBootStrap();
            var shellType = shellSelector.Invoke(this._container);
            Builded.App.MainPage = this.Shells.Single(s => s.Key == shellType).Value();
        }


        /// <summary>
        /// Bootstrap application
        /// Register services, pages, models
        /// </summary>
        private void InnerBootStrap()
        {
            ZeroIoc.UseContainer(this._container);
            ZeroIoc.RegisterPages(this._pagesAreTransient);
            ZeroIoc.RegisterViewModels(this._viewmodelsAreTransient);

            this._container.Register<IShellService, ShellService>(true);
            this._container.Register<IPageResolver, PageResolver>(true);

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
        /// <param name="isDefaultTransient">if true force all registration to transient. if is false look for transient attribute</param>
        internal static void RegisterMany(Func<Type, bool> filter, bool isDefaultTransient)
        {
            var types = AppDomain.CurrentDomain.GetAssemblies()
                .Where(w => !w.FullName.StartsWith("JetBrains")) // kludge fix error in rider
                .SelectMany(s => s.GetTypes());

            var models = types.Where(filter.Invoke).ToArray();

            models.ForEach(type =>
            {
                var transient = type.GetCustomAttribute<TransientAttribute>();
                var isTransient = isDefaultTransient || transient != null;

                ZeroIoc.Container.Register(type, isTransient);
            });
        }
    }
}