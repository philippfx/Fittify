using System;
using System.Collections.Generic;
using System.Linq;
using Fittify.Api.OuterFacingModels.Sport.Get;
using Fittify.Common.CustomExceptions;
using Fittify.DataModelRepository.Services;
using Fittify.DataModels.Models.Sport;

namespace Fittify.Api.OfmRepository.Test.TestHelper
{
    public class PropertyMappingServiceTest : IPropertyMappingService
    {
        public PropertyMappingServiceTest()
        {
            _propertyMappings.Add(new PropertyMapping<CardioSetOfmForGet, CardioSet>(_filePropertyMapping));
        }

        private readonly IList<IPropertyMapping> _propertyMappings = new List<IPropertyMapping>();

        private readonly Dictionary<string, PropertyMappingValue> _filePropertyMapping =
            new Dictionary<string, PropertyMappingValue>(StringComparer.OrdinalIgnoreCase)
            {
                {"Id", new PropertyMappingValue(new List<string>() {"Id"})},
                {"FullFileName", new PropertyMappingValue(new List<string>() { "FileName", "FileType"}, true)},
                {"FileSizeInKb", new PropertyMappingValue(new List<string>() { "FileSizeInKb"})},
                {"Age", new PropertyMappingValue(new List<string>() { "FileCreatedOnDate" })}
            };

        public Dictionary<string, PropertyMappingValue> GetPropertyMapping<TSource, TDestination>()
        {
            // get matching mapping
            var matchingMapping = _propertyMappings.OfType<PropertyMapping<TSource, TDestination>>();

            if (matchingMapping.Count() == 1)
            {
                return matchingMapping.First()._mappingDictionary;
            }

            throw new Exception($"Cannot find exact property mapping instance for <{typeof(TSource)},{typeof(TDestination)}");
        }

        public bool ValidMappingExistsFor<TSource, TDestination>(string fields, ref List<string> errorMessages)
        {
            throw new NotImplementedException();
        }
    }
}
