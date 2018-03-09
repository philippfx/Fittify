using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Fittify.Api.Helpers;
using Fittify.Api.Services;
using Fittify.Common;
using Fittify.Common.Helpers.ResourceParameters;
using Fittify.DataModelRepositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace Fittify.Api.OfmRepository
{
    public class AsyncGetOfmByDateTimeStartEnd<TCrudRepository, TEntity, TOfmForGet, TId> : AsyncGetOfm<TCrudRepository, TEntity, TOfmForGet, TId>, IAsyncGetOfmByDateTimeStartEnd<TOfmForGet, TId>
        where TOfmForGet : class
        where TId : struct
        where TEntity : class, IEntityDateTimeStartEnd<TId>
        where TCrudRepository : class, IAsyncCrudForEntityDateTimeStartEnd<TEntity, TId>
    {
        public AsyncGetOfmByDateTimeStartEnd(TCrudRepository repository,
            IUrlHelper urlHelper,
            IActionDescriptorCollectionProvider actionDescriptorCollectionProvider,
            IPropertyMappingService propertyMappingService,
            ITypeHelperService typeHelperService)
            : base(repository, urlHelper, actionDescriptorCollectionProvider, propertyMappingService, typeHelperService)
        {

        }

        public virtual async Task<IEnumerable<TOfmForGet>> GetAllPagedAndDateTimeStartEnd(IDateTimeStartEndResourceParameters resourceParameters, ControllerBase controllerBase)
        {
            //// Todo this async lacks await
            var pagedListEntityCollection = Repo.GetAllPagedDateTimeStartEnd(resourceParameters);

            var previousPageLink = pagedListEntityCollection.HasPrevious ?
                RsourceUriFactory.CreateResourceUriForIResourceParameters(resourceParameters,
                    UrlHelper,
                    ResourceUriType.PreviousPage) : null;

            var nextPageLink = pagedListEntityCollection.HasNext ?
                RsourceUriFactory.CreateResourceUriForIResourceParameters(resourceParameters,
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
