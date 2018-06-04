using System.Collections.Generic;

namespace Fittify.Api.OfmRepository.Services.PropertyMapping
{
    public interface IPropertyMappingService
    {
        bool ValidMappingExistsFor<TSource, TDestination>(string fields, ref List<string> errorMessages);
        Dictionary<string, PropertyMappingValue> GetPropertyMapping<TSource, TDestination>();
    }
}
