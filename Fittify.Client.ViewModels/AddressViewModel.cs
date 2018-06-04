using System.Diagnostics.CodeAnalysis;

namespace Fittify.Client.ViewModels
{
    [ExcludeFromCodeCoverage] //// Was just playing around with authorization policies
    public class AddressViewModel
    {
        public string Address { get; private set; }

        public AddressViewModel(string address)
        {
            Address = address;
        }
    }
}
