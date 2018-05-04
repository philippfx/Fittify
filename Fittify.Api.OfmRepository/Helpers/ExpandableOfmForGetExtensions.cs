using System;
using System.Linq;
using Fittify.Common.Extensions;

namespace Fittify.Api.OfmRepository.Helpers
{
    public static class ExpandableOfmForGetExtensions
    {
        public static ExpandableOfmForGet Shape(this ExpandableOfmForGet source, string fields)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }

            if (string.IsNullOrWhiteSpace(fields))
            {
                return source;
            }

            // the field are separated by ",", so we split it.
            var fieldsAfterSplit = fields.Split(',').Select(s => s.ToLower().Trim());

            var shapedExpandableOfmForGet = new ExpandableOfmForGet();
            foreach (var field in fieldsAfterSplit)
            {
                var property = source.FirstOrDefault(f => f.Key.ToLowerInvariant() == field);

                if (!property.IsDefault()) // in effect if the struct KeyValuePair is not null
                {
                    shapedExpandableOfmForGet.Add(property.Key, property.Value);
                }
            }

            // return
            return shapedExpandableOfmForGet;
        }
    }
}
