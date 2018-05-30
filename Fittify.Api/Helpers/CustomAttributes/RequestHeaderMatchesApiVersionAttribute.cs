using System;
using Microsoft.AspNetCore.Mvc.ActionConstraints;

namespace Fittify.Api.Helpers.CustomAttributes
{
    [AttributeUsage(AttributeTargets.All, Inherited = true, AllowMultiple = true)]
    public sealed class RequestHeaderMatchesApiVersionAttribute : Attribute, IActionConstraint
    {
        private readonly string[] _versions;
        private readonly string _requestHeaderToMatch;

        public RequestHeaderMatchesApiVersionAttribute(string requestHeaderToMatch,
            string[] versions)
        {
            _requestHeaderToMatch = ConstantHttpHeaderNames.ApiVersion;
            _versions = versions;
        }

        public int Order
        {
            get
            {
                return 0;
            }
        }

        public bool Accept(ActionConstraintContext context)
        {
            var requestHeaders = context.RouteContext.HttpContext.Request.Headers;

            if (!requestHeaders.ContainsKey(_requestHeaderToMatch))
            {
                return false;
            }

            // if one of the media types matches, return true
            foreach (var version in _versions)
            {
                var versionMatches = string.Equals(requestHeaders[_requestHeaderToMatch].ToString(),
                    version, StringComparison.OrdinalIgnoreCase);

                if (versionMatches)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
