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
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace Fittify.Api.Controllers.Sport
{
    [Route("api/mapexerciseworkouts")]
    public class MapExerciseWorkoutApiController :
        Controller,
        IAsyncGppdForHttp<int, MapExerciseWorkoutOfmForPost, MapExerciseWorkoutOfmForPatch>
    {
        private readonly GppdOfm<MapExerciseWorkoutRepository, MapExerciseWorkout, MapExerciseWorkoutOfmForGet, MapExerciseWorkoutOfmForPost, MapExerciseWorkoutOfmForPatch, int> _gppdForHttpMethods;
        private readonly MapExerciseWorkoutRepository _repo;

        public MapExerciseWorkoutApiController(FittifyContext fittifyContext)
        {
            _repo = new MapExerciseWorkoutRepository(fittifyContext);
            _gppdForHttpMethods = new GppdOfm<MapExerciseWorkoutRepository, MapExerciseWorkout, MapExerciseWorkoutOfmForGet, MapExerciseWorkoutOfmForPost, MapExerciseWorkoutOfmForPatch, int>(_repo);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var allEntites = await _gppdForHttpMethods.GetAll();
            var allOfmForGet = Mapper.Map<ICollection<MapExerciseWorkoutOfmForGet>>(allEntites);
            return new JsonResult(allOfmForGet);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var entity = await _repo.GetById(id);
            var ofmForGet = new List<MapExerciseWorkoutOfmForGet>() { Mapper.Map<MapExerciseWorkoutOfmForGet>(entity) };
            return new JsonResult(ofmForGet);
        }
        [HttpGet("range/{inputString}", Name = "GetMapExerciseWorkoutsByRangeOfIds")]
        public async Task<IActionResult> GetByRangeOfIds(string inputString)
        {
            var entityCollection = await _repo.GetByCollectionOfIds(RangeString.ToCollectionOfId(inputString));
            var ofmCollection = Mapper.Map<List<MapExerciseWorkout>, List<MapExerciseWorkoutOfmForGet>>(entityCollection.ToList());
            return Ok(ofmCollection);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] MapExerciseWorkoutOfmForPost ofmForPost)
        {
            var ofmForGet = await _gppdForHttpMethods.Post(ofmForPost);
            var result = CreatedAtRoute(routeName: "GetMapExerciseWorkoutsByRangeOfIds", routeValues: new { inputString = ofmForGet.Id }, value: ofmForGet);
            return result;
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _gppdForHttpMethods.Delete(id);
            return NoContent();
        }

        [HttpDelete("workout/{workoutId:int}/exercise/{exerciseId:int}")]
        public async Task<IActionResult> Delete(int workoutId, int exerciseId)
        {
            await _repo.DeleteByWorkoutAndExerciseId(workoutId, exerciseId);
            return NoContent();
        }

        [HttpPatch("{id:int}")]
        public async Task<IActionResult> UpdatePartially(int id, JsonPatchDocument<MapExerciseWorkoutOfmForPatch> jsonPatchDocument)
        {
            var ofmForGet = await _gppdForHttpMethods.UpdatePartially(id, jsonPatchDocument);
            return new JsonResult(ofmForGet);
        }

    }
}
