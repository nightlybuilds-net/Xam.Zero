using System;
using System.Linq;
using Xamarin.Forms;

namespace Xam.Zero.Services.Impl
{
    class ShellService : IShellService
    {

        public void SwitchContainer<T>() where T : Shell
        {
            var container = ZeroApp.Builded.Shells.Single(s => s.Key == typeof(T)).Value.Value;
            ZeroApp.Builded.App.MainPage = container;
        }
    }
}