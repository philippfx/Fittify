using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System;
using System.Collections;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Runtime.Serialization;
using System.Security.Permissions;
using Fittify.Api.OuterFacingModels.Sport.Abstract;

namespace Fittify.Api
{
    public class ExpandableOfmForGet : Dictionary<string, object>
    {

    }

    public static class OfmForGetExtensions
    {
        public static ExpandableOfmForGet ToExpandableOfm<TOfmForGet>(this TOfmForGet source) where TOfmForGet : IOfmForGet
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }

            var expandableOfm = new ExpandableOfmForGet();
            
            // all public properties should be in the ExpandoObject 
            var propertyInfos = typeof(TOfmForGet)
                    .GetProperties(BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

            foreach (var propertyInfo in propertyInfos)
            {
                // get the value of the property on the source object
                var propertyValue = propertyInfo.GetValue(source);

                // add the field to the ExpandoObject
                ((IDictionary<string, object>)expandableOfm).Add(propertyInfo.Name, propertyValue);
            }

            return expandableOfm;
        }
    }

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

    public static class StructExtensions
    {
        public static bool IsDefault<T>(this T value) where T : struct
        {
            bool isDefault = value.Equals(default(T));

            return isDefault;
        }
    }


}