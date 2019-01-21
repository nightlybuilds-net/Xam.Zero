using System;
using System.Linq;
using DryIoc;
using Xamarin.Forms.Internals;

namespace Xam.Zero
{
    public static class Ioc
    {
        public static readonly Container Container = new Container();

//        public static void RegisterPages()
//        {
//            var assemblies = Assembly.GetExecutingAssembly().GetReferencedAssemblies();
//            
//            var getAllPag
//            
//            Container.RegisterMany(assemblies,);
//        }

        public static void RegisterViewmodels()
        {
            var types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes());

            var models = types.Where(w => w.IsClass && !w.IsAbstract && w.IsSubclassOf(typeof(ZeroBaseModel))).ToArray();
            
            models.ForEach(f =>
            {
                // todo search for transient attribute
                Container.Register(f, Reuse.Singleton);
            });
        }
    }
}