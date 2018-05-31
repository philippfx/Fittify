using System.Collections.Generic;

namespace Fittify.Api.OfmRepository.Services
{
    public class PropertyMappingValue
    {
        public IEnumerable<string> DestinationProperties { get; private set; }
        public bool Revert { get; private set; } // Sometimes sorting an ofm field and consequently and entity field results in an reverted order. For example ofmAge descending requires entityDateOfBirth to be reverted!

        public PropertyMappingValue(IEnumerable<string> destinationProperties,
            bool revert = false)
        {
            DestinationProperties = destinationProperties;
            Revert = revert;
        }
    }
}
