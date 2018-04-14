namespace Fittify.Common.Helpers
{
    public static class FittifyRegularExpressions
    {
        public const string RangeOfIntIds = @"^([1-9]{1}\d*(-[1-9]{1}\d*)?((,[1-9]{1}\d*)?(-[1-9]{1}\d*)?)*|null)$";
        public const string PartialError = @"^([1-9]{{1}}\d*(-[1-9]{{1}}\d*)?((,[1-9]{{1}}\d*)?(-[1-9]{{1}}\d*)?)*|null)$";
        public const string HttpStatusCodeStartsWith2 = @"^2";

    }
}
