using System;
using System.Collections.Generic;
using System.Text;
using Xam.Zero.Popups;

namespace Xam.Zero.Services
{
    public interface IPopupResolver
    {
        P ResolvePopup<P, T>(object data = null) where P : IXamZeroPopup<T>;
    }
}
