using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xam.Zero.SimpleTabbedApp.Popups.Toolkit;
using Xam.Zero.ViewModels;
using Xam.Zero.ZCommand;

namespace Xam.Zero.SimpleTabbedApp.ViewModels
{
    public class PopupsViewModel : ZeroBaseModel
    {
        public ICommand OpenToolkitAlertCommand { get; private set; }

        public PopupsViewModel()
        {
            this.OpenToolkitAlertCommand = ZeroCommand.On(this)
                .WithAutoInvalidateWhenExecuting()
                .WithExecute((obj,context) => this.OpenPopup())
                .Build();
        }

        private async Task OpenPopup()
        {
            try
            {
                await this.ShowPopup<ToolkitAlertPopup>();
            }
            catch (Exception ex)
            {
                ;
            }
        }
    }
}
