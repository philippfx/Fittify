using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Fittify.Api.Controllers.HttpMethodInterfaces;
using Fittify.Common;
using Fittify.DataModelRepositories;
using Fittify.DataModels.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace Fittify.Api.Controllers
{

    public class GppdForHttpMethods<TCrudRepository, TEntity, TOfmForGet, TOfmForPost, TOfmForPatch, TId> :
        Controller,
        IAsyncGppdForHttp<TId, TOfmForPost, TOfmForPatch>
        
        where TCrudRepository : AsyncCrud<TEntity,TId> 
        where TEntity : class, IUniqueIdentifierDataModels<TId>
        where TOfmForGet : class
        where TOfmForPost : class
        where TOfmForPatch : class
        where TId : struct
    {
        private readonly TCrudRepository _repo;
        public GppdForHttpMethods(TCrudRepository repository)
        {
            _repo = repository;
        }

        public GppdForHttpMethods()
        {
            
        }
        
        public virtual async Task<IActionResult> GetAll()
        {
            var entityCollection = _repo.GetAll().ToList();
            var ofmCollection = Mapper.Map<List<TEntity>, List<TOfmForGet>>(entityCollection);
            return new JsonResult(ofmCollection);
        }

        public virtual async Task<IActionResult> GetById(TId id)
        {
            var entity = await _repo.GetById(id);
            var ofm = Mapper.Map<TEntity, TOfmForGet>(entity);
            return new JsonResult(ofm);
        }

        public virtual async Task<IActionResult> Post(TOfmForPost ofmForPost)
        {
            //var entity = Mapper.Map<TOfmForPost, TEntity>(ofmForPost);
            //entity = await _repo.Create(entity);
            //return new JsonResult(Mapper.Map<TEntity, TOfmForGet>(entity));

            throw new NotImplementedException();
        }
        
        public virtual async Task<IActionResult> Delete(TId id)
        {
            await _repo.Delete(id);
            return NoContent();
        }
        
        public async Task<IActionResult> UpdatePartially(TId id, JsonPatchDocument<TOfmForPatch> jsonPatchDocument)
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
                await _repo.Update(entity);

                // returning the patched ofm as response
                return new JsonResult(ofmPppToPatch);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
