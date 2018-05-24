using System;
using System.Collections.Generic;
using System.Linq;
using PropertyMappingValue = Fittify.Api.OfmRepository.Services.PropertyMappingValue;

namespace Fittify.Api.OfmRepository.Helpers
{
    public static class OfmToEntitySortFields
    {
        public static IEnumerable<string> ToEntityOrderBy(this string ofmOrderByFields, Dictionary<string, PropertyMappingValue> mappingDictionary)
        {
            if (mappingDictionary == null)
            {
                throw new ArgumentNullException("mappingDictionary");
            }

            if (string.IsNullOrWhiteSpace(ofmOrderByFields))
            {
                return null;
            }

            // the orderBy string is separated by ",", so we split it.
            var orderByAfterSplit = ofmOrderByFields.Split(',');

            List<string> orderByEntityFields = new List<string>();
            // apply each orderby clause in reverse order - otherwise, the 
            // IQueryable will be ordered in the wrong order
            foreach (var orderByClause in orderByAfterSplit)
            {
                // trim the orderByClause, as it might contain leading 
                // or trailing spaces. Can't trim the var in foreach,
                // so use another var.
                var trimmedOrderByClause = orderByClause.Trim();

                // if the sort option ends with with " desc", we order
                // descending, otherwise ascending
                var orderDescending = trimmedOrderByClause.EndsWith(" desc");

                // remove " asc" or " desc" from the orderByClause, so we 
                // get the property name to look for in the mapping dictionary
                var indexOfFirstSpace = trimmedOrderByClause.IndexOf(" ");
                var propertyName = indexOfFirstSpace == -1 ?
                    trimmedOrderByClause : trimmedOrderByClause.Remove(indexOfFirstSpace);

                // find the matching property for ofm (sourceProperty)
                if (!mappingDictionary.ContainsKey(propertyName))
                {
                    throw new ArgumentNullException($"Key mapping for '{propertyName}' is missing");
                }

                // get the PropertyMappingValue for data entity (targetPropert/y/ies)
                var propertyMappingValue = mappingDictionary[propertyName];

                if (propertyMappingValue == null || !propertyMappingValue.DestinationProperties.Any())
                {
                    throw new ArgumentNullException($"PropertyMappingValue is null. The KEY property named '{propertyName}' for the ofm was found in the mappingDictionary, but no matching VALUE propert(ies) were found for the target data entity. Add a valid VALUE (PropertyMappingValue) to the key '{propertyName}'");
                }

                // Run through the property names in reverse
                // so the orderby clauses are applied in the correct order
                foreach (var destinationProperty in propertyMappingValue.DestinationProperties)
                {
                    // revert sort order if necessary
                    if (propertyMappingValue.Revert)
                    {
                        orderDescending = !orderDescending;
                    }
                    
                    orderByEntityFields.Add(destinationProperty + (orderDescending ? " desc" : ""));
                }
            }

            return orderByEntityFields;
        }
    }
}
