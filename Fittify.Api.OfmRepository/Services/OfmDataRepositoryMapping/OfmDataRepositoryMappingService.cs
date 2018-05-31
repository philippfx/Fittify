using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fittify.Api.OfmRepository.OfmRepository.Sport;
using Fittify.Api.OfmRepository.Services.OfmDataRepositoryMapping;
using Fittify.Common.CustomExceptions;
using Fittify.DataModelRepository.Repository.Sport;

namespace Fittify.Api.OfmRepository.Services
{
    public class OfmDataRepositoryMappingService
    {
        private readonly IList<IOfmDataRepositoryMapping> _ofmDataRepositoryMappings = new List<IOfmDataRepositoryMapping>();

        public OfmDataRepositoryMappingService()
        {
            // Todo this must be refactored to TOfmForGet and Entity!
            _ofmDataRepositoryMappings.Add(new OfmDataRepositoryMapping<WorkoutOfmRepository, WorkoutRepository>());

        }
        public Dictionary<string, PropertyMappingValue> GetPropertyMapping
            <TSource, TDestination>()
        {
            // get matching mapping
            var matchingMapping = _ofmDataRepositoryMappings.OfType<PropertyMapping<TSource, TDestination>>();

            if (matchingMapping.Count() == 1)
            {
                return matchingMapping.First()._mappingDictionary;
            }

            throw new PropertyMappingNotFoundException($"Cannot find exact property mapping instance for <{typeof(TSource)},{typeof(TDestination)}");
        }
    }
}
