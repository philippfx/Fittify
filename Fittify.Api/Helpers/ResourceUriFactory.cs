using System.Collections.Generic;
using Fittify.Api.OfmRepository.OfmResourceParameters;
using Fittify.Common.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace Fittify.Api.Helpers
{
    public static class ResourceUriFactory
    {
        public static string CreateResourceUriForGeneric(
           IDictionary<string, object> resourceParametersAsDictionary,
           IUrlHelper urlHelper,
           ResourceUriType type,
           string shortCamelCasedControllerName)
        {
            switch (type)
            {
                case ResourceUriType.PreviousPage:
                {
                    // setting previous page
                    resourceParametersAsDictionary["PageNumber"] = (int)resourceParametersAsDictionary["PageNumber"] - 1;
                    
                    return urlHelper.Link("Get" + shortCamelCasedControllerName + "Collection",
                    resourceParametersAsDictionary.ToExpandoObject());
                }

                case ResourceUriType.NextPage:
                {
                    // setting next page
                    resourceParametersAsDictionary["PageNumber"] = (int)resourceParametersAsDictionary["PageNumber"] + 1;
                    return urlHelper.Link("Get" + shortCamelCasedControllerName + "Collection",
                        resourceParametersAsDictionary.ToExpandoObject());
                }
                case ResourceUriType.Current:
                default:
                    return urlHelper.Link("Get" + shortCamelCasedControllerName + "Collection",
                        resourceParametersAsDictionary.ToExpandoObject());
            }
        }
    }
}
