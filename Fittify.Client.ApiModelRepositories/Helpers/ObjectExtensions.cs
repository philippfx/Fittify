using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Fittify.Api.OfmRepository.OfmResourceParameters.Sport;

namespace Fittify.Web.View.Helpers
{
    public static class ObjectExtensions
    {
        /// <summary>
        /// Converts any instance of a resourceParameters to a query parameters string
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
                    if (pVal as string != null && pVal as string != "")
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
