﻿using System;
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
        private readonly string _shortCamelCasedControllerName;

        public CategoryApiController(FittifyContext fittifyContext)
        {
            _repo = new CategoryRepository(fittifyContext);
            _gppdForHttpMethods = new GppdOfm<CategoryRepository, Category, CategoryOfmForGet, CategoryOfmForPost, CategoryOfmForPatch, int>(_repo);
            _shortCamelCasedControllerName = nameof(CategoryApiController).ToShortCamelCasedControllerName();
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

        [HttpGet(Name = "GetAllCategories")]
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
                var entity = _repo.GetById(id).Result;
                if (entity == null)
                {
                    ModelState.AddModelError(_shortCamelCasedControllerName, "No " + _shortCamelCasedControllerName + " found for id=" + id);
                    return new EntityNotFoundObjectResult(ModelState);
                }

                // Convert entity to ofm
                var ofmForPatch = Mapper.Map<CategoryOfmForPatch>(entity);

                // Apply new values from jsonPatchDocument to ofm (the ofm that was just created based on fresh entity from context)
                jsonPatchDocument.ApplyTo(ofmForPatch);

                // Validating ofm
                TryValidateModel(ofmForPatch);
                if (!ModelState.IsValid)
                {
                    return new UnprocessableEntityObjectResult(ModelState);
                }

                // Convert ofm with new values back to entity (by overriding entity field values)
                entity = Mapper.Map(ofmForPatch, entity);

                // Update entity in context
                entity = await _repo.Update(entity);

                // returning the patched ofm as response
                var ofmForGet = Mapper.Map<CategoryOfmForGet>(entity);
                return new JsonResult(ofmForGet);

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            //var ofmForGet = await _gppdForHttpMethods.UpdatePartially(id, jsonPatchDocument);
            //return new JsonResult(ofmForGet);
        }
    }
}