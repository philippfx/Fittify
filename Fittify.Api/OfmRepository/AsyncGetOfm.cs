using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Fittify.Api.Extensions;
using Fittify.Api.Helpers;
using Fittify.Api.Services;
using Fittify.Common;
using Fittify.Common.Helpers.ResourceParameters;
using Fittify.DataModelRepositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace Fittify.Api.OfmRepository
{
    public class AsyncGetOfm<TCrudRepository, TEntity, TOfmForGet, TId> : IAsyncGetOfmById<TOfmForGet, TId>
        where TOfmForGet : class, IEntityUniqueIdentifier<TId>
        where TId : struct
        where TEntity : class, IEntityUniqueIdentifier<TId>
        where TCrudRepository : class, IAsyncCrud<TEntity, TId>
    {
        protected readonly TCrudRepository Repo;
        protected readonly IActionDescriptorCollectionProvider Adcp;
        protected readonly IUrlHelper UrlHelper;
        protected readonly IPropertyMappingService PropertyMappingService;
        protected readonly ITypeHelperService TypeHelperService;
        protected readonly HateoasLinkFactory<TOfmForGet, TId> HateoasLinkFactory;
        protected readonly string ShortPascalCasedControllerName;
        protected readonly AsyncGetOfmGuardClauses<TOfmForGet, TId> AsyncGetOfmGuardClause;
        protected readonly Controller Controller;

        public AsyncGetOfm(TCrudRepository repository,
            IUrlHelper urlHelper,
            IActionDescriptorCollectionProvider actionDescriptorCollectionProvider,
            IPropertyMappingService propertyMappingService,
            ITypeHelperService typeHelperService,
            Controller controller)
        {
            Repo = repository;
            Adcp = actionDescriptorCollectionProvider;
            UrlHelper = urlHelper;
            PropertyMappingService = propertyMappingService;
            TypeHelperService = typeHelperService;
            HateoasLinkFactory = new HateoasLinkFactory<TOfmForGet, TId>(urlHelper, controller.GetType().Name);
            ShortPascalCasedControllerName = controller.GetType().Name.ToShortPascalCasedControllerNameOrDefault();
            AsyncGetOfmGuardClause = new AsyncGetOfmGuardClauses<TOfmForGet, TId>(TypeHelperService);
            Controller = controller;
        }

        public virtual async Task<OfmForGetCollectionQueryResult<TOfmForGet>> GetCollection(IResourceParameters resourceParameters)
        {
            var ofmForGetCollectionQueryResult = new OfmForGetCollectionQueryResult<TOfmForGet>();

            ofmForGetCollectionQueryResult = await AsyncGetOfmGuardClause.ValidateGetCollection(ofmForGetCollectionQueryResult, resourceParameters);
            if (ofmForGetCollectionQueryResult.ErrorMessages.Count > 0)
            {
                return ofmForGetCollectionQueryResult;
            }

            //// Todo this async lacks await
            var pagedListEntityCollection = Repo.GetCollection(resourceParameters);
            
            var previousPageLink = pagedListEntityCollection.HasPrevious ?
                ResourceUriFactory.CreateResourceUriForIResourceParameters(resourceParameters,
                    UrlHelper,
                    ResourceUriType.PreviousPage, ShortPascalCasedControllerName) : null;

            var nextPageLink = pagedListEntityCollection.HasNext ?
                ResourceUriFactory.CreateResourceUriForIResourceParameters(resourceParameters,
                    UrlHelper,
                    ResourceUriType.NextPage, ShortPascalCasedControllerName) : null;

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
            Controller.Response.Headers.Add("X-Pagination",
                Newtonsoft.Json.JsonConvert.SerializeObject(paginationMetadata));

            ofmForGetCollectionQueryResult.ReturnedTOfmForGetCollection.OfmForGets = Mapper.Map<List<TEntity>, List<TOfmForGet>>(pagedListEntityCollection);
            return ofmForGetCollectionQueryResult;
        }

        public virtual async Task<TOfmForGet> GetById(TId id)
        {
            var entity = await Repo.GetById(id);
            var ofm = Mapper.Map<TEntity, TOfmForGet>(entity);
            return ofm;
        }
        
        public virtual async Task<OfmForGetQueryResult<TOfmForGet>> GetById(TId id, string fields)
        {
            var ofmForGetResult = new OfmForGetQueryResult<TOfmForGet>();
            ofmForGetResult = await AsyncGetOfmGuardClause.ValidateGetById(ofmForGetResult, fields);

            if (ofmForGetResult.ErrorMessages.Count > 0)
            {
                return ofmForGetResult;
            }

            var entity = await Repo.GetById(id);
            ofmForGetResult.ReturnedTOfmForGet = Mapper.Map<TEntity, TOfmForGet>(entity);
            return ofmForGetResult;
        }
    }
}
