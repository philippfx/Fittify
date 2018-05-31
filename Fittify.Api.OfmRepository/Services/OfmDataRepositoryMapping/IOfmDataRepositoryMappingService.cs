using System;
using System.Collections.Generic;

namespace Fittify.Api.OfmRepository.Services.OfmDataRepositoryMapping
{
    public interface IOfmDataRepositoryMappingService
    {
        Dictionary<Type, Type> GetPropertyMapping<TSource, TDestination>();
        Type GetDestination(Type source);
    }
}
