using System;
using Fittify.Common.Helpers;

namespace Fittify.Common.Extensions
{
    public static class StringExtensions
    {
        public static string ToPlural(this String str)
        {
            return StringPluralization.Pluralize(2, str);
        }

        public static string ToSingular(this String str)
        {
            return StringPluralization.Pluralize(1, str);
        }

        public static string ToCamelCase(this String str)
        {
            return Char.ToLowerInvariant(str[0]) + str.Substring(1);
        }
    }
}
