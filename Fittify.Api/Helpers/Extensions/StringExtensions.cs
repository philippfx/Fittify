using System;

namespace Fittify.Api.Helpers.Extensions
{
    public static class StringConvertControllerNameExtensions
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
    }
}
