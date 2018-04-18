using System;
using System.Collections.Generic;
using System.Text;

namespace Fittify.Web.ViewModels
{
    public class AddressViewModel
    {
        public string Address { get; private set; } = string.Empty;

        public AddressViewModel(string address)
        {
            Address = address;
        }
    }
}
