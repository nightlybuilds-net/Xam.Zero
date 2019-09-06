using System.Threading.Tasks;
using Xam.Zero.ViewModels;

namespace Xam.Zero.Tests.MockedResources.ViewModels
{
    public class SecondViewModel: ZeroBaseModel
    {
        private string _secondStringProperty;
        public string SecondStringProperty
        {
            get => this._secondStringProperty;
            set
            {
                if (value == this._secondStringProperty)
                    return;
                this._secondStringProperty = value;
                this.RaisePropertyChanged();
            } 
        }

        protected override void PrepareModel(object data)
        {
            if (data != null)
            {
                var stringValue = data.ToString();
                this.SecondStringProperty = stringValue;
            }

            base.PrepareModel(data);
        }

    }
}