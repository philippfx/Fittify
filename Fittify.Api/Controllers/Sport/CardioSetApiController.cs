﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Fittify.Api.Controllers.HttpMethodInterfaces;
using Fittify.Api.Extensions;
using Fittify.Api.Helpers;
using Fittify.Api.OfmRepository;
using Fittify.Api.OfmRepository.GetCollection.Sport;
using Fittify.Api.OuterFacingModels.Sport.Get;
using Fittify.Api.OuterFacingModels.Sport.Patch;
using Fittify.Api.OuterFacingModels.Sport.Post;
using Fittify.Api.Services;
using Fittify.Common.Extensions;
using Fittify.Common.Helpers;
using Fittify.Common.Helpers.ResourceParameters;
using Fittify.Common.Helpers.ResourceParameters.Sport;
using Fittify.DataModelRepositories;
using Fittify.DataModelRepositories.Repository.Sport;
using Fittify.DataModels.Models.Sport;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using IConfiguration = Microsoft.Extensions.Configuration.IConfiguration;

namespace Fittify.Api.Controllers.Sport
{
    [Route("api/cardiosets")]
    public class CardioSetApiController :
        Controller,
        IAsyncGetByIdForHttp<int>,
        //IAsyncGetCollectionByByDateTimeStartEndForHttp,
        IAsyncPostForHttp<CardioSetOfmForPost>,
        IAsyncPatchForHttp<CardioSetOfmForPatch, int>,
        IAsyncDeleteForHttp<int>
    {
        private readonly IAsyncGetOfmById<CardioSetOfmForGet, int> _asyncGetOfmById;
        private readonly AsyncGetOfmCollectionForCardioSet _asyncGetOfmCollection;
        private readonly IAsyncPostOfm<CardioSetOfmForGet, CardioSetOfmForPost> _asyncPostForHttpMethods;
        private readonly IAsyncPatchOfm<CardioSetOfmForGet, CardioSetOfmForPatch, int> _asyncPatchForHttpMethods;
        private readonly IAsyncDeleteOfm<int> _asyncDeleteForHttpMethods;
        private readonly CardioSetRepository _repo;
        private readonly string _shortCamelCasedControllerName;
        private readonly IUrlHelper _urlHelper;
        private readonly ControllerGuardClauses<CardioSetOfmForGet> _controllerGuardClause;
        private readonly HateoasLinkFactory<int> _hateoasLinkFactory;
        private readonly IncomingHeaders _incomingHeaders;

        public CardioSetApiController(FittifyContext fittifyContext,
            IActionDescriptorCollectionProvider adcProvider,
            IUrlHelper urlHelper,
            IPropertyMappingService propertyMappingService,
            ITypeHelperService typeHelperService,
            IConfiguration appConfiguration,
            IHttpContextAccessor httpContextAccesor)
        {
            _repo = new CardioSetRepository(fittifyContext);
            _asyncPostForHttpMethods = new AsyncPostOfm<CardioSetRepository, CardioSet, CardioSetOfmForGet, CardioSetOfmForPost, int>(_repo);
            _asyncPatchForHttpMethods = new AsyncPatchOfm<CardioSetRepository, CardioSet, CardioSetOfmForGet, CardioSetOfmForPatch, int>(_repo);
            _asyncDeleteForHttpMethods = new AsyncDeleteOfm<CardioSetRepository, CardioSet, int>(_repo, adcProvider);
            _shortCamelCasedControllerName = nameof(CardioSetApiController).ToShortCamelCasedControllerNameOrDefault();
            _asyncGetOfmById = new AsyncGetOfmById<CardioSetRepository, CardioSet, CardioSetOfmForGet, int>(_repo, urlHelper, adcProvider, propertyMappingService, typeHelperService, this);
            _asyncGetOfmCollection = new AsyncGetOfmCollectionForCardioSet(_repo, urlHelper, adcProvider, propertyMappingService, typeHelperService, this);
            _urlHelper = urlHelper;
            _controllerGuardClause = new ControllerGuardClauses<CardioSetOfmForGet>(this);
            _hateoasLinkFactory = new HateoasLinkFactory<int>(urlHelper, nameof(CardioSetApiController));
            _incomingHeaders = Mapper.Map<IncomingHeaders>(httpContextAccesor.HttpContext.Items[nameof(IncomingRawHeaders)] as IncomingRawHeaders);
        }

        [HttpGet("{id}", Name = "GetCardioSetById")]
        [RequestHeaderMatchesApiVersion(ConstantPropertyNames.ApiVersion, new[] { "1" })]
        public async Task<IActionResult> GetById(int id, [FromQuery] string fields)
        {
            var ofmForGetQueryResult = await _asyncGetOfmById.GetById(id, fields);
            if (!_controllerGuardClause.ValidateGetById(ofmForGetQueryResult, id, out ObjectResult objectResult))
            {
                return objectResult;
            }
            var expandable = ofmForGetQueryResult.ReturnedTOfmForGet.ToExpandableOfm();
            var shapedExpandable = expandable.Shape(fields); // Todo Improve! The data is only superficially shaped AFTER a full query was run against the database
            if (_incomingHeaders.IncludeHateoas) shapedExpandable.Add("links", _hateoasLinkFactory.CreateLinksForOfmForGet(id, fields).ToList());
            return Ok(shapedExpandable);
        }

        [HttpGet(Name = "GetCardioSetCollection")]
        [RequestHeaderMatchesApiVersion(ConstantPropertyNames.ApiVersion, new[] { "1" })]
        public async Task<IActionResult> GetCollection(CardioSetResourceParameters resourceParameters)
        {
            var ofmForGetCollectionQueryResult = await _asyncGetOfmCollection.GetCollection(resourceParameters);
            if (!_controllerGuardClause.ValidateGetCollection(ofmForGetCollectionQueryResult, out ObjectResult objectResult)) return objectResult;
            var expandableOfmForGetCollection = ofmForGetCollectionQueryResult.ReturnedTOfmForGetCollection.OfmForGets.ToExpandableOfmForGets();
            if (_incomingHeaders.IncludeHateoas) expandableOfmForGetCollection = expandableOfmForGetCollection.CreateHateoasLinksForeachExpandableOfmForGet<CardioSetOfmForGet, int>(_urlHelper, nameof(CardioSetApiController), resourceParameters.Fields).ToList(); // Todo Improve! The data is only superficially shaped AFTER a full query was run against the database
            expandableOfmForGetCollection = expandableOfmForGetCollection.Shape(resourceParameters.Fields, _incomingHeaders.IncludeHateoas).ToList();
            if (!_incomingHeaders.IncludeHateoas)
            {
                return Ok(expandableOfmForGetCollection);
            }

            dynamic result = new
            {
                value = expandableOfmForGetCollection,
                links = _hateoasLinkFactory.CreateLinksForOfmGetForCardioSet(resourceParameters,
                    ofmForGetCollectionQueryResult.HasPrevious, ofmForGetCollectionQueryResult.HasNext).ToList()
            };
            return Ok(result);
        }

        [HttpPost(Name = "CreateCardioSet")]
        [RequestHeaderMatchesApiVersion(ConstantPropertyNames.ApiVersion, new[] { "1" })]
        public async Task<IActionResult> Post([FromBody] CardioSetOfmForPost ofmForPost)
        {
            if (ofmForPost == null) return BadRequest();

            if (!ModelState.IsValid)
            {
                return new UnprocessableEntityObjectResult(ModelState);
            }

            var ofmForGet = await _asyncPostForHttpMethods.Post(ofmForPost);

            var result = CreatedAtRoute(routeName: "GetCardioSetById", routeValues: new { id = ofmForGet.Id }, value: ofmForGet);
            return result;
        }

        [HttpDelete("{id}", Name = "DeleteCardioSet")]
        [RequestHeaderMatchesApiVersion(ConstantPropertyNames.ApiVersion, new[] { "1" })]
        public async Task<IActionResult> Delete(int id)
        {
            var ofmDeletionQueryResult = await _asyncDeleteForHttpMethods.Delete(id);
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

        [HttpPatch("{id}", Name = "PartiallyUpdateCardioSet")]
        [RequestHeaderMatchesApiVersion(ConstantPropertyNames.ApiVersion, new[] { "1" })]
        public async Task<IActionResult> UpdatePartially(int id, [FromBody]JsonPatchDocument<CardioSetOfmForPatch> jsonPatchDocument)
        {
            if (jsonPatchDocument == null)
            {
                ModelState.AddModelError(_shortCamelCasedControllerName, "You sent an empty body (null) for " + _shortCamelCasedControllerName + " with id=" + id);
                return new EntityNotFoundObjectResult(ModelState);
            }

            try
            {
                // Get entity with original values from context
                var ofmForPatch = await _asyncPatchForHttpMethods.GetByIdOfmForPatch(id);
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
                var ofmForGet = _asyncPatchForHttpMethods.UpdatePartially(ofmForPatch).Result;
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
