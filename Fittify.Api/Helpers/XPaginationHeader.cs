using System.Collections.Generic;
using System.Linq;
using Fittify.Api.OfmRepository.Helpers;
using Fittify.Api.OuterFacingModels.Sport.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace Fittify.Api.Helpers
{
    public static class ControllerExtensions
    {
        public static void AddPaginationMetadata<TId, TOfmForGet>(this Controller controller,
            OfmForGetCollectionQueryResult<TOfmForGet> tOfmForGetCollectionQueryResult,
            IncomingHeaders incomingHeaders,
            IDictionary<string, object> resourceParametersAsDictionary,
            IUrlHelper urlHelper,
            string controllerName)
        where TOfmForGet : class, IOfmForGet
        where TId : struct
        {
            if (incomingHeaders.IncludeHateoas)
            {
                var paginationMetadata = new
                {
                    totalCount = tOfmForGetCollectionQueryResult.TotalCount,
                    pageSize = tOfmForGetCollectionQueryResult.PageSize,
                    currentPage = tOfmForGetCollectionQueryResult.CurrentPage,
                    totalPages = tOfmForGetCollectionQueryResult.TotalPages
                };
                controller.Response.Headers.Add("X-Pagination",
                    Newtonsoft.Json.JsonConvert.SerializeObject(paginationMetadata));
            }
            else
            {
                var hateoasLinks = new HateoasLinkFactory<TId>(urlHelper, controllerName).CreateLinksForOfmGetGeneric(
                    resourceParametersAsDictionary,
                    tOfmForGetCollectionQueryResult.HasPrevious,
                    tOfmForGetCollectionQueryResult.HasNext);

                var paginationMetadata = new
                {
                    totalCount = tOfmForGetCollectionQueryResult.TotalCount,
                    pageSize = tOfmForGetCollectionQueryResult.PageSize,
                    currentPage = tOfmForGetCollectionQueryResult.CurrentPage,
                    totalPages = tOfmForGetCollectionQueryResult.TotalPages,
                    previousPage = hateoasLinks.FirstOrDefault(w => w.Rel == "previousPage")?.Href,
                    nextPage = hateoasLinks.FirstOrDefault(w => w.Rel == "nextPage")?.Href
                };
                controller.Response.Headers.Add("X-Pagination",
                    Newtonsoft.Json.JsonConvert.SerializeObject(paginationMetadata));
            }



        }
    }
}
