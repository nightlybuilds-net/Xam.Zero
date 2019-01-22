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
            Ioc.UseContainer(container);
            Ioc.RegisterPages();
            Ioc.RegisterViewmodels();
        }
    }
}