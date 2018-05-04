using System;
using System.Collections.Generic;
using System.Reflection;
using Fittify.Api.OuterFacingModels.Sport.Abstract;

namespace Fittify.Api.OfmRepository.Helpers
{
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
}
