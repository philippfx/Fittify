using System;

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

        /// <summary>
        /// Cuts ApiController name down to entity name. Useful, for example, for the creation of dynamic controller action names. 
        /// </summary>
        /// <param name="str">Full name of the controller, for example "WorkoutApiController"</param>
        /// <returns>Short pascal cased controller name, for example "Workout", or the unmodified input string</returns>
        public static string ToShortPascalCasedControllerNameOrDefault(this String str)
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
        /// Converts a string of "1" or case-insensitive "true" to true
        /// </summary>
        /// <param name="str"></param>
        public static bool ToBool(this String str)
        {
            if (String.IsNullOrWhiteSpace(str))
            {
                return false;
            }
            if (str.Trim() == "1" || str.Trim().ToLower() == "true")
            {
                return true;
            }
            return false;
        }
    }
}
