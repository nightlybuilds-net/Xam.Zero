using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xam.Zero.ViewModels;
using Xam.Zero.ZCommand;

namespace Xam.Zero.SimpleTabbedApp.Popups.Toolkit
{
    public class ToolkitAlertViewModel : ZeroPopupBaseModel
    {
        public ICommand CloseCommand { get; private set; }

        public ToolkitAlertViewModel()
        {
            this.CloseCommand = ZeroCommand.On(this)
                .WithAutoInvalidateWhenExecuting()
                .WithExecute((obj, context) => this.DismissPopup())
                .Build();
        }
    }
}
