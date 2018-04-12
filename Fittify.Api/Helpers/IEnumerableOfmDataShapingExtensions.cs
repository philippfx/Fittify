using System;
using System.Collections.Generic;
using System.Linq;
using Fittify.Api.Helpers.Extensions;

namespace Fittify.Api.Helpers
{
    public static class IEnumerableOfmDataShapingExtensions
    {

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
    }
}
