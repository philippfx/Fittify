using System;
using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using Fittify.Common.Helpers;
using Newtonsoft.Json.Linq;

namespace Fittify.Common.Extensions
{
    public static class StringExtensions
    {
        public static string ToPlural(this String str)
        {
            return StringPluralization.Pluralize(2, str);
        }

        [ExcludeFromCodeCoverage]
        public static string ToSingular(this String str)
        {
            return StringPluralization.Pluralize(1, str);
        }

        [ExcludeFromCodeCoverage]
        public static string ToCamelCase(this String str)
        {
            return Char.ToLowerInvariant(str[0]) + str.Substring(1);
        }

        public static string PrettifyJson(this string source)
        {
            return JToken.Parse(source).ToString();
        }

        public static string MinifyJson(this string source)
        {
            return Regex.Replace(source, @"(""(?:[^""\\]|\\.)*"")|\s+", "$1");
        }

        public static string MinifyXml(this string source)
        {
            return Regex.Replace(source, @">\s*<", "><").Trim();
        }

        public static string PrettifyXml(this string source)
        {
            XDocument doc = XDocument.Parse(source);
            return doc.ToString();
        }
    }
}
