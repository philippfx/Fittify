using Fittify.Common.Helpers.ResourceParameters;
using Microsoft.AspNetCore.Mvc;

namespace Fittify.Api.Helpers
{
    public class ResourceUriFactory
    {
        // Todo Refactor so that actionName is created dynamically
        public static string CreateResourceUriForIResourceParameters(
            IResourceParameters authorsResourceParameters,
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
                            fields = authorsResourceParameters.Fields,
                            orderBy = authorsResourceParameters.OrderBy,
                            //searchQuery = authorsResourceParameters.SearchQuery,
                            //genre = authorsResourceParameters.Genre,
                            pageNumber = authorsResourceParameters.PageNumber - 1,
                            pageSize = authorsResourceParameters.PageSize
                        });
                case ResourceUriType.NextPage:
                    return urlHelper.Link("Get" + shortPascalCasedControllerName + "Collection",
                        new
                        {
                            fields = authorsResourceParameters.Fields,
                            orderBy = authorsResourceParameters.OrderBy,
                            //searchQuery = authorsResourceParameters.SearchQuery,
                            //genre = authorsResourceParameters.Genre,
                            pageNumber = authorsResourceParameters.PageNumber + 1,
                            pageSize = authorsResourceParameters.PageSize
                        });
                case ResourceUriType.Current:
                default:
                    return urlHelper.Link("Get" + shortPascalCasedControllerName + "Collection",
                        new
                        {
                            fields = authorsResourceParameters.Fields,
                            orderBy = authorsResourceParameters.OrderBy,
                            //searchQuery = authorsResourceParameters.SearchQuery,
                            //genre = authorsResourceParameters.Genre,
                            pageNumber = authorsResourceParameters.PageNumber,
                            pageSize = authorsResourceParameters.PageSize
                        });
            }
        }

        public static string CreateResourceUriForISearchQueryResourceParameters(
           ISearchQueryResourceParameters authorsResourceParameters,
           IUrlHelper urlHelper,
           ResourceUriType type,
           string shortCamelCasedControllerName)
        {
            switch (type)
            {
                case ResourceUriType.PreviousPage:
                    return urlHelper.Link("Get" + shortCamelCasedControllerName + "Collection",
                        new
                        {
                            fields = authorsResourceParameters.Fields,
                            orderBy = authorsResourceParameters.OrderBy,
                            searchQuery = authorsResourceParameters.SearchQuery,
                            //genre = authorsResourceParameters.Genre,
                            pageNumber = authorsResourceParameters.PageNumber - 1,
                            pageSize = authorsResourceParameters.PageSize
                        });
                case ResourceUriType.NextPage:
                    return urlHelper.Link("Get" + shortCamelCasedControllerName + "Collection",
                        new
                        {
                            fields = authorsResourceParameters.Fields,
                            orderBy = authorsResourceParameters.OrderBy,
                            searchQuery = authorsResourceParameters.SearchQuery,
                            //genre = authorsResourceParameters.Genre,
                            pageNumber = authorsResourceParameters.PageNumber + 1,
                            pageSize = authorsResourceParameters.PageSize
                        });
                case ResourceUriType.Current:
                default:
                    return urlHelper.Link("Get" + shortCamelCasedControllerName + "Collection",
                        new
                        {
                            fields = authorsResourceParameters.Fields,
                            orderBy = authorsResourceParameters.OrderBy,
                            searchQuery = authorsResourceParameters.SearchQuery,
                            //genre = authorsResourceParameters.Genre,
                            pageNumber = authorsResourceParameters.PageNumber,
                            pageSize = authorsResourceParameters.PageSize
                        });
            }
        }
    }
}
