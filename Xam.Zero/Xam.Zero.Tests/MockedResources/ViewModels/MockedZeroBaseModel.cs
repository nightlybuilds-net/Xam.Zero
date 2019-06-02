using System.Threading.Tasks;
using Xam.Zero.ViewModels;

namespace Xam.Zero.Tests.MockedResources.ViewModels
{
    public class MockedZeroBaseModel: ZeroBaseModel
    {
        private string _stringProperty;
        public string StringProperty
        {
            get => this._stringProperty;
            set
            {
                if (value == this._stringProperty)
                    return;
                this._stringProperty = value;
                this.RaisePropertyChanged();
            } 
        }

        protected override Task PrepareModel(object data)
        {
            var stringValue = data.ToString();
            this.StringProperty = stringValue;
            return base.PrepareModel(data);
        }

        protected override Task ReversePrepareModel(object data)
        {
            var stringValue = data.ToString();
            this.StringProperty = stringValue;
            return base.ReversePrepareModel(data);
        }
    }
}