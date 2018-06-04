using System;
using System.Reflection;

namespace Fittify.Client.ApiModelRepositories.Helpers
{
    public static class ObjectExtensions
    {
        /// <summary>
        /// Converts any instance of a resourceParameters to a query parameters string. NullOrWhiteSpace string queryParameters are ignored and not added to the returned queryParameterString.
        /// </summary>
        /// <param name="source">Resource parameter instance</param>
        /// <returns>Query parameter string</returns>
        public static string ToQueryParameterString<T>(this T source) //where T : class
        {
            PropertyInfo[] properties = source.GetType().GetProperties();
            string queryParamters = "";
            foreach (var p in properties)
            {
                var pName = p.Name;
                var pVal = p.GetValue(source);

                if (pVal != null)
                {
                    if (pVal is string s && String.IsNullOrWhiteSpace(s))
                    {
                        // Do nothing, because we don't need empty query parameters
                    }
                    else
                    {
                        if (String.IsNullOrWhiteSpace(queryParamters))
                        {
                            queryParamters = "?" + pName + "=" + pVal;
                        }
                        else
                        {
                            queryParamters += "&" + pName + "=" + pVal;
                        }
                    }
                }
            }

            return queryParamters;
        }
    }
}
