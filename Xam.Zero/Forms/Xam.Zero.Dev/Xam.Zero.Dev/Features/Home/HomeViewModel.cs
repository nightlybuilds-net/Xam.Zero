using System.Threading.Tasks;

namespace Xam.Zero.Dev.Features.Home
{
    public class HomeViewModel : ZeroBaseModel
    {
        private string _text;
        public string Text
        {
            get => this._text;
            set
            {
                this._text = value;
                this.RaisePropertyChanged(()=>this.Text);
            } 
        }

        public HomeViewModel()
        {
            this.Text = "Ctor";
        }


        protected override Task Init(object data)
        {
            this.Text = "init";
            return base.Init(data);
        }

        protected override Task ReverseInit(object data)
        {
            this.Text = "reverseinit";
            return base.ReverseInit(data);
        }
    }
}