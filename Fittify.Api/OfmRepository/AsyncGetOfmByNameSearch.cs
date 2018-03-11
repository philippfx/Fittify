using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Fittify.Api.Helpers;
using Fittify.Api.OuterFacingModels;
using Fittify.Api.OuterFacingModels.Sport.Get;
using Fittify.Api.Services;
using Fittify.Common;
using Fittify.Common.Helpers.ResourceParameters;
using Fittify.DataModelRepositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace Fittify.Api.OfmRepository
{
    public class AsyncGetOfmByNameSearch<TCrudRepository, TEntity, TOfmForGet, TId> : AsyncGetOfm<TCrudRepository, TEntity, TOfmForGet, TId>, IAsyncGetOfmByNameSearch<TOfmForGet, TId>
        where TOfmForGet : LinkedResourceBase, IEntityUniqueIdentifier<TId>
        where TId : struct
        where TEntity : class, IEntityName<TId>
        where TCrudRepository : class, IAsyncCrudForEntityName<TEntity, TId>
    {
        public AsyncGetOfmByNameSearch(TCrudRepository repository,
            IUrlHelper urlHelper,
            IActionDescriptorCollectionProvider actionDescriptorCollectionProvider,
            IPropertyMappingService propertyMappingService,
            ITypeHelperService typeHelperService,
            string controllerName) 
            : base(repository, urlHelper, actionDescriptorCollectionProvider, propertyMappingService, typeHelperService, controllerName)
        {

        }

        public virtual async Task<IEnumerable<TOfmForGet>> GetAllPagedAndSearchName(ISearchQueryResourceParameters resourceParameters, ControllerBase controllerBase)
        {
            //// Todo this async lacks await
            var pagedListEntityCollection = Repo.GetAllPagedQueryName(resourceParameters);

            var previousPageLink = pagedListEntityCollection.HasPrevious ?
                ResourceUriFactory.CreateResourceUriForISearchQueryResourceParameters(resourceParameters,
                    UrlHelper,
                    ResourceUriType.PreviousPage) : null;

            var nextPageLink = pagedListEntityCollection.HasNext ?
                ResourceUriFactory.CreateResourceUriForISearchQueryResourceParameters(resourceParameters,
                    UrlHelper,
                    ResourceUriType.NextPage) : null;

            // Todo Maybe refactor to a type safe class instead of anonymous
            var paginationMetadata = new
            {
                totalCount = pagedListEntityCollection.TotalCount,
                pageSize = pagedListEntityCollection.PageSize,
                currentPage = pagedListEntityCollection.CurrentPage,
                totalPages = pagedListEntityCollection.TotalPages,
                previousPageLink = previousPageLink,
                nextPageLink = nextPageLink
            };

            // Todo: Refactor to class taking controller as input instead of only this method
            controllerBase.Response.Headers.Add("X-Pagination",
                Newtonsoft.Json.JsonConvert.SerializeObject(paginationMetadata));

            var ofmCollection = Mapper.Map<List<TEntity>, List<TOfmForGet>>(pagedListEntityCollection);
            return ofmCollection;
        }

        public virtual async Task<IEnumerable<TOfmForGet>> GetAllPagedAndSearchNameAndOrdered(ISearchQueryResourceParameters resourceParameters, ControllerBase controllerBase)
        {
            //// Todo this async lacks await
            var pagedListEntityCollection = Repo.GetAllPagedQueryNameOrdered(resourceParameters);

            var previousPageLink = pagedListEntityCollection.HasPrevious ?
                ResourceUriFactory.CreateResourceUriForISearchQueryResourceParameters(resourceParameters,
                    UrlHelper,
                    ResourceUriType.PreviousPage) : null;

            var nextPageLink = pagedListEntityCollection.HasNext ?
                ResourceUriFactory.CreateResourceUriForISearchQueryResourceParameters(resourceParameters,
                    UrlHelper,
                    ResourceUriType.NextPage) : null;

            // Todo Maybe refactor to a type safe class instead of anonymous
            var paginationMetadata = new
            {
                totalCount = pagedListEntityCollection.TotalCount,
                pageSize = pagedListEntityCollection.PageSize,
                currentPage = pagedListEntityCollection.CurrentPage,
                totalPages = pagedListEntityCollection.TotalPages,
                previousPageLink = previousPageLink,
                nextPageLink = nextPageLink
            };

            // Todo: Refactor to class taking controller as input instead of only this method
            controllerBase.Response.Headers.Add("X-Pagination",
                Newtonsoft.Json.JsonConvert.SerializeObject(paginationMetadata));

            var ofmCollection = Mapper.Map<List<TEntity>, List<TOfmForGet>>(pagedListEntityCollection);
            return ofmCollection;
        }

        public virtual async Task<OfmForGetCollectionQueryResult<TOfmForGet>> GetAllPagedAndSearchNameAndOrderedIncludingErrorMessages(ISearchQueryResourceParameters resourceParameters, ControllerBase controllerBase)
        {
            // Todo refactor to extract error message to adhere better to Single Responsibility principle
            // Todo collect multiple different types of errors 
            var ofmForGetCollectionQueryResult = new OfmForGetCollectionQueryResult<TOfmForGet>();
            ofmForGetCollectionQueryResult.ErrorMessages = new List<string>();
            IList<string> errorMessages = new List<string>();
            if (!PropertyMappingService.ValidMappingExistsFor<TOfmForGet, TEntity>(resourceParameters.OrderBy, ref errorMessages))
            {
                ofmForGetCollectionQueryResult.ErrorMessages.AddRange(errorMessages);
            }

            errorMessages = new List<string>();
            if (!TypeHelperService.TypeHasProperties<TOfmForGet>(resourceParameters.Fields, ref errorMessages))
            {
                // Todo ref unknown fields error messages
                ofmForGetCollectionQueryResult.ErrorMessages.AddRange(errorMessages);
            }

            if (ofmForGetCollectionQueryResult.ErrorMessages.Count > 0)
            {
                return ofmForGetCollectionQueryResult;
            }

            //// Todo this async lacks await
            var pagedListEntityCollection = Repo.GetAllPagedQueryNameOrdered(resourceParameters);

            //var previousPageLink = pagedListEntityCollection.HasPrevious ?
            //    ResourceUriFactory.CreateResourceUriForISearchQueryResourceParameters(resourceParameters,
            //        UrlHelper,
            //        ResourceUriType.PreviousPage) : null;

            //var nextPageLink = pagedListEntityCollection.HasNext ?
            //    ResourceUriFactory.CreateResourceUriForISearchQueryResourceParameters(resourceParameters,
            //        UrlHelper,
            //        ResourceUriType.NextPage) : null;

            // Todo Maybe refactor to a type safe class instead of anonymous
            var paginationMetadata = new
            {
                totalCount = pagedListEntityCollection.TotalCount,
                pageSize = pagedListEntityCollection.PageSize,
                currentPage = pagedListEntityCollection.CurrentPage,
                totalPages = pagedListEntityCollection.TotalPages,
                //previousPageLink = previousPageLink,
                //nextPageLink = nextPageLink
            };

            // Todo: Refactor to class taking controller as input instead of only this method
            controllerBase.Response.Headers.Add("X-Pagination",
                Newtonsoft.Json.JsonConvert.SerializeObject(paginationMetadata));

            if (errorMessages!= null && errorMessages.Count > 0)
            {
                return ofmForGetCollectionQueryResult;
            }

            ofmForGetCollectionQueryResult.ReturnedTOfmForGetCollection.OfmForGets = Mapper.Map<List<TEntity>, List<TOfmForGet>>(pagedListEntityCollection);
            
            foreach (var ofmForGet in ofmForGetCollectionQueryResult.ReturnedTOfmForGetCollection.OfmForGets)
            {
                ofmForGet.Links = HateoasLinkFactory.CreateLinksForOfmForGet(ofmForGet.Id, resourceParameters.Fields)
                    .ToList();
            }

            ofmForGetCollectionQueryResult.ReturnedTOfmForGetCollection.HateoasLinks = HateoasLinkFactory.CreateLinksForOfmForGetCollection(
                resourceParameters,
                pagedListEntityCollection.HasPrevious,
                pagedListEntityCollection.HasNext).ToList();

            return ofmForGetCollectionQueryResult;
        }

        private IEnumerable<HateoasLink> CreateLinksForCategories(
            ISearchQueryResourceParameters resourceParameters,
            bool hasNext, bool hasPrevious)
        {
            var links = new List<HateoasLink>();

            // self 
            links.Add(
                new HateoasLink(ResourceUriFactory.CreateResourceUriForISearchQueryResourceParameters(resourceParameters, UrlHelper, ResourceUriType.Current)
                    , "self", "GET"));

            if (hasNext)
            {
                links.Add(
                    new HateoasLink(ResourceUriFactory.CreateResourceUriForISearchQueryResourceParameters(resourceParameters, UrlHelper,
                            ResourceUriType.NextPage),
                        "nextPage", "GET"));
            }

            if (hasPrevious)
            {
                links.Add(
                    new HateoasLink(ResourceUriFactory.CreateResourceUriForISearchQueryResourceParameters(resourceParameters, UrlHelper,
                            ResourceUriType.PreviousPage),
                        "previousPage", "GET"));
            }

            return links;
        }
    }
}
