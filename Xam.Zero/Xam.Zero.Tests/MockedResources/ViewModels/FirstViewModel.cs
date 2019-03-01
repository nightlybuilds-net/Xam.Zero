using System.Threading.Tasks;
using Xam.Zero.ViewModels;

namespace Xam.Zero.Tests.MockedResources.ViewModels
{
    public class FirstViewModel: ZeroBaseModel
    {
        private string _firstStringProperty;
        public string FirstStringProperty
        {
            get => _firstStringProperty;
            set
            {
                if (value == _firstStringProperty)
                    return;
                _firstStringProperty = value;
                RaisePropertyChanged();
            } 
        }

        protected override Task ReversePrepareModel(object data)
        {
            var stringValue = data.ToString();
            this.FirstStringProperty = stringValue;
            return base.ReversePrepareModel(data);
        }
    }
}