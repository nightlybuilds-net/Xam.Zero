using System.Threading.Tasks;
using Xam.Zero.ViewModels;

namespace Xam.Zero.Tests.MockedResources.ViewModels
{
    public class FirstViewModel: ZeroBaseModel
    {
        private string _firstStringProperty;
        public string FirstStringProperty
        {
            get => this._firstStringProperty;
            set
            {
                if (value == this._firstStringProperty)
                    return;
                this._firstStringProperty = value;
                this.RaisePropertyChanged();
            } 
        }

        protected override void ReversePrepareModel(object data)
        {
            var stringValue = data.ToString();
            this.FirstStringProperty = stringValue;
            base.ReversePrepareModel(data);
        }
    }
}