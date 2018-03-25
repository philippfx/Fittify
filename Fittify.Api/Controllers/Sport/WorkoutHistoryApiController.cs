using System;
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
    [Route("api/workouthistories")]
    public class WorkoutHistoryApiController :
        Controller,
        IAsyncGetByIdForHttp<int>,
        //IAsyncGetCollectionByByDateTimeStartEndForHttp,
        IAsyncPostForHttp<WorkoutHistoryOfmForPost>,
        IAsyncPatchForHttp<WorkoutHistoryOfmForPatch, int>,
        IAsyncDeleteForHttp<int>
    {
        private readonly IAsyncGetOfmById<WorkoutHistoryOfmForGet, int> _asyncGetOfmById;
        private readonly AsyncGetOfmCollectionForWorkoutHistory _asyncGetOfmCollection;
        private readonly IAsyncPostOfm<WorkoutHistoryOfmForGet, WorkoutHistoryOfmForPost> _asyncPostForHttpMethods;
        private readonly IAsyncPatchOfm<WorkoutHistoryOfmForGet, WorkoutHistoryOfmForPatch, int> _asyncPatchForHttpMethods;
        private readonly IAsyncDeleteOfm<int> _asyncDeleteForHttpMethods;
        private readonly WorkoutHistoryRepository _repo;
        private readonly string _shortCamelCasedControllerName;
        private readonly IUrlHelper _urlHelper;
        private readonly ControllerGuardClauses<WorkoutHistoryOfmForGet> _controllerGuardClause;
        private readonly HateoasLinkFactory<int> _hateoasLinkFactory;
        private readonly IncomingHeaders _incomingHeaders;

        public WorkoutHistoryApiController(FittifyContext fittifyContext,
            IActionDescriptorCollectionProvider adcProvider,
            IUrlHelper urlHelper,
            IPropertyMappingService propertyMappingService,
            ITypeHelperService typeHelperService,
            IConfiguration appConfiguration,
            IHttpContextAccessor httpContextAccesor)
        {
            _repo = new WorkoutHistoryRepository(fittifyContext);
            _asyncPostForHttpMethods = new AsyncPostOfm<WorkoutHistoryRepository, WorkoutHistory, WorkoutHistoryOfmForGet, WorkoutHistoryOfmForPost, int>(_repo);
            _asyncPatchForHttpMethods = new AsyncPatchOfm<WorkoutHistoryRepository, WorkoutHistory, WorkoutHistoryOfmForGet, WorkoutHistoryOfmForPatch, int>(_repo);
            _asyncDeleteForHttpMethods = new AsyncDeleteOfm<WorkoutHistoryRepository, WorkoutHistory, int>(_repo, adcProvider);
            _shortCamelCasedControllerName = nameof(WorkoutHistoryApiController).ToShortCamelCasedControllerNameOrDefault();
            _asyncGetOfmById = new AsyncGetOfmById<WorkoutHistoryRepository, WorkoutHistory, WorkoutHistoryOfmForGet, int>(_repo, urlHelper, adcProvider, propertyMappingService, typeHelperService, this);
            _asyncGetOfmCollection = new AsyncGetOfmCollectionForWorkoutHistory(_repo, urlHelper, adcProvider, propertyMappingService, typeHelperService, this);
            _urlHelper = urlHelper;
            _controllerGuardClause = new ControllerGuardClauses<WorkoutHistoryOfmForGet>(this);
            _hateoasLinkFactory = new HateoasLinkFactory<int>(urlHelper, nameof(WorkoutHistoryApiController));
            _incomingHeaders = Mapper.Map<IncomingHeaders>(httpContextAccesor.HttpContext.Items[nameof(IncomingRawHeaders)] as IncomingRawHeaders);
        }

        [HttpGet("{id}", Name = "GetWorkoutHistoryById")]
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

        [HttpGet(Name = "GetWorkoutHistoryCollection")]
        [RequestHeaderMatchesApiVersion(ConstantPropertyNames.ApiVersion, new[] { "1" })]
        public async Task<IActionResult> GetCollection(WorkoutHistoryResourceParameters resourceParameters)
        {
            var ofmForGetCollectionQueryResult = await _asyncGetOfmCollection.GetCollection(resourceParameters);
            if (!_controllerGuardClause.ValidateGetCollection(ofmForGetCollectionQueryResult, out ObjectResult objectResult)) return objectResult;
            var expandableOfmForGetCollection = ofmForGetCollectionQueryResult.ReturnedTOfmForGetCollection.OfmForGets.ToExpandableOfmForGets();
            if (_incomingHeaders.IncludeHateoas) expandableOfmForGetCollection = expandableOfmForGetCollection.CreateHateoasLinksForeachExpandableOfmForGet<WorkoutHistoryOfmForGet, int>(_urlHelper, nameof(WorkoutHistoryApiController), resourceParameters.Fields).ToList(); // Todo Improve! The data is only superficially shaped AFTER a full query was run against the database
            expandableOfmForGetCollection = expandableOfmForGetCollection.Shape(resourceParameters.Fields, _incomingHeaders.IncludeHateoas).ToList();
            if (!_incomingHeaders.IncludeHateoas)
            {
                return Ok(expandableOfmForGetCollection);
            }

            dynamic result = new
            {
                value = expandableOfmForGetCollection,
                links = _hateoasLinkFactory.CreateLinksForOfmGetCollectionQueryIncludeByDateTimeStartEnd(resourceParameters,
                    ofmForGetCollectionQueryResult.HasPrevious, ofmForGetCollectionQueryResult.HasNext).ToList()
            };
            return Ok(result);
        }

        [HttpGet("range/{inputString}", Name = "GetWorkoutHistoriesByRangeOfIds")]
        [RequestHeaderMatchesApiVersion(ConstantPropertyNames.ApiVersion, new[] { "1" })]
        public async Task<IActionResult> GetByRangeOfIds(string inputString)
        {
            var entityCollection = await _repo.GetByCollectionOfIds(RangeString.ToCollectionOfId(inputString));
            var ofmCollection = Mapper.Map<List<WorkoutHistory>, List<WorkoutHistoryOfmForGet>>(entityCollection.ToList());
            if (ofmCollection.Count == 0) // Todo mock "not found" as query paramter 
            {
                ModelState.AddModelError(_shortCamelCasedControllerName, $"No {_shortCamelCasedControllerName.ToPlural()} found");
                return new EntityNotFoundObjectResult(ModelState);
            }
            return Ok(ofmCollection);
        }

        [HttpPost(Name = "CreateWorkoutHistory")]
        [RequestHeaderMatchesApiVersion(ConstantPropertyNames.ApiVersion, new[] { "1" })]
        public async Task<IActionResult> Post([FromBody] WorkoutHistoryOfmForPost ofmForPost)
        {
            if (ofmForPost == null) return BadRequest();

            if (!ModelState.IsValid)
            {
                return new UnprocessableEntityObjectResult(ModelState);
            }

            var ofmForGet = await _asyncPostForHttpMethods.Post(ofmForPost);

            var result = CreatedAtRoute(routeName: "GetWorkoutHistoryById", routeValues: new { id = ofmForGet.Id }, value: ofmForGet);
            return result;
        }

        [HttpDelete("{id}", Name = "DeleteWorkoutHistory")]
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

        [HttpPatch("{id}", Name = "PartiallyUpdateWorkoutHistory")]
        [RequestHeaderMatchesApiVersion(ConstantPropertyNames.ApiVersion, new[] { "1" })]
        public async Task<IActionResult> UpdatePartially(int id, [FromBody]JsonPatchDocument<WorkoutHistoryOfmForPatch> jsonPatchDocument)
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
