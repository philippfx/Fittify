using System;
using System.Collections.Generic;
using Fittify.Api.Helpers;
using Microsoft.Extensions.Configuration;
using IConfiguration = Microsoft.Extensions.Configuration.IConfiguration;

namespace Fittify.Api.Extensions
{
    public static class IncomingRawHeadersExtensions
    {
        public static bool Validate(this IncomingRawHeaders incomingRawHeaders, IConfiguration appConfiguration, out List<string> errorMessages)
        {
            errorMessages = new List<string>();
            if (!String.IsNullOrWhiteSpace(incomingRawHeaders.IncludeHateoas))
            {
                if (incomingRawHeaders.IncludeHateoas.ToLower() != "0" &
                    incomingRawHeaders.IncludeHateoas.ToLower() != "1")
                {
                    errorMessages.Add("The header '" + nameof(incomingRawHeaders.IncludeHateoas) + "' can only take a value of '0' (false) or '1' (true)!");
                }
            }


            var latestSupportedApiVersion = appConfiguration.GetValue<int>("LatestApiVersion");
            if (!String.IsNullOrWhiteSpace(incomingRawHeaders.ApiVersion))
            {
                var unacceptableIncomingApiVersionErrorMessage = "The header 'Api-Version' can only take an integer value of greater than or equal to '1'. The latest supported version is " + latestSupportedApiVersion;
                if (!int.TryParse(incomingRawHeaders.ApiVersion, out var incomingApiVersion))
                {
                    errorMessages.Add(unacceptableIncomingApiVersionErrorMessage);
                }

                if (incomingApiVersion < 1 || incomingApiVersion > latestSupportedApiVersion)
                {
                    errorMessages.Add(unacceptableIncomingApiVersionErrorMessage);
                }
            }
            else
            {
                errorMessages.Add("A header 'Api-Version' must be specified and take an integer value greater than or equal to '1'. The latest supported version is " + latestSupportedApiVersion);
            }

            if (errorMessages.Count > 0)
            {
                return false;
            }
            return true;
        }
    }
}
