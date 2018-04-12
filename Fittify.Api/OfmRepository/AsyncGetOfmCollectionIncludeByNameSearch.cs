using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Fittify.Api.Helpers;
using Fittify.Common;
using Fittify.Common.Helpers;
using Fittify.Common.Helpers.ResourceParameters;
using Fittify.DataModelRepositories;
using Fittify.DataModelRepositories.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace Fittify.Api.OfmRepository
{
    public class AsyncGetOfmCollectionIncludeByNameSearch<TCrudRepository, TEntity, TOfmForGet, TId> : AsyncGetOfmById<TCrudRepository, TEntity, TOfmForGet, TId>, IAsyncGetOfmCollectionByNameSearch<TOfmForGet>
        where TOfmForGet : class, IEntityUniqueIdentifier<TId>
        where TId : struct
        where TEntity : class, IEntityName<TId>
        where TCrudRepository : class, IAsyncGetCollectionForEntityName<TEntity, TId>
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
            var ofmForGetCollectionQueryResult = new OfmForGetCollectionQueryResult<TOfmForGet>();

            ofmForGetCollectionQueryResult = await AsyncGetOfmGuardClause.ValidateResourceParameters(ofmForGetCollectionQueryResult, resourceParameters);
            if (ofmForGetCollectionQueryResult.ErrorMessages.Count > 0)
            {
                return ofmForGetCollectionQueryResult;
            }
            
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

            ofmForGetCollectionQueryResult.ReturnedTOfmForGetCollection.OfmForGets = Mapper.Map<List<TEntity>, List<TOfmForGet>>(pagedListEntityCollection);

            return ofmForGetCollectionQueryResult;
        }
    }
}
