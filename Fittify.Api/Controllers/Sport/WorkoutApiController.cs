﻿using System.Collections.Generic;
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
    [Route("api/workouts")]
    public class WorkoutApiController :
        Controller,
        IAsyncGppdForHttp<int, WorkoutOfmForPost, WorkoutOfmForPatch>
    {
        private readonly GppdOfm<WorkoutRepository, Workout, WorkoutOfmForGet, WorkoutOfmForPost, WorkoutOfmForPatch, int> _gppdForHttpMethods;
        private readonly WorkoutRepository _repo;
        private readonly GetMoreForHttpIntId<WorkoutRepository, Workout> _getMoreForIntId;

        public WorkoutApiController(FittifyContext fittifyContext)
        {
            _repo = new WorkoutRepository(fittifyContext);
            _gppdForHttpMethods = new GppdOfm<WorkoutRepository, Workout, WorkoutOfmForGet, WorkoutOfmForPost, WorkoutOfmForPatch, int>(_repo);
            _getMoreForIntId = new GetMoreForHttpIntId<WorkoutRepository, Workout>(_repo);
        }
        
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var allEntites = await _gppdForHttpMethods.GetAll();
            var allOfmForGet = Mapper.Map<ICollection<WorkoutOfmForGet>>(allEntites);
            return new JsonResult(allOfmForGet);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var entity = await _repo.GetById(id);
            var ofmForGet = new List<WorkoutOfmForGet>() { Mapper.Map<WorkoutOfmForGet>(entity) };
            return new JsonResult(ofmForGet);
        }
        [HttpGet("range/{inputString}", Name = "GetWorkoutsByRangeOfIds")]
        public async Task<IActionResult> GetByRangeOfIds(string inputString)
        {
            return await _getMoreForIntId.GetByRangeOfIds(inputString);
        }
        
        [HttpPost]
        public async Task<CreatedAtRouteResult> Post([FromBody] WorkoutOfmForPost ofmForPost)
        {
            var ofmForGet = await _gppdForHttpMethods.Post(ofmForPost);
            var result = CreatedAtRoute(routeName: "GetWorkoutsByRangeOfIds", routeValues: new { inputString = ofmForGet.Id }, value: ofmForGet);
            return result;
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _gppdForHttpMethods.Delete(id);
            return NoContent();
        }

        [HttpPatch("{id:int}")]
        public async Task<IActionResult> UpdatePartially(int id, JsonPatchDocument<WorkoutOfmForPatch> jsonPatchDocument)
        {
            var ofmForGet = await _gppdForHttpMethods.UpdatePartially(id, jsonPatchDocument);
            return new JsonResult(ofmForGet);
        }

    }
}
