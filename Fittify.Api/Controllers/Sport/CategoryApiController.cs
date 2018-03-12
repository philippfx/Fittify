using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Fittify.Api.Controllers.HttpMethodInterfaces;
using Fittify.Api.Extensions;
using Fittify.Api.Helpers;
using Fittify.Api.OfmRepository;
using Fittify.Api.OuterFacingModels;
using Fittify.Api.OuterFacingModels.Sport.Get;
using Fittify.Api.OuterFacingModels.Sport.Patch;
using Fittify.Api.OuterFacingModels.Sport.Post;
using Fittify.Api.Services;
using Fittify.Common.Extensions;
using Fittify.Common.Helpers;
using Fittify.Common.Helpers.ResourceParameters;
using Fittify.DataModelRepositories;
using Fittify.DataModelRepositories.Repository.Sport;
using Fittify.DataModels.Models.Sport;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Fittify.Api.Controllers.Sport
{
    [Route("api/categories")]
    public class CategoryApiController :
        Controller, 
        IAsyncGetByIdForHttp<int>,
        IAsyncGetCollectionByNameSearchForHttp,
        IAsyncPostForHttp<CategoryOfmForPost>,
        IAsyncPatchForHttp<CategoryOfmForPatch, int>,
        IAsyncDeleteForHttp<int>
    {
        private readonly AsyncPostPatchDeleteOfm<CategoryRepository, Category, CategoryOfmForGet, CategoryOfmForPost, CategoryOfmForPatch, int> _asyncPostPatchDeleteForHttpMethods;
        private readonly IAsyncGetOfmById<CategoryOfmForGet, int> _asyncGetOfmById;
        private readonly IAsyncGetOfmCollectionByNameSearch<CategoryOfmForGet> _asyncGetOfmCollectionIncludeByNameSearch;
        private readonly CategoryRepository _repo;
        private readonly string _shortCamelCasedControllerName;
        private ITypeHelperService _typeHelperService;
        private readonly IUrlHelper _urlHelper;
        private readonly ControllerGuardClauses<CategoryOfmForGet> controllerGuardClause;
        private readonly HateoasLinkFactory<CategoryOfmForGet, int> _hateoasLinkFactory;

        public CategoryApiController(FittifyContext fittifyContext,
            IActionDescriptorCollectionProvider adcProvider,
            IUrlHelper urlHelper,
            IPropertyMappingService propertyMappingService,
            ITypeHelperService typeHelperService)
        {
            _repo = new CategoryRepository(fittifyContext);
            _asyncPostPatchDeleteForHttpMethods = new AsyncPostPatchDeleteOfm<CategoryRepository, Category, CategoryOfmForGet, CategoryOfmForPost, CategoryOfmForPatch, int>(_repo, urlHelper, adcProvider, this);
            _shortCamelCasedControllerName = nameof(CategoryApiController).ToShortCamelCasedControllerNameOrDefault();
            _asyncGetOfmById = new AsyncGetOfmCollectionIncludeByNameSearch<CategoryRepository, Category, CategoryOfmForGet, int>(_repo, urlHelper, adcProvider, propertyMappingService, typeHelperService, this);
            _asyncGetOfmCollectionIncludeByNameSearch = new AsyncGetOfmCollectionIncludeByNameSearch<CategoryRepository, Category, CategoryOfmForGet, int>(_repo, urlHelper, adcProvider, propertyMappingService, typeHelperService, this);
            _urlHelper = urlHelper;
            _typeHelperService = typeHelperService;
            controllerGuardClause = new ControllerGuardClauses<CategoryOfmForGet>(this);
            _hateoasLinkFactory = new HateoasLinkFactory<CategoryOfmForGet, int>(urlHelper, nameof(CategoryApiController));
        }

        [HttpGet("{id:int}", Name = "GetCategoryById")]
        public async Task<IActionResult> GetById(int id, [FromQuery] string fields)
        {
            var ofmForGetQueryResult = await _asyncGetOfmById.GetById(id, fields);
            if (!controllerGuardClause.ValidateGetById(ofmForGetQueryResult, id, out ObjectResult objectResult))
            {
                return objectResult;
            }
            var expandable = ofmForGetQueryResult.ReturnedTOfmForGet.ToExpandableOfm();
            var shapedExpandable = expandable.Shape(fields);
            //var _hateoasLinkFactory = new _hateoasLinkFactory<CategoryOfmForGet, int>(_urlHelper, nameof(CategoryApiController));
            shapedExpandable.Add("links", _hateoasLinkFactory.CreateLinksForOfmForGet(id, fields).ToList());

            //return Ok(ofmForGetQueryResult.ReturnedTOfmForGet.ShapeData(fields)); // Todo Improve! The data is only superficially shaped AFTER a full query was run against the database
            return Ok(shapedExpandable);
        }

        [HttpGet(Name = "GetCategoryCollection")]
        public async Task<IActionResult> GetCollection(SearchQueryResourceParameters resourceParameters)
        {
            var ofmForGetCollectionQueryResult = await _asyncGetOfmCollectionIncludeByNameSearch.GetCollection(resourceParameters);
            if (!controllerGuardClause.ValidateGetCollection(ofmForGetCollectionQueryResult, out ObjectResult objectResult))
            {
                return objectResult;
            }
            var expandableOfmForGetCollection = ofmForGetCollectionQueryResult.ReturnedTOfmForGetCollection.OfmForGets.ToExpandableOfmForGets();
            var shapedExpandableOfmForGetCollection = expandableOfmForGetCollection.Shape(resourceParameters.Fields).ToList();
            var links = _hateoasLinkFactory.CreateLinksForOfmGetCollectionIncludeByNameSearch(resourceParameters,
                ofmForGetCollectionQueryResult.HasPrevious, ofmForGetCollectionQueryResult.HasNext);
            var linkedResource = new
            {
                value = shapedExpandableOfmForGetCollection,
                //links = ofmForGetCollectionQueryResult.ReturnedTOfmForGetCollection.HateoasLinks
                links = links
            };
            return Ok(linkedResource); // Todo Improve! The data is only superficially shaped AFTER a full query was run against the database
        }

        [HttpGet("range/{inputString}", Name = "GetCategoriesByRangeOfIds")]
        public async Task<IActionResult> GetByRangeOfIds(string inputString)
        {
            var entityCollection = await _repo.GetByCollectionOfIds(RangeString.ToCollectionOfId(inputString));
            var ofmCollection = Mapper.Map<List<Category>, List<CategoryOfmForGet>>(entityCollection.ToList());
            if (ofmCollection.Count == 0) // Todo mock "not found" as query paramter 
            {
                ModelState.AddModelError(_shortCamelCasedControllerName, $"No {_shortCamelCasedControllerName.ToPlural()} found");
                return new EntityNotFoundObjectResult(ModelState);
            }
            return Ok(ofmCollection);
        }

        [HttpPost("new", Name = "CreateCategory")]
        public async Task<IActionResult> Post([FromBody] CategoryOfmForPost ofmForPost)
        {
            if (ofmForPost == null) return BadRequest();

            if (!ModelState.IsValid)
            {
                return new UnprocessableEntityObjectResult(ModelState);
            }

            var ofmForGet = await _asyncPostPatchDeleteForHttpMethods.Post(ofmForPost);
            
            var result = CreatedAtRoute(routeName: "GetCategoryById", routeValues: new { id = ofmForGet.Id }, value: ofmForGet);
            return result;
        }

        [HttpDelete("{id:int}", Name = "DeleteCategory")]
        public async Task<IActionResult> Delete(int id)
        {
            bool successfullyDeleted = await _asyncPostPatchDeleteForHttpMethods.Delete(id);
            if (successfullyDeleted) return NoContent();
            else return StatusCode(409);

        }

        [HttpDelete("mydelete/{id:int}")]
        public async Task<IActionResult> MyDelete(int id)
        {
            // Todo Refactor route, merge it with the original route and make this method use less code
            var methodBase = typeof(CategoryApiController).GetMethod("GetByRangeOfIds");

            var attribute = (HttpGetAttribute)methodBase.GetCustomAttributes(typeof(HttpGetAttribute), true)[0];

            var blockingOfmForGetLists = await _asyncPostPatchDeleteForHttpMethods.MyDelete(id);
            if (blockingOfmForGetLists.Count != 0)
            {
                foreach (var blockingOfmForGet in blockingOfmForGetLists)
                {
                    ModelState.AddModelError(_shortCamelCasedControllerName, blockingOfmForGet);
                }
            }

            if (!ModelState.IsValid)
            {
                return new UnprocessableEntityObjectResult(ModelState);
            }

            return NoContent();
        }

        [HttpPatch("{id:int}", Name = "PartiallyUpdateCategory")]
        public async Task<IActionResult> UpdatePartially(int id, [FromBody]JsonPatchDocument<CategoryOfmForPatch> jsonPatchDocument)
        {
            if (jsonPatchDocument == null)
            {
                ModelState.AddModelError(_shortCamelCasedControllerName, "You sent an empty body (null) for " + _shortCamelCasedControllerName + " with id=" + id);
                return new EntityNotFoundObjectResult(ModelState);
            }

            try
            {
                // Get entity with original values from context
                var ofmForPatch = await _asyncPostPatchDeleteForHttpMethods.GetByIdOfmForPatch(id);
                if (ofmForPatch == null)
                {
                    ModelState.AddModelError(_shortCamelCasedControllerName, "No " + _shortCamelCasedControllerName + " found for id=" + id);
                    return new EntityNotFoundObjectResult(ModelState);
                }

                // Apply new values from jsonPatchDocument to ofm (the ofm that was just created based on fresh entity from context)
                jsonPatchDocument.ApplyTo(ofmForPatch, ModelState);

                // Validating ofm
                TryValidateModel(ofmForPatch);// This is important to catch invalid model states caused by applying the jsonPatch, for example if a required field (previously had a value) is now set to null
                if (!ModelState.IsValid)
                {
                    return new UnprocessableEntityObjectResult(ModelState);
                }

                // returning the patched ofm as response
                var ofmForGet = _asyncPostPatchDeleteForHttpMethods.UpdatePartially(ofmForPatch).Result;
                return new JsonResult(ofmForGet);

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            //var ofmForGet = await _asyncPostPatchDeleteForHttpMethods.UpdatePartially(id, jsonPatchDocument);
            //return new JsonResult(ofmForGet);
        }
    }
}
