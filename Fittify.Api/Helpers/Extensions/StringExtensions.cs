﻿using System;

namespace Fittify.Api.Helpers.Extensions
{
    public static class StringConvertControllerNameExtensions
    {
        /// <summary>
        /// Cuts case sensitive ApiController name down to camel cased entity name. Useful, for example, for the creation of dynamic error messages. 
        /// </summary>
        /// <param name="str">Full name of the controller, for example "WorkoutApiController"</param>
        /// <returns>Short camel cased controller name, for example "workout", or the unmodified input string</returns>
        public static string ToShortCamelCasedControllerName(this String str)
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
        /// Cuts case sensitive ApiController name down to pascal cased entity name. Useful, for example, for the creation of dynamic controller action names. 
        /// </summary>
        /// <param name="str">Full name of the controller, for example "WorkoutApiController"</param>
        /// <returns>Short pascal cased controller name, for example "WorkoutOfmCollectionResourceParameters", or the unmodified input string</returns>
        public static string ToShortPascalCasedControllerName(this String str)
        {
            if (str == null) return null;
            string apiControllerString = "ApiController";
            if (!str.Contains(apiControllerString))
            {
                return str;
            }
            return str.Replace(apiControllerString, "");
        }

        /// <summary>
        /// Cuts case sensitive ApiController name down to pascal cased ofmForGet name. Useful, for example, for the creation of dynamic controller action names. 
        /// </summary>
        /// <param name="str">Full name of the controller, for example "WorkoutApiController"</param>
        /// <returns>Short pascal cased controller name, for example "WorkoutOfmCollectionResourceParameters", or the unmodified input string</returns>
        public static string ToShortPascalCasedOfmForGetName(this String str)
        {
            if (str == null) return null;
            string ofmForGetString = "OfmForGet";
            if (!str.Contains(ofmForGetString))
            {
                return str;
            }
            return str.Replace(ofmForGetString, "");
        }
    }
}
