using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Fittify.Api.Helpers;
using Fittify.Common;
using Fittify.DataModelRepositories;
using Fittify.DataModels.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Fittify.Api.OfmRepository
{
    public class GppdOfm<TCrudRepository, TEntity, TOfmForGet, TOfmForPost, TOfmForPatch, TId> :
        Controller,
        IAsyncGppdOfm<TId, TOfmForGet, TOfmForPost, TOfmForPatch>
        
        where TCrudRepository : AsyncCrud<TEntity,TId> 
        where TEntity : class, IUniqueIdentifierDataModels<TId>
        where TOfmForGet : class
        where TOfmForPost : class
        where TOfmForPatch : class, new()
        where TId : struct
    {
        private TEntity _cachedEntity;
        private readonly TCrudRepository _repo;
        public GppdOfm(TCrudRepository repository)
        {
            _repo = repository;
        }

        public GppdOfm()
        {
            
        }

        public virtual async Task<bool> DoesEntityExist(TId id)
        {
            // Todo this async lacks await
            return  _repo.DoesEntityExist(id).Result;
        }
        
        public virtual async Task<ICollection<TOfmForGet>> GetAll()
        {
            // Todo this async lacks await
            var entityCollection = _repo.GetAll().ToList();
            var ofmCollection = Mapper.Map<List<TEntity>, List<TOfmForGet>>(entityCollection);
            return ofmCollection;
        }

        public virtual async Task<TOfmForGet> GetById(TId id)
        {
            var entity = await _repo.GetById(id);
            var ofm = Mapper.Map<TEntity, TOfmForGet>(entity);
            return ofm;
        }
        
        public virtual async Task<TOfmForGet> Post(TOfmForPost ofmForPost)
        {
            var entity = Mapper.Map<TOfmForPost, TEntity>(ofmForPost);
            try
            {
                entity = await _repo.Create(entity);
            }
            catch (Exception e)
            {
                var msg = e.Message;
            }
            
            return Mapper.Map<TEntity, TOfmForGet>(entity);
        }
        
        public virtual async Task Delete(TId id)
        {
            await _repo.Delete(id);
        }

        private async Task<StatusCodeResult> SaveChanges()
        {
            // Todo decide if SavingToContext should take place here or in data layer
            if (!await _repo.SaveContext())
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            return Ok();
        }

        public virtual async Task<TOfmForPatch> GetByIdOfmForPatch(TId id)
        {
            _cachedEntity = await _repo.GetById(id);
            var ofmForPatch = Mapper.Map<TEntity, TOfmForPatch>(_cachedEntity);
            return ofmForPatch;
        }

        public async Task<TOfmForGet> UpdatePartially(TOfmForPatch ofmForPatch)
        {
            Mapper.Map(ofmForPatch, _cachedEntity);
            var entity = await _repo.Update(_cachedEntity);
            return Mapper.Map<TEntity, TOfmForGet>(entity);
            
        }
        
    }
}
