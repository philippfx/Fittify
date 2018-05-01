﻿using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Fittify.Api.Helpers;
using Fittify.Api.Helpers.Extensions;
using Fittify.Api.OfmRepository;
using Fittify.Api.OfmRepository.GenericGppd;
using Fittify.Api.OuterFacingModels.Sport.Get;
using Fittify.Api.OuterFacingModels.Sport.Patch;
using Fittify.Api.OuterFacingModels.Sport.Post;
using Fittify.Common.Helpers;
using Fittify.Common.Helpers.ResourceParameters.Sport;
using Fittify.DataModelRepositories.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using IConfiguration = Microsoft.Extensions.Configuration.IConfiguration;

namespace Fittify.Api.Controllers.Sport
{
    [Route("api/exercisehistories")]
    public class ExerciseHistoryApiController :
        Controller
    {
        private readonly IAsyncGppd<ExerciseHistoryOfmForGet, ExerciseHistoryOfmForPost, ExerciseHistoryOfmForPatch, int, ExerciseHistoryResourceParameters> _asyncGppd;
        private readonly string _shortCamelCasedControllerName;
        private readonly IUrlHelper _urlHelper;
        private readonly ControllerGuardClauses<ExerciseHistoryOfmForGet> _controllerGuardClause;
        private readonly HateoasLinkFactory<int> _hateoasLinkFactory;
        private readonly IncomingHeaders _incomingHeaders;

        public ExerciseHistoryApiController(
            IAsyncGppd<ExerciseHistoryOfmForGet, ExerciseHistoryOfmForPost, ExerciseHistoryOfmForPatch, int, ExerciseHistoryResourceParameters> asyncGppd,
            IUrlHelper urlHelper,
            IHttpContextAccessor httpContextAccesor)
        {
            _asyncGppd = asyncGppd;
            _shortCamelCasedControllerName = nameof(ExerciseHistoryApiController).ToShortCamelCasedControllerNameOrDefault();
            _urlHelper = urlHelper;
            _controllerGuardClause = new ControllerGuardClauses<ExerciseHistoryOfmForGet>(this);
            _hateoasLinkFactory = new HateoasLinkFactory<int>(urlHelper, nameof(ExerciseHistoryApiController));
            _incomingHeaders = Mapper.Map<IncomingHeaders>(httpContextAccesor.HttpContext.Items[nameof(IncomingRawHeaders)] as IncomingRawHeaders);
        }

        [HttpGet("{id}", Name = "GetExerciseHistoryById")]
        [RequestHeaderMatchesApiVersion(ConstantHttpHeaderNames.ApiVersion, new[] { "1" })]
        public async Task<IActionResult> GetById(int id, [FromQuery] string fields)
        {
            var ofmForGetQueryResult = await _asyncGppd.GetById(id, fields);
            if (!_controllerGuardClause.ValidateGetById(ofmForGetQueryResult, id, out ObjectResult objectResult))
            {
                return objectResult;
            }
            var expandable = ofmForGetQueryResult.ReturnedTOfmForGet.ToExpandableOfm();
            var shapedExpandable = expandable.Shape(fields); // Todo Improve! The data is only superficially shaped AFTER a full query was run against the database
            if (_incomingHeaders.IncludeHateoas) shapedExpandable.Add("links", _hateoasLinkFactory.CreateLinksForOfmForGet(id, fields).ToList());
            return Ok(shapedExpandable);
        }

        [HttpGet(Name = "GetExerciseHistoryCollection")]
        [RequestHeaderMatchesApiVersion(ConstantHttpHeaderNames.ApiVersion, new[] { "1" })]
        public async Task<IActionResult> GetCollection(ExerciseHistoryResourceParameters resourceParameters)
        {
            var ofmForGetCollectionQueryResult = await _asyncGppd.GetCollection(resourceParameters);
            if (!_controllerGuardClause.ValidateGetCollection(ofmForGetCollectionQueryResult, out ObjectResult objectResult)) return objectResult;
            var expandableOfmForGetCollection = ofmForGetCollectionQueryResult.ReturnedTOfmForGetCollection.OfmForGets.ToExpandableOfmForGets();
            if (_incomingHeaders.IncludeHateoas) expandableOfmForGetCollection = expandableOfmForGetCollection.CreateHateoasLinksForeachExpandableOfmForGet<ExerciseHistoryOfmForGet, int>(_urlHelper, nameof(ExerciseHistoryApiController), resourceParameters.Fields).ToList(); // Todo Improve! The data is only superficially shaped AFTER a full query was run against the database
            expandableOfmForGetCollection = expandableOfmForGetCollection.Shape(resourceParameters.Fields, _incomingHeaders.IncludeHateoas).ToList();
            if (!_incomingHeaders.IncludeHateoas)
            {
                return Ok(expandableOfmForGetCollection);
            }

            dynamic result = new
            {
                value = expandableOfmForGetCollection,
                links = _hateoasLinkFactory.CreateLinksForOfmGetGeneric(resourceParameters.AsDictionary().RemoveNullValues(),
                    ofmForGetCollectionQueryResult.HasPrevious, ofmForGetCollectionQueryResult.HasNext).ToList()
            };
            return Ok(result);
        }

        [HttpPost(Name = "CreateExerciseHistory")]
        [RequestHeaderMatchesApiVersion(ConstantHttpHeaderNames.ApiVersion, new[] { "1" })]
        public async Task<IActionResult> Post([FromBody] ExerciseHistoryOfmForPost ofmForPost)
        {
            if (ofmForPost == null) return BadRequest();

            if (!ModelState.IsValid)
            {
                return new UnprocessableEntityObjectResult(ModelState);
            }

            var ofmForGet = await _asyncGppd.Post(ofmForPost);

            var result = CreatedAtRoute(routeName: "GetExerciseHistoryById", routeValues: new { id = ofmForGet.Id }, value: ofmForGet);
            return result;
        }

        [HttpDelete("{id}", Name = "DeleteExerciseHistory")]
        [RequestHeaderMatchesApiVersion(ConstantHttpHeaderNames.ApiVersion, new[] { "1" })]
        public async Task<IActionResult> Delete(int id)
        {
            var ofmDeletionQueryResult = await _asyncGppd.Delete(id);
            if (ofmDeletionQueryResult.IsDeleted == false)
            {
                if (ofmDeletionQueryResult.ErrorMessages.Count != 0)
                {
                    foreach (var blockingOfmForGet in ofmDeletionQueryResult.ErrorMessages)
                    {
                        ModelState.AddModelError(_shortCamelCasedControllerName, blockingOfmForGet);
                    }
                }
                else
                {
                    ModelState.AddModelError(_shortCamelCasedControllerName, "There was an unknown error deleting this entity. Please contact support.");
                }
            }

            if (!ModelState.IsValid)
            {
                return new UnprocessableEntityObjectResult(ModelState);
            }

            return NoContent();
        }

        [HttpPatch("{id}", Name = "PartiallyUpdateExerciseHistory")]
        [RequestHeaderMatchesApiVersion(ConstantHttpHeaderNames.ApiVersion, new[] { "1" })]
        public async Task<IActionResult> UpdatePartially(int id, [FromBody]JsonPatchDocument<ExerciseHistoryOfmForPatch> jsonPatchDocument)
        {
            if (jsonPatchDocument == null)
            {
                ModelState.AddModelError(_shortCamelCasedControllerName, "You sent an empty body (null) for " + _shortCamelCasedControllerName + " with id=" + id);
                return new EntityNotFoundObjectResult(ModelState);
            }

            try
            {
                // Get entity with original values from context
                var ofmForPatch = await _asyncGppd.GetByIdOfmForPatch(id);
                if (ofmForPatch == null)
                {
                    ModelState.AddModelError(_shortCamelCasedControllerName, "No " + _shortCamelCasedControllerName + " found for id=" + id);
                    return new EntityNotFoundObjectResult(ModelState);
                }

                // Apply new values from jsonPatchDocument to ofm (the ofm that was just created based on fresh entity from context)
                jsonPatchDocument.ApplyTo(ofmForPatch, ModelState);

                // Validating ofm
                TryValidateModel(ofmForPatch); // This is important to catch invalid model states caused by applying the jsonPatch, for example if a required field that previously had a value is now set to null
                if (!ModelState.IsValid)
                {
                    return new UnprocessableEntityObjectResult(ModelState);
                }

                // returning the patched ofm as response
                var ofmForGet = _asyncGppd.UpdatePartially(ofmForPatch).Result;
                return new JsonResult(ofmForGet);

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
