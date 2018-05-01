using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using Fittify.Common.CustomExceptions;

namespace Fittify.Common.Helpers
{
    public static class ObjectExtensions
    {
        public static ExpandoObject ShapeData<TSource>(this TSource source,
          string fields)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }

            var dataShapedObject = new ExpandoObject();

            if (string.IsNullOrWhiteSpace(fields))
            {
                // all public properties should be in the ExpandoObject 
                var propertyInfos = typeof(TSource)
                        .GetProperties(BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

                foreach (var propertyInfo in propertyInfos)
                {
                    // get the value of the property on the source object
                    var propertyValue = propertyInfo.GetValue(source);

                    // add the field to the ExpandoObject
                    ((IDictionary<string, object>)dataShapedObject).Add(propertyInfo.Name, propertyValue);
                }

                return dataShapedObject;
            }

            // the field are separated by ",", so we split it.
            var fieldsAfterSplit = fields.Split(',');

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

                // get the value of the property on the source object
                var propertyValue = propertyInfo.GetValue(source);

                // add the field to the ExpandoObject
                ((IDictionary<string, object>)dataShapedObject).Add(propertyInfo.Name, propertyValue);
            }

            // return
            return dataShapedObject;
        }

        public static T ToObject<T>(this IDictionary<string, object> source)
            where T : class, new()
        {
            var someObject = new T();
            var someObjectType = someObject.GetType();

            foreach (var item in source)
            {
                someObjectType
                    .GetProperty(item.Key)
                    .SetValue(someObject, item.Value, null);
            }

            return someObject;
        }
        
        public static ExpandoObject ToExpandoObject(this IDictionary<string, object> source)
        {
            var eo = new ExpandoObject();
            var eoColl = (ICollection<KeyValuePair<string, object>>)eo;
            foreach (var kvp in source)
            {
                eoColl.Add(kvp);
            }

            return eo;
        }

        public static IDictionary<string, object> RemoveNullValues(this IDictionary<string, object> source)
        {
            return source.Where(kvp => kvp.Value != null).ToDictionary(k => k.Key, v => v.Value);
        }

        public static IDictionary<string, object> AsDictionary(this object source,
            BindingFlags bindingAttr = BindingFlags.Public | BindingFlags.Instance)
        {
            return source.GetType().GetProperties(bindingAttr).ToDictionary
            (
                propInfo => propInfo.Name,
                propInfo => propInfo.GetValue(source, null)
            );

        }
        
    }
}
