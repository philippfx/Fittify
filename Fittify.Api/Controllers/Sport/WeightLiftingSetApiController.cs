﻿using System;
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
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Fittify.Api.Controllers.Sport
{
    [Route("api/weightliftingsets")]
    public class WeightLiftingSetApiController :
        Controller,
        IAsyncGppdForHttp<int, WeightLiftingSetOfmForPost, WeightLiftingSetOfmForPatch>
    {
        private readonly GppdOfm<WeightLiftingSetRepository, WeightLiftingSet, WeightLiftingSetOfmForGet, WeightLiftingSetOfmForPost, WeightLiftingSetOfmForPatch, int> _gppdForHttpMethods;
        private readonly WeightLiftingSetRepository _repo;
        private readonly ILogger<WeightLiftingSetApiController> _logger;

        public WeightLiftingSetApiController(FittifyContext fittifyContext, ILogger<WeightLiftingSetApiController> logger)
        {
            _repo = new WeightLiftingSetRepository(fittifyContext);
            _gppdForHttpMethods = new GppdOfm<WeightLiftingSetRepository, WeightLiftingSet, WeightLiftingSetOfmForGet, WeightLiftingSetOfmForPost, WeightLiftingSetOfmForPatch, int>(_repo);
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var allEntites = await _gppdForHttpMethods.GetAll();
            var allOfmForGet = Mapper.Map<ICollection<WeightLiftingSetOfmForGet>>(allEntites);
            if (allOfmForGet.Count == 0)
            {
                NoContent();
            }
            return new JsonResult(allOfmForGet);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var entity = await _repo.GetById(id);
            var ofmForGet = new List<WeightLiftingSetOfmForGet>() { Mapper.Map<WeightLiftingSetOfmForGet>(entity) };
            return new JsonResult(ofmForGet);
        }

        [HttpGet("range/{inputString}", Name = "GetWeightLiftingSetsByRangeOfIds")]
        public async Task<IActionResult> GetByRangeOfIds(string inputString)
        {
            var entityCollection = await _repo.GetByCollectionOfIds(RangeString.ToCollectionOfId(inputString));
            var ofmCollection = Mapper.Map<List<WeightLiftingSet>, List<WeightLiftingSetOfmForGet>>(entityCollection.ToList());
            return Ok(ofmCollection);
        }

        [HttpPost("new")]
        public async Task<IActionResult> Post([FromBody] WeightLiftingSetOfmForPost ofmForPost)
        {
            var ofmForGet = await _gppdForHttpMethods.Post(ofmForPost);
            var result = CreatedAtRoute(routeName: "GetWeightLiftingSetsByRangeOfIds",
                routeValues: new {inputString = ofmForGet.Id}, value: ofmForGet);
            return result;
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _gppdForHttpMethods.Delete(id);
            return NoContent();
        }

        [HttpGet("exercisehistory/{exercisehistoryId:int}")]
        public async Task<IActionResult> GetCollectionByFkWorkoutHistoryId(int exercisehistoryId)
        {
            var collectionWorkoutHistories = await _repo.GetCollectionByFkExerciseHistoryId(exercisehistoryId);
            var ofm = Mapper.Map<List<WeightLiftingSet>, List<WeightLiftingSetOfmForGet>>(collectionWorkoutHistories);
            
            _logger.LogInformation(string.Format("Creating a foo: {0}", JsonConvert.SerializeObject(ofm)));
            
            return new JsonResult(ofm);
        }

        [HttpPatch("{id:int}")]
        public async Task<IActionResult> UpdatePartially(int id, [FromBody] JsonPatchDocument<WeightLiftingSetOfmForPatch> jsonPatchDocument)
        {
            var ofmForGet = await _gppdForHttpMethods.UpdatePartially(id, jsonPatchDocument);
            return new JsonResult(ofmForGet);
        }
    }
}