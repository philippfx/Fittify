using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fittify.Common.Helpers.ResourceParameters;
using Microsoft.AspNetCore.Mvc;

namespace Fittify.Api.Helpers
{
    public class RsourceUriFactory
    {
        // Todo Refactor so that actionName is created dynamically
        public static string CreateAuthorsResourceUri(
            IResourceParameters authorsResourceParameters,
            IUrlHelper urlHelper,
            ResourceUriType type)
        {
            switch (type)
            {
                case ResourceUriType.PreviousPage:
                    return urlHelper.Link("GetAllPagedCategories",
                        new
                        {
                            searchQuery = authorsResourceParameters.SearchQuery,
                            //genre = authorsResourceParameters.Genre,
                            pageNumber = authorsResourceParameters.PageNumber - 1,
                            pageSize = authorsResourceParameters.PageSize
                        });
                case ResourceUriType.NextPage:
                    return urlHelper.Link("GetAllPagedCategories",
                        new
                        {
                            searchQuery = authorsResourceParameters.SearchQuery,
                            //genre = authorsResourceParameters.Genre,
                            pageNumber = authorsResourceParameters.PageNumber + 1,
                            pageSize = authorsResourceParameters.PageSize
                        });

                default:
                    return urlHelper.Link("GetAllPagedCategories",
                        new
                        {
                            searchQuery = authorsResourceParameters.SearchQuery,
                            //genre = authorsResourceParameters.Genre,
                            pageNumber = authorsResourceParameters.PageNumber,
                            pageSize = authorsResourceParameters.PageSize
                        });
            }
        }
    }
}
