using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Reflection;
using System.Text;
using Fittify.Common.CustomExceptions;
using Fittify.DataModelRepository.Services;
using Microsoft.EntityFrameworkCore;

namespace Fittify.DataModelRepository.Helpers
{
    public static class IQueryableExtensions
    {
        public static IQueryable<T> ApplySort<T>(this IQueryable<T> source, string orderBy,
            Dictionary<string, PropertyMappingValue> mappingDictionary)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }

            if (mappingDictionary == null)
            {
                throw new ArgumentNullException("mappingDictionary");
            }

            if (string.IsNullOrWhiteSpace(orderBy))
            {
                return source;
            }
            // the orderBy string is separated by ",", so we split it.
            var orderByAfterSplit = orderBy.Split(',');

            // apply each orderby clause in reverse order - otherwise, the 
            // IQueryable will be ordered in the wrong order
            foreach (var orderByClause in orderByAfterSplit.Reverse())
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
                foreach (var destinationProperty in propertyMappingValue.DestinationProperties.Reverse())
                {
                    // revert sort order if necessary
                    if (propertyMappingValue.Revert)
                    {
                        orderDescending = !orderDescending;
                    }

                    // This is a dynamic linq query which allows to inject sql queries as string into ef context 
                    source = source.OrderBy(destinationProperty + (orderDescending ? " descending" : " ascending"));
                }
            }
            return source;
        }

        /// <summary>
        /// Sorts the order of queried entities by the select fields
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">IQueryable of T where T is an Entiy of EntityFramework DbContext</param>
        /// <param name="orderByFields">Valid string to orderBy query. Fields must be separated by comma and descending must be signalized by 'space and desc', for example "Id desc, Name, Date desc" orders by (1) Id descending, (2) then by Name ascending and (3) then by Date descending.</param>
        /// <returns></returns>
        public static IQueryable<T> ApplySort<T>(this IQueryable<T> source, IEnumerable<string> fields)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            
            if (fields == null)
            {
                return source;
            }

            // apply each orderby clause in reverse order - otherwise, the 
            // IQueryable will be ordered in the wrong order
            foreach (var field in fields.Reverse()) // SQL requires reverse order, because it doesn't know "thenBy".
            {
                // trim the orderByClause, as it might contain leading 
                // or trailing spaces. Can't trim the var in foreach,
                // so use another var.
                var trimmedOrderByClause = field.Trim();

                // if the sort option ends with with " desc", we order
                // descending, otherwise ascending
                var orderDescending = trimmedOrderByClause.EndsWith(" desc");

                // remove " asc" or " desc" from the orderByClause, so we 
                // get the property name to look for in the mapping dictionary
                var indexOfFirstSpace = trimmedOrderByClause.IndexOf(" ");
                var propertyName = indexOfFirstSpace == -1 ?
                    trimmedOrderByClause : trimmedOrderByClause.Remove(indexOfFirstSpace);
                
                // This is a dynamic linq query which allows to inject sql queries as string into ef context 
                source = source.OrderBy(propertyName + (orderDescending ? " descending" : " ascending"));
            }
            return source;
        }

        /// <summary>
        /// Sorts the order of queried entities by the select fields
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">IQueryable of T where T is an Entiy of EntityFramework DbContext</param>
        /// <param name="orderByFields">Valid string to orderBy query. Fields must be separated by comma and descending must be signalized by 'space and desc', for example "Id desc, Name, Date desc" orders by (1) Id descending, (2) then by Name ascending and (3) then by Date descending.</param>
        /// <returns></returns>
        public static IQueryable<T> ApplySort<T>(this IQueryable<T> source, string orderByFields)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }

            if (string.IsNullOrWhiteSpace(orderByFields))
            {
                return source;
            }
            // the orderBy string is separated by ",", so we split it.
            var orderByAfterSplit = orderByFields.Split(',');
            
            return source.ApplySort(orderByAfterSplit);
        }

        /// <summary>
        /// Creates a dynamic linq to entity to query only request columns.
        /// </summary>
        /// <typeparam name="TSource">Is the type of the queried Entity. All fields that are excluded from the query are set to their default values.</typeparam>
        /// <param name="source">Is the IQueryable of TSource</param>
        /// <param name="fields"></param>
        /// <param name="doIncludeId">Defines whether Key column "Id" should be included in query, although it may be left out in the wanted fields.</param>
        /// <returns></returns>
        public static IQueryable<TSource> ShapeLinqToEntityQuery<TSource>(
            this IQueryable<TSource> source,
            string fields) where TSource : class
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }
            
            string selectClause = "new (";
            if (string.IsNullOrWhiteSpace(fields))
            {
                return source;
            }
            else
            {
                // the fields are separated by ",", so we split it.
                var fieldsAfterSplit = fields.Split(',');

                // Checking if fields really exist for TSource
                foreach (var field in fieldsAfterSplit)
                {
                    // trim each field, as it might contain leading 
                    // or trailing spaces. Can't trim the var in foreach,
                    // so use another var.
                    var propertyName = field.Trim();

                    // use reflection to get the property on the source object
                    // we need to include public and instance, b/c specifying a binding flag overwrites the
                    // already-existing binding flags.
                    var propertyInfo = typeof(TSource)
                        .GetProperty(propertyName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

                    if (propertyInfo == null)
                    {
                        throw new PropertyNotFoundException($"Property {propertyName} wasn't found on {typeof(TSource)}");
                    }
                }

                int arrayLength = fieldsAfterSplit.Length;
                for (int i = 0; i < arrayLength; i++)
                {
                    if (i == 0)
                    {
                        selectClause += fieldsAfterSplit[i];
                    }
                    else
                    {
                        selectClause += ", " + fieldsAfterSplit[i];
                    }
                }

                selectClause += ")";
            }
            
            source = source.Select<TSource>(selectClause);
            
            return source;
        }

        public static IQueryable<TSource> ShapeLinqToEntityQuery<TSource, TDbContext>(
            this IQueryable<TSource> source,
            string fields, bool doIncludeId, TDbContext dbContext) 
            where TSource : class, new()
            where TDbContext : DbContext
        {
            if (dbContext == null)
            {
                throw new ArgumentNullException(nameof(dbContext));
            }

            var entry = dbContext.Entry(new TSource());
            var primaryKey = entry.Metadata.FindPrimaryKey();
            var keys = primaryKey.Properties.Select(k => k.Name).ToArray();
            StringBuilder stringBuilder = new StringBuilder();
            int arrayLength = 0;
            for (int i = 0; i <= arrayLength; i++)
            {
                if (i == 0)
                {
                    stringBuilder.Append(keys[i]);
                }
                else
                {
                    stringBuilder.Append(",");
                    stringBuilder.Append(keys[i]);
                }
            }
            

            fields = fields.Replace(" ", "");
            if (doIncludeId)
            {
                if (!fields.ToLower().Contains(",id,"))
                {
                    fields += ",id";
                }
            }

            return ShapeLinqToEntityQuery(source, fields);
        }
    }
}

