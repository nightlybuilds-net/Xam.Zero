using System.Threading.Tasks;
using Xam.Zero.ViewModels;

namespace Xam.Zero.Tests.MockedResources.ViewModels
{
    public class MockedZeroBaseModel: ZeroBaseModel
    {
        private string _stringProperty;
        public string StringProperty
        {
            get => _stringProperty;
            set
            {
                if (value == _stringProperty)
                    return;
                _stringProperty = value;
                RaisePropertyChanged();
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