using System;
using System.Collections.Generic;
using System.Reflection;
using Fittify.Api.OuterFacingModels.Sport.Abstract;

namespace Fittify.Api.Helpers
{
    public static class IEnumerableConvertOfmExtensions
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
    }
}
