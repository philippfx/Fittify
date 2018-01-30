﻿using System;
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
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace Fittify.Api.Controllers.Sport
{
    [Route("api/exercisehistories")]
    public class ExerciseHistoryApiController :
        Controller,
        IAsyncGppdForHttp<int, ExerciseHistoryOfmForPost, ExerciseHistoryOfmForPatch>
    {
        private readonly GppdOfm<ExerciseHistoryRepository, ExerciseHistory, ExerciseHistoryOfmForGet, ExerciseHistoryOfmForPost, ExerciseHistoryOfmForPatch, int> _gppdForHttpMethods;
        private readonly ExerciseHistoryRepository _repo;
        private readonly GetMoreForHttpIntId<ExerciseHistoryRepository, ExerciseHistory> _getMoreForIntId;

        public ExerciseHistoryApiController(FittifyContext fittifyContext)
        {
            _repo = new ExerciseHistoryRepository(fittifyContext);
            _gppdForHttpMethods = new GppdOfm<ExerciseHistoryRepository, ExerciseHistory, ExerciseHistoryOfmForGet, ExerciseHistoryOfmForPost, ExerciseHistoryOfmForPatch, int>(_repo);
            _getMoreForIntId = new GetMoreForHttpIntId<ExerciseHistoryRepository, ExerciseHistory>(_repo);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var allEntites = await _gppdForHttpMethods.GetAll();
            var allOfmForGet = Mapper.Map<ICollection<ExerciseHistoryOfmForGet>>(allEntites);
            return new JsonResult(allOfmForGet);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var entity = await _repo.GetById(id);
            var ofmForGet = new List<ExerciseHistoryOfmForGet>() { Mapper.Map<ExerciseHistoryOfmForGet>(entity) };
            return new JsonResult(ofmForGet);
        }

        [HttpGet("range/{inputString}", Name = "GetExerciseHistoriesByRangeOfIds")]
        public async Task<IActionResult> GetByRangeOfIds(string inputString)
        {
            return await _getMoreForIntId.GetByRangeOfIds(inputString);
        }

        [HttpPost("new")]
        public async Task<CreatedAtRouteResult> Post([FromBody] ExerciseHistoryOfmForPost ofmForPost)
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
            ICollection<ExerciseHistoryOfmForGet> ofm = null;
            try
            {
                var collectionExerciseHistories = await _repo.GetCollectionByFkWorkoutHistoryId(workouthistoryId);
                ofm = Mapper
                    .Map<ICollection<ExerciseHistory>, ICollection<ExerciseHistoryOfmForGet>>(collectionExerciseHistories);
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
            var ofmForGet = await _gppdForHttpMethods.UpdatePartially(id, jsonPatchDocument);
            return new JsonResult(ofmForGet);
        }
    }
}
