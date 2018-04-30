using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Fittify.Api.Helpers;
using Fittify.Api.OfmRepository.Owned;
using Fittify.Api.OuterFacingModels.Sport.Get;
using Fittify.Common.Helpers;
using Fittify.Common.Helpers.ResourceParameters.Sport;
using Fittify.DataModelRepositories.Repository.Sport;
using Fittify.DataModelRepositories.Services;
using Fittify.DataModels.Models.Sport;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace Fittify.Api.OfmRepository.GetCollection.Sport
{
    public class AsyncGetOfmForCardioSet : AsyncGetOfmById<CardioSetRepository, CardioSet, CardioSetOfmForGet, int>,
        IAsyncGetOfmCollection<CardioSetOfmForGet, CardioSetResourceParameters>
    {
        public AsyncGetOfmForCardioSet(CardioSetRepository repository,
            IUrlHelper urlHelper,
            IActionDescriptorCollectionProvider actionDescriptorCollectionProvider,
            IPropertyMappingService propertyMappingService,
            ITypeHelperService typeHelperService,
            Controller controller)
            : base(repository, urlHelper, actionDescriptorCollectionProvider, propertyMappingService, typeHelperService, controller)
        {
        }

        public virtual async Task<OfmForGetCollectionQueryResult<CardioSetOfmForGet>> GetCollection(CardioSetResourceParameters resourceParameters, Guid ownerGuid)
        {
            var ofmForGetCollectionQueryResult = new OfmForGetCollectionQueryResult<CardioSetOfmForGet>();

            ofmForGetCollectionQueryResult = await AsyncGetOfmGuardClause.ValidateResourceParameters(ofmForGetCollectionQueryResult, resourceParameters);
            if (ofmForGetCollectionQueryResult.ErrorMessages.Count > 0)
            {
                return ofmForGetCollectionQueryResult;
            }

            var pagedListEntityCollection = Repo.GetCollection(resourceParameters, ownerGuid).CopyPropertyValuesTo(ofmForGetCollectionQueryResult);

            // Todo Maybe refactor to a type safe class instead of anonymous
            var paginationMetadata = new
            {
                totalCount = pagedListEntityCollection.TotalCount,
                pageSize = pagedListEntityCollection.PageSize,
                currentPage = pagedListEntityCollection.CurrentPage,
                totalPages = pagedListEntityCollection.TotalPages
            };

            // Todo: Refactor to class taking controller as input instead of only this method
            Controller.Response.Headers.Add("X-Pagination",
            Newtonsoft.Json.JsonConvert.SerializeObject(paginationMetadata));

            ofmForGetCollectionQueryResult.ReturnedTOfmForGetCollection.OfmForGets = Mapper.Map<List<CardioSet>, List<CardioSetOfmForGet>>(pagedListEntityCollection);

            return ofmForGetCollectionQueryResult;
        }
    }
}
