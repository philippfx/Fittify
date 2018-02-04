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
    [Route("api/exercises")]
    public class ExerciseApiController : 
        Controller, 
        IAsyncGppdForHttp<int, ExerciseOfmForPost, ExerciseOfmForPatch>
    {
        private readonly GppdOfm<ExerciseRepository, Exercise, ExerciseOfmForGet, ExerciseOfmForPost, ExerciseOfmForPatch, int> _gppdForHttpMethods;
        private readonly ExerciseRepository _repo;

        public ExerciseApiController(FittifyContext fittifyContext)
        {
            _repo = new ExerciseRepository(fittifyContext);
            _gppdForHttpMethods = new GppdOfm<ExerciseRepository, Exercise, ExerciseOfmForGet, ExerciseOfmForPost, ExerciseOfmForPatch, int>(_repo);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var allEntites = await _gppdForHttpMethods.GetAll();
            var allOfmForGet = Mapper.Map<ICollection<ExerciseOfmForGet>>(allEntites);
            return new JsonResult(allOfmForGet);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var entity = await _repo.GetById(id);
            var ofmForGet = new List<ExerciseOfmForGet>() { Mapper.Map<ExerciseOfmForGet>(entity) };
            return new JsonResult(ofmForGet);
        }

        [HttpGet("range/{inputString}", Name = "GetExercisesByRangeOfIds")]
        public async Task<IActionResult> GetByRangeOfIds(string inputString)
        {
            var entityCollection = await _repo.GetByCollectionOfIds(RangeString.ToCollectionOfId(inputString));
            var ofmCollection = Mapper.Map<List<Exercise>, List<ExerciseOfmForGet>>(entityCollection.ToList());
            return Ok(ofmCollection);
        }

        [HttpPost("new")]
        public async Task<IActionResult> Post([FromBody] ExerciseOfmForPost ofmForPost)
        {
            var ofmForGet = await _gppdForHttpMethods.Post(ofmForPost);
            var result = CreatedAtRoute(routeName: "GetExercisesByRangeOfIds", routeValues: new { inputString = ofmForGet.Id }, value: ofmForGet);
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
            var collectionExercises = await _repo.GetCollectionByFkWorkoutId(workoutId);
            var collectionOfm = Mapper.Map<List<Exercise>, List<ExerciseOfmForGet>>(collectionExercises);
            return new JsonResult(collectionOfm);
        }

        [HttpPatch("{id:int}")]
        public async Task<IActionResult> UpdatePartially(int id, JsonPatchDocument<ExerciseOfmForPatch> jsonPatchDocument)
        {
            var ofmForGet = await _gppdForHttpMethods.UpdatePartially(id, jsonPatchDocument);
            return new JsonResult(ofmForGet);
        }
    }
}
