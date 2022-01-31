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
        public ICommand OpenToolkitSetValueCommand { get; private set; }

        public PopupsViewModel()
        {
            this.OpenToolkitAlertCommand = ZeroCommand.On(this)
                .WithAutoInvalidateWhenExecuting()
                .WithErrorHandler(ex => this.RunWithErrorHandler(ex))
                .WithExecute((obj,context) => App.UseRgPluginPopups 
                    ? this.ShowPopup<RgAlertPopup>() 
                    : this.ShowPopup<ToolkitAlertPopup>())
                .Build();

            this.OpenToolkitSetValueCommand = ZeroCommand.On(this)
                .WithAutoInvalidateWhenExecuting()
                .WithErrorHandler(ex => this.RunWithErrorHandler(ex))
                .WithExecute(async (obj, context) =>
                {
                    var result = App.UseRgPluginPopups
                        ? await this.ShowPopup<RgSetValuePopup, string>()
                        : await this.ShowPopup<ToolkitSetValuePopup, string>();
                    await this.DisplayAlert("Info", $"The value returned by popup is '{result}'", "Cancel");
                })
                .Build();
        }

        protected override void ReversePrepareModel(object data)
        {
            base.ReversePrepareModel(data);
            Console.WriteLine(data?.ToString() ?? "no data");
        }

        private Task RunWithErrorHandler(Exception ex)
        {
#if DEBUG
            Console.WriteLine(ex.Message);
#endif
            return Task.CompletedTask;
        }
    }
}
