﻿using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Fittify.Api.Helpers;
using Fittify.Api.Helpers.CustomAttributes;
using Fittify.Api.Helpers.Extensions;
using Fittify.Api.Helpers.ObjectResults;
using Fittify.Api.OfmRepository.Helpers;
using Fittify.Api.OfmRepository.OfmRepository.GenericGppd;
using Fittify.Api.OfmRepository.OfmResourceParameters.Sport;
using Fittify.Api.OfmRepository.OfmResourceParameters.Sport.Get;
using Fittify.Api.OuterFacingModels.Sport.Get;
using Fittify.Api.OuterFacingModels.Sport.Patch;
using Fittify.Api.OuterFacingModels.Sport.Post;
using Fittify.Common.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using BadRequestObjectResult = Fittify.Api.Helpers.ObjectResults.BadRequestObjectResult;

namespace Fittify.Api.Controllers.Sport
{
    [Route("api/exercisehistories")]
    public class ExerciseHistoryApiController :
        Controller
    {
        private readonly IAsyncOfmRepository<ExerciseHistoryOfmForGet, int> _asyncOfmRepository;
        private readonly string _shortCamelCasedControllerName;
        private readonly IUrlHelper _urlHelper;
        private readonly ControllerGuardClauses<ExerciseHistoryOfmForGet, ExerciseHistoryOfmForPost, ExerciseHistoryOfmForPatch, int> _controllerGuardClause;
        private readonly HateoasLinkFactory<int> _hateoasLinkFactory;
        private readonly IncomingHeaders _incomingHeaders;

        public ExerciseHistoryApiController(
            IAsyncOfmRepository<ExerciseHistoryOfmForGet, int> asyncOfmRepository,
            IUrlHelper urlHelper,
            IHttpContextAccessor httpContextAccesor)
        {
            _asyncOfmRepository = asyncOfmRepository;
            _shortCamelCasedControllerName = nameof(ExerciseHistoryApiController).ToShortCamelCasedControllerName();
            _urlHelper = urlHelper;
            _controllerGuardClause = new ControllerGuardClauses<ExerciseHistoryOfmForGet, ExerciseHistoryOfmForPost, ExerciseHistoryOfmForPatch, int>(this);
            _hateoasLinkFactory = new HateoasLinkFactory<int>(_urlHelper, nameof(ExerciseHistoryApiController));
            _incomingHeaders = Mapper.Map<IncomingHeaders>(httpContextAccesor.HttpContext.Items[nameof(IncomingRawHeaders)] as IncomingRawHeaders);
        }

        [HttpGet("{id}", Name = "GetExerciseHistoryById")]
        [RequestHeaderMatchesApiVersion(new[] { "1" })]
        public async Task<IActionResult> GetById(int id, ExerciseHistoryOfmResourceParameters exerciseHistoryOfmResourceParameters)
        {
            var ofmForGetQueryResult = await _asyncOfmRepository.GetById(id, exerciseHistoryOfmResourceParameters.Fields);
            if (!_controllerGuardClause.ValidateGetById(ofmForGetQueryResult, id, out ObjectResult objectResult))
            {
                return objectResult;
            }
            var expandable = ofmForGetQueryResult.ReturnedTOfmForGet.ToExpandableOfm();
            var shapedExpandable = expandable.Shape(exerciseHistoryOfmResourceParameters.Fields);
            if (_incomingHeaders.IncludeHateoas)
                shapedExpandable.Add("links", _hateoasLinkFactory.CreateLinksForOfmForGet(id, exerciseHistoryOfmResourceParameters.Fields).ToList());
            return Ok(shapedExpandable);
        }

        [HttpGet(Name = "GetExerciseHistoryCollection")]
        [RequestHeaderMatchesApiVersion(new[] { "1" })]
        public async Task<IActionResult> GetCollection(ExerciseHistoryOfmCollectionResourceParameters collectionResourceParameters)
        {
            var stringGuid = User.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;
            if (String.IsNullOrWhiteSpace(stringGuid)) return Unauthorized();
            var ownerGuid = new Guid(stringGuid);

            var ofmForGetCollectionQueryResult = await _asyncOfmRepository.GetCollection(collectionResourceParameters, ownerGuid);

            if (!_controllerGuardClause.ValidateGetCollection(ofmForGetCollectionQueryResult, out ObjectResult objectResult)) return objectResult;
            var expandableOfmForGetCollection = ofmForGetCollectionQueryResult.ReturnedTOfmForGetCollection.OfmForGets.ToExpandableOfmForGets();
            if (_incomingHeaders.IncludeHateoas) expandableOfmForGetCollection = expandableOfmForGetCollection.CreateHateoasForExpandableOfmForGets<ExerciseHistoryOfmForGet, int>(_urlHelper, nameof(ExerciseHistoryApiController), collectionResourceParameters.Fields).ToList(); // Todo Improve! The data is only superficially shaped AFTER a full query was run against the database
            expandableOfmForGetCollection = expandableOfmForGetCollection.Shape(collectionResourceParameters.Fields, _incomingHeaders.IncludeHateoas).ToList();

            this.AddPaginationMetadata<int, ExerciseHistoryOfmForGet>(ofmForGetCollectionQueryResult,
                _incomingHeaders, collectionResourceParameters.AsDictionary().RemoveNullValues(), _urlHelper, nameof(ExerciseHistoryApiController));

            if (!_incomingHeaders.IncludeHateoas)
            {
                return Ok(expandableOfmForGetCollection);
            }

            dynamic result = new
            {
                value = expandableOfmForGetCollection,
                links = _hateoasLinkFactory.CreateLinksForOfmGetGeneric(collectionResourceParameters.AsDictionary().RemoveNullValues(),
                    ofmForGetCollectionQueryResult.HasPrevious, ofmForGetCollectionQueryResult.HasNext).ToList()
            };
            return Ok(result);
        }

        [HttpPost(Name = "CreateExerciseHistory")]
        [RequestHeaderMatchesApiVersion(new[] { "1" })]
        public async Task<IActionResult> Post([FromBody] ExerciseHistoryOfmForPost ofmForPost)
        {
            var stringGuid = User.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;
            if (String.IsNullOrWhiteSpace(stringGuid)) return Unauthorized();
            var ownerGuid = new Guid(stringGuid);

            if (!_controllerGuardClause.ValidatePost(ofmForPost, out ObjectResult objectResult))
                return objectResult;

            var ofmForGet = await _asyncOfmRepository.Post(ofmForPost, ownerGuid);

            var result = CreatedAtRoute(routeName: "GetExerciseHistoryById", routeValues: new { id = ofmForGet.Id }, value: ofmForGet);
            return result;
        }

        [HttpDelete("{id}", Name = "DeleteExerciseHistory")]
        [RequestHeaderMatchesApiVersion(new[] { "1" })]
        public async Task<IActionResult> Delete(int id)
        {
            var ofmDeletionQueryResult = await _asyncOfmRepository.Delete(id);

            if (!_controllerGuardClause.ValidateDelete(ofmDeletionQueryResult, id, out ObjectResult objectResult)) return objectResult;

            return NoContent();
        }

        [HttpPatch("{id}", Name = "PartiallyUpdateExerciseHistory")]
        [RequestHeaderMatchesApiVersion(new[] { "1" })]
        public async Task<IActionResult> UpdatePartially(int id, [FromBody]JsonPatchDocument<ExerciseHistoryOfmForPatch> jsonPatchDocument)
        {
            //// Todo: Prohibit trying to patch id!
            if (jsonPatchDocument == null)
            {
                ModelState.AddModelError(_shortCamelCasedControllerName, "You sent an empty body (null) for " + _shortCamelCasedControllerName + " with id=" + id);
                return new BadRequestObjectResult(ModelState);
            }

            // Get entity with original values from context
            var ofmForPatch = await _asyncOfmRepository.GetByIdOfmForPatch<ExerciseHistoryOfmForPatch>(id);
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
            var ofmForGet = await _asyncOfmRepository.UpdatePartially(ofmForPatch);
            return Ok(ofmForGet);
        }
    }
}


