using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Xam.Zero.Classes;
using Xam.Zero.Dev.Features.Second;
using Xam.Zero.Services;
using Xam.Zero.ViewModels;
using Xamarin.Forms;

namespace Xam.Zero.Dev.Features.Home
{
    public class HomeViewModel : ZeroBaseModel
    {
        private readonly IShellService _shellService;
        private string _text;
        private string _promptText;

        public string Text
        {
            get => this._text;
            set
            {
                this._text = value;
                this.RaisePropertyChanged(()=>this.Text);
            } 
        }
        
        public string PromptText
        {
            get => this._promptText;
            set
            {
                this._promptText = value;
                this.RaisePropertyChanged(()=>this._promptText);
            } 
        }

        public ICommand NavigateCommand { get; set; }
        public ICommand NavigatToTestCommandPage { get; set; }
        public ICommand GoToTabbedCommand { get; set; }
        public ICommand AlertCommand { get; set; }
        public ICommand PromptCommand { get; set; }
        
        
        
        

        public HomeViewModel(IShellService shellService)
        {
            this._shellService = shellService;
            this.Text = "Ctor";
            // this.NavigateCommand = new Command(async ()=> await this.Push<SecondPage>(null));
            this.NavigateCommand = new Command(async ()=> await this.Push(typeof(SecondPage)));
            this.GoToTabbedCommand = new Command(async () =>
            {
                this._shellService.SwitchContainer<TabbedShell>();
            });
            this.NavigatToTestCommandPage = new Command(async () => await this.Push<CommandPage.CommandPage>());
            this.AlertCommand = new Command(() => { this.DisplayAlert("Prova", "Dai che fungi", "ok"); });
            this.PromptCommand = new Command(async () => { this._promptText = await this.DisplayPrompt("Prova", "Scrivi una mail", accept: "ok", keyboard: Keyboard.Email); });
            
        }


        protected override void PrepareModel(object data)
        {
            
            this.Text = "init";
            base.PrepareModel(data);
        }

        protected override void ReversePrepareModel(object data)
        {
            this.Text = "reverseinit";
            base.ReversePrepareModel(data);
        }
      
    }
}