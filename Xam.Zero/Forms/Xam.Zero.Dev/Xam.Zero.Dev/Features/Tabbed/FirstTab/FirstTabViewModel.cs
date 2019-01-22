using Xam.Zero.ViewModels;

namespace Xam.Zero.Dev.Features.Tabbed.FirstTab
{
    public class FirstTabViewModel : ZeroBaseModel
    {
        public string Prova { get; set; }


        public FirstTabViewModel()
        {
            this.Prova = "Tab 1";
        }
    }
}