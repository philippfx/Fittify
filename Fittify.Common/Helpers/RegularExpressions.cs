using System;
using System.Collections.Generic;
using System.Text;

namespace Fittify.Common.Helpers
{
    public static class RegularExpressions
    {
        public const string RangeOfIntIds = @"^([1-9]{1}\d*(-[1-9]{1}\d*)?((,[1-9]{1}\d*)?(-[1-9]{1}\d*)?)*|null)$";
        public const string PartialError = @"^([1-9]{{1}}\d*(-[1-9]{{1}}\d*)?((,[1-9]{{1}}\d*)?(-[1-9]{{1}}\d*)?)*|null)$";
        public const string HttpStatusCodeStartsWith2 = @"^2";

    }
}
