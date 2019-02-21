using System.Threading.Tasks;
using Xam.Zero.ViewModels;

namespace Xam.Zero.Tests.MockedResources.ViewModels
{
    public class SecondViewModel: ZeroBaseModel
    {
        private string _secondStringProperty;
        public string SecondStringProperty
        {
            get => _secondStringProperty;
            set
            {
                if (value == _secondStringProperty)
                    return;
                _secondStringProperty = value;
                RaisePropertyChanged();
            } 
        }

        protected override Task PrepareModel(object data)
        {
            if (data != null)
            {
                var stringValue = data.ToString();
                this.SecondStringProperty = stringValue;
            }

            return base.PrepareModel(data);
        }

    }
}