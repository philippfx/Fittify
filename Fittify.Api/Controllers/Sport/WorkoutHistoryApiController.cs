using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using AutoMapper;
using Fittify.Api.Controllers.HttpMethodInterfaces;
using Fittify.Api.Extensions;
using Fittify.Api.Helpers;
using Fittify.Api.OfmRepository;
using Fittify.Api.OuterFacingModels.Sport.Get;
using Fittify.Api.OuterFacingModels.Sport.Patch;
using Fittify.Api.OuterFacingModels.Sport.Post;
using Fittify.Api.Services;
using Fittify.Common.Extensions;
using Fittify.Common.Helpers;
using Fittify.Common.Helpers.ResourceParameters;
using Fittify.DataModelRepositories;
using Fittify.DataModelRepositories.Repository.Sport;
using Fittify.DataModels.Models.Sport;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace Fittify.Api.Controllers.Sport
{
    [Route("api/workouthistories")]
    public class WorkoutHistoryApiController :
        Controller,
        IAsyncGppdForHttp<int, WorkoutHistoryOfmForPost, WorkoutHistoryOfmForPatch>
    {
        private readonly AsyncPostPatchDeleteOfm<WorkoutHistoryRepository, WorkoutHistory, WorkoutHistoryOfmForGet, WorkoutHistoryOfmForPost, WorkoutHistoryOfmForPatch, int> _asyncPostPatchDeleteForHttpMethods;
        private readonly IAsyncGetOfmByDateTimeStartEnd<WorkoutHistoryOfmForGet, int> _asyncGetOfm;
        private readonly WorkoutHistoryRepository _repo;
        private readonly string _shortCamelCasedControllerName;

        public WorkoutHistoryApiController(FittifyContext fittifyContext,
            IActionDescriptorCollectionProvider adcProvider,
            IUrlHelper urlHelper,
            IPropertyMappingService propertyMappingService)
        {
            _repo = new WorkoutHistoryRepository(fittifyContext);
            _asyncPostPatchDeleteForHttpMethods = new AsyncPostPatchDeleteOfm<WorkoutHistoryRepository, WorkoutHistory, WorkoutHistoryOfmForGet, WorkoutHistoryOfmForPost, WorkoutHistoryOfmForPatch, int>(_repo, urlHelper, adcProvider);
            _asyncGetOfm = new AsyncGetOfmByDateTimeStartEnd<WorkoutHistoryRepository, WorkoutHistory, WorkoutHistoryOfmForGet, int>(_repo, urlHelper, adcProvider, propertyMappingService);
            _shortCamelCasedControllerName = nameof(CategoryApiController).ToShortCamelCasedControllerNameOrDefault();
        }

        [HttpGet("{id:int}", Name = "GetWorkoutHistoryById")]
        public async Task<IActionResult> GetById(int id)
        {
            var entity = await _repo.GetById(id);
            var ofmForGet = new List<WorkoutHistoryOfmForGet>() { Mapper.Map<WorkoutHistoryOfmForGet>(entity) };
            return new JsonResult(ofmForGet);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var allEntites = await _asyncGetOfm.GetAll();
            var allOfmForGet = Mapper.Map<IEnumerable<WorkoutHistoryOfmForGet>>(allEntites);
            return new JsonResult(allOfmForGet);
        }

        [HttpGet("pagedanddatetimestartend", Name = "GetAllPagedAndDateTimeStartEndWorkoutHistories")]
        public async Task<IActionResult> GetAllPagedAndDateTimeStartEnd(DateTimeStartEndResourceParameters resourceParameters)
        {
            var allEntites = await _asyncGetOfm.GetAllPagedAndDateTimeStartEnd(resourceParameters, this);

            var allOfmForGet = Mapper.Map<IEnumerable<CategoryOfmForGet>>(allEntites).ToList();
            //allOfmForGet = new Collection<CategoryOfmForGet>(); // Todo mock "not found" as query paramter 
            if (allOfmForGet.Count == 0)
            {
                ModelState.AddModelError(_shortCamelCasedControllerName, $"No {_shortCamelCasedControllerName.ToPlural()} found");
                return new EntityNotFoundObjectResult(ModelState);
            }
            return new JsonResult(allOfmForGet);
        }

        [HttpGet("range/{inputString}", Name = "GetWorkoutHistoriesByRangeOfIds")]
        public async Task<IActionResult> GetByRangeOfIds(string inputString)
        {
            var entityCollection = await _repo.GetByCollectionOfIds(RangeString.ToCollectionOfId(inputString));
            var ofmCollection = Mapper.Map<List<WorkoutHistory>, List<WorkoutHistoryOfmForGet>>(entityCollection.ToList());
            return Ok(ofmCollection);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] WorkoutHistoryOfmForPost ofmForPost)
        {
            var ofmForGet = await _asyncPostPatchDeleteForHttpMethods.Post(ofmForPost);
            var result = CreatedAtRoute(routeName: "GetWorkoutHistoryById", routeValues: new { id = ofmForGet.Id }, value: ofmForGet);
            return result;
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _asyncPostPatchDeleteForHttpMethods.Delete(id);
            return NoContent();
        }

        [HttpDelete("mydelete/{id:int}")]
        public async Task<IActionResult> MyDelete(int id)
        {
            var methodBase = typeof(ExerciseHistoryApiController).GetMethod("GetByRangeOfIds");

            var attribute = (HttpGetAttribute)methodBase.GetCustomAttributes(typeof(HttpGetAttribute), true)[0];
            
            var blockingOfmForGetLists = _asyncPostPatchDeleteForHttpMethods.MyDelete(id).Result;
            if (blockingOfmForGetLists.Count != 0)
            {
                foreach (var tuple in blockingOfmForGetLists)
                {
                    ModelState.AddModelError(_shortCamelCasedControllerName, tuple);
                }
            }

            if (!ModelState.IsValid)
            {
                return new UnprocessableEntityObjectResult(ModelState);
            }
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
