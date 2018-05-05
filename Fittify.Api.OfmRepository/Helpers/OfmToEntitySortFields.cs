﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fittify.DataModelRepository.Services;
using PropertyMappingValue = Fittify.Api.OfmRepository.Services.PropertyMappingValue;

namespace Fittify.Api.OfmRepository.Helpers
{
    public static class OfmToEntitySortFields
    {
        public static IEnumerable<string> ToEntitySortFields(this string ofmOrderByFields, Dictionary<string, PropertyMappingValue> mappingDictionary)
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

                // find the matching property
                if (!mappingDictionary.ContainsKey(propertyName))
                {
                    throw new ArgumentException($"Key mapping for {propertyName} is missing");
                }

                // get the PropertyMappingValue
                var propertyMappingValue = mappingDictionary[propertyName];

                if (propertyMappingValue == null)
                {
                    throw new ArgumentNullException("propertyMappingValue");
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
