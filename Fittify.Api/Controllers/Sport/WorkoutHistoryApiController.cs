using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Fittify.Api.Controllers.HttpMethodInterfaces;
using Fittify.Api.OfmRepository;
using Fittify.Api.OuterFacingModels.Sport.Get;
using Fittify.Api.OuterFacingModels.Sport.Patch;
using Fittify.Api.OuterFacingModels.Sport.Post;
using Fittify.DataModelRepositories;
using Fittify.DataModelRepositories.Repository.Sport;
using Fittify.DataModels.Models.Sport;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace Fittify.Api.Controllers.Sport
{
    [Route("api/workouthistories")]
    public class WorkoutHistoryApiController :
        Controller,
        IAsyncGppdForHttp<int, WorkoutHistoryOfmForPost, WorkoutHistoryOfmForPatch>
    {
        private readonly GppdOfm<WorkoutHistoryRepository, WorkoutHistory, WorkoutHistoryOfmForGet, WorkoutHistoryOfmForPost, WorkoutHistoryOfmForPatch, int> _gppdForHttpMethods;
        private readonly WorkoutHistoryRepository _repo;
        private readonly GetMoreForHttpIntId<WorkoutHistoryRepository, WorkoutHistory> _getMoreForIntId;

        public WorkoutHistoryApiController(FittifyContext fittifyContext)
        {
            _repo = new WorkoutHistoryRepository(fittifyContext);
            _gppdForHttpMethods = new GppdOfm<WorkoutHistoryRepository, WorkoutHistory, WorkoutHistoryOfmForGet, WorkoutHistoryOfmForPost, WorkoutHistoryOfmForPatch, int>(_repo);
            _getMoreForIntId = new GetMoreForHttpIntId<WorkoutHistoryRepository, WorkoutHistory>(_repo);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var allEntites = await _gppdForHttpMethods.GetAll();
            var allOfmForGet = Mapper.Map<ICollection<WorkoutHistoryOfmForGet>>(allEntites);
            return new JsonResult(allOfmForGet);
        }

        [HttpGet("{id:int}", Name = "GetWorkoutHistoryById")]
        public async Task<IActionResult> GetById(int id)
        {
            var entity = await _repo.GetById(id);
            var ofmForGet = new List<WorkoutHistoryOfmForGet>() {Mapper.Map<WorkoutHistoryOfmForGet>(entity) };
            return new JsonResult(ofmForGet);
        }

        [HttpPost]
        public async Task<CreatedAtRouteResult> Post([FromBody] WorkoutHistoryOfmForPost ofmForPost)
        {
            var ofmForGet = await _gppdForHttpMethods.Post(ofmForPost);
            var result = CreatedAtRoute(routeName: "GetWorkoutHistoryById", routeValues: new { id = ofmForGet.Id }, value: ofmForGet);
            return result;
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _gppdForHttpMethods.Delete(id);
            return NoContent();
        }

        [HttpGet("workout/{workoutId:int}")]
        public async Task<IActionResult> GetCollectionByFkWorkoutId(int workoutId)
        {
            var collectionWorkoutHistories = await _repo.GetCollectionByFkWorkoutId(workoutId);
            var ofm = Mapper.Map<List<WorkoutHistory>, List<WorkoutHistoryOfmForGet>>(collectionWorkoutHistories);
            return new JsonResult(ofm);
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdatePartially(int id, [FromBody] JsonPatchDocument<WorkoutHistoryOfmForPatch> jsonPatchDocument)
        {
            try
            {
                // Get entity with original values from context
                var entity = _repo.GetById(id).Result;

                if (entity.DateTimeStartEnd == null)
                {
                    entity.DateTimeStartEnd = new DateTimeStartEnd();
                    entity.DateTimeStartEnd.WorkoutHistoryId = entity.Id;
                }

                // Convert entity to ofm
                var ofmPppToPatch = Mapper.Map<WorkoutHistoryOfmForPatch>(entity);

                // Apply new values from jsonPatchDocument to ofm (the ofm that was just created based on fresh entity from context)
                jsonPatchDocument.ApplyTo(ofmPppToPatch);

                // Convert ofm with new values back to entity (by overriding changed entity field values)
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
