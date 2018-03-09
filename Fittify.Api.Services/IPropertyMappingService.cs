using System.Collections.Generic;

namespace Fittify.Api.Services
{
    public interface IPropertyMappingService
    {
        bool ValidMappingExistsFor<TSource, TDestination>(string fields, ref IList<string> errorMessages);
        Dictionary<string, PropertyMappingValue> GetPropertyMapping<TSource, TDestination>();
    }
}
