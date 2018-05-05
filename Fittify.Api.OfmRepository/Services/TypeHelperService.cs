using System.Collections.Generic;
using System.Reflection;

namespace Fittify.Api.OfmRepository.Services
{
    /// <summary>
    /// Is used to validate input fields when data will be shaped https://app.pluralsight.com/player?course=asp-dot-net-core-restful-api-building&author=kevin-dockx&name=asp-dot-net-core-restful-api-building-m7&clip=9&mode=live
    /// </summary>
    public class TypeHelperService : ITypeHelperService
    {
        public bool TypeHasProperties<T>(string fields, ref List<string> errorMessages)
        {
            if (string.IsNullOrWhiteSpace(fields))
            {
                return true;
            }

            // the field are separated by ",", so we split it.
            var fieldsAfterSplit = fields.Split(',');
            
            // check if the requested fields exist on source
            foreach (var field in fieldsAfterSplit)
            {
                // trim each field, as it might contain leading 
                // or trailing spaces. Can't trim the var in foreach,
                // so use another var.
                var propertyName = field
                    .Replace(" desc", "") // excluding orderBy descending
                    .Trim();

                // use reflection to check if the property can be
                // found on T. 
                var propertyInfo = typeof(T)
                    .GetProperty(propertyName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

                // it can't be found, return false
                if (propertyInfo == null)
                {
                    errorMessages.Add("A property named '" + propertyName + "' does not exist");
                }
            }

            if (errorMessages.Count > 0)
            {
                return false;
            }

            // all checks out, return true
            return true;
        }
    }
}