using System;
using System.Collections.Generic;
using System.Text;

namespace Xam.Zero
{
    public interface IXamZeroPopup 
    { 
        
    }

    public interface IXamZeroPopup<T> : IXamZeroPopup
    {
        void DismissPopup(T result);
    }
}
