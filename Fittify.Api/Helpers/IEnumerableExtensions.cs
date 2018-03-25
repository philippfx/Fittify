﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Fittify.Api.OuterFacingModels.Sport.Abstract;
using Fittify.Common.Helpers.ResourceParameters;
using Microsoft.AspNetCore.Mvc;

namespace Fittify.Api.Helpers
{
    public static class IEnumerableExtensions
    {
        public static IEnumerable<ExpandableOfmForGet> ToExpandableOfmForGets<TOfmForGet>(
            this IEnumerable<TOfmForGet> expandableOfmForGetSourceCollection) where TOfmForGet : IOfmForGet
        {
            if (expandableOfmForGetSourceCollection == null)
            {
                throw new ArgumentNullException("ofmForGetSource");
            }

            var propertyInfoList = new List<PropertyInfo>();
            var propertyInfos = typeof(TOfmForGet).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            propertyInfoList.AddRange(propertyInfos);

            var expandableOfmForGetList = new List<ExpandableOfmForGet>();

            foreach (var ofmForGetSource in expandableOfmForGetSourceCollection)
            {
                var singleExpandableOfmForGet = new ExpandableOfmForGet();
                foreach (var propertyInfo in propertyInfoList)
                {
                    // GetValue returns the value of the property on the source object
                    var propertyValue = propertyInfo.GetValue(ofmForGetSource);

                    // add the field to the ExpandoObject
                    ((IDictionary<string, object>)singleExpandableOfmForGet).Add(propertyInfo.Name, propertyValue);
                }
                expandableOfmForGetList.Add(singleExpandableOfmForGet);
            }

            return expandableOfmForGetList;
        }

        public static IEnumerable<ExpandableOfmForGet> Shape(
            this IEnumerable<ExpandableOfmForGet> expandableOfmForGetSourceCollection,
            string fields,
            bool includeHateoasLinks)
        {
            if (expandableOfmForGetSourceCollection == null)
            {
                throw new ArgumentNullException("ofmForGetSource");
            }

            var expandableOfmForGetList = new List<ExpandableOfmForGet>();
            IEnumerable<string> fieldsAfterSplit = null;
            if (string.IsNullOrWhiteSpace(fields))
            {
                return expandableOfmForGetSourceCollection;
            }
            else
            {
                fieldsAfterSplit = fields.Split(',').Select(field => field.ToLower().Trim());
            }

            foreach (var ofmForGetSource in expandableOfmForGetSourceCollection)
            {
                var shapedExpandableOfmForGet = new ExpandableOfmForGet();

                foreach (var field in fieldsAfterSplit)
                {
                    var property = ofmForGetSource.FirstOrDefault(f => f.Key.ToLowerInvariant() == field);

                    if (!property.IsDefault()) // in effect if the struct KeyValuePair is not null
                    {
                        shapedExpandableOfmForGet.Add(property.Key, property.Value);
                    }
                }

                if (includeHateoasLinks)
                {
                    var property = ofmForGetSource.FirstOrDefault(f => f.Key.ToLowerInvariant() == "links");
                    shapedExpandableOfmForGet.Add(property.Key, property.Value);
                }

                expandableOfmForGetList.Add(shapedExpandableOfmForGet);
            }

            return expandableOfmForGetList;
        }

        public static IEnumerable<ExpandableOfmForGet> CreateHateoasLinksForeachExpandableOfmForGet<TOfmForGet, TId>(
            this IEnumerable<ExpandableOfmForGet> expandableOfmForGetCollection,
            IUrlHelper urlhelper,
            string controllerName,
            string fields)

            where TOfmForGet : class
            where TId : struct
        {
            var hateoasLinkFactory = new HateoasLinkFactory<TId>(urlhelper, controllerName);
            var expandableOfmForGets = new List<ExpandableOfmForGet>();
            foreach (var expandableOfmForGet in expandableOfmForGetCollection)
            {
                expandableOfmForGet.Add("links", hateoasLinkFactory.CreateLinksForOfmForGet((TId)expandableOfmForGet["Id"], fields).ToList());
                expandableOfmForGets.Add(expandableOfmForGet);
            }

            return expandableOfmForGets;
        }

        public static OfmForGetCollectionObjectResult CreateHateoasLinkForCollectionQueryIncludeByNameSearch<TOfmForGet, TId>(
            this IEnumerable<ExpandableOfmForGet> expandableOfmForGetCollection,
            IUrlHelper urlhelper,
            string controllerName,
            ISearchQueryResourceParameters resourceParameters,
            bool hasPrevious,
            bool hasNext)

            where TOfmForGet : class
            where TId : struct
        {
            var result = new OfmForGetCollectionObjectResult(expandableOfmForGetCollection);
            var hateoasLinkFactory = new HateoasLinkFactory<TId>(urlhelper, controllerName);
            //result.Add("value", result.OfmForGets);
            result.Add(new Dictionary<string, object> { { "links", hateoasLinkFactory
                    .CreateLinksForOfmGetCollectionQueryIncludeByNameSearch(resourceParameters, hasPrevious, hasNext)
                    .ToList()
            } });

            return result;
        }


    }
}