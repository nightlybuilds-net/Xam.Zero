using System.Reflection;
using Xam.Zero.ViewModels;

namespace Xam.Zero.Utility
{
    internal class Utility
    {
        /// <summary>
        /// Invoke PrepareModel by reflection on passed model
        /// </summary>
        /// <param name="model"></param>
        /// <param name="data"></param>
        internal static void InvokeReflectionPrepareModel(ZeroBaseModel model, object data)
        {
            // call init method
            var dynMethod = typeof(ZeroBaseModel).GetMethod("PrepareModel", 
                BindingFlags.NonPublic | BindingFlags.Instance);
            if (dynMethod != null) dynMethod.Invoke(model, new object[] {data});
        }
    }
}