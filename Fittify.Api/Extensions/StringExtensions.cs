using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fittify.Api.Extensions
{
    public static class StringExtensions
    {
        /// <summary>
        /// Cuts ApiController name down to entity name. Useful, for example, for the creation of dynamic error messages. 
        /// </summary>
        /// <param name="str">Full name of the controller, for example "WorkoutApiController"</param>
        /// <returns>Short camel cased controller name, for example "workout", or the unmodified input string</returns>
        public static string ToShortCamelCasedControllerNameOrDefault(this String str)
        {
            if (str == null) return null;
            string apiControllerString = "ApiController";
            if (!str.Contains(apiControllerString))
            {
                return str;
            }
            var shortenedString = str.Replace(apiControllerString, "");
            return Char.ToLowerInvariant(shortenedString[0]) + shortenedString.Substring(1);
        }

        /// <summary>
        /// Cuts the TOfmForGet Name down to entity name. Useful, for example, for the creation of dynamic error messages. 
        /// </summary>
        /// <param name="str">Full name of the TOfmForGet class, for example "WorkoutOfmForGet"</param>
        /// <returns>Short camel cased TOfmForGet name, for example "workout",  or the unmodified input string</returns>
        public static string ToShortCamelCasedOfmForGetNameOrDefault(this String str)
        {
            if (str == null) return null;
            string apiOfmForGetString = "OfmForGet";
            if (!str.Contains(apiOfmForGetString))
            {
                return str;
            }
            var shortenedString = str.Replace(apiOfmForGetString, "");
            return Char.ToLowerInvariant(shortenedString[0]) + shortenedString.Substring(1);
        }
    }
}
