using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xam.Zero.ViewModels;
using Xam.Zero.ZCommand;

namespace Xam.Zero.SimpleTabbedApp.Popups
{
    public class ToolkitAlertViewModel : ZeroPopupBaseModel
    {
        public ICommand CloseCommand { get; private set; }

        public ToolkitAlertViewModel()
        {
            this.CloseCommand = ZeroCommand.On(this)
                .WithAutoInvalidateWhenExecuting()
                .WithExecute((obj, context) => DismissPopup("ToolkitAlertViewModel: Use close command"))
                .Build();
        }

        protected override void PrepareModel(object data)
        {
            base.PrepareModel(data);
            Console.WriteLine($"Inside {GetType().Name} PrepareModel");
        }
    }
}
