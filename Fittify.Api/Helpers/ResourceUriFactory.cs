using Fittify.Common.Helpers.ResourceParameters;
using Microsoft.AspNetCore.Mvc;

namespace Fittify.Api.Helpers
{
    public class ResourceUriFactory
    {
        // Todo Refactor so that actionName is created dynamically
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

        public static string CreateResourceUriForISearchQueryResourceParameters(
           ISearchQueryResourceParameters resourceParameters,
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
                            fields = resourceParameters.Fields,
                            orderBy = resourceParameters.OrderBy,
                            searchQuery = resourceParameters.SearchQuery,
                            //genre = resourceParameters.Genre,
                            pageNumber = resourceParameters.PageNumber - 1,
                            pageSize = resourceParameters.PageSize
                        });
                case ResourceUriType.NextPage:
                    return urlHelper.Link("Get" + shortCamelCasedControllerName + "Collection",
                        new
                        {
                            fields = resourceParameters.Fields,
                            orderBy = resourceParameters.OrderBy,
                            searchQuery = resourceParameters.SearchQuery,
                            //genre = resourceParameters.Genre,
                            pageNumber = resourceParameters.PageNumber + 1,
                            pageSize = resourceParameters.PageSize
                        });
                case ResourceUriType.Current:
                default:
                    return urlHelper.Link("Get" + shortCamelCasedControllerName + "Collection",
                        new
                        {
                            fields = resourceParameters.Fields,
                            orderBy = resourceParameters.OrderBy,
                            searchQuery = resourceParameters.SearchQuery,
                            //genre = resourceParameters.Genre,
                            pageNumber = resourceParameters.PageNumber,
                            pageSize = resourceParameters.PageSize
                        });
            }
        }

        public static string CreateResourceUriForISearchQueryDateTimeStartEnd(
           IDateTimeStartEndResourceParameters resourceParameters,
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
                            fields = resourceParameters.Fields,
                            orderBy = resourceParameters.OrderBy,
                            dateTimeStart = resourceParameters.DateTimeStart,
                            dateTimeEnd = resourceParameters.DateTimeEnd,
                            //genre = resourceParameters.Genre,
                            pageNumber = resourceParameters.PageNumber - 1,
                            pageSize = resourceParameters.PageSize
                        });
                case ResourceUriType.NextPage:
                    return urlHelper.Link("Get" + shortCamelCasedControllerName + "Collection",
                        new
                        {
                            fields = resourceParameters.Fields,
                            orderBy = resourceParameters.OrderBy,
                            dateTimeStart = resourceParameters.DateTimeStart,
                            dateTimeEnd = resourceParameters.DateTimeEnd,
                            //genre = resourceParameters.Genre,
                            pageNumber = resourceParameters.PageNumber + 1,
                            pageSize = resourceParameters.PageSize
                        });
                case ResourceUriType.Current:
                default:
                    return urlHelper.Link("Get" + shortCamelCasedControllerName + "Collection",
                        new
                        {
                            fields = resourceParameters.Fields,
                            orderBy = resourceParameters.OrderBy,
                            dateTimeStart = resourceParameters.DateTimeStart,
                            dateTimeEnd = resourceParameters.DateTimeEnd,
                            //genre = resourceParameters.Genre,
                            pageNumber = resourceParameters.PageNumber,
                            pageSize = resourceParameters.PageSize
                        });
            }
        }
    }
}
