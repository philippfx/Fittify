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
using Fittify.Common.Helpers;
using Fittify.Common.Helpers.ResourceParameters;
using Fittify.DataModelRepositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace Fittify.Api.OfmRepository
{
    public class AsyncGetOfmCollectionIncludeByNameSearch<TCrudRepository, TEntity, TOfmForGet, TId> : AsyncGetOfm<TCrudRepository, TEntity, TOfmForGet, TId>, IAsyncGetOfmCollectionByNameSearch<TOfmForGet>
        where TOfmForGet : LinkedResourceBase, IEntityUniqueIdentifier<TId>
        where TId : struct
        where TEntity : class, IEntityName<TId>
        where TCrudRepository : class, IAsyncCrudForEntityName<TEntity, TId>
    {
        protected readonly AsyncGetOfmGuardClauses<TOfmForGet, TId> AsyncGetOfmGuardClause;
        public AsyncGetOfmCollectionIncludeByNameSearch(TCrudRepository repository,
            IUrlHelper urlHelper,
            IActionDescriptorCollectionProvider actionDescriptorCollectionProvider,
            IPropertyMappingService propertyMappingService,
            ITypeHelperService typeHelperService,
            Controller controller) 
            : base(repository, urlHelper, actionDescriptorCollectionProvider, propertyMappingService, typeHelperService, controller)
        {
            AsyncGetOfmGuardClause = new AsyncGetOfmGuardClauses<TOfmForGet, TId>(TypeHelperService);
        }
        
        public virtual async Task<OfmForGetCollectionQueryResult<TOfmForGet>> GetCollection(ISearchQueryResourceParameters resourceParameters)
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
            var pagedListEntityCollection = Repo.GetCollection(resourceParameters).CopyPropertyValuesTo(ofmForGetCollectionQueryResult);


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
            Controller.Response.Headers.Add("X-Pagination",
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
    }
}
