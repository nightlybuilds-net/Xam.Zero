using System.Threading.Tasks;
using Xamarin.Forms;

namespace Xam.Zero.Services
{
    public interface IShellService
    {
        /// <summary>
        /// Switch main container
        /// Type T must be registered in app bootstrap
        /// </summary>
        /// <typeparam name="T"></typeparam>
        void SwitchContainer<T>() where T : Shell;

    }
}