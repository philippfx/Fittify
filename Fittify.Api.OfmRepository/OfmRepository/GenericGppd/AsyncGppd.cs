using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Fittify.Api.OfmRepository.Helpers;
using Fittify.Api.OfmRepository.OfmResourceParameters;
using Fittify.Common;
using Fittify.Common.Helpers;
using Fittify.DataModelRepository.Repository;
using Fittify.DataModelRepository.ResourceParameters;
using IPropertyMappingService = Fittify.Api.OfmRepository.Services.IPropertyMappingService;
using ITypeHelperService = Fittify.Api.OfmRepository.Services.ITypeHelperService;

namespace Fittify.Api.OfmRepository.OfmRepository.GenericGppd
{
    public class AsyncGppd<TEntity, TOfmForGet, TOfmForPost, TOfmForPatch, TId, TOfmResourceParameters, TEntityResourceParameters> 
        : IAsyncGppd<TOfmForGet, TOfmForPost, TOfmForPatch, TId, TOfmResourceParameters>
        where TEntity : class, IEntityUniqueIdentifier<TId>
        where TOfmForGet : class, IEntityUniqueIdentifier<TId>
        where TOfmForPost : class
        where TOfmForPatch : class
        where TId : struct
        where TOfmResourceParameters : OfmResourceParametersBase
        where TEntityResourceParameters : EntityResourceParametersBase, IEntityOwner
    {
        protected readonly IAsyncCrud<TEntity, TId,  TEntityResourceParameters> Repo;
        protected readonly IPropertyMappingService PropertyMappingService;
        protected readonly ITypeHelperService TypeHelperService;
        protected readonly AsyncGetOfmGuardClauses<TOfmForGet, TId> AsyncGetOfmGuardClause;

        public AsyncGppd(IAsyncCrud<TEntity, TId, TEntityResourceParameters> repository,
            IPropertyMappingService propertyMappingService,
            ITypeHelperService typeHelperService)
        {
            Repo = repository;
            PropertyMappingService = propertyMappingService;
            TypeHelperService = typeHelperService;
            AsyncGetOfmGuardClause = new AsyncGetOfmGuardClauses<TOfmForGet, TId>(TypeHelperService);
        }

        public async Task<OfmForGetQueryResult<TOfmForGet>> GetById(TId id, string fields)
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

        public async Task<OfmForGetCollectionQueryResult<TOfmForGet>> GetCollection(TOfmResourceParameters ofmResourceParameters, Guid ownerGuid)
        {
            var ofmForGetCollectionQueryResult = new OfmForGetCollectionQueryResult<TOfmForGet>();

            var entityResourceParameters = Mapper.Map<TEntityResourceParameters>(ofmResourceParameters);
            entityResourceParameters.OwnerGuid = ownerGuid;
            entityResourceParameters.OrderBy = ofmResourceParameters.OrderBy.ToEntitySortFields(PropertyMappingService.GetPropertyMapping<TOfmForGet, TEntity>());

            ofmForGetCollectionQueryResult = await AsyncGetOfmGuardClause.ValidateResourceParameters(ofmForGetCollectionQueryResult, ofmResourceParameters);
            if (ofmForGetCollectionQueryResult.ErrorMessages.Count > 0)
            {
                return ofmForGetCollectionQueryResult;
            }

            var pagedListEntityCollection = Repo.GetCollection(entityResourceParameters).CopyPropertyValuesTo(ofmForGetCollectionQueryResult);
            
            ofmForGetCollectionQueryResult.ReturnedTOfmForGetCollection.OfmForGets = Mapper.Map<List<TEntity>, List<TOfmForGet>>(pagedListEntityCollection);
            return ofmForGetCollectionQueryResult;
        }

        public async Task<TOfmForGet> Post(TOfmForPost ofmForPost, Guid ownerGuid)
        {
            var entity = Mapper.Map<TOfmForPost, TEntity>(ofmForPost);
            entity = await Repo.Create(entity, ownerGuid);
            var ofm = Mapper.Map<TEntity, TOfmForGet>(entity);
            return ofm;
        }

        private TEntity _cachedEntity;
        public virtual async Task<TOfmForPatch> GetByIdOfmForPatch(TId id)
        {
            _cachedEntity = await Repo.GetById(id);
            var ofmForPatch = Mapper.Map<TEntity, TOfmForPatch>(_cachedEntity);
            return ofmForPatch;
        }

        public async Task<TOfmForGet> UpdatePartially(TOfmForPatch ofmForPatch)
        {
            _cachedEntity = Mapper.Map(ofmForPatch, _cachedEntity);
            var entity = await Repo.Update(_cachedEntity);
            return Mapper.Map<TEntity, TOfmForGet>(entity);
        }

        public async Task<OfmDeletionQueryResult<TId>> Delete(TId id)
        {
            var entityDeletionResult = await Repo.Delete(id);

            var ofmDeletionQueryResult = new OfmDeletionQueryResult<TId>();

            ofmDeletionQueryResult.DidEntityExist = entityDeletionResult.DidEntityExist;
            ofmDeletionQueryResult.IsDeleted = entityDeletionResult.IsDeleted;

            return ofmDeletionQueryResult;
        }
    }
}
