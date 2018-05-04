using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fittify.Api.OuterFacingModels.Sport.Get;
using Fittify.DataModelRepositories.Services;
using Fittify.DataModels.Models.Sport;

namespace Fittify.DataModelRepositories.Test.TestHelper.EntityFrameworkCore
{
    public static class PropertyMappingServiceTest
    {
        static PropertyMappingServiceTest()
        {
            _propertyMappings.Add(new PropertyMapping<CardioSetOfmForGet, CardioSet>(_filePropertyMapping));
        }

        private static readonly IList<IPropertyMapping> _propertyMappings = new List<IPropertyMapping>();

        private static readonly Dictionary<string, PropertyMappingValue> _filePropertyMapping =
            new Dictionary<string, PropertyMappingValue>(StringComparer.OrdinalIgnoreCase)
            {
                {"Id", new PropertyMappingValue(new List<string>() {"Id"})},
                {"FullFileName", new PropertyMappingValue(new List<string>() { "FileName", "FileType"})},
                {"Age", new PropertyMappingValue(new List<string>() {"FileCreatedOnDate"})}
            };

        public static Dictionary<string, PropertyMappingValue> GetPropertyMapping
            <TSource, TDestination>()
        {
            // get matching mapping
            var matchingMapping = _propertyMappings.OfType<PropertyMapping<TSource, TDestination>>();

            if (matchingMapping.Count() == 1)
            {
                return matchingMapping.First()._mappingDictionary;
            }

            throw new Exception($"Cannot find exact property mapping instance for <{typeof(TSource)},{typeof(TDestination)}");
        }
    }
}
