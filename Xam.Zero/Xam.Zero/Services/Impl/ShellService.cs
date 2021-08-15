using System;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Xam.Zero.Services.Impl
{
    class ShellService : IShellService
    {
        public void SwitchContainer<T>() where T : Shell
        {
            var container = ZeroApp.Builded.Shells.SingleOrDefault(s => s.Key == typeof(T)).Value();

            if (container == null)
                throw new Exception(
                    $"Cannot switch to shell of type: {typeof(T)}. Have you register using ZeroApp.RegisterShell<T>??");

            ZeroApp.Builded.App.MainPage = container;
        }
    }
}