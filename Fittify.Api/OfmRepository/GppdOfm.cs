using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
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
        where TOfmForPatch : class
        where TId : struct
    {
        private readonly TCrudRepository _repo;
        public GppdOfm(TCrudRepository repository)
        {
            _repo = repository;
        }

        public GppdOfm()
        {
            
        }
        
        public virtual async Task<ICollection<TOfmForGet>> GetAll()
        {
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
            entity = await _repo.Create(entity);
            
            return Mapper.Map<TEntity, TOfmForGet>(entity);
        }
        
        public virtual async Task Delete(TId id)
        {
            await _repo.Delete(id);
        }

        private async Task<StatusCodeResult> SaveChanges()
        {
            if (!await _repo.SaveContext())
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            return Ok();
        }
        
        public async Task<TOfmForGet> UpdatePartially(TId id, JsonPatchDocument<TOfmForPatch> jsonPatchDocument)
        {
            try
            {
                // Get entity with original values from context
                var entity = _repo.GetById(id).Result;

                // Convert entity to ofm
                var ofmPppToPatch = Mapper.Map<TOfmForPatch>(entity);

                // Apply new values from jsonPatchDocument to ofm (the ofm that was just created based on fresh entity from context)
                jsonPatchDocument.ApplyTo(ofmPppToPatch);

                // Convert ofm with new values back to entity (by overriding entity field values)
                Mapper.Map(ofmPppToPatch, entity);

                // Update entity in context
                entity = await _repo.Update(entity);

                // returning the patched ofm as response
                return Mapper.Map<TOfmForGet>(entity);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        
    }
}
