using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Fittify.Api.Controllers.HttpMethodInterfaces;
using Fittify.Api.Extensions;
using Fittify.Api.Helpers;
using Fittify.Api.OfmRepository;
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
        IAsyncGppdForHttp<int, CategoryOfmForPost, CategoryOfmForPatch>
    {
        private readonly AsyncPostPatchDeleteOfm<CategoryRepository, Category, CategoryOfmForGet, CategoryOfmForPost, CategoryOfmForPatch, int> _asyncPostPatchDeleteForHttpMethods;
        private readonly IAsyncGetOfmByNameSearch<CategoryOfmForGet, int> _asyncGetOfmByNameSearch;
        private readonly CategoryRepository _repo;
        private readonly string _shortCamelCasedControllerName;
        private ITypeHelperService _typeHelperService;

        public CategoryApiController(FittifyContext fittifyContext,
            IActionDescriptorCollectionProvider adcProvider,
            IUrlHelper urlHelper,
            IPropertyMappingService propertyMappingService,
            ITypeHelperService typeHelperService)
        {
            _repo = new CategoryRepository(fittifyContext);
            _asyncPostPatchDeleteForHttpMethods = new AsyncPostPatchDeleteOfm<CategoryRepository, Category, CategoryOfmForGet, CategoryOfmForPost, CategoryOfmForPatch, int>(_repo, urlHelper, adcProvider);
            _shortCamelCasedControllerName = nameof(CategoryApiController).ToShortCamelCasedControllerNameOrDefault();
            _asyncGetOfmByNameSearch = new AsyncGetOfmByNameSearch<CategoryRepository, Category, CategoryOfmForGet, int>(_repo, urlHelper, adcProvider, propertyMappingService, typeHelperService);
            _typeHelperService = typeHelperService;
        }

        [HttpGet("{id:int}", Name="GetCategoryById")]
        public async Task<IActionResult> GetById(int id)
        {
            var entity = await _repo.GetById(id);
            if (entity == null)
            {
                ModelState.AddModelError(_shortCamelCasedControllerName, "No " + _shortCamelCasedControllerName + " found for id=" + id);
                return new EntityNotFoundObjectResult(ModelState);
            }

            var ofmForGet = Mapper.Map<CategoryOfmForGet>(entity);
            
            return Ok(ofmForGet);
        }

        [HttpGet("datashaped/{id:int}", Name = "GetCategoryByIdDataShaped")]
        public async Task<IActionResult> GetByIdDataShaped(int id, [FromQuery] string fields)
        {
            var ofmForGetQueryResult = await _asyncGetOfmByNameSearch.GetByIdDataShaped(id, fields);

            if (ofmForGetQueryResult.ErrorMessages.Count > 0)
            {
                foreach (var errorMessage in ofmForGetQueryResult.ErrorMessages)
                {
                    ModelState.AddModelError(_shortCamelCasedControllerName, errorMessage);
                }
                return new UnprocessableEntityObjectResult(ModelState);
            }

            //var entity = await _repo.GetById(id);
            if (ofmForGetQueryResult.ReturnedTOfmForGet == null)
            {
                ModelState.AddModelError(_shortCamelCasedControllerName, "No " + _shortCamelCasedControllerName + " found for id=" + id);
                return new EntityNotFoundObjectResult(ModelState);
            }

            return Ok(ofmForGetQueryResult.ReturnedTOfmForGet.ShapeData(fields));
        }

        [HttpGet(Name = "GetAllCategories")]
        public async Task<IActionResult> GetAll()
        {
            var allEntites = await _asyncGetOfmByNameSearch.GetAll();
            var allOfmForGet = Mapper.Map<IEnumerable<CategoryOfmForGet>>(allEntites).ToList();
            //allOfmForGet = new Collection<CategoryOfmForGet>(); // Todo mock "not found" as query paramter 
            if (allOfmForGet.Count == 0)
            {
                ModelState.AddModelError(_shortCamelCasedControllerName, $"No {_shortCamelCasedControllerName.ToPlural()} found");
                return new EntityNotFoundObjectResult(ModelState);
            }
            return new JsonResult(allOfmForGet);
        }

        [HttpGet("paged", Name = "GetAllPagedCategories")]
        public async Task<IActionResult> GetAllPaged(SearchQueryResourceParameters resourceParameters)
        {
            var allEntites = await _asyncGetOfmByNameSearch.GetAllPaged(resourceParameters, this);
            
            var allOfmForGet = Mapper.Map<IEnumerable<CategoryOfmForGet>>(allEntites).ToList();
            //allOfmForGet = new Collection<CategoryOfmForGet>(); // Todo mock "not found" as query paramter 
            if (allOfmForGet.Count == 0)
            {
                ModelState.AddModelError(_shortCamelCasedControllerName, $"No {_shortCamelCasedControllerName.ToPlural()} found");
                return new EntityNotFoundObjectResult(ModelState);
            }
            return new JsonResult(allOfmForGet);
        }

        [HttpGet("pagedandsearchname", Name = "GetAllPagedAndSearchNameCategories")]
        public async Task<IActionResult> GetAllPagedAndSearchName(SearchQueryResourceParameters resourceParameters)
        {
            var allEntites = await _asyncGetOfmByNameSearch.GetAllPagedAndSearchName(resourceParameters, this);

            var allOfmForGet = Mapper.Map<IEnumerable<CategoryOfmForGet>>(allEntites).ToList();
            //allOfmForGet = new Collection<CategoryOfmForGet>(); // Todo mock "not found" as query paramter 
            if (allOfmForGet.Count == 0)
            {
                ModelState.AddModelError(_shortCamelCasedControllerName, $"No {_shortCamelCasedControllerName.ToPlural()} found");
                return new EntityNotFoundObjectResult(ModelState);
            }
            return new JsonResult(allOfmForGet);
        }

        [HttpGet("pagedandsearchnameandordered", Name = "GetAllPagedAndSearchNameAndOrderedCategories")]
        public async Task<IActionResult> GetAllPagedAndSearchNameAndOrdered(SearchQueryResourceParameters resourceParameters)
        {
            var allentities = await _asyncGetOfmByNameSearch.GetAllPagedAndSearchNameAndOrdered(resourceParameters, this);

            var allOfmForGet = Mapper.Map<IEnumerable<CategoryOfmForGet>>(allentities).ToList();
            //allOfmForGet = new Collection<CategoryOfmForGet>(); // Todo mock "not found" as query paramter 
            if (allOfmForGet.Count == 0)
            {
                ModelState.AddModelError(_shortCamelCasedControllerName, $"No {_shortCamelCasedControllerName.ToPlural()} found");
                return new EntityNotFoundObjectResult(ModelState);
            }
            return new JsonResult(allOfmForGet);
        }

        [HttpGet("pagedandsearchnameandorderedincludingerrormessages", Name = "GetAllPagedAndSearchNameAndOrderedCategoriesIncludingErrorMessages")]
        public async Task<IActionResult> GetAllPagedAndSearchNameAndOrderedIncludingErrorMessages(SearchQueryResourceParameters resourceParameters)
        {
            var ofmForGetResult = await _asyncGetOfmByNameSearch.GetAllPagedAndSearchNameAndOrderedIncludingErrorMessages(resourceParameters, this);

            if (ofmForGetResult.ErrorMessages.Count() > 0)
            {
                foreach (var errorMessage in ofmForGetResult.ErrorMessages)
                {
                    ModelState.AddModelError(_shortCamelCasedControllerName, errorMessage);
                }
                return new UnprocessableEntityObjectResult(ModelState);
            }

            var allOfmForGet = ofmForGetResult.ReturnedTOfmForGetCollection.ToList();
            //allOfmForGet = new Collection<CategoryOfmForGet>(); // Todo mock "not found" as query paramter 
            if (allOfmForGet.Count == 0)
            {
                ModelState.AddModelError(_shortCamelCasedControllerName, $"No {_shortCamelCasedControllerName.ToPlural()} found");
                return new EntityNotFoundObjectResult(ModelState);
            }
            return new JsonResult(allOfmForGet);
        }

        [HttpGet("pagedandsearchnameandorderedincludingerrormessagesanddatashaped", Name = "GetAllPagedAndSearchNameAndOrderedCategoriesIncludingErrorMessagesAndDataShaped")]
        public async Task<IActionResult> GetAllPagedAndSearchNameAndOrderedIncludingErrorMessagesAndDataShaped(SearchQueryResourceParameters resourceParameters)
        {
            var ofmForGetResult = await _asyncGetOfmByNameSearch.GetAllPagedAndSearchNameAndOrderedIncludingErrorMessages(resourceParameters, this);

            if (ofmForGetResult.ErrorMessages.Count() > 0)
            {
                foreach (var errorMessage in ofmForGetResult.ErrorMessages)
                {
                    ModelState.AddModelError(_shortCamelCasedControllerName, errorMessage);
                }
                return new UnprocessableEntityObjectResult(ModelState);
            }

            var allOfmForGet = ofmForGetResult.ReturnedTOfmForGetCollection.ToList();
            //allOfmForGet = new Collection<CategoryOfmForGet>(); // Todo mock "not found" as query paramter 
            if (allOfmForGet.Count == 0)
            {
                ModelState.AddModelError(_shortCamelCasedControllerName, $"No {_shortCamelCasedControllerName.ToPlural()} found");
                return new EntityNotFoundObjectResult(ModelState);
            }
            return new JsonResult(allOfmForGet.ShapeData(resourceParameters.Fields)); // Todo Improve! The data is only superficially shaped AFTER a full query was run against the database
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

        [HttpPost("new")]
        public async Task<IActionResult> Post([FromBody] CategoryOfmForPost ofmForPost)
        {
            if (ofmForPost == null) return BadRequest();

            if (!ModelState.IsValid)
            {
                return new UnprocessableEntityObjectResult(ModelState);
            }

            var ofmForGet = await _asyncPostPatchDeleteForHttpMethods.Post(ofmForPost);
            var result = CreatedAtRoute(routeName: "GetCategoriesByRangeOfIds", routeValues: new { inputString = ofmForGet.Id }, value: ofmForGet);
            return result;
        }

        [HttpDelete("{id:int}")]
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
            var methodBase = typeof(ExerciseHistoryApiController).GetMethod("GetByRangeOfIds");

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

        [HttpPatch("{id:int}")]
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
