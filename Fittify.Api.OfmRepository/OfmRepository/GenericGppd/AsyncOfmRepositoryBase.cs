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
using IPropertyMappingService = Fittify.Api.OfmRepository.Services.PropertyMapping.IPropertyMappingService;
using ITypeHelperService = Fittify.Api.OfmRepository.Services.TypeHelper.ITypeHelperService;

namespace Fittify.Api.OfmRepository.OfmRepository.GenericGppd
{
    public abstract class AsyncOfmRepositoryBase<TEntity, TOfmForGet, TOfmForPost, TOfmForPatch, TId, TOfmResourceParameters, TEntityResourceParameters> 
        : IAsyncOfmRepository<TOfmForGet, TOfmForPost, TOfmForPatch, TId, TOfmResourceParameters>
        where TEntity : class, IEntityUniqueIdentifier<TId>
        where TOfmForGet : class, IEntityUniqueIdentifier<TId>
        where TOfmForPost : class
        where TOfmForPatch : class
        where TId : struct
        where TOfmResourceParameters : OfmResourceParametersBase
        where TEntityResourceParameters : EntityResourceParametersBase, IEntityOwner, new()
    {
        protected readonly IAsyncCrud<TEntity, TId> Repo;
        protected readonly IPropertyMappingService PropertyMappingService;
        protected readonly ITypeHelperService TypeHelperService;
        protected readonly AsyncGetOfmGuardClauses<TOfmForGet, TId> AsyncGetOfmGuardClause;

        public AsyncOfmRepositoryBase(IAsyncCrud<TEntity, TId> repository,
            IPropertyMappingService propertyMappingService,
            ITypeHelperService typeHelperService)
        {
            Repo = repository;
            PropertyMappingService = propertyMappingService;
            TypeHelperService = typeHelperService;
            AsyncGetOfmGuardClause = new AsyncGetOfmGuardClauses<TOfmForGet, TId>(TypeHelperService);
        }

        public Task<bool> IsEntityOwner(TId id, Guid ownerGuid)
        {
            return Repo.IsEntityOwner(id, ownerGuid);
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

        public virtual async Task<OfmForGetCollectionQueryResult<TOfmForGet>> GetCollection(TOfmResourceParameters ofmResourceParameters, Guid ownerGuid)
        {
            var ofmForGetCollectionQueryResult = new OfmForGetCollectionQueryResult<TOfmForGet>();
            
            ofmForGetCollectionQueryResult = await AsyncGetOfmGuardClause.ValidateResourceParameters(ofmForGetCollectionQueryResult, ofmResourceParameters);
            if (ofmForGetCollectionQueryResult.ErrorMessages.Count > 0)
            {
                return ofmForGetCollectionQueryResult;
            }

            var entityResourceParameters = Mapper.Map<TEntityResourceParameters>(ofmResourceParameters);
            entityResourceParameters.OwnerGuid = ownerGuid;
            entityResourceParameters.OrderBy = ofmResourceParameters.OrderBy.ToEntityOrderBy(PropertyMappingService.GetPropertyMapping<TOfmForGet, TEntity>());

            var result = await Repo.GetPagedCollection(entityResourceParameters);
            var pagedListEntityCollection = result.CopyPropertyValuesTo(ofmForGetCollectionQueryResult);
            
            ofmForGetCollectionQueryResult.ReturnedTOfmForGetCollection.OfmForGets = Mapper.Map<List<TEntity>, List<TOfmForGet>>(pagedListEntityCollection);
            return ofmForGetCollectionQueryResult;
        }

        public virtual async Task<TOfmForGet> Post(TOfmForPost ofmForPost, Guid ownerGuid)
        {
            var entity = Mapper.Map<TOfmForPost, TEntity>(ofmForPost);
            entity = await Repo.Create(entity, ownerGuid);
            var ofm = Mapper.Map<TEntity, TOfmForGet>(entity);
            return ofm;
        }

        public TEntity CachedEntityForPatch;
        public virtual async Task<TOfmForPatch> GetByIdOfmForPatch(TId id)
        {
            CachedEntityForPatch = await Repo.GetById(id);
            var ofmForPatch = Mapper.Map<TEntity, TOfmForPatch>(CachedEntityForPatch);
            return ofmForPatch;
        }

        public virtual async Task<TOfmForGet> UpdatePartially(TOfmForPatch ofmForPatch)
        {
            if (CachedEntityForPatch == null)
            {
                throw new ArgumentNullException("An entity for patching has not been queried previously to updating it. Please use the method " + nameof(GetByIdOfmForPatch) + " to load the to-be-updated entity into the cache and then call " + nameof(UpdatePartially));
            }
            CachedEntityForPatch = Mapper.Map(ofmForPatch, CachedEntityForPatch);
            var entity = await Repo.Update(CachedEntityForPatch);
            return Mapper.Map<TEntity, TOfmForGet>(entity);
        }

        public virtual async Task<OfmDeletionQueryResult<TId>> Delete(TId id)
        {
            var entityDeletionResult = await Repo.Delete(id);

            var ofmDeletionQueryResult = new OfmDeletionQueryResult<TId>();

            ofmDeletionQueryResult.DidEntityExist = entityDeletionResult.DidEntityExist;
            ofmDeletionQueryResult.IsDeleted = entityDeletionResult.IsDeleted;

            return ofmDeletionQueryResult;
        }
    }
}
