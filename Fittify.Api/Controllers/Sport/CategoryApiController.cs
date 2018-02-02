using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using AutoMapper;
using Fittify.Api.Controllers.HttpMethodInterfaces;
using Fittify.Api.Extensions;
using Fittify.Api.Helpers;
using Fittify.Api.OfmRepository;
using Fittify.Api.OuterFacingModels.Sport.Get;
using Fittify.Api.OuterFacingModels.Sport.Patch;
using Fittify.Api.OuterFacingModels.Sport.Post;
using Fittify.Common.Extensions;
using Fittify.Common.Helpers;
using Fittify.DataModelRepositories;
using Fittify.DataModelRepositories.Repository.Sport;
using Fittify.DataModels.Models.Sport;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Fittify.Api.Controllers.Sport
{
    [Route("api/categories")]
    public class CategoryApiController :
        Controller,
        IAsyncGppdForHttp<int, CategoryOfmForPost, CategoryOfmForPatch>
    {
        private readonly GppdOfm<CategoryRepository, Category, CategoryOfmForGet, CategoryOfmForPost, CategoryOfmForPatch, int> _gppdForHttpMethods;
        private readonly CategoryRepository _repo;
        private readonly GetMoreForHttpIntId<CategoryRepository, Category> _getMoreForIntId;
        private readonly string _shortCamelCasedControllerName;

        public CategoryApiController(FittifyContext fittifyContext)
        {
            _repo = new CategoryRepository(fittifyContext);
            _gppdForHttpMethods = new GppdOfm<CategoryRepository, Category, CategoryOfmForGet, CategoryOfmForPost, CategoryOfmForPatch, int>(_repo);
            _getMoreForIntId = new GetMoreForHttpIntId<CategoryRepository, Category>(_repo);
            _shortCamelCasedControllerName = nameof(CategoryApiController).ToShortCamelCasedControllerName();
        }

        [HttpGet("{id:int}", Name="GetCategoryById")]
        public async Task<IActionResult> GetById(int id)
        {
            var entity = await _repo.GetById(id);
            if (entity == null)
            {
                ModelState.AddModelError(_shortCamelCasedControllerName, "Nothing found for id=" + id);
                return new EntityNotFoundObjectResult(ModelState);
            }

            var ofmForGet = Mapper.Map<CategoryOfmForGet>(entity);
            
            return Ok(ofmForGet);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var allEntites = await _gppdForHttpMethods.GetAll();
            var allOfmForGet = Mapper.Map<ICollection<CategoryOfmForGet>>(allEntites);
            //allOfmForGet = new Collection<CategoryOfmForGet>(); // Todo mock "not found" as query paramter 
            if (allOfmForGet.Count == 0)
            {
                ModelState.AddModelError(_shortCamelCasedControllerName, $"No {_shortCamelCasedControllerName.ToPlural()} found");
                return new EntityNotFoundObjectResult(ModelState);
            }
            return new JsonResult(allOfmForGet);
        }

        [HttpGet("range/{inputString}", Name = "GetCategoriesByRangeOfIds")]
        public async Task<IActionResult> GetByRangeOfIds(string inputString)
        {
            // Todo logic is executed on different levels compared to _gppdForHttpMethods methods
            // Todo Should return 200
            //var result =  await _getMoreForIntId.GetByRangeOfIds(inputString);
            //return Ok(result);
            return await _getMoreForIntId.GetByRangeOfIds(inputString);
        }

        [HttpPost("new")]
        public async Task<CreatedAtRouteResult> Post([FromBody] CategoryOfmForPost ofmForPost)
        {
            var ofmForGet = await _gppdForHttpMethods.Post(ofmForPost);
            var result = CreatedAtRoute(routeName: "GetCategoriesByRangeOfIds", routeValues: new { inputString = ofmForGet.Id }, value: ofmForGet);
            return result;
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _gppdForHttpMethods.Delete(id);
            return NoContent();
        }
        
        [HttpPatch("{id:int}")]
        public async Task<IActionResult> UpdatePartially(int id, JsonPatchDocument<CategoryOfmForPatch> jsonPatchDocument)
        {
            var ofmForGet = await _gppdForHttpMethods.UpdatePartially(id, jsonPatchDocument);
            return new JsonResult(ofmForGet);
        }
    }
}
