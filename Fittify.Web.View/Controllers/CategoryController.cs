﻿using System.Linq;
using System.Threading.Tasks;
using Fittify.Api.OuterFacingModels.Sport.Patch;
using Fittify.Api.OuterFacingModels.Sport.Post;
using Fittify.Common.Helpers.ResourceParameters.Sport;
using Fittify.Web.ViewModelRepository.Sport;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Fittify.Web.View.Controllers
{
    [Route("categories")]
    public class CategoryController : Controller
    {
        private readonly CategoryViewModelRepository _categoryViewModelRepository;
        public CategoryController(IConfiguration appConfiguration)
        {
            _categoryViewModelRepository = new CategoryViewModelRepository(appConfiguration);
        }

        public async Task<IActionResult> Overview()
        {
            var categoryViewModelCollectionResult = await _categoryViewModelRepository.GetCollection(new CategoryResourceParameters());

            if ((int)categoryViewModelCollectionResult.HttpStatusCode != 200)
            {
                // Todo: Do something when posting failed
            }

            return View("Overview", categoryViewModelCollectionResult.ViewModelForGetCollection.ToList());
        }

        [HttpPost]
        public async Task<RedirectToActionResult> CreateCategory([FromForm] CategoryOfmForPost categoryOfmForPost, [FromQuery] int workoutId)
        {
            var postResult = await _categoryViewModelRepository.Create(categoryOfmForPost);

            if ((int)postResult.HttpStatusCode != 201)
            {
                // Todo: Do something when posting failed
            }

            return RedirectToAction("Overview", "Category", new { workoutId = workoutId });
        }

        [HttpPost]
        [Route("{id}/deletion")]
        public async Task<RedirectToActionResult> Delete(int id)
        {
            var deleteResult = await _categoryViewModelRepository.Delete(id);

            if ((int)deleteResult.HttpStatusCode != 204)
            {
                // Todo: Do something when deleting failed
            }

            return RedirectToAction("Overview", "Category", null);
        }

        [HttpPost]
        [Route("{id}/patch")]
        public async Task<RedirectToActionResult> PatchName(int id, [FromForm] CategoryOfmForPatch categoryOfmForPatch)
        {
            JsonPatchDocument jsonPatchDocument = new JsonPatchDocument();

            jsonPatchDocument.Replace("/" + nameof(categoryOfmForPatch.Name), categoryOfmForPatch.Name);

            var patchResult = await _categoryViewModelRepository.PartiallyUpdate(id, jsonPatchDocument);

            if ((int)patchResult.HttpStatusCode != 200)
            {
                // Todo: Do something when posting failed
            }

            return RedirectToAction("Overview", "Category", null);
        }
    }
}
