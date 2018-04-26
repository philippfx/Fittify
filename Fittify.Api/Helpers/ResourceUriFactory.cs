using System;
using System.Collections.Generic;
using System.Dynamic;
using Fittify.Common.Helpers;
using Fittify.Common.Helpers.ResourceParameters;
using Fittify.Common.Helpers.ResourceParameters.Sport;
using Microsoft.AspNetCore.Mvc;

namespace Fittify.Api.Helpers
{
    public static class ResourceUriFactory
    {
        public static string CreateResourceUriForIResourceParameters(
            IResourceParameters resourceParameters,
            IUrlHelper urlHelper,
            ResourceUriType type,
            string shortPascalCasedControllerName)
        {
            switch (type)
            {
                case ResourceUriType.PreviousPage:
                    return urlHelper.Link("Get" + shortPascalCasedControllerName + "Collection",
                        new
                        {
                            fields = resourceParameters.Fields,
                            orderBy = resourceParameters.OrderBy,
                            //searchQuery = resourceParameters.SearchQuery,
                            //genre = resourceParameters.Genre,
                            pageNumber = resourceParameters.PageNumber - 1,
                            pageSize = resourceParameters.PageSize
                        });
                case ResourceUriType.NextPage:
                    return urlHelper.Link("Get" + shortPascalCasedControllerName + "Collection",
                        new
                        {
                            fields = resourceParameters.Fields,
                            orderBy = resourceParameters.OrderBy,
                            //searchQuery = resourceParameters.SearchQuery,
                            //genre = resourceParameters.Genre,
                            pageNumber = resourceParameters.PageNumber + 1,
                            pageSize = resourceParameters.PageSize
                        });
                case ResourceUriType.Current:
                default:
                    return urlHelper.Link("Get" + shortPascalCasedControllerName + "Collection",
                        new
                        {
                            fields = resourceParameters.Fields,
                            orderBy = resourceParameters.OrderBy,
                            //searchQuery = resourceParameters.SearchQuery,
                            //genre = resourceParameters.Genre,
                            pageNumber = resourceParameters.PageNumber,
                            pageSize = resourceParameters.PageSize
                        });
            }
        }
        
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
