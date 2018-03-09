using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Fittify.Api.Helpers;
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
        where TOfmForGet : class
        where TId : struct
        where TEntity : class, IEntityName<TId>
        where TCrudRepository : class, IAsyncCrudForEntityName<TEntity, TId>
    {
        public AsyncGetOfmByNameSearch(TCrudRepository repository,
            IUrlHelper urlHelper,
            IActionDescriptorCollectionProvider actionDescriptorCollectionProvider,
            IPropertyMappingService propertyMappingService) 
            : base(repository, urlHelper, actionDescriptorCollectionProvider, propertyMappingService)
        {

        }

        public virtual async Task<IEnumerable<TOfmForGet>> GetAllPagedAndSearchName(ISearchQueryResourceParameters resourceParameters, ControllerBase controllerBase)
        {
            //// Todo this async lacks await
            var pagedListEntityCollection = Repo.GetAllPagedQueryName(resourceParameters);

            var previousPageLink = pagedListEntityCollection.HasPrevious ?
                RsourceUriFactory.CreateResourceUriForISearchQueryResourceParameters(resourceParameters,
                    UrlHelper,
                    ResourceUriType.PreviousPage) : null;

            var nextPageLink = pagedListEntityCollection.HasNext ?
                RsourceUriFactory.CreateResourceUriForISearchQueryResourceParameters(resourceParameters,
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
                RsourceUriFactory.CreateResourceUriForISearchQueryResourceParameters(resourceParameters,
                    UrlHelper,
                    ResourceUriType.PreviousPage) : null;

            var nextPageLink = pagedListEntityCollection.HasNext ?
                RsourceUriFactory.CreateResourceUriForISearchQueryResourceParameters(resourceParameters,
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

        public virtual async Task<OfmForGetQueryResult<TOfmForGet>> GetAllPagedAndSearchNameAndOrderedIncludingErrorMessages(ISearchQueryResourceParameters resourceParameters, ControllerBase controllerBase)
        {
            // Todo refactor to extract error message to adhere better to Single Responsibility principle
            var ofmForGetResult = new OfmForGetQueryResult<TOfmForGet>();
            ofmForGetResult.ErrorMessages = new List<string>();
            IList<string> errorMessages = new List<string>();
            if (!PropertyMappingService.ValidMappingExistsFor<TOfmForGet, TEntity>(resourceParameters.OrderBy, ref errorMessages))
            {
                // Todo out error Messages
                ofmForGetResult.ErrorMessages.AddRange(errorMessages);
                return ofmForGetResult;
            }

            //// Todo this async lacks await
            var pagedListEntityCollection = Repo.GetAllPagedQueryNameOrdered(resourceParameters);

            var previousPageLink = pagedListEntityCollection.HasPrevious ?
                RsourceUriFactory.CreateResourceUriForISearchQueryResourceParameters(resourceParameters,
                    UrlHelper,
                    ResourceUriType.PreviousPage) : null;

            var nextPageLink = pagedListEntityCollection.HasNext ?
                RsourceUriFactory.CreateResourceUriForISearchQueryResourceParameters(resourceParameters,
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

            if (errorMessages!= null && errorMessages.Count > 0)
            {
                return ofmForGetResult;
            }

            ofmForGetResult.ReturnedTOfmForGetCollection = Mapper.Map<List<TEntity>, List<TOfmForGet>>(pagedListEntityCollection);
            return ofmForGetResult;
        }
    }
}
