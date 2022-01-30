using Rg.Plugins.Popup.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xam.Zero.Popups;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Xam.Zero.SimpleTabbedApp.Popups.Toolkit
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RgSetValuePopup : PopupPage, IXamZeroPopup<string>
    {
        public RgSetValuePopup()
        {
            InitializeComponent();
        }
    }
}