﻿using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Fittify.Api.Helpers;
using Fittify.Api.Helpers.Extensions;
using Fittify.Common;
using Fittify.Common.Helpers.ResourceParameters;
using Fittify.DataModelRepositories;
using Fittify.DataModelRepositories.Owned;
using Fittify.DataModelRepositories.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace Fittify.Api.OfmRepository.Unowned
{
    public class AsyncGetOfmCollection<TCrudRepository, TEntity, TOfmForGet, TId> : IAsyncGetOfmCollection<TOfmForGet>
        where TOfmForGet : class, IEntityUniqueIdentifier<TId>
        where TId : struct
        where TEntity : class, IEntityUniqueIdentifier<TId>
        where TCrudRepository : class, IAsyncCrudOwned<TEntity, TId>, IAsyncGetCollection<TEntity, TId>
    {
        protected readonly TCrudRepository Repo;
        protected readonly IActionDescriptorCollectionProvider Adcp;
        protected readonly IUrlHelper UrlHelper;
        protected readonly IPropertyMappingService PropertyMappingService;
        protected readonly ITypeHelperService TypeHelperService;
        protected readonly HateoasLinkFactory<TId> HateoasLinkFactory;
        protected readonly string ShortPascalCasedControllerName;
        protected readonly AsyncGetOfmGuardClauses<TOfmForGet, TId> AsyncGetOfmGuardClause;
        protected readonly Controller Controller;

        public AsyncGetOfmCollection(TCrudRepository repository,
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
            HateoasLinkFactory = new HateoasLinkFactory<TId>(urlHelper, controller.GetType().Name);
            ShortPascalCasedControllerName = controller.GetType().Name.ToShortPascalCasedControllerNameOrDefault();
            AsyncGetOfmGuardClause = new AsyncGetOfmGuardClauses<TOfmForGet, TId>(TypeHelperService);
            Controller = controller;
        }

        public virtual async Task<OfmForGetCollectionQueryResult<TOfmForGet>> GetCollection(IResourceParameters resourceParameters)
        {
            var ofmForGetCollectionQueryResult = new OfmForGetCollectionQueryResult<TOfmForGet>();

            ofmForGetCollectionQueryResult = await AsyncGetOfmGuardClause.ValidateResourceParameters(ofmForGetCollectionQueryResult, resourceParameters);
            if (ofmForGetCollectionQueryResult.ErrorMessages.Count > 0)
            {
                return ofmForGetCollectionQueryResult;
            }
            
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
    }
}
