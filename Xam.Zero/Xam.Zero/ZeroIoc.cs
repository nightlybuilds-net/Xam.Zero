using System;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace Xam.Zero
{
    internal static class ZeroIoc
    {
        public static IContainer Container { get; private set; }

        /// <summary>
        /// Register all content page
        /// </summary>
        public static void RegisterPages()
        {
            ZeroApp.RegisterMany(type => type.IsClass && !type.IsAbstract && type.IsSubclassOf(typeof(ContentPage)));
        }

        /// <summary>
        /// Register all viewmodel that extend ZeroBaseModel
        /// </summary>
        public static void RegisterViewmodels()
        {
            ZeroApp.RegisterMany(type => type.IsClass && !type.IsAbstract && type.IsSubclassOf(typeof(ZeroBaseModel)));
        }
        
        

        internal static void UseContainer(IContainer container)
        {
            Container = container;
        }
    }
}