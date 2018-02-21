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
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace Fittify.Api.Controllers.Sport
{
    [Route("api/mapexerciseworkouts")]
    public class MapExerciseWorkoutApiController :
        Controller,
        IAsyncGppdForHttp<int, MapExerciseWorkoutOfmForPost, MapExerciseWorkoutOfmForPatch>
    {
        private readonly AsyncPostPatchDeleteOfm<MapExerciseWorkoutRepository, MapExerciseWorkout, MapExerciseWorkoutOfmForGet, MapExerciseWorkoutOfmForPost, MapExerciseWorkoutOfmForPatch, int> _asyncPostPatchDeleteForHttpMethods;
        private readonly MapExerciseWorkoutRepository _repo;

        public MapExerciseWorkoutApiController(FittifyContext fittifyContext,
            IActionDescriptorCollectionProvider adcProvider,
            IUrlHelper urlHelper)
        {
            _repo = new MapExerciseWorkoutRepository(fittifyContext);
            _asyncPostPatchDeleteForHttpMethods = new AsyncPostPatchDeleteOfm<MapExerciseWorkoutRepository, MapExerciseWorkout, MapExerciseWorkoutOfmForGet, MapExerciseWorkoutOfmForPost, MapExerciseWorkoutOfmForPatch, int>(_repo, urlHelper, adcProvider);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var allEntites = await _asyncPostPatchDeleteForHttpMethods.GetAll();
            var allOfmForGet = Mapper.Map<IEnumerable<MapExerciseWorkoutOfmForGet>>(allEntites);
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
            var ofmForGet = await _asyncPostPatchDeleteForHttpMethods.Post(ofmForPost);
            var result = CreatedAtRoute(routeName: "GetMapExerciseWorkoutsByRangeOfIds", routeValues: new { inputString = ofmForGet.Id }, value: ofmForGet);
            return result;
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _asyncPostPatchDeleteForHttpMethods.Delete(id);
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
            throw new NotImplementedException();
            //var ofmForGet = await _asyncPostPatchDeleteForHttpMethods.UpdatePartially(id, jsonPatchDocument);
            //return new JsonResult(ofmForGet);
        }

    }
}
