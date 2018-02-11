using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Fittify.Api.Controllers.HttpMethodInterfaces;
using Fittify.Api.OfmRepository;
using Fittify.Api.OuterFacingModels.Sport.Get;
using Fittify.Api.OuterFacingModels.Sport.Patch;
using Fittify.Api.OuterFacingModels.Sport.Post;
using Fittify.Common.Helpers;
using Fittify.DataModelRepositories;
using Fittify.DataModelRepositories.Repository.Sport;
using Fittify.DataModels.Models.Sport;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace Fittify.Api.Controllers.Sport
{
    [Route("api/exercise-histories")]
    public class ExerciseHistoryApiController :
        Controller,
        IAsyncGppdForHttp<int, ExerciseHistoryOfmForPost, ExerciseHistoryOfmForPatch>
    {
        private readonly GppdOfm<ExerciseHistoryRepository, ExerciseHistory, ExerciseHistoryOfmForGet, ExerciseHistoryOfmForPost, ExerciseHistoryOfmForPatch, int> _gppdForHttpMethods;
        private readonly ExerciseHistoryRepository _repo;

        public ExerciseHistoryApiController(FittifyContext fittifyContext,
            IActionDescriptorCollectionProvider adcProvider,
            IUrlHelper urlHelper)
        {
            _repo = new ExerciseHistoryRepository(fittifyContext);
            _gppdForHttpMethods = new GppdOfm<ExerciseHistoryRepository, ExerciseHistory, ExerciseHistoryOfmForGet, ExerciseHistoryOfmForPost, ExerciseHistoryOfmForPatch, int>(_repo, urlHelper, adcProvider);
        }

        [HttpGet("{id:int}", Name = "GetExerciseHistoryById")]
        public async Task<IActionResult> GetById(int id)
        {
            var entity = await _repo.GetById(id);
            var ofmForGet = new List<ExerciseHistoryOfmForGet>() { Mapper.Map<ExerciseHistoryOfmForGet>(entity) };
            return new JsonResult(ofmForGet);
        }

        [HttpGet( Name = "GetAllExerciseHistoriesByRangeOfIds")]
        public async Task<IActionResult> GetAll()
        {
            var allEntites = await _gppdForHttpMethods.GetAll();
            var allOfmForGet = Mapper.Map<IEnumerable<ExerciseHistoryOfmForGet>>(allEntites);
            return new JsonResult(allOfmForGet);
        }

        [HttpGet("range/{inputString}", Name = "GetExerciseHistoriesByRangeOfIds")]
        public async Task<IActionResult> GetByRangeOfIds(string inputString)
        {
            var entityCollection = await _repo.GetByCollectionOfIds(RangeString.ToCollectionOfId(inputString));
            var ofmCollection = Mapper.Map<List<ExerciseHistory>, List<ExerciseHistoryOfmForGet>>(entityCollection.ToList());
            return Ok(ofmCollection);
        }

        [HttpPost("new")]
        public async Task<IActionResult> Post([FromBody] ExerciseHistoryOfmForPost ofmForPost)
        {
            var ofmForGet = await _gppdForHttpMethods.Post(ofmForPost);
            var result = CreatedAtRoute(routeName: "GetExerciseHistoriesByRangeOfIds",
                routeValues: new { inputString = ofmForGet.Id }, value: ofmForGet);
            return result;
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _gppdForHttpMethods.Delete(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                var msg = ex.Message;
            }

            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        [HttpGet("workouthistory/{workouthistoryId:int}")]
        public async Task<IActionResult> GetCollectionByFkWorkoutHistoryId(int workouthistoryId)
        {
            IEnumerable<ExerciseHistoryOfmForGet> ofm = null;
            try
            {
                var collectionExerciseHistories = await _repo.GetCollectionByFkWorkoutHistoryId(workouthistoryId);
                ofm = Mapper
                    .Map<IEnumerable<ExerciseHistory>, IEnumerable<ExerciseHistoryOfmForGet>>(collectionExerciseHistories);
            }
            catch (Exception e)
            {
                var msg = e.Message;
            }
            return new JsonResult(ofm);
        }

        [HttpPatch("{id:int}")]
        public async Task<IActionResult> UpdatePartially(int id, JsonPatchDocument<ExerciseHistoryOfmForPatch> jsonPatchDocument)
        {
            throw new NotImplementedException();
            //var ofmForGet = await _gppdForHttpMethods.UpdatePartially(id, jsonPatchDocument);
            //return new JsonResult(ofmForGet);
        }
    }
}
