using System;
using NUnit.Framework;
using Xam.Zero.Classes;
using Xam.Zero.ViewModels;

namespace Xam.Zero.Tests
{
    [TestFixture]
    public class ZeroCommandShould
    {
        [Test]
        public void Work()
        {
            var vm = new NotifyClass();
            // var command = ZeroCommand.On(vm).WithCanExcecute(() => vm.Name == "Gino" && vm.CanPlay);
        }
    }

    class NotifyClass : ZeroBaseModel
    {
        private string _name;

        public string Name
        {
            get => this._name;
            set => this._name = value;
        }

        public string SurName { get; set; }
        public bool CanPlay { get; set; }

        public ZeroCommand ZeroCommand;

        public NotifyClass()
        {
            // this.ZeroCommand = ZeroCommand.On(this).WithCanExcecute(() => DateTime.Now.Hour > 10 || this.SurName == "Gino" && this._name == "bello");
            this.ZeroCommand = ZeroCommandBuilder.On(this).WithCanExecute(() =>string.IsNullOrEmpty(this.Name)).Build();
        }
    }
}