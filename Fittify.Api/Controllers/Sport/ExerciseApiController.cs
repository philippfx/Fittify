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
    [Route("api/exercises")]
    public class ExerciseApiController : 
        Controller, 
        IAsyncGppdForHttp<int, ExerciseOfmForPost, ExerciseOfmForPatch>
    {
        private readonly AsyncPostPatchDeleteOfm<ExerciseRepository, Exercise, ExerciseOfmForGet, ExerciseOfmForPost, ExerciseOfmForPatch, int> _asyncPostPatchDeleteForHttpMethods;
        private readonly IAsyncGetOfmByNameSearch<ExerciseOfmForGet, int> _asyncGetOfmByNameSearch;
        private readonly ExerciseRepository _repo;

        public ExerciseApiController(FittifyContext fittifyContext, 
            IActionDescriptorCollectionProvider adcProvider,
            IUrlHelper urlHelper)
        {
            _repo = new ExerciseRepository(fittifyContext);
            _asyncPostPatchDeleteForHttpMethods = new AsyncPostPatchDeleteOfm<ExerciseRepository, Exercise, ExerciseOfmForGet, ExerciseOfmForPost, ExerciseOfmForPatch, int>(_repo, urlHelper, adcProvider);
            _asyncGetOfmByNameSearch = new AsyncGetOfmByNameSearch<ExerciseRepository, Exercise, ExerciseOfmForGet, int>(_repo, urlHelper, adcProvider);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var allEntites = await _asyncGetOfmByNameSearch.GetAll();
            var allOfmForGet = Mapper.Map<IEnumerable<ExerciseOfmForGet>>(allEntites);
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
            var ofmForGet = await _asyncPostPatchDeleteForHttpMethods.Post(ofmForPost);
            var result = CreatedAtRoute(routeName: "GetExercisesByRangeOfIds", routeValues: new { inputString = ofmForGet.Id }, value: ofmForGet);
            return result;
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _asyncPostPatchDeleteForHttpMethods.Delete(id);
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
            throw new NotImplementedException();
            //var ofmForGet = await _asyncPostPatchDeleteForHttpMethods.UpdatePartially(id, jsonPatchDocument);
            //return new JsonResult(ofmForGet);
        }
    }
}
