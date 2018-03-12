using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Fittify.Api.OuterFacingModels.Sport.Abstract;

namespace Fittify.Api
{
    public class ExpandableOfmCollection : IEnumerable<ExpandableOfmForGet>
    {
        private IEnumerable<ExpandableOfmForGet> ExpandableOfmForGets { get; set; }
        public IEnumerator<ExpandableOfmForGet> GetEnumerator()
        {
            foreach (var ofmForGet in ExpandableOfmForGets)
            {
                yield return ofmForGet;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }

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
            string fields)
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

                expandableOfmForGetList.Add(shapedExpandableOfmForGet);
            }

            return expandableOfmForGetList;
        }
    }
}
