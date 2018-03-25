using System;
using System.Collections.Generic;
using System.Linq;
using Fittify.Api.OuterFacingModels.Sport.Get;
using Fittify.DataModels.Models.Sport;

namespace Fittify.Api.Services
{
    public class PropertyMappingService : IPropertyMappingService
    {
        private Dictionary<string, PropertyMappingValue> _authorPropertyMapping =
            new Dictionary<string, PropertyMappingValue>(StringComparer.OrdinalIgnoreCase)
            {
                {"Id", new PropertyMappingValue(new List<string>() {"Id"})},
                {"Genre", new PropertyMappingValue(new List<string>() {"Genre"})},
                {"Age", new PropertyMappingValue(new List<string>() {"DateOfBirth"}, true)},
                {"FullName", new PropertyMappingValue(new List<string>() {"FirstName", "LastName"})},
                {"Name", new PropertyMappingValue(new List<string>() {"Name"})},
                {"DateTimeStart", new PropertyMappingValue(new List<string>() {"DateTimeStart"})},
                {"DateTimeEnd", new PropertyMappingValue(new List<string>() {"DateTimeEnd"})},
            };

        private IList<IPropertyMapping> propertyMappings = new List<IPropertyMapping>();

        public PropertyMappingService()
        {
            // Todo this must be refactored to TOfmForGet and Entity!
            propertyMappings.Add(new PropertyMapping<CardioSetOfmForGet, CardioSet>(_authorPropertyMapping));
            propertyMappings.Add(new PropertyMapping<CategoryOfmForGet, Category>(_authorPropertyMapping));
            propertyMappings.Add(new PropertyMapping<ExerciseHistoryOfmForGet, ExerciseHistory>(_authorPropertyMapping));
            propertyMappings.Add(new PropertyMapping<ExerciseOfmForGet, Exercise>(_authorPropertyMapping));
            propertyMappings.Add(new PropertyMapping<MapExerciseWorkoutOfmForGet, MapExerciseWorkout>(_authorPropertyMapping));
            propertyMappings.Add(new PropertyMapping<WeightLiftingSetOfmForGet, WeightLiftingSet>(_authorPropertyMapping));
            propertyMappings.Add(new PropertyMapping<WorkoutHistoryOfmForGet, WorkoutHistory>(_authorPropertyMapping));
            propertyMappings.Add(new PropertyMapping<WorkoutOfmForGet, Workout>(_authorPropertyMapping));

        }
        public Dictionary<string, PropertyMappingValue> GetPropertyMapping
            <TSource, TDestination>()
        {
            // get matching mapping
            var matchingMapping = propertyMappings.OfType<PropertyMapping<TSource, TDestination>>();

            if (matchingMapping.Count() == 1)
            {
                return matchingMapping.First()._mappingDictionary;
            }

            throw new Exception($"Cannot find exact property mapping instance for <{typeof(TSource)},{typeof(TDestination)}");
        }

        public bool ValidMappingExistsFor<TSource, TDestination>(string fields, ref List<string> errorMessages)
        {
            var propertyMapping = GetPropertyMapping<TSource, TDestination>();

            if (string.IsNullOrWhiteSpace(fields))
            {
                return true;
            }

            // the string is separated by ",", so we split it.
            var fieldsAfterSplit = fields.Split(',');

            // run through the fields clauses
            foreach (var field in fieldsAfterSplit)
            {
                // trim
                var trimmedField = field
                    .Replace(" desc", "") // excluding orderBy descending
                    .Trim();

                // remove everything after the first " " - if the fields 
                // are coming from an orderBy string, this part must be 
                // ignored
                var indexOfFirstSpace = trimmedField.IndexOf(" ");
                var propertyName = indexOfFirstSpace == -1 ?
                    trimmedField : trimmedField.Remove(indexOfFirstSpace);

                // find the matching property
                if (!propertyMapping.ContainsKey(propertyName))
                {
                    errorMessages.Add("A property named '" + propertyName + "' does not exist");
                }
            }

            if (errorMessages.Count > 0)
            {
                return false;
            }

            return true;
        }

    }
}
