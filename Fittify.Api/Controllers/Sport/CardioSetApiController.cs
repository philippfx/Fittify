﻿using System.Collections.Generic;
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
    [Route("api/cardiosets")]
    public class CardioSetApiController :
        Controller,
        IAsyncGppdForHttp<int, CardioSetOfmForPost, CardioSetOfmForPatch>
    {
        private readonly GppdOfm<CardioSetRepository, CardioSet, CardioSetOfmForGet, CardioSetOfmForPost, CardioSetOfmForPatch, int> _gppdForHttpMethods;
        private readonly CardioSetRepository _repo;

        public CardioSetApiController(FittifyContext fittifyContext)
        {
            _repo = new CardioSetRepository(fittifyContext);
            _gppdForHttpMethods = new GppdOfm<CardioSetRepository, CardioSet, CardioSetOfmForGet, CardioSetOfmForPost, CardioSetOfmForPatch, int>(_repo);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var allEntites = await _gppdForHttpMethods.GetAll();
            var allOfmForGet = Mapper.Map<ICollection<CardioSetOfmForGet>>(allEntites);
            return new JsonResult(allOfmForGet);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var entity = await _repo.GetById(id);
            var ofmForGet = new List<CardioSetOfmForGet>() { Mapper.Map<CardioSetOfmForGet>(entity) };
            return new JsonResult(ofmForGet);
        }

        [HttpGet("range/{inputString}", Name = "GetCardioSetsByRangeOfIds")]
        public async Task<IActionResult> GetByRangeOfIds(string inputString)
        {
            var entityCollection = await _repo.GetByCollectionOfIds(RangeString.ToCollectionOfId(inputString));
            var ofmCollection = Mapper.Map<List<CardioSet>, List<CardioSetOfmForGet>>(entityCollection.ToList());
            return Ok(ofmCollection);
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
            var ofm = Mapper.Map<List<CardioSet>, List<CardioSetOfmForGet>>(collectionWorkoutHistories);
            return new JsonResult(ofm);
        }
        
        [HttpPost("new")]
        public async Task<IActionResult> Post([FromBody] CardioSetOfmForPost ofmForPost)
        {
            var ofmForGet = await _gppdForHttpMethods.Post(ofmForPost);
            var result = CreatedAtRoute(routeName: "GetExercisesByRangeOfIds", routeValues: new { inputString = ofmForGet.Id }, value: ofmForGet);
            return result;
        }

        [HttpPatch("{id:int}")]
        public async Task<IActionResult> UpdatePartially(int id, JsonPatchDocument<CardioSetOfmForPatch> jsonPatchDocument)
        {
            var ofmForGet = await _gppdForHttpMethods.UpdatePartially(id, jsonPatchDocument);
            return new JsonResult(ofmForGet);
        }
    }
}
