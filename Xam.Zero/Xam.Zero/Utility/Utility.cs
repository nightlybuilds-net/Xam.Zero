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

        /// <summary>
        /// Invoke ReversePrepareModel by reflection on passed model
        /// </summary>
        /// <param name="model"></param>
        /// <param name="data"></param>
        internal static void InvokeReflectionReversePrepareModel(ZeroBaseModel model, object data)
        {
            // call init method
            var dynMethod = typeof(ZeroBaseModel).GetMethod("ReversePrepareModel",
                BindingFlags.NonPublic | BindingFlags.Instance);
            if (dynMethod != null) dynMethod.Invoke(model, new object[] { data });
        }

        /// <summary>
        /// Invoke PrepareModel by reflection on passed IZeroPopupBaseModel
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model"></param>
        /// <param name="data"></param>
        internal static void InvokeReflectionPrepareModel<T>(T model, object data) where T : IZeroPopupBaseModel
        {
            // call init method
            var dynMethod = typeof(T).GetMethod("PrepareModel",
                BindingFlags.NonPublic | BindingFlags.Instance);
            if (dynMethod != null) dynMethod.Invoke(model, new object[] { data });
        }
    }
}