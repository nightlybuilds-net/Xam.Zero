using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xam.Zero.ViewModels;
using Xam.Zero.ZCommand;

namespace Xam.Zero.SimpleTabbedApp.Popups
{
    public class ToolkitSetValueViewModel : ZeroPopupBaseModel<string>
    {
        public ICommand CloseCommand { get; private set; }
        public ICommand SetValueCommand { get; private set; }

        public ToolkitSetValueViewModel()
        {
            CloseCommand = ZeroCommand.On(this)
                .WithAutoInvalidateWhenExecuting()
                .WithExecute((obj, context) =>
                {
                    this.Value = null;
                    return DismissPopup();
                })
                .Build();

            SetValueCommand = ZeroCommand.On(this)
                .WithAutoInvalidateWhenExecuting()
                .WithExecute((obj, context) => DismissPopup())
                .Build();
        }

        protected override void PrepareModel(object data)
        {
            base.PrepareModel(data);
            Console.WriteLine($"Inside {GetType().Name} PrepareModel");
        }
    }
}
