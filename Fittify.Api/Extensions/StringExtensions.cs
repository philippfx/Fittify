using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fittify.Api.Extensions
{
    public static class StringExtensions
    {
        public static string ToShortCamelCasedControllerName(this String str)
        {
            string apiControllerString = "ApiController";
            if (!str.Contains(apiControllerString))
            {
                throw new ArgumentException($"The string does not contain string \"{apiControllerString}\". Please check the name of the Controller!");
            }
            var shortenedString = str.Replace(apiControllerString, "");
            return Char.ToLowerInvariant(shortenedString[0]) + shortenedString.Substring(1);
        }
    }
}
