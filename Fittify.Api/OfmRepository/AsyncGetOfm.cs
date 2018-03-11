using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Fittify.Api.Extensions;
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
    public class AsyncGetOfm<TCrudRepository, TEntity, TOfmForGet, TId> : IAsyncGetOfm<TOfmForGet, TId>
        where TOfmForGet : LinkedResourceBase, IEntityUniqueIdentifier<TId>
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

        public AsyncGetOfm(TCrudRepository repository,
            IUrlHelper urlHelper,
            IActionDescriptorCollectionProvider actionDescriptorCollectionProvider,
            IPropertyMappingService propertyMappingService,
            ITypeHelperService typeHelperService,
            string controllerName)
        {
            Repo = repository;
            Adcp = actionDescriptorCollectionProvider;
            UrlHelper = urlHelper;
            PropertyMappingService = propertyMappingService;
            TypeHelperService = typeHelperService;
            HateoasLinkFactory = new HateoasLinkFactory<TOfmForGet, TId>(urlHelper, controllerName);
        }
        

        public virtual async Task<IEnumerable<TOfmForGet>> GetAll()
        {
            // Todo this async lacks await
            var entityCollection = Repo.GetAll().ToList();
            var ofmCollection = Mapper.Map<List<TEntity>, List<TOfmForGet>>(entityCollection);
            return ofmCollection;
        }

        public virtual async Task<TOfmForGet> GetById(TId id)
        {
            var entity = await Repo.GetById(id);
            var ofm = Mapper.Map<TEntity, TOfmForGet>(entity);
            
            ofm = HateoasLinkFactory.CreateLinksForOfmForGet(ofm);
            return ofm;
        }

        public virtual async Task<OfmForGetQueryResult<TOfmForGet>> GetByIdDataShaped(TId id, string fields)
        {
            var ofmForGetResult = new OfmForGetQueryResult<TOfmForGet>();
            ofmForGetResult.ErrorMessages = new List<string>();
            IList<string> errorMessages = new List<string>();
            if (!TypeHelperService.TypeHasProperties<TOfmForGet>(fields, ref errorMessages))
            {
                // Todo ref unknown fields error messages
                ofmForGetResult.ErrorMessages.AddRange(errorMessages);
            }

            if (ofmForGetResult.ErrorMessages.Count > 0)
            {
                return ofmForGetResult;
            }

            var entity = await Repo.GetById(id);
            ofmForGetResult.ReturnedTOfmForGet = Mapper.Map<TEntity, TOfmForGet>(entity);

            ofmForGetResult.ReturnedTOfmForGet.Links = HateoasLinkFactory.CreateLinksForOfmForGet(id, fields).ToList();
            return ofmForGetResult;
        }

        // Todo Refactor improved search paramters to override virtual
        public virtual async Task<IEnumerable<TOfmForGet>> GetAllPaged(IResourceParameters resourceParameters, ControllerBase controllerBase)
        {
            //// Todo this async lacks await
            var pagedListEntityCollection = Repo.GetAllPaged(resourceParameters);

            var previousPageLink = pagedListEntityCollection.HasPrevious ?
                ResourceUriFactory.CreateResourceUriForIResourceParameters(resourceParameters,
                    UrlHelper,
                    ResourceUriType.PreviousPage) : null;

            var nextPageLink = pagedListEntityCollection.HasNext ?
                ResourceUriFactory.CreateResourceUriForIResourceParameters(resourceParameters,
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

        public virtual async Task<IEnumerable<TOfmForGet>> GetAllPagedAndOrdered(IResourceParameters resourceParameters, ControllerBase controllerBase)
        {
            //// Todo this async lacks await
            var pagedListEntityCollection = Repo.GetAllPagedAndOrdered(resourceParameters);

            var previousPageLink = pagedListEntityCollection.HasPrevious ?
                ResourceUriFactory.CreateResourceUriForIResourceParameters(resourceParameters,
                    UrlHelper,
                    ResourceUriType.PreviousPage) : null;

            var nextPageLink = pagedListEntityCollection.HasNext ?
                ResourceUriFactory.CreateResourceUriForIResourceParameters(resourceParameters,
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
    }
}
