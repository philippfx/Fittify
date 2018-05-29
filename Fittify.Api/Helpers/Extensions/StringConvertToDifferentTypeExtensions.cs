using System;

namespace Fittify.Api.Helpers.Extensions
{
    public static class StringConvertToDifferentTypeExtensions
    {
        /// <summary>
        /// Converts a string of "1" or case-insensitive "true" to true. Can handle leading and trailing whitespaces. Anything else returns false. 
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
